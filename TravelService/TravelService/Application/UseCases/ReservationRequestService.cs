using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Application.Utils;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.Repository;
using TravelService.Serializer;

namespace TravelService.Application.UseCases
{
    public class ReservationRequestService
    {
        private readonly IReservationRequestRepository _reservationRequestRepository;
        private readonly AccommodationReservationService _reservationService;
        private readonly AccommodationService _accommodationService;
        private readonly Guest1Service _guestService;
        private readonly LocationService _locationService;

        public ReservationRequestService(IReservationRequestRepository reservationRequestRepository)
        {
            _reservationRequestRepository = reservationRequestRepository;
            _reservationService = new AccommodationReservationService(Injector.CreateInstance<IAccommodationReservationRepository>());
            _guestService = new Guest1Service(Injector.CreateInstance<IGuest1Repository>());
            _accommodationService = new AccommodationService(Injector.CreateInstance<IAccommodationRepository>());
            _locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
        }
        public void Delete(ReservationRequest reservationRequest)
        {
            _reservationRequestRepository.Delete(reservationRequest);
        }

        public List<ReservationRequest> GetAll()
        {
            return _reservationRequestRepository.GetAll();
        }
        public ReservationRequest Save(ReservationRequest reservationRequest)
        {
            return _reservationRequestRepository.Save(reservationRequest);
        }

        public ReservationRequest Update(ReservationRequest reservationRequest)
        {
            return _reservationRequestRepository.Update(reservationRequest);
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

        public List<ReservationRequest> GetReservationData(List<ReservationRequest> reservationRequests)
        {
            foreach(ReservationRequest reservationRequest in reservationRequests)
            {
                AccommodationReservation reservation = _reservationService.FindById(reservationRequest.ReservationId);
                reservationRequest.Reservation = reservation;
            }
            return reservationRequests;
        }

        public List<ReservationRequest> GetAccommodationData(List<ReservationRequest> reservationRequests)
        {
            List<Accommodation> accommodations = _accommodationService.GetAll();
            foreach (ReservationRequest reservationRequest in reservationRequests)
            {
                reservationRequest.Reservation.Accommodation = accommodations.Find(a => a.Id == reservationRequest.Reservation.AccommodationId);
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
            foreach (ReservationRequest request in reservationRequests)
            {
                AVAILABILITY availability = _reservationService.CheckAvailability(request.ReservationId, request.NewStartDate, request.NewEndDate);
                request.Availability = availability;
            }
            return reservationRequests;
        }
        public List<ReservationRequest> FindRequestsByGuestId(int guestId)
        {
            return _reservationRequestRepository.FindRequestsByGuestId(guestId);
        }

        /*   public void GetReservationData(List<ReservationRequest> requests)
           {
               List<AccommodationReservation> reservations = _reservationService.GetAll();
               foreach (ReservationRequest request in requests)
               {
                   request.Reservation = reservations.Find(r => r.Id == request.ReservationId);
               }
           }*/

        public List<ReservationRequest> SetStatus(List<ReservationRequest> requests)
        { 
            foreach (ReservationRequest request in requests)
            {
                if (request.Status == STATUS.OnHold)
                {
                    request.StatusText = "On hold";
                }
                else if (request.Status == STATUS.Approved)
                {
                    request.StatusText = "Approved";
                }
                else
                {
                    request.StatusText = "Rejected";
                }
            }
            return requests;
        }

        public List<ReservationRequest> GetLocationData(List<ReservationRequest> requests)
        {
            List<Location> locations = _locationService.GetAll();
            foreach (ReservationRequest request in requests)
            {
                request.Reservation.Location = locations.Find(l => l.Id == request.Reservation.LocationId);
            }
            return requests;
        }

       /* public void GetAccommodationData(List<ReservationRequest> requests)
        {
            List<Accommodation> accommodations = _accommodationService.GetAll();
            foreach (ReservationRequest request in requests)
            {
                request.Reservation.Accommodation = accommodations.Find(a => a.Id == request.Reservation.AccommodationId);
            }
        }*/
    }
}
