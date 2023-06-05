using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Domain.Model;

namespace TravelService.WPF.ViewModel
{
    public class LocationEqualityComparer
    {
        public class MyClassEqualityComparer : IEqualityComparer<Location>
        {
            public bool Equals(Location x, Location y)
            {
                return x.Id == y.Id;
            }

            public int GetHashCode(Location obj)
            {
                return obj.Id.GetHashCode();
            }
        }
    }
}
