using System;
using System.Linq;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace VH.UI.UserControls
{
    public static class AdornerUtilities
    {
        public static bool UIElementIsAdornedWith<T>(UIElement element) where T : BaseAdorner
        {
            bool result = false;

            AdornerLayer al = AdornerLayer.GetAdornerLayer(element);
            if (al != null)
            {
                Adorner[] array = al.GetAdorners(element);
                if (array != null)
                {
                    if (array.Any(a => a is T &&
                                       ((T)a).AdornedControl == element))
                    {
                        result = true;
                    }
                }
            }
            return result;
        }
        public static void DetachAdorner<T>(UIElement element) where T : BaseAdorner
        {
            //remove adorner
            AdornerLayer al = AdornerLayer.GetAdornerLayer(element);

            if (al != null)
            {
                Adorner[] array = al.GetAdorners(element);
                if (array != null)
                {
                    foreach (Adorner a in array)
                    {
                        if (a is T &&
                           ((T)a).AdornedControl == element)
                        {
                            ((T)a).Detach();
                        }
                    }
                }
            }
        }
    }
    public abstract class BaseAdorner : Adorner
    {
        #region private members
        protected AdornerLayer _adornerLayer;
        protected Control _adornedControl;
        protected Grid contentHost = new Grid();
        protected ContentPresenter contentPresenter;
        protected object adornedControlTooltip;
        #endregion private members

        #region public properties
        #region AdornedControlType
        public static readonly DependencyProperty AdornedControlTypeProperty = DependencyProperty.Register("AdornedControlType", 
            typeof(string), typeof(BaseAdorner));

        public string AdornedControlType
        {
            get { return (string)GetValue(AdornedControlTypeProperty); }
            set { SetValue(AdornedControlTypeProperty, value); }
        }

        #endregion AdornedControlType

        public Control AdornedControl
        {
            get { return this.AdornedElement as Control; }
        }
        #endregion public properties

        #region constructors

        public BaseAdorner(UIElement adornedElement, AdornerLayer adornerLayer)
            : base(adornedElement)
        {
            this._adornerLayer = adornerLayer;
        }

        #endregion constructors

        #region overrides
        protected override Size MeasureOverride(Size constraint)
        {
            this.contentHost.Measure(constraint);
            return this.contentHost.DesiredSize;
        }
        protected override Size ArrangeOverride(Size finalSize)
        {
            this.contentHost.Arrange(new Rect(finalSize));
            return finalSize;
        }
        protected override Visual GetVisualChild(int index)
        {
            return this.contentHost;
        }
        protected override int VisualChildrenCount
        {
            get { return 1; }
        }
        #endregion overrides

        #region public methods
        public virtual void Detach()
        {
            if (this.AdornedControl != null)
                this.AdornedControl.ToolTip = this.adornedControlTooltip;

            if (this._adornerLayer != null)
                this._adornerLayer.Remove(this);
        }
        #endregion public methods
    }
    public class GetStartedAdorner : Adorner
    {
        ContentPresenter _contentPresenter;
        public GetStartedAdorner(UIElement adornedElement)
            : base(adornedElement)
        {
            this._contentPresenter = new ContentPresenter();

            Rectangle rectangle = new Rectangle();
            rectangle.Width = this.AdornedElement.DesiredSize.Width;
            rectangle.Height = this.AdornedElement.DesiredSize.Height;
            rectangle.Stroke = new SolidColorBrush(Colors.Red);
            rectangle.StrokeThickness = 3;
            rectangle.Fill = null;

            //animate the adorner
            //Storyboard s = new Storyboard();
            //DoubleAnimation aX = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromMilliseconds(2000)));
            //aX.AutoReverse = true;
            //aX.RepeatBehavior = RepeatBehavior.Forever;

            //s.Children.Add(aX);
            //Storyboard.SetTarget(aX, rectangle);
            //Storyboard.SetTargetProperty(aX, new PropertyPath(Rectangle.OpacityProperty));
            //s.Begin(this);

            this._contentPresenter.Content = rectangle;

        }

        // A common way to implement an adorner's rendering behavior is to override the OnRender
        // method, which is called by the layout system as part of a rendering pass.
        protected override void OnRender(DrawingContext drawingContext)
        {
            //can't animate this?
            //Rect adornedElementRect = new Rect(this.AdornedElement.DesiredSize);

            //// Some arbitrary drawing implements.
            //SolidColorBrush renderBrush = new SolidColorBrush(Colors.Transparent);
            //renderBrush.Opacity = 0.2;
            //Pen renderPen = new Pen(new SolidColorBrush(Colors.Red), 3);

            //drawingContext.DrawRectangle(renderBrush, renderPen, adornedElementRect);
        }

        protected override Size MeasureOverride(Size constraint)
        {
            this._contentPresenter.Measure(constraint);
            return this._contentPresenter.DesiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            this._contentPresenter.Arrange(new Rect(finalSize));
            return finalSize;
        }

        protected override Visual GetVisualChild(int index)
        {
            return this._contentPresenter;
        }

        protected override int VisualChildrenCount
        {
            get { return 1; }
        }

    }

    public class InActiveCodeComboBoxAdorner : Adorner
    {
        private Grid contentHost;
        private ContentPresenter contentPresenter;
        private AdornerLayer adornerLayer;
        public Control AdornedControl 
        {
            get { return this.AdornedElement as Control; }
        }
        private object adornedControlTooltip;
        private int adornedControlTooltipDuration;

        public InActiveCodeComboBoxAdorner(object codeItem, DataTemplate itemTemplate, UIElement adornedElement, AdornerLayer adornerLayer)
			: base(adornedElement)
		{
			this.adornerLayer = adornerLayer;

            if (this.AdornedElement != null)
            {
                //need to handle if the tooltip property has a binding.  This only takes a value.
                this.adornedControlTooltip = AdornedControl != null ? ((Control)AdornedControl).ToolTip : null;
                this.adornedControlTooltipDuration = ToolTipService.GetShowDuration(this.AdornedControl);

                ContentPresenter tooltipContent = new ContentPresenter();
                tooltipContent.ContentTemplate = this.TryFindResource("InActiveCodeTooltip") as DataTemplate;
                tooltipContent.Content = codeItem;
                ToolTipService.SetShowDuration(this.AdornedControl, 10000);
                this.AdornedControl.ToolTip = tooltipContent;

                Binding b3 = new Binding("IsVisible");
                b3.Source = adornedElement;
                b3.Converter = new BoolToVisibilityConverter();
                this.SetBinding(Adorner.VisibilityProperty, b3);

                this.contentHost = new Grid();
                Binding b1 = new Binding("ActualHeight");
                b1.Source = adornedElement;
                this.contentHost.SetBinding(Grid.HeightProperty, b1);
                Binding b2 = new Binding("ActualWidth");
                b2.Source = adornedElement;
                this.contentHost.SetBinding(Grid.WidthProperty, b2);
            }

            this.contentPresenter = new ContentPresenter();
            this.contentPresenter.Content = codeItem;
            this.contentPresenter.VerticalAlignment = VerticalAlignment.Center;
            this.contentPresenter.Margin = new Thickness(4, 0, 21, 1); //right margin should be bound to the width of the toggle button part
            this.contentPresenter.ContentTemplate = itemTemplate;

            this.contentHost.Children.Add(this.contentPresenter);

            if (this.adornerLayer != null)
                this.adornerLayer.Add(this);    
        }

        #region overrides
        protected override void OnRender(DrawingContext drawingContext)
        {
            //Rect adornedElementRect = new Rect(this.AdornedElement.DesiredSize);

            //// Some arbitrary drawing implements.
            //SolidColorBrush renderBrush = new SolidColorBrush(Colors.Aquamarine);
            //Pen renderPen = new Pen(new SolidColorBrush(Colors.Red), 3);

            //drawingContext.DrawRectangle(renderBrush, renderPen, adornedElementRect);
            base.OnRender(drawingContext);
        }
        protected override Size MeasureOverride(Size constraint)
        {
            this.contentHost.Measure(constraint);
            return this.contentHost.DesiredSize;
        }
        protected override Size ArrangeOverride(Size finalSize)
        {
            this.contentHost.Arrange(new Rect(finalSize));
            return finalSize;
        }
        protected override Visual GetVisualChild(int index)
        {
            return this.contentHost;
        }
        protected override int VisualChildrenCount
        {
            get { return 1; }
        }
        #endregion overrides

        #region public methods
        public void Detach()
        {
            if (AdornedControl != null)
            {
                AdornedControl.ToolTip = adornedControlTooltip;
                ToolTipService.SetShowDuration(this.AdornedControl, this.adornedControlTooltipDuration);
            }

            if (this.adornerLayer != null)
                this.adornerLayer.Remove(this);
        }
        #endregion public methods
    }

    public class SecurityAccessAdorner : Adorner
    {
        private Grid contentHost = new Grid();
        private ContentPresenter contentPresenter;
        private AdornerLayer adornerLayer;
        public Control AdornedControl
        {
            get { return this.AdornedElement is Control ? this.AdornedElement as Control : null; }
        }
        private object adornedControlTooltip;

        #region AdornedControlType
        public static readonly DependencyProperty AdornedControlTypeProperty = DependencyProperty.Register("AdornedControlType", typeof(string), typeof(SecurityAccessAdorner));
        
        public string AdornedControlType
        {
            get { return (string)GetValue(AdornedControlTypeProperty); }
            set { SetValue(AdornedControlTypeProperty, value); }
        }
        
        #endregion AdornedControlType
        
        public SecurityAccessAdorner(UIElement adornedElement, AdornerLayer adornerLayer, DataTemplate itemTemplate, object tooltip)
            : base(adornedElement)
        {
            this.adornerLayer = adornerLayer;
            this.AdornedControlType = adornedElement.GetType().ToString();

            if (this.AdornedControl != null)
            {
                //QKSC:  4/1/2010, Bug 7699
                //sometimes SecurityUI check for existing adorner is not returning the correct adorner layer for ribbon buttons?
                //it probably has to do with the ribbon being moved from the page to the window....
                //it seems to work correctly for other controls in the layout
                //but the bug was reported as multiple tooltips being added when switching between ribbon tabs
                //I used Snoop to see if the SecurityAccessAdorner was being compounded as well as the tooltip
                //and I did not see any evidence of that, so simply put a check in the adorner not to compound the tooltip.
                this.adornedControlTooltip = this.AdornedControl.ToolTip;
                if (this.AdornedControl.ToolTip is string &&
                    tooltip is string)
                {
                    string ttExisting = (string)this.AdornedControl.ToolTip;
                    string ttAdorned = (string)tooltip;
                    if (!ttExisting.Contains(ttAdorned))
                        tooltip += Environment.NewLine + this.AdornedControl.ToolTip.ToString();
                    else
                        tooltip = ttExisting;
                }
                this.AdornedControl.ToolTip = tooltip;
            }

            if (this.AdornedElement != null)
            {
                Binding b3 = new Binding("IsVisible");
                b3.Source = adornedElement;
                b3.Converter = new BoolToVisibilityConverter();
                this.SetBinding(Adorner.VisibilityProperty, b3);

                Binding b1 = new Binding("ActualHeight");
                b1.Source = adornedElement;
                this.contentHost.SetBinding(Grid.HeightProperty, b1);
                Binding b2 = new Binding("ActualWidth");
                b2.Source = adornedElement;
                this.contentHost.SetBinding(Grid.WidthProperty, b2);
                //this.contentHost.Margin = this.adornedControl.Margin;

                this.contentPresenter = new ContentPresenter();
                this.contentPresenter.Content = this;
                this.contentPresenter.ContentTemplate = itemTemplate;

                //this is a workaround when the data triggers in the template aren't working
                //this.contentPresenter.ApplyTemplate();
                //if (this.AdornedControlType == "Infragistics.Windows.Ribbon.ButtonTool")
                //{
                //    Border b = (Border)this.contentPresenter.ContentTemplate.FindName("Border", this.contentPresenter);
                //    b.Opacity = .3;
                //}

                this.contentHost.Children.Add(this.contentPresenter);
            }

            if (this.adornerLayer != null)
                this.adornerLayer.Add(this);    
        }

        #region overrides
        protected override Size MeasureOverride(Size constraint)
        {
            this.contentHost.Measure(constraint);
            return this.contentHost.DesiredSize;
        }
        protected override Size ArrangeOverride(Size finalSize)
        {
            this.contentHost.Arrange(new Rect(finalSize));
            return finalSize;
        }
        protected override Visual GetVisualChild(int index)
        {
            return this.contentHost;
        }
        protected override int VisualChildrenCount
        {
            get { return 1; }
        }
        #endregion overrides

        #region public methods
        public void Detach()
        {
            if (this.AdornedControl != null)
                this.AdornedControl.ToolTip = this.adornedControlTooltip;
            if (this.adornerLayer != null)
                this.adornerLayer.Remove(this);
        }
        #endregion public methods
    }

    public class RequiredFieldAdorner : BaseAdorner
    {
        public RequiredFieldAdorner(UIElement adornedElement, AdornerLayer adornerLayer, DataTemplate itemTemplate, object tooltip)
            : base(adornedElement, adornerLayer)
        {
            this.AdornedControlType = adornedElement.GetType().ToString();

            if (this.AdornedControl != null)
            {
                this.adornedControlTooltip = this.AdornedControl.ToolTip;
                if (this.AdornedControl.ToolTip is string)
                    tooltip += Environment.NewLine + this.AdornedControl.ToolTip.ToString();
                //this.AdornedControl.ToolTip = tooltip;
                this.ToolTip = tooltip;
                this.Tag = tooltip;
                this.DataContext = this;
            }

            if (this.AdornedElement != null)
            {
                Binding b3 = new Binding("IsVisible");
                b3.Source = adornedElement;
                b3.Converter = new BoolToVisibilityConverter();
                this.SetBinding(Adorner.VisibilityProperty, b3);

                Binding b1 = new Binding("ActualHeight");
                b1.Source = adornedElement;
                this.contentHost.SetBinding(Grid.HeightProperty, b1);
                Binding b2 = new Binding("ActualWidth");
                b2.Source = adornedElement;
                this.contentHost.SetBinding(Grid.WidthProperty, b2);
                //this.contentHost.Margin = this.adornedControl.Margin;

                this.contentPresenter = new ContentPresenter();
                this.contentPresenter.Content = this;
                this.contentPresenter.ContentTemplate = itemTemplate;

                //this is a workaround when the data triggers in the template aren't working
                //this.contentPresenter.ApplyTemplate();
                //if (this.AdornedControlType == "Infragistics.Windows.Ribbon.ButtonTool")
                //{
                //    Border b = (Border)this.contentPresenter.ContentTemplate.FindName("Border", this.contentPresenter);
                //    b.Opacity = .3;
                //}

                this.contentHost.Children.Add(this.contentPresenter);
            }
                base.AddVisualChild(this.contentHost);

            if (this._adornerLayer != null)
                this._adornerLayer.Add(this);    
        }
    }
    public class UserSystemNotVisibleFieldAdorner : BaseAdorner
    {
        public UserSystemNotVisibleFieldAdorner(UIElement adornedElement, AdornerLayer adornerLayer, DataTemplate itemTemplate, object tooltip)
            : base(adornedElement, adornerLayer)
        {
            this.AdornedControlType = adornedElement.GetType().ToString();

            if (this.AdornedControl != null)
            {
                this.adornedControlTooltip = this.AdornedControl.ToolTip;
                if (this.AdornedControl.ToolTip is string)
                    tooltip += Environment.NewLine + this.AdornedControl.ToolTip.ToString();
                this.AdornedControl.ToolTip = tooltip;
                this.ToolTip = tooltip;
                this.DataContext = this;
            }

            if (this.AdornedElement != null)
            {
                Binding b3 = new Binding("IsVisible");
                b3.Source = adornedElement;
                b3.Converter = new BoolToVisibilityConverter();
                this.SetBinding(Adorner.VisibilityProperty, b3);

                Binding b1 = new Binding("ActualHeight");
                b1.Source = adornedElement;
                this.contentHost.SetBinding(Grid.HeightProperty, b1);
                Binding b2 = new Binding("ActualWidth");
                b2.Source = adornedElement;
                this.contentHost.SetBinding(Grid.WidthProperty, b2);
                //this.contentHost.Margin = this.adornedControl.Margin;

                this.contentPresenter = new ContentPresenter();
                this.contentPresenter.Content = this;
                this.contentPresenter.ContentTemplate = itemTemplate;

                //this is a workaround when the data triggers in the template aren't working
                //this.contentPresenter.ApplyTemplate();
                //if (this.AdornedControlType == "Infragistics.Windows.Ribbon.ButtonTool")
                //{
                //    Border b = (Border)this.contentPresenter.ContentTemplate.FindName("Border", this.contentPresenter);
                //    b.Opacity = .3;
                //}

                this.contentHost.Children.Add(this.contentPresenter);
            }
            base.AddVisualChild(this.contentHost);

            if (this._adornerLayer != null)
                this._adornerLayer.Add(this);
        }

    }

    /// <summary>
    /// This adorner can be customized by passing in any object DataItem and any templates
    /// You can pass null for dataItem as long as you provide a contentTemplate that is not dependent on the dataItem
    /// You can also pass null for the tooltipTemplate if you do not want a tooltip applied
    /// </summary>
    public class GenericObjectTemplatedAdorner : BaseAdorner
    {
        public GenericObjectTemplatedAdorner(Object dataItem,
                                             UIElement adornedElement,
                                             AdornerLayer adornerLayer,
                                             DataTemplate contentTemplate,
                                             DataTemplate tooltipTemplate,
                                             object tooltip)
            : base(adornedElement, adornerLayer)
        {
            this.AdornedControlType = adornedElement.GetType().ToString();

            if (this.AdornedControl != null && tooltipTemplate != null)
            {
                //need to handle if the tooltip property has a binding.  This only takes a value.
                this.adornedControlTooltip = AdornedControl != null ? ((Control)AdornedControl).ToolTip : null;

                ContentPresenter tooltipContent = new ContentPresenter();
                tooltipContent.ContentTemplate = tooltipTemplate;
                tooltipContent.Content = dataItem;
                this.AdornedControl.ToolTip = tooltipContent;
            }

            if (this.AdornedElement != null)
            {
                Binding b3 = new Binding("IsVisible");
                b3.Source = adornedElement;
                b3.Converter = new BoolToVisibilityConverter();
                this.SetBinding(Adorner.VisibilityProperty, b3);

                Binding b1 = new Binding("ActualHeight");
                b1.Source = adornedElement;
                this.contentHost.SetBinding(Grid.HeightProperty, b1);
                Binding b2 = new Binding("ActualWidth");
                b2.Source = adornedElement;
                this.contentHost.SetBinding(Grid.WidthProperty, b2);

                this.contentPresenter = new ContentPresenter();
                this.contentPresenter.Content = dataItem;
                this.contentPresenter.ContentTemplate = contentTemplate;

                this.contentHost.Children.Add(this.contentPresenter);
            }
            base.AddVisualChild(this.contentHost);

            if (this._adornerLayer != null)
                this._adornerLayer.Add(this);
        }
    }

    /// <summary>
    /// The DataTemplate should contain a single button.  The adorener handles the click event and detaches itself.
    /// the moduleFieldRule parameter is optional and can contain a ModuleFieldRule object with attributes of the entity being adorned.
    /// Just pass null for this value if not applicable
    /// </summary>
    public class MultiValueAdorner : BaseAdorner
    {
        public MultiValueAdorner(UIElement adornedElement, AdornerLayer adornerLayer, DataTemplate contentTemplate, object tooltip)
            : base(adornedElement, adornerLayer)
        {
            this.AdornedControlType = adornedElement.GetType().ToString();
            this.ToolTip = tooltip;
            this.DataContext = this;

            if (this.AdornedElement != null)
            {
                Binding b3 = new Binding("IsVisible");
                b3.Source = adornedElement;
                b3.Converter = new BoolToVisibilityConverter();
                this.SetBinding(Adorner.VisibilityProperty, b3);

                Binding b1 = new Binding("ActualHeight");
                b1.Source = adornedElement;
                this.contentHost.SetBinding(Grid.HeightProperty, b1);
                Binding b2 = new Binding("ActualWidth");
                b2.Source = adornedElement;
                this.contentHost.SetBinding(Grid.WidthProperty, b2);

                this.contentPresenter = new ContentPresenter();
                this.contentPresenter.Content = this;
                this.contentPresenter.ContentTemplate = contentTemplate;

                this.contentHost.Children.Add(this.contentPresenter);
            }
            this.AddHandler(Button.ClickEvent, new RoutedEventHandler(OnButtonDetachClick));
            base.AddVisualChild(this.contentHost);

            if (this._adornerLayer != null)
                this._adornerLayer.Add(this);
        }
        private void OnButtonDetachClick(object source, RoutedEventArgs e)
        {
            this.AdornedElement.Focus();
            this.Detach();
        }
    }

    /// <summary>
    /// This data template is for data assist.  It has a single button used to copy the text to the clipboard
    /// </summary>
    public class DataAssistAdorner : BaseAdorner
    {
        public DataAssistAdorner(UIElement adornedElement, AdornerLayer adornerLayer, DataTemplate contentTemplate, object tooltip, string dataAssistText)
            : base(adornedElement, adornerLayer)
        {
            this.AdornedControlType = adornedElement.GetType().ToString();
            this.ToolTip = tooltip;
            this.DataAssistText = dataAssistText;
            this.DataContext = this;

            if (this.AdornedElement != null)
            {
                Binding b3 = new Binding("IsVisible");
                b3.Source = adornedElement;
                b3.Converter = new BoolToVisibilityConverter();
                this.SetBinding(Adorner.VisibilityProperty, b3);

                Binding b1 = new Binding("ActualHeight");
                b1.Source = adornedElement;
                this.contentHost.SetBinding(Grid.HeightProperty, b1);
                Binding b2 = new Binding("ActualWidth");
                b2.Source = adornedElement;
                this.contentHost.SetBinding(Grid.WidthProperty, b2);

                this.contentPresenter = new ContentPresenter();
                this.contentPresenter.Content = this;
                this.contentPresenter.ContentTemplate = contentTemplate;

                this.contentHost.Children.Add(this.contentPresenter);
            }
            this.AddHandler(Button.ClickEvent, new RoutedEventHandler(OnButtonCopyToClipboard));
            base.AddVisualChild(this.contentHost);

            if (this._adornerLayer != null)
                this._adornerLayer.Add(this);
        }
        private void OnButtonCopyToClipboard(object source, RoutedEventArgs e)
        {
            this.AdornedElement.Focus();
            Clipboard.SetText(DataAssistText ?? string.Empty);
        }

        public static readonly DependencyProperty DataAssistTextProperty = DependencyProperty.Register("DataAssistText",
            typeof(string), typeof(DataAssistAdorner));

        public string DataAssistText
        {
            get { return (string)GetValue(DataAssistTextProperty); }
            set { SetValue(DataAssistTextProperty, value); }
        }
    }
}
