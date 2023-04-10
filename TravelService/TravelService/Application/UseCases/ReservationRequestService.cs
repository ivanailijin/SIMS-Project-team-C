using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Application.ServiceInterface;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.Repository;

namespace TravelService.Application.UseCases
{
    public class ReservationRequestService: IReservationRequestService
    {
        private readonly IReservationRequestRepository _reservationRequestRepository;
        
        public AccommodationReservationService _reservationService;

        public Guest1Service _guestService;

        public ReservationRequestService(IReservationRequestRepository reservationRequestRepository)
        {
            _reservationRequestRepository = reservationRequestRepository;
            _reservationService = new AccommodationReservationService(new AccommodationReservationRepository());
            _guestService = new Guest1Service(new Guest1Repository());
        }
        public void Delete(ReservationRequest reservationRequest)
        {
            _reservationRequestRepository.Delete(reservationRequest);
        }

        public List<ReservationRequest> GetAll()
        {
            return _reservationRequestRepository.GetAll();
        }
        public List<ReservationRequest> GetAllUnsolvedRequests()
        {
            List<ReservationRequest> unsolvedRequests = new List<ReservationRequest>();
            List<ReservationRequest> allRequests = GetAll();

            foreach(ReservationRequest request in allRequests)
            {
                if(request.Status == STATUS.OnHold)
                {
                    unsolvedRequests.Add(request);
                }
            }

            return unsolvedRequests;
        }

        public ReservationRequest Save(ReservationRequest reservationRequest)
        {
            ReservationRequest savedReservationRequest = _reservationRequestRepository.Save(reservationRequest);
            return savedReservationRequest;
        }
        public List<ReservationRequest> GetReservationData(List<ReservationRequest> reservationRequests)
        {
            foreach(ReservationRequest reservationRequest in reservationRequests)
            {
                AccommodationReservation reservation = _reservationService.FindById(reservationRequest.ReservationId);
                reservationRequest.Reservation = reservation;
            }

            return reservationRequests;
        }

        public List<ReservationRequest> GetGuestData(List<ReservationRequest> reservationRequests)
        {
            foreach (ReservationRequest reservationRequest in reservationRequests)
            {
                Guest1 guest = _guestService.FindById(reservationRequest.GuestId);
                reservationRequest.Guest = guest;
            }

            return reservationRequests;
        }

        public List<ReservationRequest> GetAvailabilities(List<ReservationRequest> reservationRequests)
        {
            foreach(ReservationRequest request in reservationRequests)
            {
                AVAILABILITY availability = _reservationService.CheckAvailability(request.ReservationId, request.NewStartDate, request.NewEndDate);
                request.Availability = availability;
            }

            return reservationRequests;
        }
        public void Update(ReservationRequest reservationRequest)
        {
            _reservationRequestRepository.Update(reservationRequest);
        }
    }
}
