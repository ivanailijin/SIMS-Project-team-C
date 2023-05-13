using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Domain.Model;
using TravelService.Repository;
using TravelService.Serializer;

namespace TravelService.Domain.RepositoryInterface
{
    public interface IAccommodationReservationRepository
    {
        public List<AccommodationReservation> GetAll();
        public AccommodationReservation Save(AccommodationReservation accommodationReservation);
        public int NextId();
        public void Delete(AccommodationReservation accommodationReservation);
        public AccommodationReservation FindById(int id);
        public AccommodationReservation Update(AccommodationReservation accommodationReservation);
    }
}
