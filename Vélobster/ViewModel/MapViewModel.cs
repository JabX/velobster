using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using Vélobster.Model;
using Vélobster.Util;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Controls.Maps;

namespace Vélobster.ViewModel
{
    class MapViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private Geopoint mapCenter = MapUtils.NotreDame;
        public Geopoint MapCenter {
            get { return mapCenter; }
            set { mapCenter = value; OnPropertyChanged(nameof(MapCenter)); }
        }

        private int mapZoom = 13;
        public int MapZoom
        {
            get { return mapZoom; }
            set { mapZoom = value; OnPropertyChanged(nameof(MapZoom)); }
        }

        private Geopoint location;
        public Geopoint Location
        {
            get { return location; }
            set { location = value; OnPropertyChanged(nameof(Location)); }
        }

        private HashSet<Station> stationSet = new HashSet<Station>();

        private ObservableList<Station> stations = new ObservableList<Station>();
        public ObservableList<Station> Stations
        {
            get { return stations; }
            set { stations = value; OnPropertyChanged(nameof(Stations)); }
        }

        private StationData displayedData = StationData.Bikes;
        public StationData DisplayedData
        {
            get { return displayedData; }
            set { displayedData = value; OnPropertyChanged(nameof(DisplayedData)); }
        }

        public AllStationList AllStations { get; set; }

        public bool BlockRefresh = true;

        Timer timer;
        public void RefreshDisplayedStations(MapControl map)
        {
            if (map.ZoomLevel >= Config.MinZoom && AllStations != null)
            {
                if (!BlockRefresh)
                {
                    BlockRefresh = true;
                
                    var mapBounds = map.GetBounds(0.1);
                    var minLatIndex = getTrueIndex(AllStations.Latitudes.BinarySearch(mapBounds.SoutheastCorner.Latitude));
                    var maxLatIndex = getTrueIndex(AllStations.Latitudes.BinarySearch(mapBounds.NorthwestCorner.Latitude));
                    var minLonIndex = getTrueIndex(AllStations.Longitudes.BinarySearch(mapBounds.NorthwestCorner.Longitude));
                    var maxLonIndex = getTrueIndex(AllStations.Longitudes.BinarySearch(mapBounds.SoutheastCorner.Longitude));

                    var comparer = new StationComparer();
                    var nearbyStations = new HashSet<Station>(comparer);
                    var stationsWithLon = new HashSet<Station>(comparer);
                    for (int i = minLatIndex; i < maxLatIndex; i++)
                        nearbyStations.Add(AllStations.StationsByLatitude[i]);
                    for (int i = minLonIndex; i < maxLonIndex; i++)
                        stationsWithLon.Add(AllStations.StationsByLongitude[i]);

                    nearbyStations.IntersectWith(stationsWithLon);

                    var interStations = new HashSet<Station>(nearbyStations, comparer);
                    interStations.IntersectWith(stationSet);

                    var stationsToRemove = new HashSet<Station>(stationSet, comparer);
                    stationsToRemove.ExceptWith(interStations);

                    var stationsToAdd = new HashSet<Station>(nearbyStations, comparer);
                    stationsToAdd.ExceptWith(interStations);

                    foreach (var station in stationsToRemove)
                        Stations.Remove(station);
                    Stations.AddRange(stationsToAdd);

                    stationSet = nearbyStations;
                }
                TimerCallback tcb = TimerHandler;
                timer = new Timer(tcb, null, 100, Timeout.Infinite);
            }
            else
            {
                stations.Clear();
                stationSet.Clear();
            } 
        }

        public void TimerHandler(object sender)
        {
            BlockRefresh = false;
            timer.Dispose();
        }

        private int getTrueIndex(int index)
        {
            return index >= 0 ? index : ~index;
        }

        public void SwitchDisplayedData()
        {
            DisplayedData = displayedData == StationData.Bikes ? StationData.Stands : StationData.Bikes;
        }
    }
}
