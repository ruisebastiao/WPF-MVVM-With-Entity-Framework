using System;
using System.Globalization;
using System.Windows.Input;

namespace VH.UI.UserControls
{
    public class Utilities
    {
        internal class KeyboardUtilities
        {
            internal static bool IsKeyModifyingPopupState(KeyEventArgs e)
            {
                return ((((Keyboard.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt) && ((e.SystemKey == Key.Down) || (e.SystemKey == Key.Up)))
                      || (e.Key == Key.F4));
            }
        }

        internal static bool IsFormatStringValid(object value)
        {
            try
            {
                // Test the format string if it be used
                DateTime.MinValue.ToString((string)value, CultureInfo.CurrentCulture);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}