using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Applications.UseCases;
using TravelService.Applications.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;

namespace TravelService.WPF.ViewModel
{
    public class ShowGuestsNotificationViewModel : ViewModelBase
    {
        private readonly NewTourNotificationService _notificationService;

        private readonly TourService _tourService;

        private readonly TourReviewService _tourReviewService;

        private readonly LocationService _locationService;

        private readonly LanguageService _languageService;

        private readonly CheckPointService _checkpointService;

        public NewTourNotification SelectedNotification { get; set; }
        public ObservableCollection<Tour> Tours { get; set; }
        public Guest2 Guest2 { get; set; }
        public Action CloseAction { get; set; }
        public ObservableCollection<NewTourNotification> Notifications { get; set; }

        public ShowGuestsNotificationViewModel(NewTourNotification selectedNotification, Guest2 guest2) 
        {
            _notificationService = new NewTourNotificationService(Injector.CreateInstance<INewTourNotificationRepository>());
            _tourService = new TourService(Injector.CreateInstance<ITourRepository>());
            _tourReviewService = new TourReviewService(Injector.CreateInstance<ITourReviewRepository>());
            _locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            _languageService = new LanguageService(Injector.CreateInstance<ILanguageRepository>());
            _checkpointService = new CheckPointService(Injector.CreateInstance<ICheckPointRepository>());
            List<Tour> tours = _tourService.GetAll();
            List<Location> Locations = _locationService.GetAll();
            List<Language> Languages = _languageService.GetAll();
            List<CheckPoint> CheckPoints = _checkpointService.GetAll();

            Guest2 = guest2;
            SelectedNotification = selectedNotification;
            Tour tour = _notificationService.FindTourById(selectedNotification.TourId,tours);
            Tours = new ObservableCollection<Tour>(_notificationService.ShowTourList(Locations, Languages, CheckPoints, tours, tour));

        }
    }
}
