using NoteBunny.FrontEnd.Wpf.DotNetSix.Context;
using System;
using System.Globalization;
using System.Windows.Data;

namespace NoteBunny.Frontend.Wpf.DotNetSix.ValueConverters
{
    internal class IsPinnedToTextValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value) switch
            {
                true    => "Unpin",
                false   => "Pin"
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
