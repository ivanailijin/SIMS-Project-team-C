using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Application.UseCases;
using TravelService.Application.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class SearchAccommodationViewModel : ViewModelBase
    {
        private AccommodationViewModel _accommodationViewModel;
        private AccommodationService _accommodationService;
        public ObservableCollection<Accommodation> FilteredAccommodations { get; set; }
        public ObservableCollection<string> LocationsComboBox { get; set; }
        public Action CloseAction { get; set; }

        private string _accommodationName;
        public string AccommodationName
        {
            get => _accommodationName;
            set
            {
                if (value != _accommodationName)
                {
                    _accommodationName = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _location;
        public string Location
        {
            get => _location;
            set
            {
                if (value != _location)
                {
                    _location = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _type;
        public string Type
        {
            get => _type;
            set
            {
                if (value != _type)
                {
                    _type = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _guestNumber;
        public string GuestNumber
        {
            get => _guestNumber;
            set
            {
                if (value != _guestNumber)
                {
                    _guestNumber = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _lengthOfStay;
        public string LengthOfStay
        {
            get => _lengthOfStay;
            set
            {
                if (value != _lengthOfStay)
                {
                    _lengthOfStay = value;
                    OnPropertyChanged();
                }
            }
        }

        private RelayCommand _searchAccommodation;
        public RelayCommand SearchAccommodationCommand
        {
            get => _searchAccommodation;
            set
            {
                if (value != _searchAccommodation)
                {
                    _searchAccommodation = value;
                    OnPropertyChanged();
                }

            }
        }

        public SearchAccommodationViewModel(AccommodationViewModel accommodationViewModel)
        {
            _accommodationViewModel = accommodationViewModel;
            LocationsComboBox = new ObservableCollection<string>();
            _accommodationService = new AccommodationService(Injector.CreateInstance<IAccommodationRepository>());
            FilteredAccommodations = new ObservableCollection<Accommodation>();
            List<Accommodation> accommodations = new List<Accommodation>(_accommodationService.GetAll());
            accommodations = _accommodationService.GetLocationData(accommodations);

            foreach (Accommodation accommodation in accommodations)
            {
                LocationsComboBox.Add(accommodation.Location.CityAndCountry);
            }
            LocationsComboBox.Insert(0, "");

            SearchAccommodationCommand = new RelayCommand(Execute_Search, CanExecute_Command);
        }

        private bool CanExecute_Command(object parameter)
        {
            return true;
        }

        private void Execute_Search(object sender)
        {
            string name = null;
            string[] nameWords = null;
            if (AccommodationName != null)
            {
                name = AccommodationName.ToLower();
                nameWords = name.Split(' ');
            }
            string location = Location?.Replace(",", "").Replace(" ", "");
            string type = Type;
            string guestNumber = GuestNumber;
            string daysForReservation = LengthOfStay;

            List<Accommodation> filteredAccommodations = _accommodationService.Search(name, nameWords, location, type, guestNumber, daysForReservation);

            foreach(var accommodation in filteredAccommodations)
            {
                FilteredAccommodations.Add(accommodation);
            }

            _accommodationViewModel.Accommodations = FilteredAccommodations;
            CloseAction();
        }
    }
}
