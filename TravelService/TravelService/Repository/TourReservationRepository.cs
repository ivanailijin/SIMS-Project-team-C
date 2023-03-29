using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using TravelService.Model;
using TravelService.Serializer;
using TravelService.View;
using System.Collections.ObjectModel;

namespace TravelService.Repository
{
    public class TourReservationRepository
    {
        private const string FilePath = "../../../Resources/Data/tourReservation.csv";

        private readonly Serializer<TourReservation> _serializer;

        private List<TourReservation> _reservation;
        public Tour SelectedTour { get; set; }

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
            _reservation.Insert(index, reservations);       
            _serializer.ToCSV(FilePath, _reservation);
            return reservations;
        }

        public void FindActiveTourList(Tour tour, List<Tour> ActiveTours)
        {
            if (IsInPorgress(tour))
            {
                AddActiveTours(tour, ActiveTours);
            }
        }
        public void AddActiveTours(Tour tour, List<Tour> ActiveTours)
        {
            ActiveTours.Add(tour);
        }
        public bool IsInPorgress(Tour tour)
        {
            DateTime currentDate = DateTime.Now.Date;
            if (tour.TourStart.Date == currentDate)
            {
                return true;
            }
            return false;
        }

        public void showAllActiveTours(ObservableCollection<Tour> Tours, List<Location> Locations, List<Language> Languages, List<CheckPoint> CheckPoints, List<Tour> ActiveTours)
        {

            foreach (Tour tour in Tours)
            {
                List<CheckPoint> ListCheckPoints = new List<CheckPoint>();
                tour.Location = Locations.Find(loc => loc.Id == tour.LocationId);
                tour.Language = Languages.Find(lan => lan.Id == tour.LanguageId);

                tour.CheckPoints.Clear();
                ListCheckPoints.Clear();

                int currentId = tour.Id;
                foreach (CheckPoint c in CheckPoints)
                {
                    int currentCheckPointTourId = c.TourId;
                    if ((currentCheckPointTourId == currentId))
                    {
                        ListCheckPoints.Add(c);

                    }
                }

                tour.CheckPoints.AddRange(ListCheckPoints);
                FindActiveTourList(tour, ActiveTours);
            }
        }
    }
}
