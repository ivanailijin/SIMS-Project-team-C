using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Model;
using TravelService.Serializer;

namespace TravelService.Repository
{
    public class TourReservationRepository
    {
        private const string FilePath = "../../../Resources/Data/tourReservation.csv";

        private readonly Serializer<TourReservation> _serializer;

        private List<TourReservation> _reservation;

        public TourReservationRepository()
        {
            _serializer = new Serializer<TourReservation>();
            _reservation = _serializer.FromCSV(FilePath);
        }

        public List<TourReservation> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public TourReservation Save(TourReservation reservations)
        {
            reservations.Id = NextId();
            _reservation = _serializer.FromCSV(FilePath);
            _reservation.Add(reservations);
            _serializer.ToCSV(FilePath, _reservation);
            return reservations;
        }

        public int NextId()
        {
            _reservation = _serializer.FromCSV(FilePath);
            if (_reservation.Count < 1)
            {
                return 1;
            }
            return _reservation.Max(c => c.Id) + 1;
        }

        public void Delete(TourReservation reservations)
        {
            _reservation = _serializer.FromCSV(FilePath);
            TourReservation founded = _reservation.Find(c => c.Id == reservations.Id);
            _reservation.Remove(founded);
            _serializer.ToCSV(FilePath, _reservation);
        }

        public TourReservation Update(TourReservation reservations)
        {
            _reservation = _serializer.FromCSV(FilePath);
            TourReservation current = _reservation.Find(c => c.Id == reservations.Id);
            int index = _reservation.IndexOf(current);
            _reservation.Remove(current);
            _reservation.Insert(index, reservations);       // keep ascending order of ids in file 
            _serializer.ToCSV(FilePath, _reservation);
            return reservations;
        }


    }
}