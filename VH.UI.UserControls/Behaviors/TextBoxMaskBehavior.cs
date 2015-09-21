using System;
using System.Windows;
using System.Windows.Controls;
using VH.SimpleUI.Entities;

namespace VH.UI.UserControls
{
    public class TextBoxMaskBehavior
    {
        public static MaskType GetMask(DependencyObject obj)
        {
            return (MaskType)obj.GetValue(MaskProperty);
        }

        public static void SetMask(DependencyObject obj, MaskType value)
        {
            obj.SetValue(MaskProperty, value);
        }

        public static readonly DependencyProperty MaskProperty =
           DependencyProperty.RegisterAttached(
           "Mask",
           typeof(MaskType),
           typeof(TextBoxMaskBehavior),
           new FrameworkPropertyMetadata(MaskChangedCallback)
           );

        private static void MaskChangedCallback(DependencyObject d,
                            DependencyPropertyChangedEventArgs e)
        {
            var textBox = (TextBox)d;

            textBox.PreviewTextInput += (sender, args) => { args.Handled = !IsTextAllowed(args.Text); };

            DataObject.AddPastingHandler(textBox, PastingHandler);
        }

        private static Boolean IsTextAllowed(String text)
        {
            return Array.TrueForAll<Char>(text.ToCharArray(),
                                          c => Char.IsDigit(c) || Char.IsControl(c));
        }

        private static void PastingHandler(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                var text = (String)e.DataObject.GetData(typeof(String));
                if (!IsTextAllowed(text)) e.CancelCommand();
            }
            else e.CancelCommand();
        }
    }
}