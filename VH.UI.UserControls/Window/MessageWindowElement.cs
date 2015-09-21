using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using VH.SimpleUI.Entities;

namespace VH.UI.UserControls
{
    /// <summary>
    /// Provides the ability to create modal message windows (like messageboxes)
    /// </summary>
    public class MessageWindowElement 
        : Window
    {
        #region · Dependency Properties ·

        /// <summary>
        /// Identifies the Buttons dependency property.
        /// </summary>
        public static readonly DependencyProperty ButtonsProperty =
            DependencyProperty.Register("Buttons", typeof(DialogButton), typeof(MessageWindowElement),
                new FrameworkPropertyMetadata(DialogButton.Ok));

        public static readonly DependencyProperty DialogResultProperty =
            DependencyProperty.Register("DialogResult", typeof (DialogResult), typeof (MessageWindowElement), new PropertyMetadata(default(DialogResult)));

        public new DialogResult DialogResult
        {
            get { return (DialogResult) GetValue(DialogResultProperty); }
            set { SetValue(DialogResultProperty, value); }
        }

        #endregion

        #region · Sync Object ·

        static readonly object SyncObject = new object();

        #endregion

        #region · Static Commands ·

        /// <summary>
        /// Accept command
        /// </summary>
        public static RoutedCommand AcceptCommand;

        /// <summary>
        /// Cancel command
        /// </summary>
        public static RoutedCommand CancelCommand;

        private System.Windows.Window _owner;

        #endregion

        #region · Static Constructors ·

        /// <summary>
        /// Initializes the <see cref="MessageWindowElement"/> class.
        /// </summary>
        static MessageWindowElement()
        {
            MessageWindowElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(MessageWindowElement),
                new FrameworkPropertyMetadata(typeof(MessageWindowElement)));

            KeyboardNavigation.IsTabStopProperty.OverrideMetadata(typeof(MessageWindowElement),
                new FrameworkPropertyMetadata(false));

            MessageWindowElement.AcceptCommand = new RoutedCommand("Accept", typeof(MessageWindowElement));
            MessageWindowElement.CancelCommand = new RoutedCommand("Cancel", typeof(MessageWindowElement));

        }

        #endregion
        
        #region · Show "Factory" Methods ·

        /// <summary>
        /// Shows a new <see cref="MessageWindowElement"/> with the given message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static DialogResult Show(string message)
        {
            return Show("Message", message, DialogButton.OkCancel);
        }

        public void ShowWindow()
        {
            var rectangle = new Rectangle();
            if (this.Owner == null)
            {
                if (Application.Current.Windows.Count > 0)
                    this.Owner = Application.Current.Windows[0];
            }
            if (this.Owner != null)
            {
                rectangle = (Rectangle) this.Owner.FindName("ModelOverlay");
                if (rectangle != null)
                    rectangle.Visibility = Visibility.Visible;
            }

            this.ShowDialog();

            if (rectangle != null)
                rectangle.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Shows a new <see cref="MessageWindowElement"/> with the given caption and message
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static DialogResult Show(string caption, string message)
        {
            return MessageWindowElement.Show(caption, message, DialogButton.OkCancel);
        }

        /// <summary>
        /// Shows a new <see cref="MessageWindowElement"/> with the given caption, message and buttons
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="message"></param>
        /// <param name="buttons"></param>
        /// <returns></returns>
        public static DialogResult Show(string caption, string message, DialogButton buttons) 
        {
            lock (SyncObject)
            {
                DialogResult result = DialogResult.None;

                Application.Current.Dispatcher.Invoke(
                    (Action)delegate
                    {
                        var window = new MessageWindowElement
                        {
                            Title           = caption,
                            Content         = message,
                            Buttons         = buttons,
                            //StartupLocation = StartupPosition.CenterParent
                        };

                        window.ShowDialog();
                    });

                return result;
            }
        }

        #endregion

        #region · Properties ·

        /// <summary>
        /// Gets or sets the button combination to be shown
        /// </summary>
        public DialogButton Buttons
        {
            get { return (DialogButton)base.GetValue(MessageWindowElement.ButtonsProperty); }
            set { base.SetValue(MessageWindowElement.ButtonsProperty, value); }
        }

        #endregion

        #region · Constructors ·

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageWindowElement"/> class
        /// </summary>
        public MessageWindowElement()
            : base()
        {
            var bindinAccept = new CommandBinding(AcceptCommand, OnAccept);
            this.CommandBindings.Add(bindinAccept);

            var bindingCancel = new CommandBinding(CancelCommand, OnCancel);
            this.CommandBindings.Add(bindingCancel);
            CreateContainer();
        }

        public MessageWindowElement(MessageDailog dm)
            : base()
        {
            var bindinAccept = new CommandBinding(AcceptCommand, OnAccept);
            this.CommandBindings.Add(bindinAccept);

            var bindingCancel = new CommandBinding(CancelCommand, OnCancel);
            this.CommandBindings.Add(bindingCancel);

            if (dm != null)
            {
                Buttons = dm.DialogButton;
                Title = dm.Title;
                Content = dm.Caption;

                if (dm.DialogHeight != 0)
                    this.Height = dm.DialogHeight;
                if (dm.DialogWidth != 0)
                    this.Height = dm.DialogWidth;

                if(dm.IsSizeToContent)
                    this.SizeToContent = SizeToContent.WidthAndHeight;
            }

            CreateContainer();
        }
        #endregion

        #region · Protected Methods ·

        /// <summary>
        /// Focuses the window
        /// </summary>
        //protected void GiveFocus()
        //{
        //    this.SetFocus();
        //}

        #endregion

        #region · Command Actions ·

        private void OnAccept(object sender, ExecutedRoutedEventArgs e)
        {
            if (this.Buttons == DialogButton.Ok ||
                this.Buttons == DialogButton.OkCancel)
            {
                this.DialogResult = DialogResult.Ok;
            }
            else
            {
                this.DialogResult = DialogResult.Yes;
            }

            this.Close();
        }

        private void OnCancel(object sender, ExecutedRoutedEventArgs e)
        {
            if (this.Buttons == DialogButton.OkCancel)
            {
                this.DialogResult = DialogResult.Cancel;
            }
            else
            {
                this.DialogResult = DialogResult.No;
            }

            this.Close();
        }

        private void CreateContainer()
        {
            this.Owner = ComputeOwnerWindow();

            this.WindowStartupLocation = this.Owner != null ? WindowStartupLocation.CenterOwner : WindowStartupLocation.CenterScreen;

            if (double.IsNaN(this.Height))
                this.Height = 180;
            if (double.IsNaN(this.Width))
                this.Width = 425;

            this.ShowInTaskbar = false;
            this.ResizeMode = ResizeMode.NoResize;
            this.WindowStyle = WindowStyle.None;
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
        #endregion
    }
}
