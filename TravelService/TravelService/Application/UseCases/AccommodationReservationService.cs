using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.Application.Utils;
using TravelService.Repository;

namespace TravelService.Application.UseCases
{
    public class AccommodationReservationService
    {
        private readonly IAccommodationReservationRepository _accommodationReservationRepository;
        private readonly AccommodationService _accommodationService;
        private readonly LocationService _locationService;
        public AccommodationReservationService(IAccommodationReservationRepository accommodationReservationRepository)
        {
            _accommodationReservationRepository = accommodationReservationRepository;
            _accommodationService = new AccommodationService(Injector.CreateInstance<IAccommodationRepository>());
            _locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
        }

        public List<AccommodationReservation> GetAll()
        {
            return _accommodationReservationRepository.GetAll();
        }

        public AccommodationReservation Save(AccommodationReservation accommodationReservation)
        {
            return _accommodationReservationRepository.Save(accommodationReservation);
        }

        public void Delete(AccommodationReservation accommodationReservation)
        {
            _accommodationReservationRepository.Delete(accommodationReservation);
        }

        public AccommodationReservation Update(AccommodationReservation accommodationReservation)
        {
            return _accommodationReservationRepository.Update(accommodationReservation);
        }

        public AccommodationReservation FindById(int id)
        {
            return _accommodationReservationRepository.FindById(id);
        }

        public List<AccommodationReservation> FindByGuestId(int guestId)
        {
            return _accommodationReservationRepository.FindReservationsByGuestId(guestId);
        }

        public void SetLocation(List<Location> locations)
        {
            _accommodationReservationRepository.SetLocation(locations);
        }

        public void CancelReservation(AccommodationReservation selectedReservation)
        {
            AccommodationReservation reservation = _accommodationReservationRepository.FindById(selectedReservation.Id);
            reservation.IsCancelled = true;
            _accommodationReservationRepository.Update(reservation);
        }

        public bool IsCancellingLimitFulfilled(int id)
        {
            AccommodationReservation accommodationReservation = _accommodationReservationRepository.FindById(id);
            TimeSpan dayDifference = accommodationReservation.CheckInDate - DateTime.Now; 
            if(dayDifference.TotalHours > 24)
            {
                return true;
            }
            return false;
        }

        public bool IsMinimumDaysForCancellingFulfilled(AccommodationReservation reservation)
        {
            Accommodation accommodation = _accommodationService.FindById(reservation.AccommodationId);
            TimeSpan daydifference = reservation.CheckInDate - DateTime.Now;
            if(daydifference.TotalDays > accommodation.DaysBeforeCancellingReservation)
            {
                return true;
            }
            return false;
        }

        public void GetAccommodationData(List<AccommodationReservation> reservations)
        {
            List<Accommodation> accommodations = _accommodationService.GetAll();
            foreach (AccommodationReservation reservation in reservations)
            {
                reservation.Accommodation = accommodations.Find(a => a.Id == reservation.AccommodationId);
            }
        }

        public void GetLocationData(List<AccommodationReservation> reservations)
        {
            List<Location> locations = _locationService.GetAll();
            foreach (AccommodationReservation reservation in reservations)
            {
                reservation.Location = locations.Find(l => l.Id == reservation.LocationId);
            }
        }
    }
}
