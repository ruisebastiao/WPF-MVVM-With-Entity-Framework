using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace VH.UI.UserControls
{
    public class BoolToOppositeBoolConverter : IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is bool))
                throw new InvalidOperationException("The target must be a boolean");
            
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is bool))
                throw new InvalidOperationException("The target must be a boolean");

            return !(bool)value;
        }
        #endregion
    }
    public sealed class BoolToVisibilityConverter : IValueConverter
    {
        private bool _useHiddenVisibility = false;
        private bool _negateInputValue = false;

        public bool UseHiddenVisibility
        {
            get { return _useHiddenVisibility; }
            set { _useHiddenVisibility = value; }
        }

        public bool NegateInputValue
        {
            get { return _negateInputValue; }
            set { _negateInputValue = value; }
        }
        
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility NotVisible = (UseHiddenVisibility) ? Visibility.Hidden : Visibility.Collapsed;

            if (value == null)
                return NotVisible;
            if (value is bool)
            {
                bool boolValue =  (this.NegateInputValue) ? !(bool)value : (bool)value;
                return boolValue ? Visibility.Visible : NotVisible;
            }

            return NotVisible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is bool && (bool)value)
                return Visibility.Visible;

            return (this.UseHiddenVisibility) ? Visibility.Hidden : Visibility.Collapsed;
        }

        #endregion
    }
    public class LabelConcatenator : IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                string concatenatedLabel = String.Empty;
                if (value is int)
                {
                    string valueString = value.ToString();
                    if (parameter != null && parameter is string)
                    {
                        concatenatedLabel = String.Format("{0} {1}", parameter, value);
                    }
                }
                else if (value is string)
                {
                    if (parameter != null && parameter is string)
                    {
                        concatenatedLabel = String.Format("{0} {1}", parameter, value);
                    }
                }
                //string concatenatedLabel = String.Format("{0} {1}", parameter, value);
                return concatenatedLabel;
            }
            catch
            {
                return String.Empty;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }

    public class VisibilityCollapsedIfNull : IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return Visibility.Collapsed;
            else
            {
                if (value is string)
                    return ((string)value).Trim() == string.Empty ? Visibility.Collapsed : Visibility.Visible;

                return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
    /// <summary>
    /// takes various object types and converts to a Visibility
    /// set the UseHiddenVisibility="True" property if you want to use Visibility.Hidden 
    /// instead of Visibility.Collapse
    /// </summary>
    public sealed class VisibilityConverter : IValueConverter
    {
        private bool _useHiddenVisibility = false;

        public bool UseHiddenVisibility
        {
            get { return _useHiddenVisibility; }
            set { _useHiddenVisibility = value; }
        }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility notVisible = (UseHiddenVisibility) ? Visibility.Hidden : Visibility.Collapsed;

            if (value == null)
                return notVisible;
            if (value is bool)
            {
                bool boolValue = (bool)value;
                return boolValue ? Visibility.Visible : notVisible;
            }
            if (value is int)
            {
                int intValue = (int)value;
                return intValue > 0 ? Visibility.Visible : notVisible;
            }
            if (value is string)
            {
                string s = (string)value;
                return string.IsNullOrEmpty(s) ? notVisible : Visibility.Visible;
            }
            if (value is double)
            {
                double d = (double)value;
                return d > 0 ? Visibility.Visible : notVisible;
            }
            return value != null ? Visibility.Visible : notVisible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
    /// <summary>
    /// takes various object types and converts to a Visibility
    /// </summary>
    public sealed class InvertedVisibilityConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Visibility.Visible;
            if (value is bool)
            {
                bool boolValue = (bool)value;
                return boolValue ? Visibility.Collapsed : Visibility.Visible;
            }
            if (value is int)
            {
                int intValue = (int)value;
                return intValue > 0 ? Visibility.Collapsed : Visibility.Visible;
            }
            if (value is Visibility)
            {
                Visibility v = (Visibility)value;
                if (v == Visibility.Visible)
                    return Visibility.Collapsed;
                else
                    return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
    /// <summary>
    /// Use to convert Visibility to GridLength...GridLength is used to set Width or Height properties on FrameworkElements
    /// The parameter is optional and specifies the Star multiplier
    /// </summary>
    public sealed class VisibilityToGridLengthConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double d = parameter == null ? 1 : Double.Parse((String)parameter);
            GridLength result = new GridLength(d, GridUnitType.Star); //default to "1*"

            if (value is Visibility)
            {
                Visibility v = (Visibility)value;
                if (v == Visibility.Collapsed)
                    result = new GridLength(d, GridUnitType.Auto);
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("VisibilityToGridLengthConverter.ConvertBack");
        }

        #endregion
    }
    public sealed class ImageByteConverter : IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                var imageBytes = (byte[])value;
                var stream = new MemoryStream(imageBytes);
                var image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = stream;
                image.EndInit();
                return image;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
    public sealed class ImageResourceConverter : IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is String && parameter is FrameworkElement)
            {
                object resource = ((FrameworkElement)parameter).TryFindResource((String)value);
                if (targetType.IsInstanceOfType(resource))
                    return (BitmapImage)resource;
                else
                    return null;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
    public sealed class NullableValueConverter : IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(value.ToString()))
                return null;
            return value;
        }
        #endregion
    }
    public sealed class CountGreaterThanZeroBoolConverter : IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if (parameter == null)
                {
                    try
                    {
                        int count = (int)value;
                        return count > 0;
                    }
                    catch
                    { }
                }
                else
                {
                    double p;
                    if (double.TryParse(parameter.ToString(), out p))
                    {
                        //the parameter is the number for comparison
                        double v;
                        if (double.TryParse(value.ToString(), out v))
                            return v > p;
                    }
                }
            }
            return false;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
    public sealed class DecimalGreaterThanZeroBoolConverter : IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if (parameter == null)
                {
                    try
                    {
                        Decimal decValue = (Decimal)value;
                        return decValue > 0;
                    }
                    catch
                    { }
                }
                else
                {
                    double p;
                    if (double.TryParse(parameter.ToString(), out p))
                    {
                        //the parameter is the number for comparison
                        double v;
                        if (double.TryParse(value.ToString(), out v))
                            return v > p;
                    }
                }
            }
            return false;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }

    public sealed class MakeNegativeValueConverter : IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                try
                {
                    if ((int)value > 0)
                        return (int)value * -1;
                }
                catch
                { }
            }
            return value;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }

    public sealed class ObjectToBoolConverter : IValueConverter, IMultiValueConverter
    {
        private bool _negateResult = false;

        public bool NegateResult
        {
            get { return _negateResult; }
            set { _negateResult = value; }
        }
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool result = false;
            if (value != null)
            {
                result = true;
            }

            // invert result
            if (NegateResult) result = !result;
            return result;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
        #endregion

        #region IMultiValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool result = false;

            if (parameter != null)
            {
                string configuration = parameter as string;
                switch (configuration)
                {
                    case "firstor":
                        if (values[0] == null || values[1] != null)
                            result = true;
                        break;
                    case "secondor":
                        if (values[0] != null || values[1] == null)
                            result = true;
                        break;
                    case "firstand":
                        if (values[0] == null && values[1] != null)
                            result = true;
                        break;
                    case "secondand":
                        if (values[0] != null && values[1] == null)
                            result = true;
                        break;
                }
            }
            // default condition
            else if (values[0] != null && values[1] != null)
                result = true;

            return (NegateResult) ? !result : result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class IsStringEmptyOrNullConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return true;

            if (value is string)
                return string.IsNullOrWhiteSpace((string)value) ? true : false;

            return false;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public sealed class StringToUriConverter : IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return string.Empty;
            else
            {
                return new Uri(value.ToString());
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return string.Empty;
            if (value is Uri)
            {
                Uri uri = (Uri)value;
                return uri.OriginalString;
            }
            else
                return value.ToString();

        }
        #endregion
    }

    [ValueConversion(typeof(int?),typeof(int))]
    public sealed class IndexToDisplayValueConverter : IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //string result = string.Empty;

            if (value != null)
            {
                if (value is int)
                {
                    int i = (int)value;// int.Parse(value.ToString());

                    if (i != -1)
                        return ++i;
                }
            }


            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (string.IsNullOrEmpty(value.ToString()))
                return -1;
            try
            {
                int i = int.Parse(value.ToString());
                return i - 1;
            }
            catch
            {
                return -1;
            }

        }
        #endregion
    }
    public sealed class ValueChangerByMultiplicationValueConverter : IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return string.Empty;
            else if (parameter == null)
                return value;

            try
            {
                double d = double.Parse(value.ToString(), CultureInfo.InvariantCulture);
                double p = double.Parse(parameter.ToString(), CultureInfo.InvariantCulture);
                return (d * p).ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
    public sealed class DoubleMaskFromLengthValueConverter : IValueConverter
    {
        private int _precision = 0;
        public int Precision
        {
            get { return this._precision; }
            set { this._precision = value; }
        }
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            try
            {
                int d = int.Parse(value.ToString(), CultureInfo.InvariantCulture);
                string mask = string.Format("double:{0}.{1}",
                                             d,
                                             this._precision);
                return "{" + mask + "}";
            }
            catch
            {
                return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
    public sealed class AddPunctuationValueConverter : IValueConverter
    {
        private bool _addPadding = true;
        private char _punctuationCharacter = ':';
        private int _paddingLength = 2;
        private string _originalValue = string.Empty;

        public bool AddPadding
        {
            get { return this._addPadding; }
            set { this._addPadding = value; }
        }
        public char PunctuationCharacter
        {
            get { return this._punctuationCharacter; }
            set { this._punctuationCharacter = value; }
        }
        public int PaddingLength
        {
            get { return this._paddingLength; }
            set { this._paddingLength = value; }
        }

        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                this._originalValue = string.Empty;
                return string.Empty;
            }

            this._originalValue = value.ToString();
            string returnValue = string.Empty;
            try
            {
                //if (value is string)
                //{
                returnValue = value.ToString().Trim().TrimEnd(this._punctuationCharacter).Trim() +  " " + this._punctuationCharacter.ToString();
                if (this._addPadding)
                    returnValue += new string(' ', this._paddingLength);
                //}
            }
            catch { }

            return returnValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return _originalValue;
        }
        #endregion
    }

    public sealed class BoolToVisibilityValueConverter : MarkupExtension, IValueConverter
    {
        public bool IsNegateValue { get; set; }
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool && (bool)value)
                return IsNegateValue ? Visibility.Collapsed : Visibility.Visible;

            return !IsNegateValue ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
        #endregion

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }

    public sealed class ObjectToVisibilityValueConverter : MarkupExtension, IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return Visibility.Collapsed;

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
        #endregion

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
    public sealed class ValueParamCompareToVisibilityConverter : IValueConverter
    {
        #region IValueConverter Members

        /// <summary>
        /// if the value equals the param, returns Visibility.Visible
        /// defaults to Visibility.Collapsed
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility vis = Visibility.Collapsed;

            if (parameter != null && value != null)
            {
                if (parameter.Equals(value))
                    vis = Visibility.Visible;
            }
            return vis;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public sealed class VisibilityToBoolConverter : IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool result = false;

            if (value is Visibility)
            {
                Visibility v = (Visibility)value;
                result = (v == Visibility.Visible);
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Visibility v = Visibility.Collapsed;

            if (value is bool)
            {
                bool b = (bool)value;
                if (b) v = Visibility.Visible;
            }

            return v;
        }
        #endregion
    }

    public sealed class VisibilityToOppositeBoolConverter : IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool result = true;

            if (value is Visibility)
            {
                Visibility v = (Visibility)value;
                result = !(v == Visibility.Visible);
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Visibility v = Visibility.Visible;

            if (value is bool)
            {
                bool b = (bool)value;
                if (b) v = Visibility.Collapsed;
            }

            return v;
        }
        #endregion
    }

    /// <summary>
    /// Use this converter to make comparisions to Enums by passing the string representation of the
    /// desired value to compared to in ConverterParameter
    /// </summary>
    public class EnumStringComparisionConverter : IValueConverter
    {
        private bool _negateResult = false;

        public bool NegateResult
        {
            get { return _negateResult; }
            set { _negateResult = value; }
        }

        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool result = false;
            Enum sourceEnum = value as Enum;

            if (parameter != null && sourceEnum != null)
            {
                int compareResult = string.Compare(sourceEnum.ToString().Trim(), parameter.ToString().Trim(), true);

                result = (compareResult == 0);

                // invert result
                if (NegateResult) result = !result;
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }

    /// <summary>
    /// Multibinding converter that performs a boolean AND with all passed values
    /// Use the NegateResult property (true/false) if you want to apply a NOT to the final result
    /// </summary>
    public class MultiBooleanAndConverter : IMultiValueConverter
    {
        private bool _negateResult = false;

        public bool NegateResult
        {
            get { return _negateResult; }
            set { _negateResult = value; }
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool result = true;

            foreach (object value in values)
            {
                bool? valueAsBool = value as bool?;
                if (valueAsBool.HasValue)
                    result = result && valueAsBool.Value;
                else
                    result = false;
            }

            // invert result
            if (NegateResult) result = !result;

            return result;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            // This converter is a OneWay converter only
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// Multibinding converter that performs a boolean AND with all passed values
    /// values can be either bool or bool? or Visibility
    /// if all values are true or Visible, returns Visible, else Collapsed or Hidden
    /// Use the NegateResult property (true/false) if you want to apply a NOT to the final result
    /// Use the UseHidden property if you want to return Hidden
    /// </summary>
    public class MultiBoolAndToVisibilityConverter : IMultiValueConverter
    {
        private bool _negateResult = false;
        private bool _useHidden = false;

        public bool NegateResult
        {
            get { return _negateResult; }
            set { _negateResult = value; }
        }
        public bool UseHidden
        {
            get { return _useHidden; }
            set { _useHidden = value; }
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility result = Visibility.Visible;

            bool bCollection = true;

            foreach (object value in values)
            {
                if (value is bool || value is bool?)
                {
                    bool? valueAsBool = value as bool?;
                    if (valueAsBool.HasValue)
                        bCollection = bCollection && valueAsBool.Value;
                    else
                        bCollection = false;
                }
                else if (value is Visibility)
                {
                    bCollection = bCollection & (((Visibility)value) == Visibility.Visible);
                }
                else if (value == DependencyProperty.UnsetValue)
                {
                    bCollection = bCollection & false;
                }
            }

            // invert result?
            if (NegateResult) bCollection = !bCollection;

            if (!bCollection)
                result = this._useHidden ? Visibility.Hidden : Visibility.Collapsed;

            return result;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            // This converter is a OneWay converter only
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Multibinding converter that performs a boolean OR with all passed values
    /// Use the NegateResult property (true/false) if you want to apply a NOT to the final result
    /// </summary>
    public class MultiBooleanOrConverter : IMultiValueConverter
    {
        private bool _negateResult = false;

        public bool NegateResult
        {
            get { return _negateResult; }
            set { _negateResult = value; }
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool result = false;

            foreach (object value in values)
            {
                if (value == DependencyProperty.UnsetValue)
                    result = result || false;
                else
                    result = result || (bool)value;
            }

            // invert result
            if (NegateResult) result = !result;

            return result;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            // This converter is a OneWay converter only
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// Multibinding converter that returns visiblility=visible if any of the  passed values is visible
    /// </summary>
    public class MultiVisibilityToVisibilityConverter : IMultiValueConverter
    {
        public bool NegateResult { get; set; }
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility result = Visibility.Collapsed;

            foreach (object value in values)
            {
                Visibility? visibility = value as Visibility?;
                if (visibility == Visibility.Visible)
                {
                    result = Visibility.Visible;
                    break;
                }
            }

            if (NegateResult)
            {
                result = result == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            }

            return result;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            // This converter is a OneWay converter only
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Multibinding converter that returns visiblility=visible if all of the  passed values are visible
    /// </summary>
    public class MultiAndVisibilityToVisibilityConverter : IMultiValueConverter
    {
        public bool NegateResult { get; set; }
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility result = Visibility.Visible;

            foreach (object value in values)
            {
                Visibility? visibility = value as Visibility?;
                if (visibility != Visibility.Visible)
                {
                    result = Visibility.Collapsed;
                    break;
                }
            }

            if (NegateResult)
            {
                result = result == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            }

            return result;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            // This converter is a OneWay converter only
            throw new NotImplementedException();
        }
    }
    
    public class PopupSizeConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value != null)
                {
                    double size = (double)value;
                    if (parameter != null)
                    {
                        double margin = Double.Parse(parameter.ToString());
                        if (margin < size)
                            size = size - margin;
                    }
                    return size;
                }
                else
                {
                    return value;
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("PopupSizeConverter : " + ex.Message);
                return null;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        #endregion
    }

    public class PopupDisableBackgroundGridConverter : IMultiValueConverter
    {
        #region IMultiValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility visibility = Visibility.Collapsed;
            try
            {
                foreach (object value in values)
                {
                    if (value != null)
                    {
                        if (value.GetType() == typeof(Visibility))
                        {
                            Visibility _visibility = (Visibility)value;
                            if (_visibility == Visibility.Visible)
                            {
                                visibility = _visibility;
                                break;
                            }
                        }

                        if (value.GetType() == typeof(bool))
                        {
                            bool isOpen = (bool)value;
                            if (isOpen)
                            {
                                visibility = Visibility.Visible;
                                break;
                            }
                        }
                    }
                }
                return visibility;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("PopupDisableBackgroundGridConverter : {0}", ex.ToString());
                return visibility;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            System.Diagnostics.Debug.WriteLine("PopupDisableBackgroundGridConverter : ConvertBack not implemented");
            return null;
        }

        #endregion
    }

    public sealed class DateOnlyConverter : IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime cutOffDate = DateTime.Parse("1/1/1900");

            if (value == null)
                return String.Empty;

            if (value.GetType() != typeof(DateTime))
                throw new ArgumentException("Expecting a DateTime");

            if ((DateTime)value < cutOffDate)
                return String.Empty;
            return ((DateTime)value).ToShortDateString();
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (String.IsNullOrEmpty((string)value))
                return null;
            if (value.GetType() != typeof(String))
                throw new ArgumentException("Expecting a String");

            DateTime resultDateTime;
            if (DateTime.TryParse(value as String, out resultDateTime))
            {
                return resultDateTime;
            }
            return DependencyProperty.UnsetValue;
        }
        #endregion
    }
    public sealed class TimeOnlyConverter : IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime cutOffDate = DateTime.Parse("1/1/1900");

            if (value == null)
                return String.Empty;

            if (value.GetType() != typeof(DateTime))
                throw new ArgumentException("Expecting a DateTime");

            if ((DateTime)value < cutOffDate)
                return String.Empty;
            return ((DateTime)value).ToShortTimeString();
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // This converter is a OneWay converter only
            throw new NotImplementedException();
        }
        #endregion
    }
    public sealed class DebugHelperConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        #endregion
    }

    public sealed class StringConcatenator : IMultiValueConverter
    {
        #region IMultiValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            StringBuilder s = new StringBuilder();
            string separator = parameter == null ? string.Empty : parameter.ToString();

            if (!string.IsNullOrEmpty(separator))
            {
                foreach (object o in values)
                {
                    s.Append(o.ToString());
                    s.Append(separator);
                }
                return s.ToString().TrimEnd(separator.ToCharArray());
            }
            else
            {
                foreach (object o in values)
                    s.Append(o.ToString());
            }

            return s.ToString();

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
    public sealed class StringFormatter : IMultiValueConverter
    {
        #region IMultiValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Format(parameter.ToString(), values);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    /// <summary>
    /// Determine if the value is NOT null
    /// </summary>
    public class IsNotNullConverter : IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (value != null);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }

    public class ChainedConverter : IValueConverter
    {
        #region Fields
        private System.Collections.ArrayList _converters;
        #endregion

        #region Properties
        public System.Collections.ArrayList Converters
        {
            get
            {
                if (this._converters == null)
                    this._converters = new System.Collections.ArrayList();
                return this._converters;
            }
            set { this._converters = value; }
        }
        #endregion


        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            foreach (object converter in this.Converters)
                value = ((IValueConverter)converter).Convert(value, targetType, parameter, culture);

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            foreach (object converter in this.Converters)
                value = ((IValueConverter)converter).ConvertBack(value, targetType, parameter, culture);

            return value;
        }
        #endregion
    }
    public sealed class AlignmentConverter : IValueConverter
    {
        private HorizontalAlignment trueHorizontalAlignment = HorizontalAlignment.Center;
        public HorizontalAlignment TrueHorizontalAlignment
        {
            get { return this.trueHorizontalAlignment; }
            set { this.trueHorizontalAlignment = value; }
        }
        private HorizontalAlignment falseHorizontalAlignment = HorizontalAlignment.Stretch;
        public HorizontalAlignment FalseHorizontalAlignment
        {
            get { return this.falseHorizontalAlignment; }
            set { this.falseHorizontalAlignment = value; }
        }
        private VerticalAlignment trueVerticalAlignment = VerticalAlignment.Center;
        public VerticalAlignment TrueVerticalAlignment
        {
            get { return this.trueVerticalAlignment; }
            set { this.trueVerticalAlignment = value; }
        }
        private VerticalAlignment falseVerticalAlignment = VerticalAlignment.Stretch;
        public VerticalAlignment FalseVerticalAlignment
        {
            get { return this.falseVerticalAlignment; }
            set { this.falseVerticalAlignment = value; }
        }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                bool b = (bool)value;
                if (targetType == typeof(HorizontalAlignment))
                    return b ? trueHorizontalAlignment : falseHorizontalAlignment;
                else if (targetType == typeof(VerticalAlignment))
                    return b ? trueVerticalAlignment : falseVerticalAlignment;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
    public sealed class BoolToScrollBarVisibilityConverter : IValueConverter
    {
        private ScrollBarVisibility trueScrollBarVisibility = ScrollBarVisibility.Auto;
        public ScrollBarVisibility TrueScrollBarVisibility
        {
            get { return this.trueScrollBarVisibility; }
            set { this.trueScrollBarVisibility = value; }
        }
        private ScrollBarVisibility falseScrollBarVisibility = ScrollBarVisibility.Disabled;
        public ScrollBarVisibility FalseScrollBarVisibility
        {
            get { return this.falseScrollBarVisibility; }
            set { this.falseScrollBarVisibility = value; }
        }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                bool b = (bool)value;
                if (targetType == typeof(ScrollBarVisibility))
                    return b ? trueScrollBarVisibility : falseScrollBarVisibility;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }


    public class GridLengthToDoubleConverter : IValueConverter
    {
        #region Fields
        private GridLengthConverter _typeConverter;
        #endregion

        #region Constructors
        public GridLengthToDoubleConverter()
        {
            this._typeConverter = new GridLengthConverter();
        }
        #endregion

        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is GridLength))
                throw new ArgumentException("value must be of type GridLength");

            return ((GridLength)value).Value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            GridLength length = new GridLength(System.Convert.ToDouble(value));

            return length;
        }
        #endregion
    }

    public sealed class ObjectToNullTextConverter : IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value != null) ? "{Identifier will be generated}" : "{Enter an identifier}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
    public sealed class DateLessThanTodayConverter : IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value.ToString() == string.Empty)
                return true;
            if (parameter == null)
            {
                try
                {
                    DateTime dt = DateTime.Parse(value.ToString());
                    string strDate = dt.ToShortDateString();
                    dt = DateTime.Parse(strDate);
                    DateTime today = DateTime.Today;
                    return dt < today;
                }
                catch
                { }
            }
            else
            {
                //TODO: write routine if date parameter passd in
            }
            return true;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }

    public sealed class TruncLongToolTipConverter : IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            
            if (value != null && value.ToString().Length > 150)
            {
                value = value.ToString().Substring(0, 150) + "...";
            }
            
            return value;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }

    public sealed class DateLessThanEqualTodayConverter : IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value.ToString() == string.Empty)
                return true;
            if (parameter == null)
            {
                try
                {
                    DateTime dt = DateTime.Parse(value.ToString());
                    string strDate = dt.ToShortDateString();
                    dt = DateTime.Parse(strDate);
                    DateTime today = DateTime.Today;
                    return dt <= today;
                }
                catch
                { }
            }
            else
            {
                //TODO: write routine if date parameter passd in
            }
            return true;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }

    public sealed class ValidDateConverter : IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value.ToString() == string.Empty)
                return false;
            if (parameter == null)
            {
                try
                {
                    DateTime dt = DateTime.Parse(value.ToString());
                    return true;
                }
                catch
                { }
            }
            else
            {
                //TODO: write routine if date parameter passd in
            }
            return false;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
    #region DatesLessThanTodayOrNullConverter
    public class DatesLessThanTodayOrNullConverter : IMultiValueConverter
    {
        #region IValueConverter Members
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (values.GetLength(0) == 2)
                {
                    string stEstimatedDate = values[0].ToString();
                    string stScheduledDate = values[1].ToString();
                    DateTime dtEstimatedDate;
                    DateTime dtScheduledDate;
                    try
                    {
                        dtEstimatedDate = DateTime.Parse(stEstimatedDate);
                    }
                    catch
                    {
                        dtEstimatedDate = DateTime.MinValue;
                   }
                    try
                    {
                        dtScheduledDate = DateTime.Parse(stScheduledDate);
                    }
                    catch
                    {
                        dtScheduledDate = DateTime.MinValue;
                    }
                //determine which date to use
                    if (dtScheduledDate == DateTime.MinValue)
                        dtScheduledDate = dtEstimatedDate;
                    if (dtScheduledDate == DateTime.MinValue)
                        return false;
                    stScheduledDate = dtScheduledDate.ToShortDateString();
                    dtScheduledDate = DateTime.Parse(stScheduledDate);
                    DateTime today = DateTime.Today;
                    return dtScheduledDate < today;
                }
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return null;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("DatesLessThanTodayOrNullConverter");
        }
        #endregion
    }
    #endregion
    #region DatesEqualTodayOrNullConverter
    public class DatesEqualTodayOrNullConverter : IMultiValueConverter
    {
        #region IValueConverter Members
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (values.GetLength(0) == 2)
                {
                    bool retval = false;
                    string stEstimatedDate = values[0].ToString();
                    string stScheduledDate = values[1].ToString();
                    DateTime dtEstimatedDate;
                    DateTime dtScheduledDate;
                    try
                    {
                        dtEstimatedDate = DateTime.Parse(stEstimatedDate);
                    }
                    catch
                    {
                        dtEstimatedDate = DateTime.MinValue;
                    }
                    try
                    {
                        dtScheduledDate = DateTime.Parse(stScheduledDate);
                    }
                    catch
                    {
                        dtScheduledDate = DateTime.MinValue;
                    }
                    //determine which date to use
                    if (dtScheduledDate == DateTime.MinValue)
                        dtScheduledDate = dtEstimatedDate;
                    if (dtScheduledDate == DateTime.MinValue)
                        return false;
                    stScheduledDate = dtScheduledDate.ToShortDateString();
                    dtScheduledDate = DateTime.Parse(stScheduledDate);
                    DateTime today = DateTime.Today;
                    if (dtScheduledDate == today)
                        retval = true;
                    return retval;
                }
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return null;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("DatesEqualTodayOrNullConverter");
        }
        #endregion
    }
    #endregion


    public sealed class WizardStepToVisibilityComverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter != null)
            {
                string target = parameter.ToString();
                switch (target)
                {
                    case "previous":
                        if (value is int)
                        {
                            return System.Convert.ToInt32(value) == 1 ? Visibility.Collapsed : Visibility.Visible;
                        }
                        break;
                }
            }           
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [MarkupExtensionReturnType(typeof(IValueConverter))]
    public class DateTimeFormatConverter:MarkupExtension,IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime)
                return String.Format("{0:tt}", ((DateTime) value));
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
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
                return ((IList)value).Count != 0;
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
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }

}
