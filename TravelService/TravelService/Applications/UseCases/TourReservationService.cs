using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.Repository;
using TravelService.Serializer;
using TravelService.WPF.View;

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
        public int NextId()
        {
            if (_tourReservationRepository.GetAll().Count < 1)
            {
                return 1;
            }
            return _tourReservationRepository.GetAll().Max(c => c.Id) + 1;
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

        public List<Tour> showAllActiveTours(List<Tour> Tours, List<Location> Locations, List<Language> Languages, List<CheckPoint> CheckPoints, List<Tour> ActiveTours)
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
            return ActiveTours;
        }
        public List<Tour> showAllActiveToursNew(List<Tour> Tours, List<Location> Locations, List<Language> Languages, List<CheckPoint> CheckPoints)
        {

            List<Tour> ActiveTours = new List<Tour>();
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
            return ActiveTours;
        }

        public bool TryReserving(Tour selectedTour, string numberOfGuests, List<TourReservation> TourReservations, List<TourReservation> ReservationsByTour, List<Tour> OtherTours, TourReservationView tourReservationView, Guest2 guest2)
        {
            int number = int.Parse(numberOfGuests);
            if (number <= 0)
            {
                MessageBox.Show("Inserted number of people is not valid.");
                return false;
            }
            else
            {
                if (ReservationSuccess(selectedTour, numberOfGuests, TourReservations, ReservationsByTour, OtherTours, tourReservationView, guest2))
                {
                    MessageBox.Show("You have successfully booked a tour!");
                    return true;
                }
            }
            return false;
        }

        public bool ReservationSuccess(Tour selectedTour, string numberOfGuests, List<TourReservation> TourReservations, List<TourReservation> ReservationsByTour, List<Tour> OtherTours, TourReservationView tourReservationView, Guest2 guest2)
        {

            TourReservation lastReservation = FindLastCurrentReservation(selectedTour.Id, TourReservations, ReservationsByTour, guest2);

            if (TourReservations.Count() == 0 || lastReservation == null)
            {
                if (int.Parse(numberOfGuests) <= selectedTour.MaxGuestNumber)
                {
                    SaveValidReservation(selectedTour, numberOfGuests, TourReservations, guest2);
                    return true;
                }
                else
                {
                    MessageBox.Show("There is " + selectedTour.MaxGuestNumber + " more available spots!");
                    return false;
                }
            }
            else
            {
                if (SuccessOfTheLastReservation(selectedTour, numberOfGuests, TourReservations, ReservationsByTour, OtherTours, tourReservationView, guest2))
                    return true;
                else
                    return false;
            }
        }

        public bool SuccessOfTheLastReservation(Tour selectedTour, string numberOfGuests, List<TourReservation> TourReservations, List<TourReservation> ReservationsByTour, List<Tour> OtherTours, TourReservationView tourReservationView, Guest2 guest2)
        {
            TourReservation lastReservation = FindLastCurrentReservation(selectedTour.Id, TourReservations, ReservationsByTour, guest2);
            string lastGuestNumber = lastReservation.GuestNumber.ToString();
            foreach (TourReservation tourReservation in TourReservations)
            {
                if (tourReservation.Id == lastReservation.Id)
                {
                    if (tourReservation.TourId == selectedTour.Id)
                    {
                        if (int.Parse(numberOfGuests) <= tourReservation.GuestNumber)
                        {
                            SaveSameReservation(selectedTour, tourReservation, numberOfGuests, TourReservations, guest2);
                            return true;
                        }
                        else
                        {
                            if (tourReservation.GuestNumber == 0)
                            {
                                FullyBookedTour(selectedTour, OtherTours, tourReservationView);
                                return false;
                            }
                            else
                            {
                                MessageBox.Show("There is " + tourReservation.GuestNumber + " more available spots!");
                                return false;
                            }
                        }
                    }
                }
            }
            return false;
        }

        private TourReservation FindLastCurrentReservation(int tourId, List<TourReservation> TourReservations, List<TourReservation> ReservationsByTour, Guest2 guest2)
        {
            foreach (TourReservation tourReservation in TourReservations)
                if (tourReservation.TourId == tourId)
                    ReservationsByTour.Add(tourReservation);
            if (ReservationsByTour.Count() == 0)
            {
                return null;
            }
            else
            {
                TourReservation lastReservation = ReservationsByTour.Last();
                return lastReservation;
            }
        }

        public bool FullyBookedTour(Tour selectedTour, List<Tour> OtherTours, TourReservationView tourReservationView)
        {
            MessageBox.Show("Selected tour is fully booked. Try other tours on this location! ");
            
            return true;
        }

        private void SaveSameReservation(Tour selectedTour, TourReservation reservation, string numberOfGuests, List<TourReservation> TourReservations, Guest2 guest2)
        {
            if (guest2.Id == reservation.GuestId)
                TourReservations.Remove(reservation);
            int newGuestNumber = reservation.GuestNumber - int.Parse(numberOfGuests);
            TourReservation tourReservation = new TourReservation(NextId(), selectedTour.Id, newGuestNumber, guest2.Id);
            TourReservations.Add(tourReservation);
            Save(tourReservation);
        }

        private void SaveValidReservation(Tour selectedTour, string numberOfGuests, List<TourReservation> TourReservations, Guest2 guest2)
        {
            int newGuestNumber = selectedTour.MaxGuestNumber - int.Parse(numberOfGuests);
            TourReservation tourReservation = new TourReservation(NextId(), selectedTour.Id, newGuestNumber, guest2.Id);
            TourReservations.Add(tourReservation);
            Save(tourReservation);
        }

        public List<Tour> showGuestsTours(List<Tour> Tours, List<Location> Locations, List<Language> Languages, List<CheckPoint> CheckPoints, List<Tour> ActiveTours, List<TourReservation> TourReservations, Guest2 guest2)
        {
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

        public Tour FindTourInActiveTours(Tour selectedTour, List<Tour> Tours, List<Location> Locations, List<Language> Languages, List<CheckPoint> CheckPoints)
        {
            List<Tour> activeTours = showAllActiveToursNew(Tours, Locations, Languages, CheckPoints);

            Tour currentTour = activeTours.FirstOrDefault(tour => tour.Id == selectedTour.Id);
            return currentTour;
        }
    }
}
