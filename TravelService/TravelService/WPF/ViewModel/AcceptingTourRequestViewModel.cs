using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using TravelService.Applications.UseCases;
using TravelService.Applications.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.Repository;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class AcceptingTourRequestViewModel : ViewModelBase
    {


        public NavigationService NavigationService;
        public AcceptingTourRequestView AcceptingTourRequestView { get; set; }  
        public TourRequest SelectedTourRequest { get; set; }
        public Action CloseAction { get; set; }

        private readonly TourRequestService _tourRequestService;

        private readonly LocationService _locationService;

        private readonly LanguageService _languageService;

        public ObservableCollection<string> LocationsComboBox { get; set; }
        public ObservableCollection<Language> LanguageComboBox { get; set; }
        public RelayCommand SearchDates { get; set; }
        public RelayCommand Search { get; set; }
        public RelayCommand Accept { get; set; }
        public RelayCommand Stats { get; set; }



        public ObservableCollection<TourRequest> FilteredRequests { get; set; }
        public List<Location> Locations { get; set; }
        public List<Language> Languages { get; set; }
        private App app;
        private const string SRB = "sr-Latn-RS";
        private const string ENG = "en-US";


        private int _guideId;
        public int GuideId
        {
            get => _guideId;
            set
            {
                if (value != _guideId)
                {
                    _guideId = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime _startDate;
        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                if (value != _startDate)
                {
                    _startDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime _endDate;
        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                if (value != _endDate)
                {
                    _endDate = value;
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
        public Guide Guide { get; set; }

        private ObservableCollection<TourRequest> _tourRequests;
        public ObservableCollection<TourRequest> TourRequests
        {
            get => _tourRequests;
            set
            {
                if (value != _tourRequests)
                {
                    _tourRequests = value;
                    OnPropertyChanged();
                    List<TourRequest> tourRequest = _tourRequests.ToList();
                }
            }
        }
        private Location _selectedLocation;
        public Location SelectedLocation
        {
            get { return _selectedLocation; }
            set
            {
                _selectedLocation = value;
                OnPropertyChanged(nameof(SelectedLocation));
            }
        }

        private Language _selectedLangauge;
        public Language SelectedLanguage
        {
            get { return _selectedLangauge; }
            set
            {
                _selectedLangauge = value;
                OnPropertyChanged(nameof(SelectedLanguage));
            }
        }


        public AcceptingTourRequestViewModel(Guide guide, TourRequest selectedTourRequest, AcceptingTourRequestView acceptingTourRequestView,NavigationService navigationService)
        {
            SelectedTourRequest = selectedTourRequest;
            AcceptingTourRequestView = acceptingTourRequestView;
            NavigationService = navigationService;

            _tourRequestService = new TourRequestService(Injector.CreateInstance<ITourRequestRepository>());
            _locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            _languageService = new LanguageService(Injector.CreateInstance<ILanguageRepository>());

            List<TourRequest> tourRequests = new List<TourRequest>(_tourRequestService.GetAll());
            TourRequests = new ObservableCollection<TourRequest>(tourRequests);
            FilteredRequests = new ObservableCollection<TourRequest>();
            LocationsComboBox = new ObservableCollection<string>();
            LanguageComboBox = new ObservableCollection<Language>();
            Locations = new List<Location>(_locationService.GetAll());
            Languages = new List<Language>(_languageService.GetAll());
            Guide = guide;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now.AddDays(1);

            _tourRequestService.ShowTourRequests(TourRequests.ToList(), Locations, Languages);
            foreach (TourRequest tourRequest in TourRequests)
            {
                LocationsComboBox.Add(tourRequest.Location.CityAndCountry);
                LanguageComboBox.Add(tourRequest.Language);
            }
            LocationsComboBox.Insert(0, "");
            SearchDates = new RelayCommand(Execute_SearchAvailableDates, CanExecute_Command);
            Search = new RelayCommand(Execute_SearchCommand, CanExecute_Command);
            Accept = new RelayCommand(Execute_Accept, CanExecute_Command);
            Stats = new RelayCommand(Execute_StatsCommand, CanExecute_Command);
            AcceptingTourRequestView = acceptingTourRequestView;

            app = (App)Application.Current;
            app.ChangeLanguage(SRB);
        }

        private void Execute_SearchAvailableDates(object sender)
        {
            List<TourRequest> tourRequests = _tourRequestService.FindTourRequestsByDate(StartDate, EndDate);
            FilteredRequests.Clear();

            foreach (TourRequest tourRequest in tourRequests)
            {
                FilteredRequests.Add(tourRequest);
            }

            // Update the TourRequests property with the filtered requests
            TourRequests.Clear();
            foreach (TourRequest tourRequest in FilteredRequests)
            {
                TourRequests.Add(tourRequest);
            }
        }



        private void Execute_SearchCommand(object obj)
        {
            string location = Location?.Replace(",", "").Replace(" ", "");
            string language = Language;
            string guestNumber = GuestNumber;

            List<TourRequest> filteredTourRequests = _tourRequestService.Search(location, language, guestNumber);

            FilteredRequests.Clear();

            foreach (var tourRequest in filteredTourRequests)
            {
                FilteredRequests.Add(tourRequest);
            }

            TourRequests = new ObservableCollection<TourRequest>(FilteredRequests);

            // Set the searched values in the input fields
            Location = location;
            Language = language;
            GuestNumber = guestNumber;
        }

        private void Execute_StatsCommand(object obj)
        {
            NavigationService.Navigate(new RequestsStatsView(NavigationService));
        }


        private void Execute_CancelCommand(object obj)
        {
            CloseAction();
        }

        private bool CanExecute_Command(object arg)
        {
            return true;
        }

        private void Execute_Accept(object sender)
        {
            NavigationService.Navigate(new ScheduleDateView(SelectedTourRequest, NavigationService));
        }
     
    }
}
