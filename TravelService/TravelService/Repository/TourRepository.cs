using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Model;
using TravelService.Serializer;

namespace TravelService.Repository
{
    public class TourRepository
    {
        private const string FilePath = "../../../Resources/Data/tour.csv";

        private readonly Serializer<Tour> _serializer;

        private List<Tour> _tours;

        public TourRepository()
        {
            _serializer = new Serializer<Tour>();
            _tours = _serializer.FromCSV(FilePath);
        }

        public List<Tour> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public Tour Save(Tour tours)
        {
            tours.Id = NextId();
            _tours = _serializer.FromCSV(FilePath);
            _tours.Add(tours);
            _serializer.ToCSV(FilePath, _tours);
            return tours;
        }

        public int NextId()
        {
            _tours = _serializer.FromCSV(FilePath);
            if (_tours.Count < 1)
            {
                return 1;
            }
            return _tours.Max(c => c.Id) + 1;
        }

        public void Delete(Tour tours)
        {
            _tours = _serializer.FromCSV(FilePath);
            Tour founded = _tours.Find(c => c.Id == tours.Id);
            _tours.Remove(founded);
            _serializer.ToCSV(FilePath, _tours);
        }

        public Tour Update(Tour tours)
        {
            _tours = _serializer.FromCSV(FilePath);
            Tour current = _tours.Find(c => c.Id == tours.Id);
            int index = _tours.IndexOf(current);
            _tours.Remove(current);
            _tours.Insert(index, tours);
            _serializer.ToCSV(FilePath, _tours);
            return tours;
        }
        public void Check(List<CheckPoint> checkPoints, Tour tour, int TourId)
        {
            int count = 0;
            foreach (var checkpoint in checkPoints)
            {
                if (checkpoint.TourId == TourId)
                {
                    count++;
                }
            }
            if (count >= 2)
            {
                Save(tour);
            }
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

        public List<Tour> showAllActiveTours(List<Tour> Tours, List<Location> Locations, List<Language> Languages, List<CheckPoint> CheckPoints, List<Tour> ActiveTours)
        {

            foreach (Tour tour in Tours)
            {
                
                tour.Location = Locations.Find(loc => loc.Id == tour.LocationId);
                tour.Language = Languages.Find(lan => lan.Id == tour.LanguageId);

                ShowListCheckPointList(tour.Id, Tours, CheckPoints);
                FindActiveTourList(tour, ActiveTours);
            }
            return ActiveTours;
        }

        public List<CheckPoint> ShowListCheckPointList(int TourId, List<Tour> Tours,List<CheckPoint> CheckPoints)
        {
            List<CheckPoint> ListCheckPoints = new List<CheckPoint>();
            foreach (Tour tour in Tours)
            {
                if (tour.Id == TourId)
                {

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
                }

                tour.CheckPoints.AddRange(ListCheckPoints);

            }
            return ListCheckPoints;
        }

        public void ShowTourList(List<Tour> Tours, List<Location> Locations, List<Language> Languages, List<CheckPoint> CheckPoints)
        {
            foreach (Tour tour in Tours)
            {
                tour.Location = Locations.Find(loc => loc.Id == tour.LocationId);
                tour.Language = Languages.Find(lan => lan.Id == tour.LanguageId);

                ShowCheckPointList(tour, CheckPoints);
            }
        }

        private void ShowCheckPointList(Tour tour, List<CheckPoint> CheckPoints)
        {
            List<CheckPoint> ListCheckPoints = new List<CheckPoint>();
            tour.CheckPoints.Clear();
            ListCheckPoints.Clear();

            foreach (CheckPoint c in CheckPoints)
            {
                if (TourIdMatched(c.TourId, tour.Id))
                    ListCheckPoints.Add(c);
            }
            tour.CheckPoints.AddRange(ListCheckPoints);
        }

        public bool TourIdMatched(int checkPointTourId, int tourId)
        {
            if ((checkPointTourId == tourId))
            {
                return true;
            }
            return false;
        }
        public bool isTourSearchable(Tour tour, string inputLocation, string inputDuration, string inputLanguage, string inputGuestNumber)
        {
            if (((tour.Location.CityAndCountry.Replace(",", "").Replace(" ", "")).Contains(inputLocation) || string.IsNullOrEmpty(inputLocation)) &&
                (tour.Language.Name.Contains(inputLanguage) || string.IsNullOrEmpty(inputLanguage)) &&
                (isDurationCorrect(tour, inputDuration) || string.IsNullOrEmpty(inputDuration)) &&
                (IsGuestNumberLessThanMax(tour, inputGuestNumber) || string.IsNullOrEmpty(inputGuestNumber))
                )
            {
                return true;
            }
            return false;
        }
        private bool isDurationCorrect(Tour tour, string inputDuration)
        {
            if (int.TryParse(inputDuration, out int duration) && duration == tour.Duration)
            {
                return true;
            }
            return false;
        }
        private bool IsGuestNumberLessThanMax(Tour tour, string inputGuestNumber)
        {
            if (int.TryParse(inputGuestNumber, out int GuestNumber) && GuestNumber <= tour.MaxGuestNumber)
            {
                return true;
            }
            return false;
        }
    }
}
