using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Vélobster.Converter
{
    public class ZoomToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (double)value < Config.MinZoom ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
