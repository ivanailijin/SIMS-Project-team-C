using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TravelService.Application.UseCases;
using TravelService.Application.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class GuestsRequestsStatisticsViewModel : ViewModelBase
    {
        private readonly TourRequestService _tourRequestService;
        private readonly LanguageService _languageService;
        private readonly LocationService _locationService;
        public ObservableCollection<TourRequest> GuestsRequests { get; set; }
        public ObservableCollection<Location> Locations { get; set; }
        public List<int> Years { get; set; }
        public Guest2 Guest2 { get; set; }
        public Action CloseAction { get; set; }

        private string _approvedRequests;
        public string ApprovedRequests
        {
            get => _approvedRequests;
            set
            {
                if (value != _approvedRequests)
                {
                    _approvedRequests = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _approvedRequestsByYear;
        public string ApprovedRequestsByYear
        {
            get => _approvedRequestsByYear;
            set
            {
                if (value != _approvedRequestsByYear)
                {
                    _approvedRequestsByYear = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _invalidRequests;
        public string InvalidRequests
        {
            get => _invalidRequests;
            set
            {
                if (value != _invalidRequests)
                {
                    _invalidRequests = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _invalidRequestsByYear;
        public string InvalidRequestsByYear
        {
            get => _invalidRequestsByYear;
            set
            {
                if (value != _invalidRequestsByYear)
                {
                    _invalidRequestsByYear = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _selectedYear;
        public int SelectedYear
        {
            get => _selectedYear;
            set
            {
                if (value != _selectedYear)
                {
                    _selectedYear = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _averageGuestNumber;
        public string AverageGuestNumber
        {
            get => _averageGuestNumber;
            set
            {
                if (value != _averageGuestNumber)
                {
                    _averageGuestNumber = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _averageGuestNumberByYear;
        public string AverageGuestNumberByYear
        {
            get => _averageGuestNumberByYear;
            set
            {
                if (value != _averageGuestNumberByYear)
                {
                    _averageGuestNumberByYear = value;
                    OnPropertyChanged();
                }
            }
        }
        private ObservableCollection<LocationDataPoint> _locationDataPoints;
        public ObservableCollection<LocationDataPoint> LocationDataPoints
        {
            get => _locationDataPoints;
            set
            {
                if (value != _locationDataPoints)
                {
                    _locationDataPoints = value;
                    OnPropertyChanged();
                }
            }
        }
        private RelayCommand _percentageByYearCommand;
        public RelayCommand PercentageByYearCommand
        {
            get => _percentageByYearCommand;
            set
            {
                if (value != _percentageByYearCommand)
                {
                    _percentageByYearCommand = value;
                    OnPropertyChanged();
                }
            }
        }
        private RelayCommand _guestNumberByYearCommand;
        public RelayCommand GuestNumberByYearCommand
        {
            get => _guestNumberByYearCommand;
            set
            {
                if (value != _guestNumberByYearCommand)
                {
                    _guestNumberByYearCommand = value;
                    OnPropertyChanged();
                }
            }
        }
        private RelayCommand _languageGraphCommand;
        public RelayCommand LanguageGraphCommand
        {
            get => _languageGraphCommand;
            set
            {
                if (value != _languageGraphCommand)
                {
                    _languageGraphCommand = value;
                    OnPropertyChanged();
                }
            }
        }
        

        public GuestsRequestsStatisticsViewModel(Guest2 guest2)
        {
            _tourRequestService = new TourRequestService(Injector.CreateInstance<ITourRequestRepository>());
            _locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            Guest2 = guest2;
            double RequestNumber = 0;
            List<TourRequest> tourRequests = new List<TourRequest>(_tourRequestService.GetAll());
            List<Location> locations = new List<Location>(_locationService.GetAll());
            List<TourRequest> guestsRequests = new List<TourRequest>(_tourRequestService.FindGuestsRequests(tourRequests, guest2.Id));
            
            Locations = new ObservableCollection<Location>(locations);
            GuestsRequests = new ObservableCollection<TourRequest>(guestsRequests);
            Years = _tourRequestService.FindYears(GuestsRequests);
            List<LocationDataPoint> locationDataPoints = new List<LocationDataPoint>(_tourRequestService.GetLocationDataPoints(locations, RequestNumber, GuestsRequests));
            LocationDataPoints = new ObservableCollection<LocationDataPoint>(_tourRequestService.CalculateLocationDataPointPositions(locationDataPoints, Locations, GuestsRequests));
            AverageGuestNumber = _tourRequestService.FindAverageGuestNumber(GuestsRequests).ToString();

            string approvedRequestsPercentage = _tourRequestService.GetApprovedRequestsPercentage(GuestsRequests).ToString();
            ApprovedRequests = approvedRequestsPercentage + '%';
            string invalidRequests = _tourRequestService.GetInvalidRequestsPercentage(GuestsRequests).ToString();
            InvalidRequests = invalidRequests + '%';

            PercentageByYearCommand = new RelayCommand(Execute_PercentageByYearCommand, CanExecute_Command);
            LanguageGraphCommand = new RelayCommand(Execute_LanguageGraphCommand, CanExecute_Command);
        }

        private bool CanExecute_Command(object parameter)
        {
            return true;
        }

        private void Execute_PercentageByYearCommand(object sender)
        {
            string approvedRequestsByYear = _tourRequestService.GetApprovedRequestsPercentageByYear(GuestsRequests, SelectedYear).ToString();
            ApprovedRequestsByYear = approvedRequestsByYear + '%';
            string invalidRequestsByYear = _tourRequestService.GetInvalidRequestsPercentageByYear(GuestsRequests, SelectedYear).ToString();
            InvalidRequestsByYear = invalidRequestsByYear + '%';
            AverageGuestNumberByYear = _tourRequestService.GetGuestNumberByYear(GuestsRequests, SelectedYear).ToString();
        }
        private void Execute_LanguageGraphCommand(object sender)
        {
            LanguageStatisticsGraph languageStatisticsGraph = new LanguageStatisticsGraph(Guest2);
            languageStatisticsGraph.Show();
        }
    }
}
