using System;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.UI.Core;

namespace Vélobster.Provider
{
    public class LocationProvider 
    {
        private Func<CoreDispatcher> getDispatcher;
        private Action<Geopoint> setLocation;

        public LocationProvider(Func<CoreDispatcher> getDispatcher, Action<Geopoint> setLocation) 
        {
            this.getDispatcher = getDispatcher;
            this.setLocation = setLocation;
        }

        async public Task GetLocation() 
        {
            var accessStatus = await Geolocator.RequestAccessAsync();

            if (accessStatus == GeolocationAccessStatus.Allowed) 
            {
                var geoposition = new Geolocator { MovementThreshold = 5 };
                var position = await geoposition.GetGeopositionAsync();
                setLocation(position.Coordinate.Point);
                geoposition.PositionChanged += OnPositionChanged;
            }
        }
        
        async private void OnPositionChanged(Geolocator sender, PositionChangedEventArgs e) 
        {
            await getDispatcher().RunAsync(CoreDispatcherPriority.Normal, () => 
            {
                setLocation(e.Position.Coordinate.Point);
            });
        }
    }
}
