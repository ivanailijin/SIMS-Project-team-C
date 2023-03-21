using System;
using System.Collections.Generic;
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
using TravelService.Observer;
using TravelService.Repository;
using System.Collections.ObjectModel;
using TravelService.Model;
using System.Printing;
using System.Diagnostics.Metrics;

namespace TravelService.View
{
    /// <summary>
    /// Interaction logic for AccommodationView.xaml
    /// </summary>
    public partial class AccommodationView : Window
    {
        private readonly AccommodationRepository _accomodationRepository;

        private readonly LocationRepository _locationRepository;
        public Guest1 Guest1 { get; set; }
        public static ObservableCollection<Accommodation> Accommodations { get; set; }

        public Accommodation SelectedAccommodation { get; set; }

        public static List<Location> Locations { get; set; }

        public ObservableCollection<Accommodation> FilteredAccommodations { get; set; }

        public ObservableCollection<string> Types { get; set; }

        //accommodation
        public AccommodationView(Guest1 guest1)
        {
            InitializeComponent();
            DataContext = this;
            _accomodationRepository = new AccommodationRepository();
            _locationRepository = new LocationRepository();

            this.Guest1 = guest1;
            Accommodations = new ObservableCollection<Accommodation>(_accomodationRepository.GetAll());
            Locations = new List<Location>(_locationRepository.GetAll());

            foreach (Accommodation accommodation in Accommodations)
            {
                accommodation.Location = Locations.Find(l => l.Id == accommodation.LocationId);
            }

            foreach(Accommodation accommodation in Accommodations)
            {
                if(accommodation.Type == TYPE.HOUSE)
                {
                    accommodation.TypeText = "House";
                }
                else if(accommodation.Type == TYPE.APARTMENT)
                {
                    accommodation.TypeText = "Apartment";
                }
                else
                {
                    accommodation.TypeText = "Cottage";
                }
            }

            foreach(Accommodation accommodation in Accommodations)
            {
                LocationComboBox.Items.Add(accommodation.Location.CityAndCountry);
            }
            LocationComboBox.Items.Insert(0, "");

            FilteredAccommodations = new ObservableCollection<Accommodation>();
            Types = new ObservableCollection<string>();
            Types.Add("");
            Types.Add("Apartment");
            Types.Add("House");
            Types.Add("Cottage");
        }

        private void Find_Accommodation_Click(object sender, RoutedEventArgs e)
        {
            FilteredAccommodations.Clear();

            foreach (Accommodation accommodation in Accommodations)
            {
                if (IsAccommodationMatchingSearchCriteria(accommodation))
                {
                    if (!FilteredAccommodations.Contains(accommodation))
                        FilteredAccommodations.Add(accommodation);

                    dataGridAccommodations.ItemsSource = FilteredAccommodations;
                }
                
            }

            dataGridAccommodations.ItemsSource = FilteredAccommodations;

        }

        public bool IsAccommodationMatchingSearchCriteria(Accommodation accommodation)
        {
            string name = NameBox.Text.ToLower();
            string[] nameWords = name.Split(' ');
            string location = (string)LocationComboBox.Text.Replace(",", "").Replace(" ", "");
            string type = (string)AccommodationTypeComboBox.SelectedItem;
            string guestNumber = GuestNumberBox.Text;
            string daysForReservation = NumberOfDaysForReservationBox.Text;

            bool matches = false;

            if ((IsContainingNameWords(accommodation,nameWords) || string.IsNullOrEmpty(name)) &&
               ((accommodation.Location.CityAndCountry.Replace(",", "").Replace(" ", "")).Contains(location) || string.IsNullOrEmpty(location)) &&
               (HasMatchingAccommodationType(accommodation, type) || string.IsNullOrEmpty(type)) &&
               (IsGuestNumberLessThanMaximum(accommodation, guestNumber) || string.IsNullOrEmpty(guestNumber)) &&
               (IsReservationGreaterThanMinimum(accommodation, daysForReservation) || string.IsNullOrEmpty(daysForReservation)))
            {
                matches = true;
            }

            return matches;
        }

        public bool IsContainingNameWords(Accommodation accommodation, string[] nameWords)
        {
            bool containsAllWords = true;

            foreach(string word in nameWords)
            {
                if (!accommodation.Name.ToLower().Contains(word))
                {
                    containsAllWords = false;
                    break;
                }
            }
            return containsAllWords;
        }

        public bool HasMatchingAccommodationType(Accommodation accommodation, string type)
        {
            bool result = false;

            if (!string.IsNullOrEmpty(type))
            {
                result = accommodation.Type.ToString().ToLower().Contains(type.ToLower());
            }
            return result;
        }

        public bool IsGuestNumberLessThanMaximum(Accommodation accommodation, string guestNumber)
        {
            bool isLess = false;
            if(int.TryParse(guestNumber, out int parsedGuestNumber) && parsedGuestNumber <= accommodation.MaxGuestNumber)
            {
                isLess = true;
            }
            return isLess;
        }

        public bool IsReservationGreaterThanMinimum(Accommodation accommodation, string daysForReservation)
        {
            bool isGreater = false;
            if(int.TryParse(daysForReservation, out int parsedDaysForReservation) && parsedDaysForReservation >= accommodation.MinReservationDays)
            {
                isGreater = true;
            }
            return isGreater;
        }

        private void AccommodationTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AccommodationTypeComboBox.SelectedItem != null)
            {
                string inputType = (string)AccommodationTypeComboBox.SelectedItem;

            }
        }

        private void LocationComboBox_KeyUp(object sender, KeyEventArgs e)
        {
            string locationInput = LocationComboBox.Text;

            List<string> filteredLocations = GetFilteredLocations(locationInput);
            LocationComboBox.ItemsSource = filteredLocations;
        }

        public List<string> GetFilteredLocations(string locationInput)
        {
            List<string> locationSuggestions = new List<string>();
            foreach (Location location in Locations)
            {
                locationSuggestions.Add(location.CityAndCountry);
            }

            List<string> filteredLocations = locationSuggestions.Where(l => l.StartsWith(locationInput, StringComparison.InvariantCultureIgnoreCase)).ToList();

            return filteredLocations;
         
        }

        private void ReserveAccommodation_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedAccommodation != null)
            {
                AccommodationReservationView reservationView = new AccommodationReservationView(SelectedAccommodation, Guest1);
                reservationView.Show();
            }
            else
            {
                MessageBox.Show("Please select accommodation for reservation.");
            }
        }
    }
}

       

