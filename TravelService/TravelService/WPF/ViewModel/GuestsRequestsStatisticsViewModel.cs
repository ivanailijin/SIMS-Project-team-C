using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public class GuestsRequestsStatisticsViewModel : ViewModelBase
    {
        private readonly TourRequestService _tourRequestService;
        private readonly LanguageService _languageService;
        public ObservableCollection<TourRequest> GuestsRequests { get; set; }
        public ObservableCollection<Language> Languages { get; set; }
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
        private ObservableCollection<DataPoint> _dataPoints;
        public ObservableCollection<DataPoint> DataPoints
        {
            get => _dataPoints;
            set
            {
                if (value != _dataPoints)
                {
                    _dataPoints = value;
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
        public GuestsRequestsStatisticsViewModel(Guest2 guest2)
        {
            _tourRequestService = new TourRequestService(Injector.CreateInstance<ITourRequestRepository>());
            _languageService = new LanguageService(Injector.CreateInstance<ILanguageRepository>());

            List<TourRequest> tourRequests = new List<TourRequest>(_tourRequestService.GetAll());
            List<Language> languages = new List<Language>(_languageService.GetAll());
            List<TourRequest> guestsRequests = new List<TourRequest>(_tourRequestService.FindGuestsRequests(tourRequests, guest2.Id));
            double RequestNumber = 0;

            Languages = new ObservableCollection<Language>(languages);
            GuestsRequests = new ObservableCollection<TourRequest>(guestsRequests);
            DataPoints = new ObservableCollection<DataPoint>(_tourRequestService.GetDataPoints(languages, RequestNumber,GuestsRequests));
            CalculateDataPointPositions();
            Guest2 = guest2;

            string approvedRequestsPercentage = _tourRequestService.GetApprovedRequestsPercentage(GuestsRequests).ToString();
            ApprovedRequests = approvedRequestsPercentage + '%';
            string invalidRequests = _tourRequestService.GetInvalidRequestsPercentage(GuestsRequests).ToString();
            InvalidRequests = invalidRequests + '%';
            
            PercentageByYearCommand = new RelayCommand(Execute_PercentageByYearCommand, CanExecute_Command);
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
        }
        private void CalculateDataPointPositions()
        {
            // Get the maximum values for X and Y axes
            double maxX = 300; // Maximum value on X-axis (adjust as needed)
            double maxY = 50; // Maximum value on Y-axis (adjust as needed)

            for (int i = 0; i < DataPoints.Count; i++)
            {
                var dataPoint = DataPoints[i];

                // Calculate the X and Y positions based on the language and number of requests
                double x = CalculateXPosition(dataPoint.Language, maxX);
                double y = CalculateYPosition(dataPoint.RequestNumber, maxY);

                // Set the X and Y values of the data point
                dataPoint.Language = x.ToString();
                dataPoint.RequestNumber = y;
                
                dataPoint.Id = i;
            }
        }
        private double CalculateXPosition(string language, double maxX)
        {
            int languageIndex = Array.IndexOf(Languages.Select(l => l.Name).ToArray(), language);
            double interval = maxX / (Languages.Count - 1);
            double x = (languageIndex) * interval;
            
            return Math.Round(x, 2);
        }

        private double CalculateYPosition(double requests, double maxY)
        {
            double y = (double)requests / GuestsRequests.Count * maxY;
            return Math.Round(y, 2);
        }

    }
    public class DataPoint
        {
            public int Id { get; set; }
            public string Language { get; set; }
            public string LanguageName { get; set; }
            public double RequestNumber { get; set; }
            public double LanguageRequestNumber { get; set; }
        }
}
