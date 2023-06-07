using System;
using System.Collections.Generic;
using System.Linq;
using TravelService.Applications.Utils;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.Repository;

namespace TravelService.Applications.UseCases
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
        public List<AccommodationReservation> GetReservationsByLocation(Location location)
        {
            List<AccommodationReservation> reservations = GetAll();
            GetAccommodationData(reservations);
            GetLocationData(reservations);
            List<AccommodationReservation> reservationsByLocation = new List<AccommodationReservation>();

            foreach(AccommodationReservation reservation in reservations)
            {
                if(reservation.Accommodation.Location.Country == location.Country && reservation.Accommodation.Location.City == location.City)
                {
                    reservationsByLocation.Add(reservation);
                }
            }
            return reservationsByLocation;
        }

        public double GetBusynessByLocation(Location location)
        {
            List<AccommodationReservation> reservations = GetReservationsByLocation(location);

            double busyness = 0;

            foreach(AccommodationReservation reservation in reservations)
            {
                for(int year = reservation.Accommodation.DateCreated.Year; year < DateTime.Today.Year; year++)
                {
                    busyness += GetBusynessPerYear(reservation.Accommodation, year);
                }
            }
            return busyness;
        }

        public List<AccommodationReservation> FindByGuestId(int guestId)
        {
            List<AccommodationReservation> foundReservations = new List<AccommodationReservation>();
            List<AccommodationReservation> reservations = _accommodationReservationRepository.GetAll();

            foreach (AccommodationReservation reservation in reservations)
            {
                if (reservation.GuestId == guestId && reservation.IsCancelled == false)
                {
                    foundReservations.Add(reservation);
                }
            }
            return foundReservations;
        }

        public List<AccommodationReservation> GetReservationsInNextYearByAccommmodation(int accommodationId, DateTime currentDate, DateTime endDate)
        {
            List<AccommodationReservation> accommodationReservations = GetAll();
            List<AccommodationReservation> reservationsInLastYear = new List<AccommodationReservation>();
            foreach (AccommodationReservation reservation in accommodationReservations)
            {
                if (reservation.IsCancelled == false && reservation.AccommodationId == accommodationId && reservation.CheckInDate >= currentDate && reservation.CheckOutDate <= endDate)
                {
                    reservationsInLastYear.Add(reservation);
                }
            }
            return reservationsInLastYear;
        }

        public List<AccommodationReservation> GetReservationsInLastYearByGuest(int guestId, DateTime startDate, DateTime endDate)
        {
            List<AccommodationReservation> accommodationReservations = GetAll();
            List<AccommodationReservation> reservationsInLastYear = new List<AccommodationReservation>();
            foreach (AccommodationReservation reservation in accommodationReservations)
            {
                if (reservation.IsCancelled == false && reservation.GuestId == guestId && reservation.CheckInDate >= startDate && reservation.CheckOutDate <= endDate)
                {
                    reservationsInLastYear.Add(reservation);
                }
            }
            return reservationsInLastYear;
        }

        public int GetReservationYearNumber(int year, int accommodationId)
        {
            List<AccommodationReservation> reservations = GetAll();
            int reservationsNumber = 0;

            foreach(AccommodationReservation reservation in reservations)
            {
                if(reservation.AccommodationId == accommodationId && (reservation.CheckInDate.Year == year))
                {
                    reservationsNumber++;
                }
            }

            return reservationsNumber;
        }
        public int GetCancelledReservationYearNumber(int year, int accommodationId)
        {
            List<AccommodationReservation> reservations = GetAll();
            int reservationsNumber = 0;

            foreach (AccommodationReservation reservation in reservations)
            {
                if (reservation.AccommodationId == accommodationId && reservation.CheckInDate.Year == year && reservation.IsCancelled == true)
                {
                    reservationsNumber++;
                }
            }

            return reservationsNumber;
        }

        public int GetReservationMonthNumber(int month, int year, int accommodationId)
        {
            List<AccommodationReservation> reservations = GetAll();
            int reservationsNumber = 0;

            foreach (AccommodationReservation reservation in reservations)
            {
                if (reservation.AccommodationId == accommodationId && (reservation.CheckInDate.Year == year || reservation.CheckOutDate.Year == year) && (reservation.CheckInDate.Month == month))
                {
                    reservationsNumber++;
                }
            }

            return reservationsNumber;
        }
        public int GetCancelledReservationMonthNumber(int month, int year, int accommodationId)
        {
            List<AccommodationReservation> reservations = GetAll();
            int reservationsNumber = 0;

            foreach (AccommodationReservation reservation in reservations)
            {
                if (reservation.AccommodationId == accommodationId && (reservation.CheckInDate.Year == year || reservation.CheckOutDate.Year == year) && (reservation.CheckInDate.Month == month) && reservation.IsCancelled == true)
                {
                    reservationsNumber++;
                }
            }

            return reservationsNumber;
        }
        public double GetBusynessPerYear(Accommodation accommodation, int year)
        {
            List<AccommodationReservation> reservations = GetAll();
            int daysCount = 0;
            int daysInYear = DateTime.IsLeapYear(year) ? 366 : 365;

            foreach (AccommodationReservation reservation in reservations)
            {
                if(reservation.AccommodationId == accommodation.Id && reservation.CheckInDate.Year == year && reservation.CheckOutDate.Year == year)
                {
                    daysCount += reservation.LengthOfStay;
                }
            }
            return (double)daysCount / daysInYear;
        }
        public double GetBusynessPerMonth(Accommodation accommodation, int year, int month)
        {
            List<AccommodationReservation> yearsReservations = GetReservationsInYear(accommodation, year);

            int daysCount = 0; 
            int daysInMonth = DateTime.DaysInMonth(year, month);

            foreach (AccommodationReservation reservation in yearsReservations)
            {
                if(reservation.CheckInDate.Month <= month && reservation.CheckOutDate.Month >= month)
                {
                    DateTime firstDate = new DateTime(year, month,1);
                    DateTime lastDate = new DateTime(year, month, daysInMonth);
                    if (reservation.CheckInDate > firstDate)
                        firstDate = reservation.CheckInDate;

                    if(reservation.CheckOutDate < lastDate)
                        lastDate = reservation.CheckOutDate;

                    TimeSpan reservedDays = lastDate - firstDate;
                    daysCount += reservedDays.Days + 1;
                }
            }
            return (double)daysCount / daysInMonth;
        }

        public int GetReservationsNumberByLocation(Location location)
        {
            List<AccommodationReservation> reservations = GetAll();
            reservations = GetAccommodationData(reservations);
            GetLocationData(reservations);

            int reservationsNumber = 0;

            foreach(AccommodationReservation reservation in reservations)
            {
                if(reservation.Accommodation.Location.Id == location.Id)
                {
                    reservationsNumber++;
                }
            }

            return reservationsNumber;
        }
        public List<AccommodationReservation> GetReservationsInYear(Accommodation accommodation, int year)
        {
            List<AccommodationReservation> yearsReservations = new List<AccommodationReservation>();
            List<AccommodationReservation> reservations = GetAll();

            foreach(AccommodationReservation reservation in reservations)
            {
                if(reservation.AccommodationId ==accommodation.Id && reservation.CheckInDate.Year == year && reservation.CheckOutDate.Year == year)
                    yearsReservations.Add(reservation);
            }

            return yearsReservations;
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
                return AVAILABILITY.Unavailable;
            }
            else
            {
                return AVAILABILITY.Available;
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
                reservation.Accommodation.Location = locations.Find(l => l.Id == reservation.Accommodation.LocationId);
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

        public Dictionary<string, int> CalculateReservationCountByMonth(Accommodation accommodation)
        {
            var currentDate = DateTime.Now;
            var endDate = currentDate.AddYears(1);
            var reservations = GetReservationsInNextYearByAccommmodation(accommodation.Id, currentDate, endDate);
            var reservationCountByMonth = new Dictionary<string, int>();

            while (currentDate < endDate)
            {
                var monthYear = currentDate.ToString("MMM/yyyy");
                var count = reservations.Count(r => r.CheckInDate.ToString("MMM/yyyy") == monthYear);
                reservationCountByMonth.Add(monthYear, count);

                currentDate = currentDate.AddMonths(1);
            }
            return reservationCountByMonth;
        }

        public Dictionary<string, int> CalculateReservationCountByMonthInPreviousYear(Guest1 guest)
        {
            var endDate = DateTime.Now;
            var startDate = endDate.AddYears(-1);
            var reservations = GetReservationsInLastYearByGuest(guest.Id, startDate, endDate);
            var reservationCountByMonth = new Dictionary<string, int>();

            while (startDate < endDate)
            {
                var monthYear = startDate.ToString("MMM/yyyy");
                var count = reservations.Count(r => r.CheckInDate.ToString("MMM/yyyy") == monthYear);
                reservationCountByMonth.Add(monthYear, count);

                startDate = startDate.AddMonths(1);
            }
            return reservationCountByMonth;
        }

        public bool HasGuestVisitedLocation(int guestId, int locationId)
        {
            List<AccommodationReservation> reservations = GetAll();

            bool hasReservation = reservations.Exists(r => r.GuestId == guestId && r.LocationId == locationId && r.CheckOutDate < DateTime.Today && !r.IsCancelled);
            return hasReservation;
        }

        public bool IsAccommodationAvailable(Accommodation selectedAccommodation, DateTime startDate, DateTime endDate, int daysOfStaying)
        {
            List<DateTime> reservedDates = FindReservedDates(selectedAccommodation);
            int daysCount = 0;

            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                if (!reservedDates.Contains(date))
                {
                    daysCount++;
                }
                else
                {
                    daysCount = 0;
                }

                if (daysCount >= daysOfStaying)
                {
                    return true;
                }
            }
            return false;
        }

        public List<Accommodation> FilterAvailableAccommodations(DateTime checkInDate, DateTime checkOutDate, int numDays, int guestNumber)
        {
            List<Accommodation> accommodations = _accommodationService.GetAll();
            accommodations = _accommodationService.GetLocationData(accommodations);
            accommodations = _accommodationService.GetOwnerData(accommodations);
            accommodations = _accommodationService.SortBySuperowner(accommodations);
            List<Accommodation> availableAccommodations = new List<Accommodation>();

            foreach (Accommodation accommodation in accommodations)
            {
                if (accommodation.MaxGuestNumber >= guestNumber)
                {
                    if (IsAccommodationAvailable(accommodation, checkInDate, checkOutDate, numDays))
                    {
                        availableAccommodations.Add(accommodation);
                    }
                }
            }
            return availableAccommodations;
        }
    }
}
