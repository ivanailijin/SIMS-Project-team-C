using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.Serializer;

namespace TravelService.Repository
{
    public class TourRepository : ITourRepository
    {
        private const string FilePath = "../../../Resources/Data/tour.csv";

        private readonly Serializer<Tour> _serializer;
        private readonly GuideRepository _guideRepository;
        private readonly GuestRepository _guestRepository;
        private readonly GuestVoucherRepository _guestVoucherRepository;
        private readonly CheckPointRepository _repositoryCheckPoint;
        private readonly LanguageRepository _languageRepository;

        private List<Tour> _tours;


        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                if (value != _username)
                {
                    _username = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TourRepository()
        {
            _serializer = new Serializer<Tour>();
            _tours = _serializer.FromCSV(FilePath);
            _guideRepository = new GuideRepository();
            _guestRepository = new GuestRepository();
            _guestVoucherRepository = new GuestVoucherRepository();
            _repositoryCheckPoint = new CheckPointRepository();
            _languageRepository = new LanguageRepository();
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
            Console.WriteLine("Izbrisana");
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

        public void FindFutureActive(Tour tour, List<Tour> FutureTours)
        {
            if (IsInFuture(tour))
            {
                AddFutureTours(tour, FutureTours);
            }
        }
        public void AddFutureTours(Tour tour, List<Tour> FutureTours)
        {
            FutureTours.Add(tour);
        }

        public bool IsInFuture(Tour tour)
        {
            DateTime currentDate = DateTime.Now.Date;
            if (tour.TourStart.Date > currentDate)
            {
                return true;
            }
            return false;
        }

        public void FindPastTurs(Tour tour, List<Tour> PastTours)
        {
            if (IsInPast(tour))
            {
                AddPastTours(tour, PastTours);
            }
        }
        public void AddPastTours(Tour tour, List<Tour> PastTours)
        {
            PastTours.Add(tour);
        }

        public bool IsInPast(Tour tour)
        {
            DateTime currentDate = DateTime.Now.Date;
            if (tour.TourStart.Date < currentDate)
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

        public void ShowTourList(List<Tour> Tours, List<Location> Locations, List<Language> Languages, List<CheckPoint> CheckPoints)
        {
            foreach (Tour tour in Tours)
            {
                tour.Location = Locations.Find(loc => loc.Id == tour.LocationId);
                tour.Language = Languages.Find(lan => lan.Id == tour.LanguageId);

                ShowCheckPointList(tour, CheckPoints);
            }
        }


        public List<Tour> ShowFutureTourList(List<Tour> Tours, List<Location> Locations, List<Language> Languages, List<CheckPoint> CheckPoints, List<Tour> FutureTours, int guideId, TourRepository _tourRepository)

        {
            foreach (Tour tour in Tours)
            {
                tour.Location = Locations.Find(loc => loc.Id == tour.LocationId);
                tour.Language = Languages.Find(lan => lan.Id == tour.LanguageId);


                ShowListCheckPointList(tour.Id, _tours, CheckPoints);
                FindFutureActive(tour, FutureTours);
            }

            return FutureTours;
        }

        public List<Tour> ShowPastTourList(List<Tour> tours, List<Location> locations, List<Language> languages, List<CheckPoint> checkPoints, List<Tour> pastTours, int guideId, TourRepository _tourRepository)
        {
            foreach (Tour tour in tours)
            {
                tour.Location = locations.Find(loc => loc.Id == tour.LocationId);
                tour.Language = languages.Find(lan => lan.Id == tour.LanguageId);

                ShowListCheckPointList(tour.Id, _tours, checkPoints);

                if (tour.Done)
                {
                    pastTours.Add(tour);
                    ShowListCheckPointList(tour.Id, _tours, checkPoints);

                    if (tour.Done)
                    {
                        pastTours.Add(tour);

                    }
                }
            }
            return pastTours;
        }

        public Tour FindById(int id)
        {
            _tours = _serializer.FromCSV(FilePath);
            foreach (Tour tours in _tours)
            {
                if (tours.Id == id)
                {
                    return tours;
                }
            }
            return null;
        }

        public List<Tour> FindGuidesTours(int guideId)
        {
            List<Tour> tours = new List<Tour>();
            foreach (Tour tour in _tours)
            {
                if (tour.GuideId == guideId)
                {
                    tours.Add(tour);
                }
            }
            return tours;
        }


        public bool CancelTour(int tourId)
        {
            Tour tour = FindById(tourId);
            if (tour == null) return false;
            TimeSpan timeDifference = tour.TourStart - DateTime.Now;
            if (timeDifference.TotalHours >= 48)
            {
                Delete(tour);


                return true;
            }

            return false;
        }

        public void SendVouchers(Tour tour)
        {

            List<Guest> guests = _guestRepository.FindByTourId(tour.Id);

            foreach (Guest guest in guests)
            {

                GuestVoucher newVoucher = new GuestVoucher("Vaucer", VOUCHERTYPE.QUIT, 200, "17f", false, guest.Id, tour.Id, DateTime.Now.AddYears(1));
                _guestVoucherRepository.Save(newVoucher);
                if (guest.VoucherList == null)
                {
                    guest.VoucherList = new List<GuestVoucher> { newVoucher };
                }
                else
                {
                    guest.VoucherList.Add(newVoucher);
                }

                _guestRepository.Save(guest);
                _guestRepository.Update(guest);

            }

        }

        public List<CheckPoint> ShowListCheckPointList(int TourId, List<Tour> Tours, List<CheckPoint> CheckPoints)
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

        public Tour GetTourByCheckPointId(int checkPointId)
        {
            var checkPoint = _repositoryCheckPoint.GetById(checkPointId);
            if (checkPoint != null)
            {
                var tour = GetAll().FirstOrDefault(t => t.CheckPoints.Contains(checkPoint));
                return tour;
            }
            return null;
        }

        public List<Tour> GetTourByLanguageId(int languageId)
        {
            var language = _languageRepository.GetById(languageId);
            if (language != null)
            {
                var tours = GetAll().Where(t => t.LanguageId == languageId).ToList();
                return tours;
            }
            return null;
        }




        public Tour GetMostVisitedTour(List<Tour> tours, List<Guest> guests, List<Location> locations, int year = 0)
        {
            int maxVisits = 0;
            Tour mostVisitedTour = null;

            foreach (Tour tour in tours)
            {
                if (tour.Done) // Check if the tour is done
                {
                    tour.Location = locations.Find(loc => loc.Id == tour.LocationId);
                    int visits = 0;
                    foreach (Guest guest in guests)
                    {
                        if (guest.TourId == tour.Id && (year == 0 || tour.TourStart.Year == year))
                        {
                            visits++;
                        }
                    }

                    if (visits >= maxVisits)
                    {
                        if (visits > maxVisits)
                        {
                            maxVisits = visits;
                            mostVisitedTour = tour;
                        }
                        else if (mostVisitedTour == null)
                        {
                            mostVisitedTour = tour;
                        }
                    }
                }
            }

            return mostVisitedTour;
        }


        public bool Otkaz(int guideId)
        {
            List<Tour> allTours = GetAll();
            List<Tour> toursToDelete = FindByGuideId(guideId,allTours);

            if (toursToDelete.Count == 0) return false;

            foreach (Tour tour in toursToDelete)
            {
                if (tour.TourStart > DateTime.Now)
                {
                    Delete(tour);
                }
            }

            return true;
        }
        private List<Tour> FindByGuideId(int guideId, List<Tour> allTours)
        {
            List<Tour> tours = new List<Tour>();

            foreach (Tour tour in allTours)
            {
                if (tour.GuideId == guideId)
                {
                    tours.Add(tour);
                }
            }

            return tours;
        }


        public void PoslatiVaucer()
        {
            List<Tour> tours = GetAll(); // Pretpostavljamo da postoji metoda koja vraća sve ture
            List<Guest> guests = _guestRepository.GetAll(); // Pretpostavljamo da postoji metoda koja vraća sve goste

            foreach (Tour tour in tours)
            {
                foreach (Guest guest in guests)
                {
                    if (guest.TourId == tour.Id)
                    {
                        GuestVoucher newVoucher = new GuestVoucher("Vaucer zbog otkaza", VOUCHERTYPE.QUIT, 200, "17f", false, guest.Id, -1, DateTime.Now.AddYears(2));
                        _guestVoucherRepository.Save(newVoucher);

                        if (guest.VoucherList == null)
                        {
                            guest.VoucherList = new List<GuestVoucher> { newVoucher };
                        }
                        else
                        {
                            guest.VoucherList.Add(newVoucher);
                            UpdateVouchersForAnyTour();
                        }

                        _guestRepository.Save(guest);
                        _guestRepository.Update(guest);
                    }
                }
            }
        }




        public void UpdateVouchersForAnyTour()
        {
            if (_guestRepository != null)
            {
                List<Guest> guests = _guestRepository.GetAllGuestsWithVouchers();

                foreach (Guest guest in guests)
                {
                    foreach (GuestVoucher voucher in guest.VoucherList)
                    {
                        voucher.TourId = -1;
                        _guestVoucherRepository.Update(voucher);
                    }

                    _guestRepository.Save(guest);
                    _guestRepository.Update(guest);
                }
            }
        }
    }
}