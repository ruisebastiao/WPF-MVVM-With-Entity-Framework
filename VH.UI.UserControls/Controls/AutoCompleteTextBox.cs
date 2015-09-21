using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace VH.UI.UserControls
{
    [TemplatePart(Name = "PART_txtSerach", Type = typeof(TextBox)),
     TemplatePart(Name = "PART_ListSearchResult", Type = typeof(ListBox))]
    public class AutoCompleteTextBox : Control
    {
        #region Fileds
        private TextBox txtSerach;
        private ListBox lstSearchResult;

        #endregion

        #region Dependence Prperties
        #region ListBox

        public static readonly DependencyProperty ListBoxItemTemplateProperty =
            DependencyProperty.Register("ListBoxItemTemplate", typeof (DataTemplate), typeof (AutoCompleteTextBox), new PropertyMetadata(default(DataTemplate)));

        public DataTemplate ListBoxItemTemplate
        {
            get { return (DataTemplate) GetValue(ListBoxItemTemplateProperty); }
            set { SetValue(ListBoxItemTemplateProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(Object), typeof(AutoCompleteTextBox), new PropertyMetadata(default(Object)));

        public Object SelectedItem
        {
            get { return (Object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public static readonly DependencyProperty SearchResultCollectionProperty =
            DependencyProperty.Register("SearchResultCollection", typeof(IDictionary<String, String>), typeof(AutoCompleteTextBox), new PropertyMetadata(default(IDictionary<String, String>)));

        public IDictionary<String, String> SearchResultCollection
        {
            get { return (IDictionary<String, String>)GetValue(SearchResultCollectionProperty); }
            set { SetValue(SearchResultCollectionProperty, value); }
        }

        public static readonly DependencyProperty DisplayMemberPathProperty =
            DependencyProperty.Register("DisplayMemberPath", typeof (String), typeof (AutoCompleteTextBox), new PropertyMetadata(default(String)));

        public String DisplayMemberPath
        {
            get { return (String) GetValue(DisplayMemberPathProperty); }
            set { SetValue(DisplayMemberPathProperty, value); }
        }

        public static readonly DependencyProperty SelectedValueProperty =
            DependencyProperty.Register("SelectedValue", typeof (Object), typeof (AutoCompleteTextBox), new PropertyMetadata(default(Object)));

        public Object SelectedValue
        {
            get { return (Object) GetValue(SelectedValueProperty); }
            set { SetValue(SelectedValueProperty, value); }
        }

        public static readonly DependencyProperty SelectedValuePathProperty =
            DependencyProperty.Register("SelectedValuePath", typeof (String), typeof (AutoCompleteTextBox), new PropertyMetadata(default(String)));

        public String SelectedValuePath
        {
            get { return (String) GetValue(SelectedValuePathProperty); }
            set { SetValue(SelectedValuePathProperty, value); }
        }

        public static readonly DependencyProperty ListStyleProperty =
           DependencyProperty.Register("ListStyle", typeof(Style), typeof(AutoCompleteTextBox), new PropertyMetadata(default(Style)));

        public Style ListStyle
        {
            get { return (Style)GetValue(ListStyleProperty); }
            set { SetValue(ListStyleProperty, value); }
        }
        #endregion

        #region TextBox

        public static readonly DependencyProperty SearchTextProperty =
            DependencyProperty.Register("SearchText", typeof (String), typeof (AutoCompleteTextBox), new PropertyMetadata(default(String)));

        public String SearchText
        {
            get { return (String) GetValue(SearchTextProperty); }
            set { SetValue(SearchTextProperty, value); }
        }

        public static readonly DependencyProperty TextBoxStyleProperty =
            DependencyProperty.Register("TextBoxStyle", typeof (Style), typeof (AutoCompleteTextBox), new PropertyMetadata(default(Style)));

        public Style TextBoxStyle
        {
            get { return (Style) GetValue(TextBoxStyleProperty); }
            set { SetValue(TextBoxStyleProperty, value); }
        }

        public static readonly DependencyProperty TextBoxSelectionChangeCommandProperty =
            DependencyProperty.Register("TextBoxSelectionChangeCommand", typeof (ICommand), typeof (AutoCompleteTextBox), new PropertyMetadata(default(ICommand)));

        public ICommand TextBoxSelectionChangeCommand
        {
            get { return (ICommand) GetValue(TextBoxSelectionChangeCommandProperty); }
            set { SetValue(TextBoxSelectionChangeCommandProperty, value); }
        }
        #endregion

        #region WaterMarker

        public static readonly DependencyProperty WaterMarkerTextProperty =
            DependencyProperty.Register("WaterMarkerText", typeof(String), typeof(AutoCompleteTextBox), new PropertyMetadata(default(String)));

        public String WaterMarkerText
        {
            get { return (String)GetValue(WaterMarkerTextProperty); }
            set { SetValue(WaterMarkerTextProperty, value); }
        }

        public static readonly DependencyProperty WaterMarkerStyleProperty =
            DependencyProperty.Register("WaterMarkerStyle", typeof(Style), typeof(AutoCompleteTextBox), new PropertyMetadata(default(Style)));
       
        public Style WaterMarkerStyle
        {
            get { return (Style)GetValue(WaterMarkerStyleProperty); }
            set { SetValue(WaterMarkerStyleProperty, value); }
        }
        #endregion

        #region SelectedItemCommand
        public static readonly DependencyProperty SelectedItemCommandProperty =
         DependencyProperty.Register("SelectedItemCommand", typeof(ICommand), typeof(AutoCompleteTextBox), new PropertyMetadata(default(ICommand), PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            ((UIElement)dependencyObject).InputBindings.Add(new KeyBinding((ICommand)dependencyPropertyChangedEventArgs.NewValue, Key.Enter, ModifierKeys.None));
        }

        public ICommand SelectedItemCommand
        {
            get { return (ICommand)GetValue(SelectedItemCommandProperty); }
            set { SetValue(SelectedItemCommandProperty, value); }
        }
        #endregion

        #region ItemDoubleClickCommand

        public static readonly DependencyProperty ItemDoubleClickCommandProperty =
            DependencyProperty.Register("ItemDoubleClickCommand", typeof (ICommand), typeof (AutoCompleteTextBox), new PropertyMetadata(default(ICommand)));

        public ICommand ItemDoubleClickCommand
        {
            get { return (ICommand) GetValue(ItemDoubleClickCommandProperty); }
            set { SetValue(ItemDoubleClickCommandProperty, value); }
        }
        #endregion

        #region Popup

        public static readonly DependencyProperty IsPopupOpenProperty =
            DependencyProperty.Register("IsPopupOpen", typeof (bool), typeof (AutoCompleteTextBox), new PropertyMetadata(default(bool)));

        public bool IsPopupOpen
        {
            get { return (bool) GetValue(IsPopupOpenProperty); }
            set { SetValue(IsPopupOpenProperty, value); }
        }

        public static readonly DependencyProperty PopupMaxHeightProperty =
            DependencyProperty.Register("PopupMaxHeight", typeof (double), typeof (AutoCompleteTextBox), new PropertyMetadata(default(double)));

        public double PopupMaxHeight
        {
            get { return (double) GetValue(PopupMaxHeightProperty); }
            set { SetValue(PopupMaxHeightProperty, value); }
        }
        #endregion
        #endregion

        #region Constructors
        static AutoCompleteTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AutoCompleteTextBox),
                                                     new FrameworkPropertyMetadata(typeof(AutoCompleteTextBox)));
        }

        public AutoCompleteTextBox()
        {
            this.OnApplyTemplate();
        }
        #endregion

        #region Override Methods
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.txtSerach = this.GetTemplateChild("PART_txtSerach") as TextBox;
            this.lstSearchResult = this.GetTemplateChild("PART_ListSearchResult") as ListBox;

            if (txtSerach != null)
            {
                this.txtSerach.Style = TextBoxStyle;
            }

            if (lstSearchResult != null)
            {
                this.lstSearchResult.Style = ListStyle;
            }

            ItemDoubleClickCommand = new RelayCommand(() =>
                {
                    if (SelectedItemCommand != null)
                        this.SelectedItemCommand.Execute(null);
                });
        }

        public void LstSearchResultOnMouseDoubleClick(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Events

        #endregion
    }
}
