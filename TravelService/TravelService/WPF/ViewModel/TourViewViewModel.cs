using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TravelService.Applications.UseCases;
using TravelService.Applications.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.Repository;

namespace TravelService.WPF.ViewModel
{
    public class TourViewViewModel : ViewModelBase
    {
        public Action CloseAction { get; set; }

        private readonly TourService _tourService;

        private readonly LocationService _locationService;

        private readonly LanguageService _languageService;

        private readonly CheckPointService _checkpointService;
        public ObservableCollection<string> LocationsComboBox { get; set; }
        public ObservableCollection<Language> LanguageComboBox { get; set; }

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
        private string _duration;
        public string Duration
        {
            get => _duration;
            set
            {
                if (value != _duration)
                {
                    _duration = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _language;
        public string Language
        {
            get => _language;
            set
            {
                if (value != _language)
                {
                    _language = value;
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
        private RelayCommand _cancelCommand;
        public RelayCommand CancelCommand
        {
            get => _cancelCommand;
            set
            {
                if (value != _cancelCommand)
                {
                    _cancelCommand = value;
                    OnPropertyChanged();
                }
            }
        }
        private RelayCommand _showAllToursCommand;
        public RelayCommand ShowAllToursCommand
        {
            get => _showAllToursCommand;
            set
            {
                if (value != _showAllToursCommand)
                {
                    _showAllToursCommand = value;
                    OnPropertyChanged();
                }
            }
        }
        private RelayCommand _searchTourCommand;
        public RelayCommand SearchTourCommand
        {
            get => _searchTourCommand;
            set
            {
                if (value != _searchTourCommand)
                {
                    _searchTourCommand = value;
                    OnPropertyChanged();
                }
            }
        }
        public Guest2 Guest2 { get; set; }

        private ObservableCollection<Tour> _tours;
        public ObservableCollection<Tour> Tours
        {
            get => _tours;
            set
            {
                if (value != _tours)
                {
                    _tours = value;
                    OnPropertyChanged();
                    List<Tour> tours = _tours.ToList();
                }
            }
        }
        public ObservableCollection<Tour> FilteredTours { get; set; }
        public List<Location> Locations { get; set; }
        public List<Language> Languages { get; set; }
        public List<CheckPoint> CheckPoints { get; set; }
        public TourViewViewModel(Guest2 guest2)
        {
            _tourService = new TourService(Injector.CreateInstance<ITourRepository>());
            _locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            _languageService = new LanguageService(Injector.CreateInstance<ILanguageRepository>());
            _checkpointService = new CheckPointService(Injector.CreateInstance<ICheckPointRepository>());

            List<Tour> tours = new List<Tour>(_tourService.GetAll());
            Tours = new ObservableCollection<Tour>(tours);
            FilteredTours = new ObservableCollection<Tour>();
            LocationsComboBox = new ObservableCollection<string>();
            LanguageComboBox = new ObservableCollection<Language>();
            Locations = new List<Location>(_locationService.GetAll());
            Languages = new List<Language>(_languageService.GetAll());
            CheckPoints = new List<CheckPoint>(_checkpointService.GetAll());
            Guest2 = guest2;

            _tourService.ShowTourList(Tours.ToList(), Locations, Languages, CheckPoints);

            CancelCommand = new RelayCommand(Execute_CancelCommand, CanExecute_Command);
            ShowAllToursCommand = new RelayCommand(Execute_ShowAllToursCommand, CanExecute_Command);
            SearchTourCommand = new RelayCommand(Execute_SearchTourCommand, CanExecute_Command);

            foreach (Tour tour in Tours)
            {
                LocationsComboBox.Add(tour.Location.CityAndCountry);
                LanguageComboBox.Add(tour.Language);
            }
            LocationsComboBox.Insert(0, "");

        }
        private bool CanExecute_Command(object parameter)
        {
            return true;
        }
        private void Execute_CancelCommand(object sender)
        {
            CloseAction();
        }
        private void Execute_ShowAllToursCommand(object sender)
        {
            Tours.Clear();
            List<Tour> tours = new List<Tour>(_tourService.GetAll());
            Tours = new ObservableCollection<Tour>(tours);
            _tourService.ShowTourList(Tours.ToList(), Locations, Languages, CheckPoints);
        }
        private void Execute_SearchTourCommand(object sender)
        {
            FilteredTours.Clear();

            string inputDuration = Duration;
            string inputLocation = Location?.Replace(",", "").Replace(" ", "");
            string inputLanguage = Language;
            string inputGuestNumber = GuestNumber;
            List<Tour> filteredTours = new List<Tour>();

            foreach (Tour tour in Tours)
            {
                if (_tourService.isTourSearchable(tour, inputLocation, inputDuration, inputLanguage, inputGuestNumber))
                {
                    if (!filteredTours.Contains(tour))
                        filteredTours.Add(tour);
                }
            }
            foreach (var tour in filteredTours)
            {
                FilteredTours.Add(tour);
            }
            Tours = FilteredTours;
            CloseAction();
        }
    }
}
