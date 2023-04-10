using System;
using System.Collections.Generic;
using System.Linq;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.Repository;

namespace TravelService.Application.UseCases
{
    public class AccommodationReservationService
    {
        private readonly IAccommodationReservationRepository _accommodationReservationRepository;

        public AccommodationService _accommodationService;

        public AccommodationReservationService(IAccommodationReservationRepository accommodationReservationRepository)
        {
            _accommodationReservationRepository = accommodationReservationRepository;
            _accommodationService = new AccommodationService(new AccommodationRepository());
        }
        public List<AccommodationReservation> GetAll()
        {
            return _accommodationReservationRepository.GetAll();
        }

        public AccommodationReservation Save(AccommodationReservation accommodationReservation)
        {
            AccommodationReservation savedAccommodationReservation = _accommodationReservationRepository.Save(accommodationReservation);
            return savedAccommodationReservation;
        }

        public void Delete(AccommodationReservation accommodationReservation)
        {
            _accommodationReservationRepository.Delete(accommodationReservation);
        }

        public AccommodationReservation FindById(int id)
        {
            AccommodationReservation accommodationReservation = _accommodationReservationRepository.FindById(id);
            return accommodationReservation;
        }
        public AccommodationReservation Update(AccommodationReservation accommodationReservation)
        {
            AccommodationReservation updatedAccommodationReservation = _accommodationReservationRepository.Update(accommodationReservation);
            return updatedAccommodationReservation;
        }
        List<AccommodationReservation> FindReservationsByAccommodation(int accommodationId)
        {
            List<AccommodationReservation> filteredReservations = new List<AccommodationReservation>();
            List<AccommodationReservation> allReservations = GetAll();

            foreach(AccommodationReservation reservation in allReservations)
            {
                if(reservation.AccommodationId == accommodationId)
                {
                    filteredReservations.Add(reservation);
                }
            }
            return filteredReservations;
        }
        public bool CheckMatchingDates(AccommodationReservation checkReservation, DateTime newStartDate, DateTime newEndDate)
        {
            if(checkReservation.CheckOutDate < newStartDate || newEndDate < checkReservation.CheckInDate)
            {
                return false;
            }
            return true;
        }
        public AVAILABILITY CheckAvailability(int reservationId, DateTime newStartDate, DateTime newEndDate)
        {
            AccommodationReservation reservation = FindById(reservationId);
            Accommodation accommodation = _accommodationService.FindById(reservation.AccommodationId);

            List<AccommodationReservation> reservationsForCheck = FindReservationsByAccommodation(accommodation.Id);
            reservationsForCheck.Remove(reservation);
            bool hasOverlap = false;

            foreach (AccommodationReservation checkReservation in reservationsForCheck)
            {
                bool matchingDates = CheckMatchingDates(checkReservation, newStartDate, newEndDate);
                if (matchingDates)
                {
                    hasOverlap = true;
                    break;
                }
            }

            if (hasOverlap)
            {
                return AVAILABILITY.Unavailable;
            }
            else
            {
                return AVAILABILITY.Available;
            }
        }
    }
}
