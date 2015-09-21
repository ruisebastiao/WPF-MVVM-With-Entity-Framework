using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.ComponentModel;

namespace VH.UI.UserControls.Controls
{
    public class VHLabel : Label
    {
        private AddPunctuationValueConverter _addPunc;

        #region AddPaddingAfterPunctuation
        public static readonly DependencyProperty AddPaddingAfterPunctuationProperty = DependencyProperty.Register("AddPaddingAfterPunctuation", typeof(bool), typeof(VHLabel), new FrameworkPropertyMetadata(true));
        
        [Category("OSLabelX Properties"),
         Description("AddPaddingAfterPunctuation.")]
        public bool AddPaddingAfterPunctuation
        {
            get { return (bool)GetValue(AddPaddingAfterPunctuationProperty); }
            set { SetValue(AddPaddingAfterPunctuationProperty, value); }
        }
        
        #endregion AddPaddingAfterPunctuation
        
        #region EndPunctuationCharacter
        public static readonly DependencyProperty EndPunctuationCharacterProperty = DependencyProperty.Register("EndPunctuationCharacter", typeof(char), typeof(VHLabel), new FrameworkPropertyMetadata(':'));
        
        [Category("OSLabelX Properties"),
         Description("EndPunctuationCharacter.")]
        public char EndPunctuationCharacter
        {
            get { return (char)GetValue(EndPunctuationCharacterProperty); }
            set { SetValue(EndPunctuationCharacterProperty, value); }
        }
        
        #endregion EndPunctuationCharacter
        
        #region AddEndingPunctuation
        public static readonly DependencyProperty AddEndingPunctuationProperty = DependencyProperty.Register("AddEndingPunctuation", typeof(bool), typeof(VHLabel), new FrameworkPropertyMetadata(true));
        
        [Category("OSLabelX Properties"),
         Description("AddEndingPunctuation.")]
        public bool AddEndingPunctuation
        {
            get { return (bool)GetValue(AddEndingPunctuationProperty); }
            set { SetValue(AddEndingPunctuationProperty, value); }
        }
        
        #endregion AddEndingPunctuation

        #region PaddingLength
        public static readonly DependencyProperty PaddingLengthProperty = DependencyProperty.Register("PaddingLength", typeof(int), typeof(VHLabel), new FrameworkPropertyMetadata(2));
        
        [Category("OSLabelX Properties"),
         Description("PaddingLength.")]
        public int PaddingLength
        {
            get { return (int)GetValue(PaddingLengthProperty); }
            set { SetValue(PaddingLengthProperty, value); }
        }
        
        #endregion PaddingLength
        
        public VHLabel()
            : base()
        {
            _addPunc = new AddPunctuationValueConverter();
            _addPunc.AddPadding = this.AddPaddingAfterPunctuation;
            _addPunc.PunctuationCharacter = this.EndPunctuationCharacter;
            _addPunc.PaddingLength = this.PaddingLength;
            
            //create a default style here so it will be carried up the inheritance tree to WPSLabel
            //if (this.Style == null)
            //{
            //    System.Windows.Style style = new Style(typeof(OSLabelX));
            //    Setter s1 = new Setter(OSLabelX.VerticalAlignmentProperty, VerticalAlignment.Top);
            //    Setter s2 = new Setter(OSLabelX.MarginProperty, new Thickness(0, 4, 0, 0));
            //    style.Setters.Add(s1);
            //    style.Setters.Add(s2);
            //    this.Style = style;
            //}
        }
        protected override void OnInitialized(EventArgs e)
        {
            if (this.AddEndingPunctuation)
            {
                BindingExpression be = this.GetBindingExpression(TextBlock.TextProperty);
                if (be != null)
                {
                    //if there is a binding expression on the text property.. 
                    //copy the binding and add the converter to the expression
                    Binding b = be.ParentBinding;

                    Binding bNew = new Binding();
                    bNew.Path = b.Path;

                    if (b.Source == null && b.RelativeSource == null)
                        bNew.ElementName = b.ElementName;
                    else if (b.Source == null)
                        bNew.RelativeSource = b.RelativeSource;
                    else
                        bNew.Source = b.Source;

                    bNew.Mode = b.Mode;
                    bNew.UpdateSourceTrigger = b.UpdateSourceTrigger;
                    bNew.XPath = b.XPath;
                    bNew.FallbackValue = b.FallbackValue;
                    bNew.StringFormat = b.StringFormat;
                    bNew.TargetNullValue = b.TargetNullValue;
                    bNew.UpdateSourceExceptionFilter = b.UpdateSourceExceptionFilter;
                    bNew.ValidatesOnDataErrors = b.ValidatesOnDataErrors;
                    bNew.ValidatesOnExceptions = b.ValidatesOnExceptions;
                    bNew.Converter = _addPunc;

                    this.SetBinding(TextBlock.TextProperty, bNew);
                }
                else
                {
                    this.Content = this.FormatTextProperty(this.Content);
                }
            }
            base.OnInitialized(e);
        }
        private string FormatTextProperty(object text)
        {
            return (string)_addPunc.Convert(text, typeof(string), null, System.Globalization.CultureInfo.CurrentCulture);
        }
    }
}
