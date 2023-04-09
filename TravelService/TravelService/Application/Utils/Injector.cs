using System;
using System.Collections.Generic;
using TravelService.Domain.RepositoryInterfaces;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Repository;
using TravelService.Domain.RepositoryInterface;

namespace TravelService.Application.Utils
{
    public class Injector
    {
        private static Dictionary<Type, object> _implementations = new Dictionary<Type, object>
        {
        { typeof(IReservationRequestRepository), new ReservationRequestRepository() },
        { typeof(ILocationRepository), new LocationRepository() },
        { typeof(IAccommodationReservationRepository), new AccommodationReservationRepository() },
        { typeof(IAccommodationRepository), new AccommodationRepository() },
        // Add more implementations here
    };

        public static T CreateInstance<T>()
        {
            Type type = typeof(T);

            if (_implementations.ContainsKey(type))
            {
                return (T)_implementations[type];
            }

            throw new ArgumentException($"No implementation found for type {type}");
        }
    }
}
