using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VH.UI.UserControls
{
    public sealed class TextBlockAdorner
         : System.Windows.Documents.Adorner
    {
        #region · Fields ·

        private readonly TextBlock adornerTextBlock;

        #endregion

        #region · Properties ·

        protected override int VisualChildrenCount
        {
            get { return 1; }
        }

        #endregion

        #region · Constructors ·

        public TextBlockAdorner(UIElement adornedElement, string label, Style labelStyle)
            : base(adornedElement)
        {
            this.adornerTextBlock = new TextBlock { Style = labelStyle, Text = label };
        }

        #endregion

        #region · Overriden Methods ·

        protected override Size MeasureOverride(Size constraint)
        {
            this.adornerTextBlock.Measure(constraint);

            return constraint;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            this.adornerTextBlock.Arrange(new Rect(finalSize));

            return finalSize;
        }

        protected override Visual GetVisualChild(int index)
        {
            return this.adornerTextBlock;
        }

        #endregion
    }
}
