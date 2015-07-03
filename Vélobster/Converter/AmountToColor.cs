using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Vélobster.Converter
{
    public class AmountToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var amount = (double)value;
            if (amount < 2)
                return new SolidColorBrush(AppColor.Red);
            else if (amount < 5)
                return new SolidColorBrush(AppColor.Yellow);
            else
                return new SolidColorBrush(AppColor.Green);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
