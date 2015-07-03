using System;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace Vélobster.Provider
{
    public static class LocationProvider
    {
        async public static Task<Geopoint> GetLocation()
        {
            var accessStatus = await Geolocator.RequestAccessAsync();

            if (accessStatus == GeolocationAccessStatus.Allowed)
            {
                var geoposition = new Geolocator();
                var position = await geoposition.GetGeopositionAsync();
                return position.Coordinate.Point;
                
            }
            else
                return null;
        }
    }
}
