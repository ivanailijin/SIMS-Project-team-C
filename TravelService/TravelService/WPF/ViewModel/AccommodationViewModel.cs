using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using TravelService.Application.UseCases;
using TravelService.Application.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class AccommodationViewModel : ViewModelBase
    {
        private AccommodationService _accommodationService;
        public Guest1 Guest1 { get; set; }
        public ObservableCollection<Accommodation> FilteredAccommodations { get; set; }
        public ObservableCollection<string> Types { get; set; }
        public Action CloseAction { get; set; }

        private ObservableCollection<Accommodation> _accommodations;
        public ObservableCollection<Accommodation> Accommodations
        {
            get => _accommodations;
            set
            {
                if (value != _accommodations)
                {
                    _accommodations = value;
                    OnPropertyChanged();
                    List<Accommodation> accommodations = _accommodations.ToList();
                    accommodations = _accommodationService.GetTypeData(accommodations);
                    accommodations = _accommodationService.GetOwnerData(accommodations);
                    accommodations = _accommodationService.SortBySuperowner(accommodations);
                }
            }
        }

        private Accommodation _selectedAccommodation;
        public Accommodation SelectedAccommodation
        {
            get => _selectedAccommodation;
            set
            {
                if (value != _selectedAccommodation)
                {
                    _selectedAccommodation = value;
                    OnPropertyChanged();
                }
                else
                {
                    _selectedAccommodation = value;
                    OnPropertyChanged();
                }
            }
        }

        private RelayCommand _searchWindowCommand;
        public RelayCommand SearchWindowCommand
        {
            get => _searchWindowCommand;
            set
            {
                if (value != _searchWindowCommand)
                {
                    _searchWindowCommand = value;
                    OnPropertyChanged();
                }
            }
        }

        private RelayCommand _reserveCommand;
        public RelayCommand ReserveCommand
        {
            get => _reserveCommand;
            set
            {
                if (value != _reserveCommand)
                {
                    _reserveCommand = value;
                    OnPropertyChanged();
                }
            }
        }

        private RelayCommand _ratingWindowCommand;
        public RelayCommand RatingWindowCommand
        {
            get => _ratingWindowCommand;
            set
            {
                if (value != _ratingWindowCommand)
                {
                    _ratingWindowCommand = value;
                    OnPropertyChanged();
                }
            }
        }

        private RelayCommand _reservationsWindowCommand;
        public RelayCommand ReservationsWindowCommand
        {
            get => _reservationsWindowCommand;
            set
            {
                if (value != _reservationsWindowCommand)
                {
                    _reservationsWindowCommand = value;
                    OnPropertyChanged();
                }
            }
        }

        private RelayCommand _accommodationSelectedCommand;
        public RelayCommand AccommodationSelectedCommand
        {
            get => _accommodationSelectedCommand;
            set
            {
                if (value != _accommodationSelectedCommand)
                {
                    _accommodationSelectedCommand = value;
                    OnPropertyChanged();
                }
            }
        }

        public AccommodationViewModel(Guest1 guest1)
        {
            this.Guest1 = guest1;

            _accommodationService = new AccommodationService(Injector.CreateInstance<IAccommodationRepository>());
            List<Accommodation> accommodations = new List<Accommodation>(_accommodationService.GetAll());
            accommodations = _accommodationService.GetLocationData(accommodations);
            accommodations = _accommodationService.GetTypeData(accommodations);
            accommodations = _accommodationService.GetOwnerData(accommodations);
            accommodations = _accommodationService.SortBySuperowner(accommodations);
            Accommodations = new ObservableCollection<Accommodation>(accommodations);

            SearchWindowCommand = new RelayCommand(Execute_SearchWindow, CanExecute_Command);
            ReserveCommand = new RelayCommand(Execute_ReserveWindow, CanExecute_Command);
            AccommodationSelectedCommand = new RelayCommand(Execute_OnItemSelected, CanExecute_Command);
        }

        private bool CanExecute_Command(object parameter)
        {
            return true;
        }

        private void Execute_SearchWindow(object sender)
        {
            SearchAccommodationView searchAccommodationView = new SearchAccommodationView(this);
            searchAccommodationView.Show();
        }

        private void Execute_ReserveWindow(object sender)
        {
            AccommodationAvailabilityView accommodationAvailabilityView = new AccommodationAvailabilityView(SelectedAccommodation, Guest1);
            accommodationAvailabilityView.Show();
        }

        private void Execute_OnItemSelected(object sender)
        {
            SelectedAccommodationView selectedAccommodationView = new SelectedAccommodationView(SelectedAccommodation, Guest1);
            selectedAccommodationView.Show();
        }
    }
}
