using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TravelService.Applications.UseCases;
using TravelService.Applications.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.WPF.Services;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class AccommodationViewModel : ViewModelBase
    {
        private AccommodationService _accommodationService;
        public Guest1 Guest1 { get; set; }
        public AccommodationView AccommodationView { get; set; }
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

        private RelayCommand _findAccommodationsWindowCommand;
        public RelayCommand FindAccommodationsWindowCommand
        {
            get => _findAccommodationsWindowCommand;
            set
            {
                if (value != _findAccommodationsWindowCommand)
                {
                    _findAccommodationsWindowCommand = value;
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

        public AccommodationViewModel(AccommodationView accommodationView, Guest1 guest1)
        {
            this.Guest1 = guest1;
            AccommodationView = accommodationView;

            _accommodationService = new AccommodationService(Injector.CreateInstance<IAccommodationRepository>());
            List<Accommodation> accommodations = new List<Accommodation>(_accommodationService.GetAll());
            accommodations = _accommodationService.GetLocationData(accommodations);
            accommodations = _accommodationService.GetOwnerData(accommodations);
            accommodations = _accommodationService.SortBySuperowner(accommodations);
            Accommodations = new ObservableCollection<Accommodation>(accommodations);

            SearchWindowCommand = new RelayCommand(Execute_SearchWindow, CanExecute_Command);
            FindAccommodationsWindowCommand = new RelayCommand(Execute_FindAccommodationsWindow, CanExecute_Command);
            AccommodationSelectedCommand = new RelayCommand(Execute_OnItemSelected, CanExecute_Command);
        }

        private bool CanExecute_Command(object parameter)
        {
            return true;
        }

        private void Execute_SearchWindow(object sender)
        {
            SearchAccommodationView searchAccommodationView = new SearchAccommodationView(this);
            FirstGuestWindow firstGuestWindow = Window.GetWindow(AccommodationView) as FirstGuestWindow ?? new(Guest1);
            firstGuestWindow?.SwitchToPage(searchAccommodationView);
        }

        private void Execute_FindAccommodationsWindow(object sender)
        {
            FindAvailableAccommodationsView findAvailableAccommodationsView = new FindAvailableAccommodationsView(Guest1);
            FirstGuestWindow firstGuestWindow = Window.GetWindow(AccommodationView) as FirstGuestWindow ?? new(Guest1);
            firstGuestWindow?.SwitchToPage(findAvailableAccommodationsView);
        }

        private void Execute_OnItemSelected(object sender)
        {
            SelectedAccommodationView selectedAccommodationView = new SelectedAccommodationView(SelectedAccommodation, Guest1);
            FirstGuestWindow firstGuestWindow = Window.GetWindow(AccommodationView) as FirstGuestWindow ?? new(Guest1);
            firstGuestWindow?.SwitchToPage(selectedAccommodationView);
        }
    }
}
