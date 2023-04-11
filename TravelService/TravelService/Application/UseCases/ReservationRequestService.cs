using System.Collections.Generic;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.Serializer;
using TravelService.Application.Utils;
using System;

namespace TravelService.Application.UseCases
{
    public class ReservationRequestService
    {
        private readonly IReservationRequestRepository _reservationRequestRepository;
        private readonly AccommodationReservationService _reservationService;
        private readonly LocationService _locationService;
        private readonly AccommodationService _accommodationService;

        public ReservationRequestService(IReservationRequestRepository reservationRequestRepository)
        {
            _reservationRequestRepository = reservationRequestRepository;
            _reservationService = new AccommodationReservationService(Injector.CreateInstance<IAccommodationReservationRepository>());
            _locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            _accommodationService = new AccommodationService(Injector.CreateInstance<IAccommodationRepository>());
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

        public void GetReservationData(List<ReservationRequest> requests)
        {
            List<AccommodationReservation> reservations = _reservationService.GetAll();
            foreach (ReservationRequest request in requests)
            {
                request.Reservation = reservations.Find(r => r.Id == request.ReservationId);
            }
        }

        public void SetStatus(List<ReservationRequest> requests)
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
        }

        public void GetLocationData(List<ReservationRequest> requests)
        {
            List<Location> locations = _locationService.GetAll();
            foreach (ReservationRequest request in requests)
            {
                request.Reservation.Location = locations.Find(l => l.Id == request.Reservation.LocationId);
            }
        }

        public void GetAccommodationData(List<ReservationRequest> requests)
        {
            List<Accommodation> accommodations = _accommodationService.GetAll();
            foreach (ReservationRequest request in requests)
            {
                request.Reservation.Accommodation = accommodations.Find(a => a.Id == request.Reservation.AccommodationId);
            }
        }
    }
}
