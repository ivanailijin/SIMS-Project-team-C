using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
using System.Xml.Linq;
using TravelService.Repository;
using TravelService.Model;
using System.Text.RegularExpressions;

namespace TravelService.View
{

    public partial class TourView : Window
    {
        public readonly TourRepository _tourRepository;

        public readonly LocationRepository _locationRepository;

        public readonly LanguageRepository _languageRepository;

        public readonly CheckPointRepository _checkpointRepository;
        public static List<Location> Locations { get; set; }
        public static ObservableCollection<Tour> Tours { get; set; } 
        public static List<Language> Languages { get; set; }
        public static List<CheckPoint> CheckPoints { get; set; }
        public static List<CheckPoint> FilteredCheckPoints { get; set; }
        public ObservableCollection<Tour> FilteredTours { get; set; }
        public TourView()
        {
            InitializeComponent();
            DataContext = this;
            _tourRepository = new TourRepository();
            _locationRepository = new LocationRepository();
            _languageRepository = new LanguageRepository();
            _checkpointRepository = new CheckPointRepository();

            Tours = new ObservableCollection<Tour>(_tourRepository.GetAll());
            FilteredTours = new ObservableCollection<Tour>();
            FilteredCheckPoints = new List<CheckPoint>();
            Locations = new List<Location>(_locationRepository.GetAll());
            Languages = new List<Language>(_languageRepository.GetAll());
            CheckPoints = new List<CheckPoint>(_checkpointRepository.GetAll());

            foreach (Tour tour in Tours)
            {
                List<CheckPoint> ListCheckPoints = new List<CheckPoint>();
                tour.Location = Locations.Find(loc => loc.Id == tour.LocationId);
                tour.Language = Languages.Find(lan => lan.Id == tour.LanguageId);

                
                tour.CheckPoints.Clear();
                ListCheckPoints.Clear();

                int currentId=tour.Id;
                

                foreach (CheckPoint c in CheckPoints)
                {
                    int currentCheckPointTourId = c.TourId;
                    if((currentCheckPointTourId == currentId)) {
                        ListCheckPoints.Add(c);
                        
                    }
                }

                tour.CheckPoints.AddRange(ListCheckPoints);
                
            }
        }

       private void searchTour_Click(object sender, RoutedEventArgs e)
        {
            FilteredTours.Clear();

            string inputDuration = (string)durationComboBox.Text;
            string inputLocation = (string)locationComboBox.Text.Replace(",", "").Replace(" ", "");
            string inputLanguage = (string)languageComboBox.Text;
            string inputGuestNumber = guestsTextBox.Text;



            foreach (Tour tour in Tours) {
                if (isTourSearchable(tour, inputLocation, inputDuration, inputLanguage, inputGuestNumber)) {
                    if (!FilteredTours.Contains(tour))
                        FilteredTours.Add(tour);

                    allTours.ItemsSource = FilteredTours;
                }
            }

            allTours.ItemsSource = FilteredTours;
        }

        private bool isTourSearchable(Tour tour, string inputLocation, string inputDuration, string inputLanguage, string inputGuestNumber)
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

        private void showAllTours_Click(object sender, RoutedEventArgs e)
        {
            allTours.ItemsSource = Tours;
        }

        
    }
 }
