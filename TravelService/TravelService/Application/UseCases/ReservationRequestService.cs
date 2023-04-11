using System.Collections.Generic;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;

namespace TravelService.Application.UseCases
{
    public class ReservationRequestService
    {
        private readonly IReservationRequestRepository _reservationRequestRepository;

        public ReservationRequestService(IReservationRequestRepository reservationRequestRepository)
        {
            _reservationRequestRepository = reservationRequestRepository;
        }

        public List<ReservationRequest> GetAll()
        {
            return _reservationRequestRepository.GetAll();
        }

        public ReservationRequest Save(ReservationRequest reservationRequest)
        {
            return _reservationRequestRepository.Save(reservationRequest);
        }

        public void Delete(ReservationRequest reservationRequest)
        {
            _reservationRequestRepository.Delete(reservationRequest);
        }

        public ReservationRequest Update(ReservationRequest reservationRequest)
        {
            return _reservationRequestRepository.Update(reservationRequest);
        }

        public List<ReservationRequest> FindRequestsByGuestId(int guestId)
        {
            return _reservationRequestRepository.FindRequestsByGuestId(guestId);
        }

        public void SetStatus()
        {
            _reservationRequestRepository.SetStatus();
        }
    }
}
