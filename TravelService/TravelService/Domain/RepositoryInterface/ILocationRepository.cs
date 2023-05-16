using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Domain.Model;

namespace TravelService.Domain.RepositoryInterface
{
    public interface ILocationRepository
    {
        public List<Location> GetAll();
        public Location Save(Location location);
        public int NextId();
        public void Delete(Location location);
        public Location Update(Location location);
        public Location GetById(int id);    
        public Location GetByCityAndCountry(string words);
    }
}
