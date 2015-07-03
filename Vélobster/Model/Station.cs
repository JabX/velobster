using System;
using Windows.Devices.Geolocation;

namespace Vélobster.Model
{
    public class Station
    {
        public int Number { get; private set; }
        public string Name { get; private set; }
        public string Address { get; private set; }
        public Geopoint Position { get; private set; }
        public bool Banking { get; private set; }
        public bool Bonus { get; private set; }
        public StationStatus Status { get; private set; }
        public int TotalStands { get; private set; }
        public double AvailableBikes { get; private set; }
        public double AvailableStands { get; private set; }
        public DateTime LastUpdate { get; private set; }

        public Station(int number, string name, string address, double latitude, double longitude, bool banking, bool bonus, string status, int totalStands, int availableBikes, int availableStands, long lastUpdate)
        {
            Number = number;
            Name = name;
            Address = address;
            Position = new Geopoint(new BasicGeoposition() { Latitude = latitude, Longitude = longitude });
            Banking = banking;
            Bonus = bonus;
            Status = status == "OPEN" ? StationStatus.Open : StationStatus.Closed;
            TotalStands = totalStands;
            AvailableBikes = availableBikes;
            AvailableStands = availableStands;
            LastUpdate = new DateTime(lastUpdate);
        }

        public override int GetHashCode()
        {
            return Number;
        }
    }
}
