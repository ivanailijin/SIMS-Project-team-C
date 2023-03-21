using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TravelService.Model;
using TravelService.Repository;

namespace TravelService.View
{
    /// <summary>
    /// Interaction logic for TourReservationView.xaml
    /// </summary>
    public partial class TourReservationView : Window
    {
        public string EnteredNumberOfGuests { get; set; } = string.Empty;

        public readonly TourReservationRepository _tourReservationRepository;

        public readonly TourRepository _tourRepository;

        public readonly LocationRepository _locationRepository;

        public readonly LanguageRepository _languageRepository;

        public readonly CheckPointRepository _checkpointRepository;
        public static ObservableCollection<TourReservation> TourReservations { get; set; }
        public static ObservableCollection<Tour> Tours { get; set; }
        public static List<Location> Locations { get; set; }
        public static List<Language> Languages { get; set; }
        public static List<CheckPoint> CheckPoints { get; set; }
        public List<Tour> ActiveTours { get; set; }
        public List<Tour> OtherTours { get; set; }
        public List<TourReservation> ReservationsByTour { get; set; }
        public Tour SelectedTour { get; set; }

        public TourReservationView(Tour selectedActiveTour)
        {
            InitializeComponent();
            DataContext = this;
            _tourReservationRepository = new TourReservationRepository();
            _tourRepository = new TourRepository();
            _locationRepository = new LocationRepository();
            _languageRepository = new LanguageRepository();
            _checkpointRepository = new CheckPointRepository();


            TourReservations = new ObservableCollection<TourReservation>(_tourReservationRepository.GetAll());
            Tours = new ObservableCollection<Tour>(_tourRepository.GetAll());
            Locations = new List<Location>(_locationRepository.GetAll());
            Languages = new List<Language>(_languageRepository.GetAll());
            CheckPoints = new List<CheckPoint>(_checkpointRepository.GetAll());
            ReservationsByTour = new List<TourReservation>();

            ActiveTours = new List<Tour>();
            OtherTours = new List<Tour>();

            SelectedTour = selectedActiveTour;

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
                FindActiveTourList(tour);
            }
        }
        public void FindActiveTourList(Tour tour) {
            if (IsInPorgress(tour))
            {
                AddActiveTours(tour);
            }
        }
        public void AddActiveTours(Tour tour) {
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
        private void CheckTourButton_Click(object sender, RoutedEventArgs e)
        {
            TryReserving(SelectedTour, EnteredNumberOfGuests);
        }

        private void TryReserving(Tour selectedTour, string numberOfGuests)
        {
            int number = int.Parse(numberOfGuests);
            if (number <= 0)
            {
                MessageBox.Show("Inserted number of people is not valid.");
            }
            else
            {
                if (ReservationSuccess(selectedTour, numberOfGuests))
                {
                    MessageBox.Show("You have successfully booked a tour!");
                }
            }
        }

        private bool ReservationSuccess(Tour selectedTour, string numberOfGuests)
        {
            if (TourReservations.Count() == 0)
            {
                if (int.Parse(numberOfGuests) <= selectedTour.MaxGuestNumber)
                {
                    SaveValidReservation(selectedTour, numberOfGuests);
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
                TourReservation lastReservation =FindLastCurrentReservation(selectedTour.Id);
                foreach (TourReservation tourReservation in TourReservations) {
                    if (tourReservation.Id == lastReservation.Id) {
                        if (tourReservation.TourId == selectedTour.Id)
                        {
                            if (int.Parse(numberOfGuests) <= tourReservation.GuestNumber)
                            {
                                SaveSameReservation(selectedTour, tourReservation, numberOfGuests);
                                return true;
                            }
                            else
                            {
                                if (tourReservation.GuestNumber == 0)
                                {
                                    FullyBookedTour(selectedTour);
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

                if (int.Parse(numberOfGuests) <= selectedTour.MaxGuestNumber)
                {
                    SaveValidReservation(selectedTour, numberOfGuests);
                    return true;
                }
                else {
                     MessageBox.Show("There is " + selectedTour.MaxGuestNumber + " more available spots!");
                    return false;
                }
            }
        }

        private TourReservation FindLastCurrentReservation(int tourId) {
            foreach(TourReservation tourReservation in TourReservations)
                if (tourReservation.TourId == tourId)
                    ReservationsByTour.Add(tourReservation);
            TourReservation lastReservation = ReservationsByTour.Last();
            return lastReservation;
        }
        private void FullyBookedTour(Tour selectedTour)
        {
            MessageBox.Show("Selected tour is fully booked. Try other tours on this location! ");
            FindOtherTours(selectedTour);            
        }

        private void FindOtherTours(Tour selectedTour)
        {
            OtherTours.Remove(selectedTour);
            ActiveTours.Remove(selectedTour);
            foreach (Tour tour in ActiveTours)
            {
                if (tour.LocationId == selectedTour.LocationId)
                {
                    OtherTours.Add(tour);
                    allActiveTours.ItemsSource = OtherTours;
                }
                
            }
            allActiveTours.ItemsSource = OtherTours;
            
            RunOutOfTours(selectedTour);
        }

        private void RunOutOfTours(Tour tour)
        {
            if (OtherTours.Count() < 1)
            {
                MessageBox.Show("There are no more avaliable tours, please change the number of people!");
                ActiveTours.Add(tour);
                allActiveTours.ItemsSource = ActiveTours;
            }
        }

        private void SaveSameReservation(Tour selectedTour, TourReservation reservation, string numberOfGuests)
        {
            TourReservations.Remove(reservation);
            //int newGuestNumber = selectedTour.MaxGuestNumber - int.Parse(numberOfGuests);
            TourReservation tourReservation = new TourReservation(_tourReservationRepository.NextId(), selectedTour.Id, reservation.GuestNumber - int.Parse(numberOfGuests));
            TourReservations.Add(tourReservation);
            _tourReservationRepository.Save(tourReservation);
        }

        private void SaveValidReservation(Tour selectedTour, string numberOfGuests)
        {
            int newGuestNumber = selectedTour.MaxGuestNumber - int.Parse(numberOfGuests);
            TourReservation tourReservation = new TourReservation(_tourReservationRepository.NextId() , selectedTour.Id, newGuestNumber);
            TourReservations.Add(tourReservation);
            _tourReservationRepository.Save(tourReservation);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();            
        }
    }
}