using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Command;
using VH.Bases;
using VH.SimpleUI.Entities;

//using Chronos.Extensions.Windows;
//using Chronos.Presentation.Core.Windows;
//using Chronos.Presentation.Windows.Controls;
//using nRoute.Components;

namespace VH.UI.UserControls
{
    [TemplatePart(Name = PART_CloseButton, Type = typeof (ButtonBase))]
    [TemplatePart(Name = PART_ContentPresenter, Type = typeof (ContentPresenter))]
    [TemplateVisualState(Name = NormalVisualState, GroupName = WindowStateGroup)]
    [TemplateVisualState(Name = MinimizedVisualState, GroupName = WindowStateGroup)]
    [TemplateVisualState(Name = MaximizedVisualState, GroupName = WindowStateGroup)]
    public class ModalDailogWindow : UserControl
    {
        #region . Fields .

        // private Panel parent;
        private DispatcherFrame dispatcherFrame;

        #endregion

        #region · Template Parts ·

        private const string PART_ContentPresenter = "PART_ContentPresenter";
        private const string PART_Root = "PART_Root";
        private const string PART_MaximizeButton = "PART_MaximizeButton";
        private const string PART_MinimizeButton = "PART_MinimizeButton";
        private const string PART_CloseButton = "PART_CloseButton";

        #endregion

        #region · Visual States ·

        private const string WindowStateGroup = "WindowState";
        private const string NormalVisualState = "Normal";
        private const string MinimizedVisualState = "Minimized";
        private const string MaximizedVisualState = "Maximized";

        #endregion

        #region · Misc ·

        private const int MaximizeMargin = 20;

        #endregion

        public VMMessageDailog VMMessageDailog { get; set; }
        protected Window Container
        {
            get;
            private set;
        }

        public ModalDailogWindow()
        {
            
        }

        public ModalDailogWindow(VMMessageDailog vmMessageDailog)
        {
            VMMessageDailog = vmMessageDailog;
            ApplyVM();
            Container = CreateContainer();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.contentPresenter = this.GetTemplateChild(PART_ContentPresenter) as ContentPresenter;
        }

        #region · Dependency Properties ·

        public static readonly DependencyProperty IdProperty =
            DependencyProperty.Register("Id", typeof (Guid), typeof (ModalDailogWindow),
                                        new FrameworkPropertyMetadata(Guid.NewGuid()));

        public static readonly DependencyProperty ParentWindowProperty =
            DependencyProperty.Register("ParentWindow",
                                        typeof (UIElement),
                                        typeof (ModalDailogWindow),
                                        new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register("IsActive",
                                        typeof (bool),
                                        typeof (ModalDailogWindow),
                                        new FrameworkPropertyMetadata(true));


        /// <summary>
        /// Identifies the Title dependency property.
        /// </summary>
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof (String), typeof (ModalDailogWindow),
                                        new FrameworkPropertyMetadata(String.Empty));

        /// <summary>
        /// Identifies the WindowState dependency property.
        /// </summary>
        public static readonly DependencyProperty WindowStateProperty =
            DependencyProperty.Register("WindowState", typeof (WindowState), typeof (ModalDailogWindow),
                                        new FrameworkPropertyMetadata(WindowState.Normal));

        /// <summary>
        /// Identifies the ShowCloseButton dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowCloseButtonProperty =
            DependencyProperty.Register("ShowCloseButton", typeof (bool), typeof (ModalDailogWindow),
                                        new FrameworkPropertyMetadata(true));

        /// <summary>
        /// Identifies the ShowMaximizeButton dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowMaximizeButtonProperty =
            DependencyProperty.Register("ShowMaximizeButton", typeof (bool), typeof (ModalDailogWindow),
                                        new FrameworkPropertyMetadata(false));

        /// <summary>
        /// Identifies the ShowMinimizeButton dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowMinimizeButtonProperty =
            DependencyProperty.Register("ShowMinimizeButton", typeof (bool), typeof (ModalDailogWindow),
                                        new FrameworkPropertyMetadata(false));

        ///// <summary>
        ///// Identifies the DialogResult dependency property.
        ///// </summary>
        public static readonly DependencyProperty DialogResultProperty =
            DependencyProperty.Register("DialogResult", typeof (DialogResult), typeof (ModalDailogWindow),
                                        new FrameworkPropertyMetadata(DialogResult.None));

        /// <summary>
        /// Identifies the ViewMode dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewModeProperty =
            DependencyProperty.Register("ViewMode", typeof (ViewModeType), typeof (ModalDailogWindow),
                                        new FrameworkPropertyMetadata(ViewModeType.Default,
                                                                      OnViewModeChanged));

        #endregion

        #region · Dependency Properties Callback Handlers ·

        private static void OnViewModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d != null)
            {
                ModalDailogWindow window = d as ModalDailogWindow;

                if (window.IsActive)
                {
                    ViewModeType oldViewModel = (ViewModeType) e.OldValue;

                    if (oldViewModel == ViewModeType.Add
                        || oldViewModel == ViewModeType.Edit)
                    {
                        window.UpdateActiveElementBindings();
                    }
                    else
                    {
                        //window.MoveFocus(FocusNavigationDirection.Next);
                    }
                }
            }
        }

        #endregion

        #region · Events ·

        /// <summary>
        /// Occurs when the window is about to close.
        /// </summary>
        public event EventHandler Closed;

        ///// <summary>
        ///// Occurs directly after System.Windows.Window.Close() is called, and can be
        /////     handled to cancel window closure.
        ///// </summary>
        public event CancelEventHandler Closing;

        /// <summary>
        /// Occurs when the window's System.Windows.Window.WindowState property changes.
        /// </summary>
        public event EventHandler WindowStateChanged;

        #endregion

        #region · Fields ·

        private ContentPresenter contentPresenter;
        private WindowState oldWindowState;
        private bool isShowed;
        private bool isModal;

        #region · Commands ·

        private ICommand minimizeCommand;
        private ICommand maximizeCommand;
        private ICommand closeCommand;

        #endregion

        #endregion

        #region · IWindow Commands ·

        /// <summary>
        /// Gets the maximize window command
        /// </summary>
        /// <value></value>
        public ICommand MaximizeCommand
        {
            get
            {
                if (this.maximizeCommand == null)
                {
                    this.maximizeCommand = new RelayCommand(() => OnMaximizeWindow());
                }

                return this.maximizeCommand;
            }
        }

        /// <summary>
        /// Gets the minimize window command
        /// </summary>
        /// <value></value>
        public ICommand MinimizeCommand
        {
            get
            {
                if (this.minimizeCommand == null)
                {
                    this.minimizeCommand = new RelayCommand(() => OnMinimizeWindow());
                }

                return this.minimizeCommand;
            }
        }

        /// <summary>
        /// Gets the close window command
        /// </summary>
        /// <value></value>
        public ICommand CloseCommand
        {
            get
            {
                if (this.closeCommand == null)
                {
                    this.closeCommand = new RelayCommand(() => OnCloseWindow());
                }

                return this.closeCommand;
            }
        }

        #endregion

        #region · Properties ·

        public Guid Id
        {
            get { return (Guid) base.GetValue(IdProperty); }
            set { base.SetValue(IdProperty, value); }
        }

        /// <summary>
        /// Gets the window parent control
        /// </summary>
        public UIElement ParentWindow
        {
            get { return (UIElement) base.GetValue(ParentWindowProperty); }
            set { base.SetValue(ParentWindowProperty, value); }
        }

        public virtual bool IsActive
        {
            get { return (bool) base.GetValue(IsActiveProperty); }
            set { base.SetValue(IsActiveProperty, value); }
        }

        /// <summary>
        /// Gets or sets a window's title. This is a dependency property. 
        /// </summary>
        public String Title
        {
            get { return (String) base.GetValue(ModalDailogWindow.TitleProperty); }
            set { base.SetValue(ModalDailogWindow.TitleProperty, value); }
        }

        /// <summary>
        /// Gets the dialog result
        /// </summary>
        public DialogResult DialogResult
        {
            get { return (DialogResult) base.GetValue(DialogResultProperty); }
            set { base.SetValue(DialogResultProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether a window is restored, minimized, or maximized. 
        /// This is a dependency property.
        /// </summary>
        /// <value>A <see cref="WindowState"/> that determines whether a window is restored, minimized, or maximized. The default is Normal (restored).</value>
        public WindowState WindowState
        {
            get { return (WindowState) base.GetValue(WindowStateProperty); }
            set
            {
                if ((WindowState) this.GetValue(WindowStateProperty) != value)
                {
                    if (this.oldWindowState == System.Windows.WindowState.Maximized
                        && this.WindowState == System.Windows.WindowState.Minimized
                        && value == System.Windows.WindowState.Normal)
                    {
                        //this.UpdateWindowState(this.WindowState, this.oldWindowState);

                        base.SetValue(WindowStateProperty, this.oldWindowState);
                    }
                    else
                    {
                        // this.UpdateWindowState(this.WindowState, value);

                        base.SetValue(WindowStateProperty, value);
                    }

                    if (this.WindowStateChanged != null)
                    {
                        this.WindowStateChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the close button is visible. 
        /// This is a dependency property. 
        /// </summary>
        public bool ShowCloseButton
        {
            get { return (bool) base.GetValue(ShowCloseButtonProperty); }
            set { base.SetValue(ShowCloseButtonProperty, value); }
        }

        /// <summary>
        /// Gets a value that indicates whether the maximize button is visible. 
        /// This is a dependency property. 
        /// </summary>
        public bool ShowMaximizeButton
        {
            get { return (bool) base.GetValue(ShowMaximizeButtonProperty); }
            set { base.SetValue(ShowMaximizeButtonProperty, value); }
        }

        /// <summary>
        /// Gets a value that indicates whether the minimize button is visible. 
        /// This is a dependency property. 
        /// </summary>
        public bool ShowMinimizeButton
        {
            get { return (bool) base.GetValue(ShowMinimizeButtonProperty); }
            set { base.SetValue(ShowMinimizeButtonProperty, value); }
        }

        /// <summary>
        /// Gets a value indicating whether the element can be dragged.
        /// </summary>
        /// <value><c>true</c> if this instance can drag; otherwise, <c>false</c>.</value>
        //public override bool CanDrag
        //{
        //    get { return (bool)base.GetValue(CanDragProperty) && this.WindowState == System.Windows.WindowState.Normal; }
        //    set { base.SetValue(CanDragProperty, value); }
        //}

        /// <summary>
        /// Gets the view mode
        /// </summary>
        public ViewModeType ViewMode
        {
            get { return (ViewModeType) base.GetValue(ViewModeProperty); }
            set
            {
                base.SetValue(ViewModeProperty, value);

                if (value == ViewModeType.Busy)
                {
                    this.GiveFocus();
                    // Application.Current.DoEvents();
                }
            }
        }

        #endregion

        #region · Command Execution Methods ·

        protected virtual void OnCloseWindow()
        {
            if (this.isModal)
            {
                this.Hide();
            }
            else
            {
                this.Close();
            }
        }

        protected virtual void OnMaximizeWindow()
        {
            if (this.WindowState != WindowState.Maximized)
            {
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.WindowState = WindowState.Normal;
            }
        }

        protected virtual void OnMinimizeWindow()
        {
            if (this.WindowState != WindowState.Minimized)
            {
                this.WindowState = WindowState.Minimized;
            }
            else
            {
                this.WindowState = WindowState.Normal;
            }
        }

        #endregion

        #region Public Methods

        public void Close()
        {
            var e = new CancelEventArgs();

            if (this.Closing != null)
            {
                this.Closing(this, e);
            }

            if (!e.Cancel)
            {
                // Clean up
                this.maximizeCommand = null;
                this.minimizeCommand = null;
                this.closeCommand = null;
                this.dispatcherFrame = null;
                this.isModal = false;
                this.isShowed = false;

                this.CommandBindings.Clear();

                if (this.Closed != null)
                {
                    this.Closed(this, EventArgs.Empty);
                }
            }
        }

        public void Show()
        {
            if (!this.isShowed)
            {
                this.isShowed = true;
            }

            //this.OnActivated();
        }

        public void ShowDialog()
        {
            try
            {
                var rectangle = (Rectangle)Container.Owner.FindName("ModelOverlay");
                if (rectangle != null)
                    rectangle.Visibility = Visibility.Visible;

                Container.ShowDialog();

                if (rectangle != null)
                    rectangle.Visibility = Visibility.Collapsed;
            }
            finally
            {
                // Pop the current thread from modal state
            }
        }

        public void Hide()
        {
            this.Visibility = System.Windows.Visibility.Collapsed;

            if (this.isModal && this.dispatcherFrame != null)
            {
                this.dispatcherFrame.Continue = false;
                this.dispatcherFrame = null;
            }
        }

        #endregion

        private void UpdateActiveElementBindings()
        {
            if (Keyboard.FocusedElement != null &&
                Keyboard.FocusedElement is DependencyObject)
            {
                DependencyObject element = (DependencyObject) Keyboard.FocusedElement;
                LocalValueEnumerator localValueEnumerator = element.GetLocalValueEnumerator();

                while (localValueEnumerator.MoveNext())
                {
                    BindingExpressionBase bindingExpression = BindingOperations.GetBindingExpressionBase(element,
                                                                                                         localValueEnumerator
                                                                                                             .Current
                                                                                                             .Property);

                    if (bindingExpression != null)
                    {
                        bindingExpression.UpdateSource();
                        bindingExpression.UpdateTarget();
                    }
                }
            }
        }

        protected void GiveFocus()
        {
            // this.MoveFocus(FocusNavigationDirection.Next);
        }

        private Window CreateContainer()
        {
            var newWindow = new Window {Content = this, Owner = ComputeOwnerWindow()};
            newWindow.Content = this;
            newWindow.WindowStartupLocation = newWindow.Owner != null
                                                  ? System.Windows.WindowStartupLocation.CenterOwner
                                                  : System.Windows.WindowStartupLocation.CenterScreen;

            newWindow.ShowInTaskbar = false;
            newWindow.SizeToContent = System.Windows.SizeToContent.WidthAndHeight;
            newWindow.ResizeMode = System.Windows.ResizeMode.NoResize;
            newWindow.WindowStyle = System.Windows.WindowStyle.None;
            return newWindow;
        }

        private static Window ComputeOwnerWindow()
        {
            Window owner = null;
            if (Application.Current != null)
            {
                foreach (var w in from Window w in Application.Current.Windows where w.IsActive select w)
                {
                    owner = w;
                    break;
                }
            }
            return owner;
        }

        private void ApplyVM()
        {
            dynamic parentWindow = ComputeOwnerWindow();

                if (parentWindow != null)
                {
                    this.Content = VMMessageDailog.ChildViewModel;
                    this.Title = VMMessageDailog.ChildViewModel.Title;
                    ShowCloseButton = VMMessageDailog.ChildViewModel.ShowCloseButton;
                    ShowMaximizeButton = VMMessageDailog.ChildViewModel.ShowMaximizeButton;
                    ShowMinimizeButton = VMMessageDailog.ChildViewModel.ShowMinimizeButton;

                    if (Visibility == Visibility.Collapsed)
                        Visibility = Visibility.Visible;

                    double fullWidth = parentWindow.ActualWidth;
                    double fullHeight = parentWindow.ActualHeight;

                    if (VMMessageDailog.ChildViewModel.DialogType != DialogType.BySizeInPixel)
                    {
                        Height = fullHeight * VMMessageDailog.ChildViewModel.DialogStartupSizePercentage / 100;
                        Width = fullWidth * VMMessageDailog.ChildViewModel.DialogStartupSizePercentage / 100;
                    }
                    else
                    {
                        this.Height = (fullHeight > VMMessageDailog.ChildViewModel.DialogStartupCustomHeight) ? VMMessageDailog.ChildViewModel.DialogStartupCustomHeight : fullHeight;
                        this.Width = (fullWidth > VMMessageDailog.ChildViewModel.DialogStartupCustomWidth) ? VMMessageDailog.ChildViewModel.DialogStartupCustomWidth : fullWidth;
                    }
                    //Hookup Events
                    if (ShowCloseButton)
                        Closed += ModalDialogClosed;

                    VMMessageDailog.ChildViewModel.CloseWindow += CloseDialog;
                }
        }

        void ModalDialogClosed(object sender, EventArgs e)
        {
            this.maximizeCommand = null;
            this.minimizeCommand = null;
            this.closeCommand = null;
            this.dispatcherFrame = null;
            this.isModal = false;
            this.isShowed = false;

            this.CommandBindings.Clear();
            if (VMMessageDailog.ChildViewModel != null)
                VMMessageDailog.ChildViewModel.Unload();
            Container.Close();
        }

        void CloseDialog()
        {
            this.maximizeCommand = null;
            this.minimizeCommand = null;
            this.closeCommand = null;
            this.dispatcherFrame = null;
            this.isModal = false;
            this.isShowed = false;
            if (VMMessageDailog.ChildViewModel != null)
                VMMessageDailog.ChildViewModel.Unload();
            //this.CommandBindings.Clear();
            Container.Close();
        }

        private static dynamic FindParentUserControl(DependencyObject reference)
        {
            //
            // find parent user control
            DependencyObject dp = VisualTreeHelper.GetParent(reference);

            if (dp == null)
                return null;
            else if (dp is UserControl)
                return (UserControl)dp;
            else if (dp is Window)
                return (Window)dp;
            else
                return FindParentUserControl(dp);
        }
    }
}
