using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using TravelService.Application.UseCases;
using TravelService.Application.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;

namespace TravelService.WPF.ViewModel

{
    public class RequestsStatsViewModel :ViewModelBase
    {


        public Action CloseAction { get; set; }

        private readonly TourRequestService _tourRequestService;

        private readonly LocationService _locationService;

        private readonly LanguageService _languageService;

        public ObservableCollection<string> LocationsComboBox { get; set; }
        public ObservableCollection<Language> LanguageComboBox { get; set; }
        public ObservableCollection<int> Years { get; set; }
        public ObservableCollection<int> Months { get; set; }

        public List<Location> Locations { get; set; }
        public List<Language> Languages { get; set; }
        public int SelectedYear { get; set; }
        public int SelectedMonth { get; set; }

        private string _selectedLocation;
        public string SelectedLocation
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

        public RelayCommand ShowLocationStatisticsCommand { get; }
        public RelayCommand ShowLanguageStatisticsCommand { get; }

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
        private string _formattedLocation;
        public string FormattedLocation
        {
            get { return _formattedLocation; }
            set
            {
                _formattedLocation = value;
                OnPropertyChanged(nameof(FormattedLocation));
            }
        }


        public RequestsStatsViewModel()
        {
            Years = new ObservableCollection<int>(Enumerable.Range(DateTime.Now.Year - 10, 10));
            Months = new ObservableCollection<int>(Enumerable.Range(1, 12));
            SelectedYear = DateTime.Now.Year;
            SelectedMonth = DateTime.Now.Month;


            _tourRequestService = new TourRequestService(Injector.CreateInstance<ITourRequestRepository>());
            _locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            _languageService = new LanguageService(Injector.CreateInstance<ILanguageRepository>());

            List<TourRequest> tourRequests = new List<TourRequest>(_tourRequestService.GetAll());
            TourRequests = new ObservableCollection<TourRequest>(tourRequests);

            LocationsComboBox = new ObservableCollection<string>();
            LanguageComboBox = new ObservableCollection<Language>();
            Locations = new List<Location>(_locationService.GetAll());
            Languages = new List<Language>(_languageService.GetAll());
            ShowLocationStatisticsCommand = new RelayCommand(ExecuteShowLocationStatistics, CanExecute_Command);
            ShowLanguageStatisticsCommand = new RelayCommand(ExecuteShowLanguageStatistics, CanExecute_Command);

            _tourRequestService.ShowTourRequests(TourRequests.ToList(), Locations, Languages);
            foreach (TourRequest tourRequest in TourRequests)
            {
                LocationsComboBox.Add(tourRequest.Location.CityAndCountry);
                LanguageComboBox.Add(tourRequest.Language);
            }

            LocationsComboBox.Insert(0, "");


        }
        private void ExecuteShowLocationStatistics(object parameter)
        {
            if (SelectedLocation != null)
            {
                string location = SelectedLocation?.Replace(",", "").Replace(" ", "");

                if (!string.IsNullOrEmpty(location))
                {
                    int numRequestsByLocation = _tourRequestService.GetRequestCountForLocation(location, TourRequests, SelectedYear, SelectedMonth);
                    MessageBox.Show($"Number of tour requests for {SelectedLocation}: {numRequestsByLocation}");
                }
                else
                {
                    MessageBox.Show("Invalid location selected.");
                }
            }
        }







        private void ExecuteShowLanguageStatistics(object parameter)
        {
            if (SelectedLanguage != null)
            {
                int numRequestsByLanguage = _tourRequestService.GetRequestCountForLanguage(SelectedLanguage, TourRequests, SelectedYear, SelectedMonth);
                MessageBox.Show($"Number of tour requests for {SelectedLanguage.Name}: {numRequestsByLanguage}");
            }
        }
        private bool CanExecute_Command(object arg)
        {
            return true;
        }

    }
}
