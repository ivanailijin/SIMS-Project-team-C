using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.Serializer;

namespace TravelService.Repository
{
    public class AccommodationReservationRepository : IAccommodationReservationRepository
    {
        private const string FilePath = "../../../Resources/Data/accommodationReservations.csv";

        private readonly Serializer<AccommodationReservation> _serializer;

        private List<AccommodationReservation> _accommodationReservations;
        private AccommodationRepository _accommodationRepository;

        public AccommodationReservationRepository()
        {
            _serializer = new Serializer<AccommodationReservation>();
            _accommodationReservations = _serializer.FromCSV(FilePath);
            _accommodationRepository = new AccommodationRepository();
        }

        public List<AccommodationReservation> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public AccommodationReservation Save(AccommodationReservation accommodationReservation)
        {
            accommodationReservation.Id = NextId();
            _accommodationReservations = _serializer.FromCSV(FilePath);
            _accommodationReservations.Add(accommodationReservation);
            _serializer.ToCSV(FilePath, _accommodationReservations);
            return accommodationReservation;
        }

        public int NextId()
        {
            _accommodationReservations = _serializer.FromCSV(FilePath);
            if (_accommodationReservations.Count < 1)
            {
                return 1;
            }
            return _accommodationReservations.Max(c => c.Id) + 1;
        }

        public void Delete(AccommodationReservation accommodationReservation)
        {
            _accommodationReservations = _serializer.FromCSV(FilePath);
            AccommodationReservation founded = _accommodationReservations.Find(c => c.Id == accommodationReservation.Id);
            _accommodationReservations.Remove(founded);
            _serializer.ToCSV(FilePath, _accommodationReservations);
        }

        public AccommodationReservation FindById(int id)
        {
            _accommodationReservations = _serializer.FromCSV(FilePath);
            foreach (AccommodationReservation reservation in _accommodationReservations)
            {
                if (reservation.Id == id)
                {
                    return reservation;
                }
            }
            return null;
        }

        public List<AccommodationReservation> FindUnratedReservations(AccommodationRepository _accommodationRepository, int OwnerId)
        {
            List<AccommodationReservation> UnratedReservations = new List<AccommodationReservation>();

            foreach (AccommodationReservation reservation in _accommodationReservations)
            {
                Accommodation reservedAccommodation = _accommodationRepository.FindById(reservation.AccommodationId);
                TimeSpan dayDifference = DateTime.Today - reservation.CheckOutDate;
                if (!reservation.IsRated && dayDifference.Days < 5 && dayDifference.Days > 0 && reservedAccommodation.OwnerId == OwnerId)
                {
                    UnratedReservations.Add(reservation);
                }
            }

            return UnratedReservations;
        }

        public List<AccommodationReservation> GetAccommodationData(List<AccommodationReservation> reservations)
        {
            List<Accommodation> accommodations = _accommodationRepository.GetAll();
            foreach(AccommodationReservation reservation in reservations)
            {
                reservation.Accommodation = accommodations.Find(a => a.Id == reservation.AccommodationId);
            }

            return reservations;
        }
        public AccommodationReservation Update(AccommodationReservation accommodationReservation)
        {
            _accommodationReservations = _serializer.FromCSV(FilePath);
            AccommodationReservation current = _accommodationReservations.Find(c => c.Id == accommodationReservation.Id);
            int index = _accommodationReservations.IndexOf(current);
            _accommodationReservations.Remove(current);
            _accommodationReservations.Insert(index, accommodationReservation);
            _serializer.ToCSV(FilePath, _accommodationReservations);
            return accommodationReservation;
        }

        public List<AccommodationReservation> FindReservationsByGuestId(int guestId)
        {
            List<AccommodationReservation> Reservations = new List<AccommodationReservation>();

            foreach (AccommodationReservation reservation in _accommodationReservations)
            {
                if (reservation.GuestId == guestId && reservation.IsCancelled == false)
                {
                    Reservations.Add(reservation);
                }
            }
            return Reservations;
        }
    }
}
