using Vélobster.Model;
using Windows.UI.Xaml;

namespace Vélobster.ViewModel
{
    class DisplayedDataViewModel : DependencyObject
    {
        public double AvailableBikes
        {
            get { return (double)GetValue(AvailableBikesProperty); }
            set { SetValue(AvailableBikesProperty, value); }
        }

        public double AvailableStands
        {
            get { return (double)GetValue(AvailableStandsProperty); }
            set { SetValue(AvailableStandsProperty, value); }
        }

        public StationData DisplayedData
        {
            get { return (StationData)GetValue(DisplayedDataProperty); }
            set { SetValue(DisplayedDataProperty, value); }
        }

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var model = (DisplayedDataViewModel)d;

            model.Value = model.DisplayedData == StationData.Bikes ? model.AvailableBikes : model.AvailableStands;
        }

        public static readonly DependencyProperty AvailableBikesProperty = DependencyProperty.Register(
            "AvailableBikes", typeof(double), typeof(DisplayedDataViewModel), new PropertyMetadata(null, OnPropertyChanged));
        public static readonly DependencyProperty AvailableStandsProperty = DependencyProperty.Register(
            "AvailableStands", typeof(double), typeof(DisplayedDataViewModel), new PropertyMetadata(null, OnPropertyChanged));
        public static readonly DependencyProperty DisplayedDataProperty = DependencyProperty.Register(
            "DisplayedData", typeof(StationData), typeof(DisplayedDataViewModel), new PropertyMetadata(null, OnPropertyChanged));
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof(double), typeof(DisplayedDataViewModel), null);
    }
}
