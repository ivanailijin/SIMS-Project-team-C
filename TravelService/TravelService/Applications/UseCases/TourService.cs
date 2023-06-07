using System.Collections.Generic;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.WPF.View;
using System.IO;
using TravelService.Repository;
using TravelService.Applications.Utils;

namespace TravelService.Applications.UseCases
{
    public class TourService
    {
        private readonly ICheckPointRepository _checkPointRepsitory;
        private readonly ITourRepository _tourRepository;
        private readonly IGuestRepository _guestRepository;
        private readonly IGuestVoucherRepository _guestVoucherRepository;
        private readonly GuideService guideService;

        public TourService(ITourRepository tourRepository)
        {
            _tourRepository = tourRepository;
            guideService = new GuideService(Injector.CreateInstance<IGuideRepository>());
        }
        public void Delete(Tour tour)
        {
            _tourRepository.Delete(tour);
        }

        public List<Tour> GetAll()
        {
            return _tourRepository.GetAll();
        }

        public Tour Save(Tour tour)
        {
            Tour savedTour = _tourRepository.Save(tour);
            return savedTour;
        }

        public void Update(Tour tour)
        {
            _tourRepository.Update(tour);
        }
        public List<Tour> GetGuideData(List<Tour> tours)
        {
            List<Guide> guides = guideService.GetAll();
            foreach (Tour tour in tours)
            {
                tour.Guide = guides.Find(g => g.Id == tour.GuideId);
            }
            return tours;
        }

        public List<Tour> ShowPastTourList(List<Tour> Tours, List<Location> Locations, List<Language> Languages, List<CheckPoint> CheckPoints, List<Guest> Guests, Guest2 guest2)
        {
            List<Tour> PastTours = new List<Tour>();
            foreach (Tour tour in Tours)
            {
                tour.Location = Locations.Find(loc => loc.Id == tour.LocationId);
                tour.Language = Languages.Find(lan => lan.Id == tour.LanguageId);

                ShowCheckPointList(tour.Id, Tours, CheckPoints);

                if (tour.Done)
                {
                    PastTours.Add(tour);
                }
            }
            return PastTours;
        }
        public List<Tour> SortBySuperGuide(List<Tour> tours)
        {
            return tours.OrderByDescending(a => a.Guide.SuperGuide).ToList();
        }

       

        public List<CheckPoint> ShowCheckPointList(int TourId, List<Tour> Tours, List<CheckPoint> CheckPoints)
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

        public List<Tour> ShowGuestTourList(List<Tour> Tours, List<Location> Locations, List<Language> Languages, List<CheckPoint> CheckPoints, List<Guest> guests,  Guest2 guest2)
        {
            List<Tour> pastTours = ShowPastTourList(Tours, Locations, Languages, CheckPoints, guests, guest2);
            List<Guest> guestList = FindGuestByUsername(guests, guest2);
            List<Tour> tourList = new List<Tour>();

            foreach (Tour pastTour in pastTours)
            {
                Guest currentGuest = guestList.Find(guest => guest.TourId == pastTour.Id);

                if (currentGuest != null && guestList.Contains(currentGuest))
                {
                    tourList.Add(pastTour);
                }
            }
            return tourList;
        }

        public List<Guest> FindGuestByUsername(List<Guest> guests, Guest2 guest2)
        {
            List<Guest> guestList = new List<Guest>();
            foreach (Guest guest in guests)
            {
                if (guest.Username == guest2.Username && guest.Attendence == true)
                {
                    guestList.Add(guest);
                }
            }
            return guestList;
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


                ShowListCheckPointList(tour.Id, Tours, CheckPoints);
                FindFutureActive(tour, FutureTours);
            }

            return FutureTours;
        }

        public List<Tour> ShowPastTour(List<Tour> tours, List<Location> locations, List<Language> languages, List<CheckPoint> checkPoints, List<Tour> pastTours, int guideId)
        {
            foreach (Tour tour in tours)
            {
                tour.Location = locations.Find(loc => loc.Id == tour.LocationId);
                tour.Language = languages.Find(lan => lan.Id == tour.LanguageId);

                ShowListCheckPointList(tour.Id, tours, checkPoints);

                FindGuidesTours(guideId);

                FindPastTurs(tour, pastTours);
                
            }
            return pastTours;
        }

      

        public List<Tour> FindGuidesTours(int guideId)
        {
            List<Tour> tours = new List<Tour>();
            foreach (Tour tour in tours)
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
            Tour tour = _tourRepository.FindById(tourId);
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
                GuestVoucher newVoucher = new GuestVoucher("Vaucer", VOUCHERTYPE.CANCELLATION, 200, "17f", false, guest.Id, tour.Id, DateTime.Now.AddYears(1));

                if (guest.VoucherList == null)
                {
                    guest.VoucherList = new List<GuestVoucher> { newVoucher };
                }
                else
                {
                    guest.VoucherList.Add(newVoucher);
                }

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
            var checkPoint = _checkPointRepsitory.GetById(checkPointId);
            if (checkPoint != null)
            {
                var tour = GetAll().FirstOrDefault(t => t.CheckPoints.Contains(checkPoint));
                return tour;
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


        public bool CancelFutureTours(int tourId)
        {
            Tour tour = _tourRepository.FindById(tourId);
            if (tour == null) return false;

            List<Tour> tours = _tourRepository.GetAll();

            foreach (Tour futureTour in tours)
            {
                if (futureTour.TourStart > tour.TourStart)
                {
                    Delete(futureTour);
                }
            }

            return true;
        }



        public void PoslatiVaucer(Tour tour, Guest guest)
        {
            GuestVoucher newVoucher = new GuestVoucher("Vaucer zbog otkaza", VOUCHERTYPE.QUIT, 200, "17f", false, guest.Id, -1, DateTime.Now.AddYears(2));
            // Samo dodajte novi vaučer u listu
            guest.VoucherList.Add(newVoucher);
            _guestRepository.Update(guest);
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

                    _guestRepository.Update(guest);
                }
            }
        }





    }
}

