using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VH.UI.UserControls
{
    /// <summary>
    /// Adds icon content to a standard button.
    /// </summary>
    public class ModernButton
        : Button
    {
        /// <summary>
        /// Identifies the IconData property.
        /// </summary>
        public static readonly DependencyProperty IconDataProperty = DependencyProperty.Register("IconData", typeof(Geometry), typeof(ModernButton));
        /// <summary>
        /// Identifies the IconHeight property.
        /// </summary>
        public static readonly DependencyProperty IconHeightProperty = DependencyProperty.Register("IconHeight", typeof(double), typeof(ModernButton), new PropertyMetadata(10D));
        /// <summary>
        /// Identifies the IconWidth property.
        /// </summary>
        public static readonly DependencyProperty IconWidthProperty = DependencyProperty.Register("IconWidth", typeof(double), typeof(ModernButton), new PropertyMetadata(10D));

        /// <summary>
        /// Initializes a new instance of the <see cref="ModernButton"/> class.
        /// </summary>
        public ModernButton()
        {
            this.DefaultStyleKey = typeof(ModernButton);
        }

        /// <summary>
        /// Gets or sets the icon path data.
        /// </summary>
        /// <value>
        /// The icon path data.
        /// </value>
        public Geometry IconData
        {
            get { return (Geometry)GetValue(IconDataProperty); }
            set { SetValue(IconDataProperty, value); }
        }

        /// <summary>
        /// Gets or sets the icon height.
        /// </summary>
        /// <value>
        /// The icon height.
        /// </value>
        public double IconHeight
        {
            get { return (double)GetValue(IconHeightProperty); }
            set { SetValue(IconHeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets the icon width.
        /// </summary>
        /// <value>
        /// The icon width.
        /// </value>
        public double IconWidth
        {
            get { return (double)GetValue(IconWidthProperty); }
            set { SetValue(IconWidthProperty, value); }
        }

        public static readonly DependencyProperty ControlHeightProperty =
            DependencyProperty.Register("ControlHeight", typeof (double), typeof (ModernButton), new PropertyMetadata(default(double)));

        public double ControlHeight
        {
            get { return (double) GetValue(ControlHeightProperty); }
            set { SetValue(ControlHeightProperty, value); }
        }

        public static readonly DependencyProperty ControlWidthProperty =
            DependencyProperty.Register("ControlWidth", typeof (double), typeof (ModernButton), new PropertyMetadata(default(double)));

        public double ControlWidth
        {
            get { return (double) GetValue(ControlWidthProperty); }
            set { SetValue(ControlWidthProperty, value); }
        }
    }
}
