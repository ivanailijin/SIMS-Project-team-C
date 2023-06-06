using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Navigation;
using TravelService.Applications.UseCases;
using TravelService.Applications.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.WPF.Services;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class SearchAccommodationViewModel : ViewModelBase
    {
        private AccommodationService _accommodationService;
        private INavigationInterface _navigationInterface;
        public ObservableCollection<Accommodation> FilteredAccommodations { get; set; }
        public ObservableCollection<string> LocationsComboBox { get; set; }
        public AccommodationViewModel _accommodationViewModel;
        public SearchAccommodationView SearchAccommodationView { get; set; }

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

        private RelayCommand _decreaseGuestNumberCommand;
        public RelayCommand DecreaseGuestNumberCommand
        {
            get => _decreaseGuestNumberCommand;
            set
            {
                if (value != _decreaseGuestNumberCommand)
                {
                    _decreaseGuestNumberCommand = value;
                    OnPropertyChanged();
                }
            }
        }

        private RelayCommand _increaseGuestNumberCommand;
        public RelayCommand IncreaseGuestNumberCommand
        {
            get => _increaseGuestNumberCommand;
            set
            {
                if (value != _increaseGuestNumberCommand)
                {
                    _increaseGuestNumberCommand = value;
                    OnPropertyChanged();
                }
            }
        }

        private RelayCommand _decreaseLengthOfStayCommand;
        public RelayCommand DecreaseLengthOfStayCommand
        {
            get => _decreaseLengthOfStayCommand;
            set
            {
                if (value != _decreaseLengthOfStayCommand)
                {
                    _decreaseLengthOfStayCommand = value;
                    OnPropertyChanged();
                }
            }
        }

        private RelayCommand _increaseLengthOfStayCommand;
        public RelayCommand IncreaseLengthOfStayCommand
        {
            get => _increaseLengthOfStayCommand;
            set
            {
                if (value != _increaseLengthOfStayCommand)
                {
                    _increaseLengthOfStayCommand = value;
                    OnPropertyChanged();
                }
            }
        }

        private RelayCommand _previousPageCommand;
        public RelayCommand PreviousPageCommand
        {
            get => _previousPageCommand;
            set
            {
                if (value != _previousPageCommand)
                {
                    _previousPageCommand = value;
                    OnPropertyChanged();
                }
            }
        }

        public SearchAccommodationViewModel(INavigationInterface navigationInterface, AccommodationViewModel accommodationViewModel)
        {
            _accommodationViewModel = accommodationViewModel;
            _navigationInterface = navigationInterface;
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
            DecreaseGuestNumberCommand = new RelayCommand(Execute_DecreaseGuestNumber, CanExecute_Command);
            IncreaseGuestNumberCommand = new RelayCommand(Execute_IncreaseGuestNumber, CanExecute_Command);
            DecreaseLengthOfStayCommand = new RelayCommand(Execute_DecreaseLengthOfStay, CanExecute_Command);
            IncreaseLengthOfStayCommand = new RelayCommand(Execute_IncreaseLengthOfStay, CanExecute_Command);
            PreviousPageCommand = new RelayCommand(Execute_PreviousPage, CanExecute_Command);
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

            foreach (var accommodation in filteredAccommodations)
            {
                FilteredAccommodations.Add(accommodation);
            }

            _accommodationViewModel.Accommodations = FilteredAccommodations;
            _navigationInterface.GoBack();
        }

        private void Execute_DecreaseGuestNumber(object sender)
        {
            if (int.TryParse(GuestNumber, out int guestNumber))
            {
                if (guestNumber > 0)
                {
                    guestNumber--;
                    GuestNumber = guestNumber.ToString();
                }
            }
        }

        private void Execute_IncreaseGuestNumber(object sender)
        {
            int.TryParse(GuestNumber, out int guestNumber);
            guestNumber++;
            GuestNumber = guestNumber.ToString();
        }

        private void Execute_DecreaseLengthOfStay(object sender)
        {
            if (int.TryParse(LengthOfStay, out int lengthOfStay))
            {
                if (lengthOfStay > 0)
                {
                    lengthOfStay--;
                    LengthOfStay = lengthOfStay.ToString();
                }
            }
        }

        private void Execute_IncreaseLengthOfStay(object sender)
        {
            int.TryParse(LengthOfStay, out int lengthOfStay);
            lengthOfStay++;
            LengthOfStay = lengthOfStay.ToString();
        }

        private void Execute_PreviousPage(object sender)
        {
            _navigationInterface.GoBack();
        }
    }
}
