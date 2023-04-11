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
using TravelService.Domain.Model;
using TravelService.WPF.View;
using System.Printing;
using System.Diagnostics.Metrics;
using System.ComponentModel;

namespace TravelService.WPF.View
{
    /// <summary>
    /// Interaction logic for AccommodationView.xaml
    /// </summary>
    public partial class AccommodationView : Window
    {
        private readonly AccommodationRepository _accomodationRepository;
        private readonly LocationRepository _locationRepository;
        private readonly OwnerRepository _ownerRepository;
        public Guest1 Guest1 { get; set; }
        public static ObservableCollection<Accommodation> Accommodations { get; set; }
        public Accommodation SelectedAccommodation { get; set; }
        public ObservableCollection<Accommodation> FilteredAccommodations { get; set; }
        public ObservableCollection<string> Types { get; set; }

        public AccommodationView(Guest1 guest1)
        {
            InitializeComponent();
            DataContext = this;
            _accomodationRepository = new AccommodationRepository();
            _locationRepository = new LocationRepository();
            _ownerRepository = new OwnerRepository();

            this.Guest1 = guest1;
            List<Location> locations = new List<Location>(_locationRepository.GetAll());
            List<Accommodation> accommodations = new List<Accommodation>(_accomodationRepository.GetAll());
            List<Owner> owners = new List<Owner>(_ownerRepository.GetAll());
            _accomodationRepository.SetLocationForAccommodation(locations, accommodations);
            _accomodationRepository.SetTypeForAccommodation(accommodations);
            _accomodationRepository.SetOwnerForAccommodation(owners, accommodations);
            List<Accommodation> sortedAccommodations = accommodations.OrderByDescending(a => a.Owner.SuperOwner).ToList();

            Accommodations = new ObservableCollection<Accommodation>(sortedAccommodations);

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
            List<Location> locations = new List<Location>(_locationRepository.GetAll());
            List<Accommodation> Filtered = new List<Accommodation>();
            List<Accommodation> SortedFiltered = new List<Accommodation>();

            string name = NameBox.Text.ToLower();
            string[] nameWords = name.Split(' ');
            string location = (string)LocationComboBox.Text.Replace(",", "").Replace(" ", "");
            string type = (string)AccommodationTypeComboBox.SelectedItem;
            string guestNumber = GuestNumberBox.Text;
            string daysForReservation = NumberOfDaysForReservationBox.Text;

            FilteredAccommodations.Clear();

            Filtered = _accomodationRepository.Search(name, nameWords, location, type, guestNumber, daysForReservation, locations);
            List<Owner> owners = new List<Owner>(_ownerRepository.GetAll());
            _accomodationRepository.SetOwnerForAccommodation(owners, Filtered);
            SortedFiltered = Filtered.OrderByDescending(a => a.Owner.SuperOwner).ToList();

            foreach (var accommodation in SortedFiltered)
            {
                FilteredAccommodations.Add(accommodation);
            }

            dataGridAccommodations.ItemsSource = FilteredAccommodations;
        }

        private void AccommodationTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AccommodationTypeComboBox.SelectedItem != null)
            {
                string inputType = (string)AccommodationTypeComboBox.SelectedItem;
            }
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

        private void OwnerRating_Click(object sender, RoutedEventArgs e)
        {
            RatingView ratingView = new RatingView(Guest1);
            ratingView.Show();
;        }

        private void ShowReservations_Click(object sender, RoutedEventArgs e)
        {
            ReservationsView reservationsView = new ReservationsView(Guest1);
            reservationsView.Show();
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            SignInForm signIn = new SignInForm();
            signIn.Show();
            Close();
        }
    }
}

       

