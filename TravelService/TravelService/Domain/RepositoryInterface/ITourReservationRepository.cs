using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TravelService.Domain.Model;
using System.Threading.Tasks;
using TravelService.Repository;


namespace TravelService.Domain.RepositoryInterface
{
    public interface ITourReservationRepository
    {
        public List<TourReservation> GetAll();

        public TourReservation Save(TourReservation tourReservation);

        public int NextId();

        public void Delete(TourReservation tourReservation);

        public TourReservation Update(TourReservation tourReservation);
    }
}