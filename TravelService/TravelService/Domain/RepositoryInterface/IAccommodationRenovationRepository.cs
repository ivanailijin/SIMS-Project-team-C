using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Domain.Model;

namespace TravelService.Domain.RepositoryInterface
{
    public interface IAccommodationRenovationRepository
    {
        public List<AccommodationRenovation> GetAll();
        public AccommodationRenovation Save(AccommodationRenovation accommodationRenovation);
        public int NextId();
        public void Delete(AccommodationRenovation accommodationRenovation);
        public AccommodationRenovation FindById(int id);
        public AccommodationRenovation Update(AccommodationRenovation accommodationRenovation);
    }
}
