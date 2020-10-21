using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace KeyIndicator.Client
{
    public class KeyToVisibilityConverter : IValueConverter
    {
        public KeyboardHook.VKeys ShowOn { get; set; }
        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value is KeyboardHook.VKeys && (KeyboardHook.VKeys)value == ShowOn) ? Visibility.Visible : Visibility.Hidden;
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
