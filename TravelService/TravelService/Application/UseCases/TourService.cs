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


namespace TravelService.Application.UseCases
{
    public class TourService
    {
        private readonly ITourRepository _tourRepository;

        public TourService(ITourRepository tourRepository)
        {
            _tourRepository = tourRepository;
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
    }
}

