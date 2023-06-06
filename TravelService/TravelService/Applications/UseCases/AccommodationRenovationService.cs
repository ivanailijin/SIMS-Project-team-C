using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TravelService.Applications.Utils;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.Repository;

namespace TravelService.Applications.UseCases
{
    public class AccommodationRenovationService
    {
        private readonly IAccommodationRenovationRepository _accommodationRenovationRepository;

        private readonly AccommodationReservationService _accommodationReservationService;

        public AccommodationRenovationService(IAccommodationRenovationRepository repository)
        {
            _accommodationRenovationRepository = repository;
            _accommodationReservationService = new AccommodationReservationService(Injector.CreateInstance<IAccommodationReservationRepository>());
        }

        public List<AccommodationRenovation> GetAll()
        {
            return _accommodationRenovationRepository.GetAll();
        }

        public List<Accommodation> FillRenovationData(List<Accommodation> accommodations)
        {
            foreach (Accommodation accommodation in accommodations)
            {
                Tuple<DateTime, DateTime> lastRenovation = FindLastRenovation(accommodation);
                TimeSpan dayDifference = DateTime.Today - lastRenovation.Item2;
                if (dayDifference.Days <= 365)
                {
                    accommodation.RecentlyRenovated = true;
                }
                else
                {
                    accommodation.RecentlyRenovated = false;
                }
            }

            return accommodations;
        }
        public AccommodationRenovation Save(AccommodationRenovation accommodationRenovation)
        {
            return _accommodationRenovationRepository.Save(accommodationRenovation);
        }

        public void Delete(AccommodationRenovation accommodationRenovation)
        {
            _accommodationRenovationRepository.Delete(accommodationRenovation);
        }

        public AccommodationRenovation Update(AccommodationRenovation accommodationRenovation)
        {
            return _accommodationRenovationRepository.Update(accommodationRenovation);
        }

        public AccommodationRenovation FindById(int id)
        {
            return _accommodationRenovationRepository.FindById(id);
        }
        public List<AccommodationRenovation> GetLastRenovations()
        {
            List<AccommodationRenovation> renovations = _accommodationRenovationRepository.GetAll();

            List<AccommodationRenovation> lastRenovations = new List<AccommodationRenovation>();

            foreach(AccommodationRenovation renovation in renovations)
            {
                if(renovation.StartDate < DateTime.Today)
                    lastRenovations.Add(renovation);
            }

            return lastRenovations;
        }
        public List<AccommodationRenovation> GetFutureRenovations()
        {
            List<AccommodationRenovation> renovations = _accommodationRenovationRepository.GetAll();

            List<AccommodationRenovation> futureRenovations = new List<AccommodationRenovation>();

            foreach (AccommodationRenovation renovation in renovations)
            {
                if (renovation.StartDate > DateTime.Today)
                    futureRenovations.Add(renovation);
            }

            return futureRenovations;
        }

        public AccommodationRenovation GetLastRenovation(int accommodationId)
        {
            List<AccommodationRenovation> renovations = FindRenovationsByAccommodationId(accommodationId);

            AccommodationRenovation lastRenovation = renovations[0];

            foreach(AccommodationRenovation renovation in renovations)
            {
                if(renovation.EndDate > lastRenovation.EndDate)
                    lastRenovation = renovation;
            }

            return lastRenovation;
        }

        public List<Accommodation> GetRenovationData(List<Accommodation> accommodations)
        {
            foreach(Accommodation accommodation in accommodations)
            {
                AccommodationRenovation lastRenovation = GetLastRenovation(accommodation.Id);

                accommodation.LastRenovation = lastRenovation.EndDate;
            }

            return accommodations;
        }
        public List<AccommodationRenovation> FindRenovationsByAccommodationId(int id)
        {
            List<AccommodationRenovation> renovations = GetAll();
            List<AccommodationRenovation> accommodationRenovations = new List<AccommodationRenovation>();

            foreach(AccommodationRenovation renovation in renovations)
            {
                if (renovation.AccommodationId == id)
                    accommodationRenovations.Add(renovation);
            }

            return accommodationRenovations;
        }

        public Tuple<DateTime,DateTime> FindLastRenovation(Accommodation accommodation)
        {
            List<AccommodationRenovation> accommodationRenovations = FindRenovationsByAccommodationId(accommodation.Id);

            Tuple<DateTime, DateTime> lastRenovation = Tuple.Create(accommodationRenovations[0].StartDate, accommodationRenovations[0].EndDate);

            foreach(AccommodationRenovation renovation in accommodationRenovations)
            {
                if(renovation.EndDate >  lastRenovation.Item2)
                    lastRenovation = Tuple.Create(renovation.StartDate, renovation.EndDate);
            }

            return lastRenovation;
        }
        public List<Tuple<DateTime, DateTime>> FindAvailableDates(DateTime startDate, DateTime endDate, int duration, Accommodation selectedAccommodation)
        {
            List<DateTime> reservedDates = FindReservedDates(selectedAccommodation);
            List<DateTime> renovationDates = FindRenovationDates(selectedAccommodation);
            List<DateTime> availableDates = new List<DateTime>();
            List<Tuple<DateTime, DateTime>> availableDatesPair = new List<Tuple<DateTime, DateTime>>();

            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                CheckDateAvailability(availableDates, reservedDates, renovationDates, date);
                CheckDatePairExistence(availableDates, availableDatesPair, duration);
            }
            return availableDatesPair;
        }

        public void CheckDateAvailability(List<DateTime> availableDates, List<DateTime> reservedDates, List<DateTime> renovationDates, DateTime date)
        {
            if (!reservedDates.Contains(date) && !renovationDates.Contains(date))
            {
                availableDates.Add(date);
            }
            else
            {
                availableDates.Clear();
            }
        }

        public void CheckDatePairExistence(List<DateTime> availableDates, List<Tuple<DateTime, DateTime>> availableDatesPair, int duration)
        {
            if (availableDates.Count == duration)
            {
                availableDatesPair.Add(Tuple.Create(availableDates[0].Date, availableDates[availableDates.Count - 1].Date));
                availableDates.RemoveAt(0);
            }
        }

        public List<DateTime> FindRenovationDates(Accommodation selectedAccommodation)
        {
            List<AccommodationRenovation> renovations = GetAll();
            List<DateTime> renovationDates = new List<DateTime>();

            foreach (AccommodationRenovation renovation in renovations)
            {
                if (selectedAccommodation.Id == renovation.AccommodationId)
                {
                    DateTime startDate = renovation.StartDate;
                    DateTime endDate = renovation.EndDate;

                    for (DateTime currentDate = startDate; currentDate <= endDate; currentDate = currentDate.AddDays(1))
                    {
                        renovationDates.Add(currentDate);
                    }
                }
            }
            return renovationDates;
        }
        public List<DateTime> FindReservedDates(Accommodation selectedAccommodation)
        {
            List<AccommodationReservation> reservations = _accommodationReservationService.GetAll();
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
    }
}
