using System.Collections.Generic;
using System.Linq;

namespace Vélobster.Model
{
    public class AllStationList
    {
        public List<double> Latitudes { get; } = new List<double>();
        public List<double> Longitudes { get; } = new List<double>();
        public List<Station> StationsByLatitude { get; } = new List<Station>();
        public List<Station> StationsByLongitude { get; } = new List<Station>();

        public AllStationList(IEnumerable<Station> stations)
        {
            var stationsByLatitude = new SortedDictionary<double, Station>(stations.ToDictionary(x => x.Position.Position.Latitude, x => x));
            var stationsByLongitude = new SortedDictionary<double, Station>(stations.ToDictionary(x => x.Position.Position.Longitude, x => x));

            foreach (var station in stationsByLatitude)
            {
                Latitudes.Add(station.Key);
                StationsByLatitude.Add(station.Value);
            }

            foreach (var station in stationsByLongitude)
            {
                Longitudes.Add(station.Key);
                StationsByLongitude.Add(station.Value);
            }
        }
    }
}
