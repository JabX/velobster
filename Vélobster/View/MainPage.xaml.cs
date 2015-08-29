using System;
using Vélobster.Provider;
using Vélobster.Util;
using Windows.Devices.Geolocation;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;

namespace Vélobster.View
{
    public sealed partial class MainPage : Page
    {
        private bool isLoaded = false;
        private bool isLocated = false;

        public MainPage()
        {
            InitializeComponent();

            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.BackgroundColor = Colors.Black;
            titleBar.ForegroundColor = Colors.White;
            titleBar.ButtonBackgroundColor = Colors.Black;
            titleBar.ButtonForegroundColor = Colors.White;
            titleBar.ButtonHoverBackgroundColor = Color.FromArgb(0xFF, 0x25, 0x25, 0x25);
            titleBar.ButtonHoverForegroundColor = Colors.White;
            titleBar.ButtonPressedBackgroundColor = Color.FromArgb(0xFF, 0x40, 0x40, 0x40);
            titleBar.ButtonPressedForegroundColor = Colors.White;

            mainMap.MapServiceToken = "McriANNF5FTINRW5ZTFc~dQWfJ3dVZv9P-Bc-CYkgOA~Au4RFUJBhI5ab7oDdjm_6UTWkf1_vSvkuJ_JxTJxdHGxKvzjN678jh_T_oWWtXBs";
            mainMap.Center = MapUtils.NotreDame;
            mainMap.ZoomLevel = Config.InitZoom;
            locationPin.Visibility = Visibility.Collapsed;

            getLocation();
            loadApi();
        }

        async private void getLocation()
        {
            mapViewModel.Location = await LocationProvider.GetLocation();
            isLocated = true;
            locationPin.Visibility = Visibility.Visible;

            if (mapViewModel.Location != null)
                setLocation(mapViewModel.Location);
        }

        async private void setLocation(Geopoint point)
        {
            await mainMap.TrySetViewAsync(point, Config.CenterZoom);
        }

        async private void loadApi()
        {
            mapViewModel.AllStations = await StationProvider.GetStationData();
            isLoaded = true;
            mapViewModel.RefreshDisplayedStations(mainMap);
        }

        private void mainMap_ZoomOrCenterChanged(MapControl map, object args)
        {
            if(isLoaded)
                mapViewModel.RefreshDisplayedStations(map);
        }

        private void Location_Click(object sender, RoutedEventArgs e)
        {
            if(isLocated)
                setLocation(mapViewModel.Location);
        }

        private void Switch_Click(object sender, RoutedEventArgs e)
        {
            mapViewModel.SwitchDisplayedData();
        }

        async private void ZoomIn_Click(object sender, RoutedEventArgs e)
        {
            await mainMap.TrySetViewAsync(mainMap.Center, Config.MinZoom);
        }
    }
}
