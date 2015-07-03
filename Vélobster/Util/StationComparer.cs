using System.Collections.Generic;
using Vélobster.Model;

namespace Vélobster.Util
{
    public class StationComparer : EqualityComparer<Station>
    {
        public override bool Equals(Station x, Station y)
        {
            return x.Number == y.Number;
        }

        public override int GetHashCode(Station x)
        {
            return x.GetHashCode();
        }
    }
}
