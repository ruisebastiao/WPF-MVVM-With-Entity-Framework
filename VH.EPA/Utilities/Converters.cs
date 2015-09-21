using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using VH.Model;

namespace VH.View.Converters
{
    [MarkupExtensionReturnType(typeof(IValueConverter))]
    public abstract class MarkupConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        protected abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);
        protected abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return Convert(value, targetType, parameter, culture);
            }
            catch
            {
                return DependencyProperty.UnsetValue;
            }
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return ConvertBack(value, targetType, parameter, culture);
            }
            catch
            {
                return DependencyProperty.UnsetValue;
            }
        }
    }
    public class NullObjectToVisibiltyConverter : MarkupExtension, IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value == null) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }

    public class NullObjectToBoolConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return false;

            if (value is IList)
            {
                return ((IList) value).Count != 0;
            }

            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }

    public class ToLowerConverter
        : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                var strValue = value.ToString();


                return strValue.ToLowerInvariant();
            }
            return null;
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter,
                                  System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }

    public class BooleanOrConverter : MarkupExtension, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter,
                              System.Globalization.CultureInfo culture)
        {
            return values.Any(value => value != DependencyProperty.UnsetValue && (bool) value);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter,
                                    System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }

    public class ToUpperConverter : MarkupConverter
    {
        protected override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = value as string;
            return val != null ? val.ToUpper() : value;
        }

        protected override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }

    public class ToTitleCaseConverter : MarkupConverter
    {
        protected override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = value as string;
            TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
            return val != null ? ti.ToTitleCase(val) : value;
        }

        protected override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }

    public class VisibilityByRepairStatusConverter : MarkupConverter
    {
        public string StatusParameter { get; set; }
        protected override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = value is RepairStatus ? (RepairStatus) value : RepairStatus.NotProcessed;

            return (status.ToString() == StatusParameter) ? Visibility.Visible : Visibility.Collapsed;
        }

        protected override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }

    public class EnumToResourceConverter : MarkupConverter
    {
        protected override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                var resourceString =
                    Resources.EnumResources.ResourceManager.GetString(value.GetType().Name + "_" + value.ToString(),
                                                                      CultureInfo.CurrentCulture);
                return (!string.IsNullOrEmpty(resourceString)) ? resourceString : value;
            }
            return string.Empty;
        }

        protected override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }

    public class VisibilityOnEarMoldType : MarkupConverter
    {
        protected override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is EarMoldType)
            {
                var result = (EarMoldType) value;
                return result == EarMoldType.Soft ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        protected override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
