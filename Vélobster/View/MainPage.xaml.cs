using System;
using Vélobster.Provider;
using Vélobster.Util;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;

namespace Vélobster.View
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();

            mainMap.MapServiceToken = "McriANNF5FTINRW5ZTFc~dQWfJ3dVZv9P-Bc-CYkgOA~Au4RFUJBhI5ab7oDdjm_6UTWkf1_vSvkuJ_JxTJxdHGxKvzjN678jh_T_oWWtXBs";
            mainMap.Center = MapUtils.NotreDame;
            mainMap.ZoomLevel = Config.InitZoom;

            getLocation();
            loadApi();
        }

        async private void getLocation()
        {
            mapViewModel.Location = await LocationProvider.GetLocation();
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
            mapViewModel.BlockRefresh = false;
        }

        private void mainMap_ZoomOrCenterChanged(MapControl map, object args)
        {
            mapViewModel.RefreshDisplayedStations(map);
        }

        private void Location_Click(object sender, RoutedEventArgs e)
        {
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
