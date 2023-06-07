using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.Repository;

namespace TravelService.Applications.UseCases
{
    public class TourReservationService
    {
        private readonly ITourReservationRepository _tourReservationRepository;

        public TourReservationService(ITourReservationRepository tourReservationRepository)
        {
            _tourReservationRepository = tourReservationRepository;
        }
        public void Delete(TourReservation tourReservation)
        {
            _tourReservationRepository.Delete(tourReservation);
        }

        public List<TourReservation> GetAll()
        {
            return _tourReservationRepository.GetAll();
        }

        public TourReservation Save(TourReservation tourReservation)
        {
            TourReservation savedTourReservation = _tourReservationRepository.Save(tourReservation);
            return savedTourReservation;
        }

        public void Update(TourReservation tourReservation)
        {
            _tourReservationRepository.Update(tourReservation);
        }
        public List<Tour> showGuestsTours(List<Tour> Tours, List<Location> Locations, List<Language> Languages, List<CheckPoint> CheckPoints, List<TourReservation> TourReservations, Guest2 guest2)
        {
            List<Tour> ActiveTours = new List<Tour>();
            List<TourReservation> guestReservations = getGuestsReservations(Tours, Locations, Languages, CheckPoints, ActiveTours, TourReservations, guest2);
            foreach (Tour tour in Tours)
            {
                TourReservation currentReservation = guestReservations.Find(tourReservation => tourReservation.TourId == tour.Id);
                if (currentReservation != null)
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
            return ActiveTours;
        }
        public List<TourReservation> getGuestsReservations(List<Tour> Tours, List<Location> Locations, List<Language> Languages, List<CheckPoint> CheckPoints, List<Tour> ActiveTours, List<TourReservation> TourReservations, Guest2 guest2)
        {
            List<TourReservation> guestReservations = new List<TourReservation>();
            foreach (TourReservation tourReservation in TourReservations)
            {
                Tour currentTour = null;
                if (tourReservation.GuestId == guest2.Id)
                {
                    guestReservations.Add(tourReservation);
                }
            }
            return guestReservations;
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
    }
}
