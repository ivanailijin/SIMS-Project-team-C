using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.Repository;

namespace TravelService.Applications.UseCases
{
    public class LocationService
    {
        private readonly ILocationRepository _locationRepository;

        public LocationService(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public List<Location> GetAll()
        {
            return _locationRepository.GetAll();
        }

        public Location Save(Location location)
        {
            return _locationRepository.Save(location);
        }

        public void Delete(Location location)
        {
            _locationRepository.Delete(location);
        }

        public Location Update(Location location)
        {
            return _locationRepository.Update(location);
        }

        public Location GetLocationById(int locationId)
        {
            return _locationRepository.GetById(locationId);
        }
        public Location GetByCityAndCountry(string words)
        {
            return _locationRepository.GetByCityAndCountry(words);
        }
        public Location FindLocationId(string locationName)
        {
            List<Location> locations = GetAll();
            foreach(Location location in locations)
            {
                if(location.CityAndCountry.Replace(",", "").Replace(" ", "").Contains(locationName))
                {
                    return location;
                }
            }
            return null;
        }
    }
}
