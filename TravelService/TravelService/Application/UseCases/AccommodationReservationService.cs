using System;
using System.Collections.Generic;
using TravelService.Application.Utils;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.Repository;

namespace TravelService.Application.UseCases
{
    public class AccommodationReservationService
    {
        private readonly IAccommodationReservationRepository _accommodationReservationRepository;
        private readonly AccommodationService _accommodationService;
        private readonly LocationService _locationService;
        private OwnerService _ownerService;

        public AccommodationReservationService(IAccommodationReservationRepository accommodationReservationRepository)
        {
            _accommodationReservationRepository = accommodationReservationRepository;
            _accommodationService = new AccommodationService(Injector.CreateInstance<IAccommodationRepository>());
            _locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            _ownerService = new OwnerService(Injector.CreateInstance<IOwnerRepository>());
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
            List<AccommodationReservation> foundReservations = new List<AccommodationReservation>();
            List<AccommodationReservation> reservations = GetAll();

            foreach (AccommodationReservation reservation in reservations)
            {
                if (reservation.GuestId == guestId && reservation.IsCancelled == false)
                {
                    foundReservations.Add(reservation);
                }
            }
            return foundReservations;
        }

        public List<AccommodationReservation> GetReservationsInLastYear(Guest1 guest)
        {
            DateTime oneYearAgo = DateTime.Today.AddYears(-1);

            List<AccommodationReservation> guestReservations = FindByGuestId(guest.Id);
            List<AccommodationReservation> reservationsInLastYear = new List<AccommodationReservation>();
            foreach (AccommodationReservation reservation in guestReservations)
            {
                if (reservation.CheckInDate >= oneYearAgo && reservation.CheckInDate < DateTime.Today)
                {
                    reservationsInLastYear.Add(reservation);
                }
            }
            return reservationsInLastYear;
        }

        public List<Tuple<DateTime, DateTime>> FindAvailableDates(Accommodation selectedAccommodation, DateTime startDate, DateTime endDate, int daysOfStaying)
        {
            List<DateTime> reservedDates = FindReservedDates(selectedAccommodation);
            List<DateTime> availableDates = new List<DateTime>();
            List<Tuple<DateTime, DateTime>> availableDatesPair = new List<Tuple<DateTime, DateTime>>();

            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                if (!reservedDates.Contains(date))
                {
                    availableDates.Add(date);
                }
                else
                {
                    availableDates.Clear();
                }

                if (availableDates.Count == daysOfStaying)
                {
                    availableDatesPair.Add(Tuple.Create(availableDates[0].Date, availableDates[availableDates.Count - 1].Date));
                    availableDates.RemoveAt(0);
                }
            }
            return availableDatesPair;
        }

        public List<Tuple<DateTime, DateTime>> FindAvailableDatesOutsideRange(Accommodation selectedAccommodation, DateTime startDate, DateTime endDate, int daysOfStaying)
        {
            DateTime recommendedStartDate = startDate;
            DateTime recommendedEndDate = endDate;
            List<DateTime> reservedDates = FindReservedDates(selectedAccommodation);
            List<DateTime> availableDates = new List<DateTime>();
            List<Tuple<DateTime, DateTime>> availableDatesPair = new List<Tuple<DateTime, DateTime>>();

            while (!(availableDatesPair.Count >= 5))
            {
                recommendedStartDate = recommendedStartDate.Equals(DateTime.Today) ? recommendedStartDate : recommendedStartDate.AddDays(-1);
                recommendedEndDate = recommendedEndDate.AddDays(1);

                availableDates.Clear();
                for (DateTime date = recommendedStartDate; date <= recommendedEndDate; date = date.AddDays(1))
                {
                    if (!reservedDates.Contains(date))
                    {
                        availableDates.Add(date);
                    }
                    else
                    {
                        availableDates.Clear();
                    }

                    if (availableDates.Count == daysOfStaying)
                    {
                        if (!availableDatesPair.Contains(Tuple.Create(availableDates[0].Date, availableDates[availableDates.Count - 1].Date)))
                            availableDatesPair.Add(Tuple.Create(availableDates[0].Date, availableDates[availableDates.Count - 1].Date));
                        availableDates.RemoveAt(0);
                    }
                }
            }
            return availableDatesPair;
        }


        public List<DateTime> FindReservedDates(Accommodation selectedAccommodation)
        {
            List<AccommodationReservation> reservations = GetAll();
            List<DateTime> reservedDates = new List<DateTime>();

            foreach (AccommodationReservation reservation in reservations)
            {
                if (selectedAccommodation.Id == reservation.AccommodationId && reservation.IsCancelled == false)
                {
                    DateTime checkIn = reservation.CheckInDate;
                    DateTime checkOut = reservation.CheckOutDate;

                    for (DateTime currentDate = checkIn; currentDate <= checkOut; currentDate = currentDate.AddDays(1))
                    {
                        reservedDates.Add(currentDate);
                    }
                }
            }
            return reservedDates;
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
            if (dayDifference.TotalHours > 24)
            {
                return true;
            }
            return false;
        }

        public bool IsMinimumDaysForCancellingFulfilled(AccommodationReservation reservation)
        {
            Accommodation accommodation = _accommodationService.FindById(reservation.AccommodationId);
            TimeSpan daydifference = reservation.CheckInDate - DateTime.Now;
            if (daydifference.TotalDays > accommodation.DaysBeforeCancellingReservation)
            {
                return true;
            }
            return false;
        }

        public List<AccommodationReservation> FindReservationsByAccommodation(int accommodationId)
        {
            List<AccommodationReservation> filteredReservations = new List<AccommodationReservation>();
            List<AccommodationReservation> allReservations = GetAll();

            foreach (AccommodationReservation reservation in allReservations)
            {
                if (reservation.AccommodationId == accommodationId)
                {
                    filteredReservations.Add(reservation);
                }
            }
            return filteredReservations;
        }


        public bool CheckMatchingDates(AccommodationReservation checkReservation, DateTime newStartDate, DateTime newEndDate)
        {
            if (checkReservation.CheckOutDate < newStartDate || newEndDate < checkReservation.CheckInDate)
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
                return AVAILABILITY.Zauzet;
            }
            else
            {
                return AVAILABILITY.Slobodan;

            }
        }

        public List<AccommodationReservation> FindUnratedOwners(int guestId)
        {
            List<AccommodationReservation> reservations = GetAll();
            List<AccommodationReservation> UnratedOwners = new List<AccommodationReservation>();

            foreach (AccommodationReservation reservation in reservations)
            {
                TimeSpan dayDifference = DateTime.Today - reservation.CheckOutDate;
                if (!reservation.IsOwnerRated && dayDifference.Days < 5 && dayDifference.Days > 0 && reservation.GuestId == guestId)
                {
                    UnratedOwners.Add(reservation);
                }
            }

            return UnratedOwners;
        }

        public List<AccommodationReservation> GetAccommodationData(List<AccommodationReservation> reservations)
        {
            List<Accommodation> accommodations = _accommodationService.GetAll();
            foreach (AccommodationReservation reservation in reservations)
            {
                reservation.Accommodation = accommodations.Find(a => a.Id == reservation.AccommodationId);
            }

            return reservations;
        }
        public List<AccommodationReservation> GetLocationData(List<AccommodationReservation> reservations)
        {
            List<Location> locations = _locationService.GetAll();
            foreach (AccommodationReservation reservation in reservations)
            {
                reservation.Location = locations.Find(l => l.Id == reservation.LocationId);
            }
            return reservations;
        }

        public List<AccommodationReservation> GetOwnerData(List<AccommodationReservation> reservations)
        {
            List<Owner> owners = _ownerService.GetAll();
            foreach (AccommodationReservation reservation in reservations)
            {
                reservation.Owner = owners.Find(o => o.Id == reservation.OwnerId);
            }
            return reservations;
        }


        public List<AccommodationReservation> FindUnratedReservations(int OwnerId)
        {
            List<AccommodationReservation> UnratedReservations = new List<AccommodationReservation>();
            List<AccommodationReservation> accommodationReservations = GetAll();

            foreach (AccommodationReservation reservation in accommodationReservations)
            {
                Accommodation reservedAccommodation = _accommodationService.FindById(reservation.AccommodationId);
                TimeSpan dayDifference = DateTime.Today - reservation.CheckOutDate;
                if (!reservation.IsRated && dayDifference.Days < 5 && dayDifference.Days > 0 && reservedAccommodation.OwnerId == OwnerId)
                {
                    UnratedReservations.Add(reservation);
                }
            }

            return UnratedReservations;
        }

        public Dictionary<string, int> CalculateReservationsByMonth(List<AccommodationReservation> reservations)
        {
            var reservationsByMonth = new Dictionary<string, int>();

            foreach (var reservation in reservations)
            {
                string month = reservation.CheckInDate.ToString("MMM-yy");
                if (!reservationsByMonth.ContainsKey(month))
                {
                    reservationsByMonth[month] = 0;
                }
                reservationsByMonth[month]++;
            }
            return reservationsByMonth;
        }
    }
}
