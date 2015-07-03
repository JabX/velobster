using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.UI.Xaml.Controls.Maps;
using static System.Math;

namespace Vélobster.Util
{
    public static class MapUtils
    {
        public static Geopoint NotreDame = new Geopoint(new BasicGeoposition()
        {
            Latitude = 48.852966,
            Longitude = 2.349902
        });

        public static double Distance(BasicGeoposition pos1, BasicGeoposition pos2)
        {
            var R = 6371d;
            var dLat = toRadian(pos2.Latitude - pos1.Latitude);
            var dLon = toRadian(pos2.Longitude - pos1.Longitude);
            var a = Sin(dLat / 2) * Sin(dLat / 2) +
                Cos(toRadian(pos1.Latitude)) * Cos(toRadian(pos2.Latitude)) *
                Sin(dLon / 2) * Sin(dLon / 2);
            var c = 2 * Asin(Min(1, Sqrt(a)));
            return R * c;
        }

        private static double toRadian(double val)
        {
            return (PI / 180) * val;
        }

        public static GeoboundingBox GetBounds(this MapControl map, double extraPercentage)
        {
            Geopoint topLeft = null;

            try
            {
                map.GetLocationFromOffset(new Point(0, 0), out topLeft);
            }
            catch
            {
                var topOfMap = new Geopoint(new BasicGeoposition()
                {
                    Latitude = 85,
                    Longitude = 0
                });

                Point topPoint;
                map.GetOffsetFromLocation(topOfMap, out topPoint);
                map.GetLocationFromOffset(new Point(0, topPoint.Y), out topLeft);
            }

            Geopoint bottomRight = null;
            try
            {
                map.GetLocationFromOffset(new Point(map.ActualWidth, map.ActualHeight), out bottomRight);
            }
            catch
            {
                var bottomOfMap = new Geopoint(new BasicGeoposition()
                {
                    Latitude = -85,
                    Longitude = 0
                });

                Point bottomPoint;
                map.GetOffsetFromLocation(bottomOfMap, out bottomPoint);
                map.GetLocationFromOffset(new Point(0, bottomPoint.Y), out bottomRight);
            }

            if (topLeft != null && bottomRight != null)
            {
                var width = Abs(bottomRight.Position.Longitude - topLeft.Position.Longitude);
                var height = Abs(bottomRight.Position.Latitude - topLeft.Position.Latitude);
                var adjustedTopLeft = new BasicGeoposition()
                {
                    Latitude = topLeft.Position.Latitude + extraPercentage * height,
                    Longitude = topLeft.Position.Longitude - extraPercentage * width,
                };
                var adjustedBottomRight = new BasicGeoposition()
                {
                    Latitude = bottomRight.Position.Latitude - extraPercentage * height,
                    Longitude = bottomRight.Position.Longitude + extraPercentage * width,
                };
                return new GeoboundingBox(adjustedTopLeft, adjustedBottomRight);
            }

            return null;
        }

        public static bool Contains(this GeoboundingBox boundingBox, BasicGeoposition point)
        {
            if (
                boundingBox.NorthwestCorner.Latitude >= point.Latitude
             && boundingBox.SoutheastCorner.Latitude <= point.Latitude
             && boundingBox.NorthwestCorner.Longitude <= point.Longitude
             && boundingBox.SoutheastCorner.Longitude >= point.Longitude
             )
                return true;
            else
                return false;
        }
    }
}
