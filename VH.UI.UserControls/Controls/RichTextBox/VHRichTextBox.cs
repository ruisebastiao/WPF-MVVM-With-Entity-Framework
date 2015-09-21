using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;

namespace VH.UI.UserControls.Controls
{
    public class VHRichTextBox : System.Windows.Controls.RichTextBox
    {
        #region Private Members

        private bool _textHasLoaded;
        private bool _isInvokePending;

        #endregion //Private Members

        #region Constructors

        public VHRichTextBox()
        {
            Loaded += RichTextBox_Loaded;
        }

        public VHRichTextBox(System.Windows.Documents.FlowDocument document)
            : base(document){}

        #endregion //Constructors

        #region Properties

        #region Text

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(VHRichTextBox), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnTextPropertyChanged));//, CoerceTextProperty, true, UpdateSourceTrigger.LostFocus
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VHRichTextBox rtb = (VHRichTextBox)d;

            if ((!rtb._textHasLoaded) || (!rtb._isInvokePending && rtb._textHasLoaded))
            {
                rtb.TextFormatter.SetText(rtb.Document, (string)e.NewValue);
                rtb._textHasLoaded = true;
            }
        }

        private static object CoerceTextProperty(DependencyObject d, object value)
        {
            return value ?? "";
        }

        #endregion //Text

        #region TextFormatter

        private ITextFormatter _textFormatter;
        /// <summary>
        /// The ITextFormatter the is used to format the text of the RichTextBox.
        /// Deafult formatter is the RtfFormatter
        /// </summary>
        public ITextFormatter TextFormatter
        {
            get
            {
                if (_textFormatter == null)
                    _textFormatter = new RtfFormatter(); //default is rtf

                return _textFormatter;
            }
            set
            {
                _textFormatter = value;

                //if the Text has already been set and we are changing the TextFormatter then re-set the text
                if (_textHasLoaded)
                    _textFormatter.SetText(Document, Text);

            }
        }

        #endregion //TextFormatter

        #endregion //Properties

        #region Methods

        private void InvokeUpdateText()
        {
            if (!_isInvokePending)
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(UpdateText));
                _isInvokePending = true;
            }
        }

        private void UpdateText()
        {
            //when the Text is null and the Text hasn't been loaded, it indicates that the OnTextPropertyChanged event hasn't exceuted
            //and since we are initializing the text from here, we don't want the OnTextPropertyChanged to execute, so set the loaded flag to true.
            //this prevents the cursor to jumping to the front of the textbox after the first letter is typed.
            if (!_textHasLoaded && string.IsNullOrEmpty(Text))
                _textHasLoaded = true;

            if (_textHasLoaded)
                this.SetValue(TextProperty, TextFormatter.GetText(Document));
                //Text = TextFormatter.GetText(Document);

            _isInvokePending = false;
        }

        #endregion //Methods

        #region Event Hanlders

        private void RichTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            Binding binding = BindingOperations.GetBinding(this, TextProperty);
            this.DataContextChanged += new DependencyPropertyChangedEventHandler(OSRichTextBox_DataContextChanged);
            if (binding != null)
            {
                if (binding.UpdateSourceTrigger == UpdateSourceTrigger.Default || binding.UpdateSourceTrigger == UpdateSourceTrigger.LostFocus)
                    PreviewLostKeyboardFocus += (o, ea) => UpdateText(); //do this synchronously
                else
                    TextChanged += (o, ea) => InvokeUpdateText(); //do this async
            }
        }

        void OSRichTextBox_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
                _textHasLoaded = false;
            
        }

        #endregion //Event Hanlders
    }
}
