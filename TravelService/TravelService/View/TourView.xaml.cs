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
        public static ObservableCollection<Tour> Tours { get; set; } 
        public static List<Location> Locations { get; set; }
        public static List<Language> Languages { get; set; }
        public static List<CheckPoint> CheckPoints { get; set; }
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
            Locations = new List<Location>(_locationRepository.GetAll());
            Languages = new List<Language>(_languageRepository.GetAll());
            CheckPoints = new List<CheckPoint>(_checkpointRepository.GetAll());

            _tourRepository.ShowTourList(convertTourList(Tours), Locations, Languages, CheckPoints);
        }
        public List<Tour> convertTourList(ObservableCollection<Tour> observableCollection)
        {
            List<Tour> convertedList = observableCollection.ToList();
            return convertedList;
        }

        private void searchTour_Click(object sender, RoutedEventArgs e)
        {
            FilteredTours.Clear();

            string inputDuration = (string)durationComboBox.Text;
            string inputLocation = (string)locationComboBox.Text.Replace(",", "").Replace(" ", "");
            string inputLanguage = (string)languageComboBox.Text;
            string inputGuestNumber = guestsTextBox.Text;

            foreach (Tour tour in Tours) {
                if (_tourRepository.isTourSearchable(tour, inputLocation, inputDuration, inputLanguage, inputGuestNumber)) {
                    if (!FilteredTours.Contains(tour))
                        FilteredTours.Add(tour);

                    allTours.ItemsSource = FilteredTours;
                }
            }
            allTours.ItemsSource = FilteredTours;
        }
        
        private void showAllTours_Click(object sender, RoutedEventArgs e)
        {
            allTours.ItemsSource = Tours;
        }
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }        
    }
 }