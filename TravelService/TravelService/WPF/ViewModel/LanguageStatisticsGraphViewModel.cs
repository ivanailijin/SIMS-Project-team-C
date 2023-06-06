using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Applications.UseCases;
using TravelService.Applications.Utils;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;

namespace TravelService.WPF.ViewModel
{
    public class LanguageStatisticsGraphViewModel : ViewModelBase
    {
        private readonly TourRequestService _tourRequestService;
        private readonly LanguageService _languageService;
        public ObservableCollection<TourRequest> GuestsRequests { get; set; }
        public ObservableCollection<Language> Languages { get; set; }
        public Guest2 Guest2 { get; set; }
        public Action CloseAction { get; set; }

        private ObservableCollection<LanguageDataPoint> _languageDataPoints;
        public ObservableCollection<LanguageDataPoint> LanguageDataPoints
        {
            get => _languageDataPoints;
            set
            {
                if (value != _languageDataPoints)
                {
                    _languageDataPoints = value;
                    OnPropertyChanged();
                }
            }
        }
        public LanguageStatisticsGraphViewModel(Guest2 guest2)
        {
            _tourRequestService = new TourRequestService(Injector.CreateInstance<ITourRequestRepository>());
            _languageService = new LanguageService(Injector.CreateInstance<ILanguageRepository>());
            List<TourRequest> tourRequests = new List<TourRequest>(_tourRequestService.GetAll());
            List<Language> languages = new List<Language>(_languageService.GetAll());
            List<TourRequest> guestsRequests = new List<TourRequest>(_tourRequestService.FindGuestsRequests(tourRequests, guest2.Id));
            Languages = new ObservableCollection<Language>(languages);
            GuestsRequests = new ObservableCollection<TourRequest>(guestsRequests);
            double RequestNumber = 0;
            Guest2 = guest2;
            List<LanguageDataPoint> langaugeDataPoints = new List<LanguageDataPoint>(_tourRequestService.GetLanguageDataPoints(languages, RequestNumber, GuestsRequests));
            LanguageDataPoints = new ObservableCollection<LanguageDataPoint>(_tourRequestService.CalculateLanguageDataPointPositions(langaugeDataPoints, Languages, GuestsRequests));
        }
        private bool CanExecute_Command(object parameter)
        {
            return true;
        }
    }
}
