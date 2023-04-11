using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Domain.Model;
using TravelService.Serializer;

namespace TravelService.Domain.RepositoryInterface
{
    public interface IReservationRequestRepository
    {
        public List<ReservationRequest> GetAll();

        public ReservationRequest Save(ReservationRequest reservationRequest);

        public int NextId();

        public void Delete(ReservationRequest reservationRequest);

        public ReservationRequest Update(ReservationRequest reservationRequest);

        public List<ReservationRequest> FindRequestsByGuestId(int guestId);
    }
}
