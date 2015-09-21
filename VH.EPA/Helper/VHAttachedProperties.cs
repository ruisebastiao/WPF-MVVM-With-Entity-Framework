using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using GalaSoft.MvvmLight.Command;
using VH.Bases;
using VH.Model.Utilities;
using VH.SimpleUI.Entities;
using VH.UI.UserControls;

namespace VH.View
{
    public class VHAttachedProperties : DependencyObject
    {
        #region ModelDialogContent
        public static readonly DependencyProperty ModelDialogContentProperty = DependencyProperty.RegisterAttached("ModelDialogContent",
            typeof(IBaseViewModel), typeof(VHAttachedProperties),
            new FrameworkPropertyMetadata(null, OnModelDialogContentChanged));

        public static void SetModelDialogContent(UIElement element, IBaseViewModel value)
        {
            element.SetValue(ModelDialogContentProperty, value);
        }
        public static IBaseViewModel GetModelDialogContent(UIElement element)
        {
            return (IBaseViewModel)element.GetValue(ModelDialogContentProperty);
        }

        /// <summary>
        /// Use this attached property instead of the Content property.
        /// This one will ensure that the dialog is correctly sized.
        /// </summary>
        [Category("VHAttachedProperties Properties"),
        DescriptionAttribute("ModelDialogContent.")]
        public IBaseViewModel ModelDialogContent
        {
            get { return (IBaseViewModel)GetValue(ModelDialogContentProperty); }
            set { SetValue(ModelDialogContentProperty, value); }
        }

        private static void OnModelDialogContentChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            var xdlg = source as ModalDailogWindow;

            if (e.NewValue is IBaseViewModel)
            {
                var vm = (IBaseViewModel)e.NewValue;

                //
                // find parent user control and get the parent's dimensions
                dynamic parentWindow = FindParentUserControl(xdlg);

                if (parentWindow != null)
                {
                    xdlg.Content = vm;
                    xdlg.Title = vm.Title;
                    xdlg.ShowCloseButton = vm.ShowCloseButton;
                    xdlg.ShowMaximizeButton = vm.ShowMaximizeButton;
                    xdlg.ShowMinimizeButton = vm.ShowMinimizeButton;

                    if(xdlg.Visibility == Visibility.Collapsed)
                        xdlg.Visibility = Visibility.Visible;

                    double fullWidth = parentWindow.ActualWidth;
                    double fullHeight = parentWindow.ActualHeight;

                    if (vm.DialogType != DialogType.BySizeInPixel)
                    {
                        xdlg.Height = fullHeight * vm.DialogStartupSizePercentage / 100;
                        xdlg.Width = fullWidth * vm.DialogStartupSizePercentage / 100;
                        //xdlg.IsResizable = vm.DialogStartupCustomResizable;
                    }
                    else
                    {
                        xdlg.Height = (fullHeight > vm.DialogStartupCustomHeight) ? vm.DialogStartupCustomHeight : fullHeight;
                        xdlg.Width = (fullWidth > vm.DialogStartupCustomWidth) ? vm.DialogStartupCustomWidth : fullWidth;
                        //xdlg.IsResizable = vm.DialogStartupCustomResizable;
                    }


                    //Hookup Events
                    if (xdlg.ShowCloseButton)
                        xdlg.Closed += ModalDialogClosed;
                }
            }
            else
            {
                xdlg.Content = null;
                xdlg.Visibility = Visibility.Collapsed;
            }
        }

        static void ModalDialogClosed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
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
        private static dynamic FindParentUserControl<T>(DependencyObject reference) where T : UIElement
        {
            //
            // find parent user control
            DependencyObject dp = VisualTreeHelper.GetParent(reference);

            if (dp == null)
                return null;
            else if (dp is UserControl)
                return (UserControl)dp;
            else if (dp is T)
                return (T)dp;
            else
                return FindParentUserControl(dp);
        }

        private static List<T> FindChildrenControl<T>(DependencyObject reference) where T : UIElement
        {
            var list = new List<T>();
            int intCount = VisualTreeHelper.GetChildrenCount(reference);

            for (int i = 0; i < intCount; i++)
            {
                var dp = VisualTreeHelper.GetChild(reference, i);

                if (dp is T)
                    list.Add((T) dp);
            }

            return list;
        }

        #endregion ModelDialogContent

        #region HandleElementChecked
        
        public static readonly DependencyProperty HandleElementCheckedProperty =
            DependencyProperty.RegisterAttached("HandleElementChecked", typeof(bool), typeof(VHAttachedProperties), new FrameworkPropertyMetadata(false, OnHandleElementCheckedChanged));

        private static void OnHandleElementCheckedChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
           if (source is ToggleButton)
           {
               (source as ToggleButton).Checked += OnChecked;
           }
        }

        private static void OnChecked(object sender, RoutedEventArgs routedEventArgs)
        {
            var parent = FindParentUserControl<StackPanel>(sender as DependencyObject);
            if (parent is StackPanel)
            {
                var children = FindChildrenControl<ToggleButton>(parent);
                if (children != null && ((IList)children).Cast<object>().Any())
                {
                    foreach (var child in ((IList)children).Cast<object>().Where(child => !child.Equals(sender) && ((ToggleButton) child).IsChecked.Value))
                    {
                        ((ToggleButton) child).IsChecked = false;
                    }
                }
            }
        }

        public static void SetHandleElementChecked(UIElement element, bool value)
        {
            element.SetValue(HandleElementCheckedProperty, value);
        }

        public static bool GetHandleElementChecked(UIElement element)
        {
            return (bool) element.GetValue(HandleElementCheckedProperty);
        }
        #endregion

        #region BindEnumToItemSource

        public static readonly DependencyProperty BindEnumToItemSourceProperty =
            DependencyProperty.RegisterAttached("BindEnumToItemSource", typeof(string), typeof(VHAttachedProperties), new FrameworkPropertyMetadata(default(string), OnBindEnumToItemSourceChanged));

        private static void OnBindEnumToItemSourceChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            try
            {
               var value = GetBindEnumToItemSource((UIElement) dependencyObject);
                var tempCollection = new Dictionary<Enum, string>();
              
                var enumType = Type.GetType(value);

                if (enumType != null && (enumType.BaseType != null && (enumType.BaseType.Name.Equals("Enum"))))
                {
                    Array enumValues = Enum.GetValues(enumType);
                    foreach (Enum enumValue in enumValues)
                    {
                        var enumResourceValue = enumValue.GetResourceValueForEnum();
                        tempCollection.Add(enumValue, !string.IsNullOrEmpty(enumResourceValue) ? enumResourceValue : enumValue.ToString());
                    }
                }

                if (dependencyObject is ComboBox)
                {
                    ((ComboBox) dependencyObject).ItemsSource = tempCollection;
                    ((ComboBox) dependencyObject).DisplayMemberPath = "Value";
                    ((ComboBox)dependencyObject).SelectedValuePath = "Key";
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void SetBindEnumToItemSource(UIElement element, string value)
        {
            element.SetValue(BindEnumToItemSourceProperty, value);
        }

        public static string GetBindEnumToItemSource(UIElement element)
        {
            return (string) element.GetValue(BindEnumToItemSourceProperty);
        }
        #endregion

        #region OnDoubleClick
        public static readonly DependencyProperty OnDoubleClickProperty =
           DependencyProperty.RegisterAttached("OnDoubleClick", typeof(ICommand), typeof(VHAttachedProperties), new PropertyMetadata(default(ICommand), OnDoubleClickPropertyChangedCallback));

        private static void OnDoubleClickPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var element = dependencyObject as Selector;
            if (element != null)
                element.MouseDoubleClick += (sender, args) =>
                {
                    ICommand command = GetOnDoubleClick(dependencyObject);
                    if (command != null)
                        command.Execute(null);
                };
        }

        public static void SetOnDoubleClick(DependencyObject element, ICommand value)
        {
            element.SetValue(OnDoubleClickProperty, value);
        }

        public static ICommand GetOnDoubleClick(DependencyObject element)
        {
            return (ICommand)element.GetValue(OnDoubleClickProperty);
        }
        #endregion

       
    }
}
