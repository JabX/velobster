using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Vélobster.Converter
{
    class DisplayedDataToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value.ToString() == (string)parameter ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
