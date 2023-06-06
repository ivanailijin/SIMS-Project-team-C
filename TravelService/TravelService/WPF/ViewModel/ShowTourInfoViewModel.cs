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
    public class ShowTourInfoViewModel : ViewModelBase
    {
        public Tour SelectedTour { get; set; }
        public Tour CurrentSelectedTour { get; set; }
        public Guest2 Guest2 { get; set; }
        public Action CloseAction { get; set; }

        private readonly TourReservationService _tourReservationService;

        private readonly TourService _tourService;

        private readonly LocationService _locationService;

        private readonly LanguageService _languageService;

        private readonly CheckPointService _checkpointService;
        public ObservableCollection<Tour> Tours { get; set; }
        public ObservableCollection<Tour> CurrentTours { get; set; }
        public List<Location> Locations { get; set; }
        public List<Language> Languages { get; set; }
        public List<CheckPoint> CheckPoints { get; set; }
        public ShowTourInfoViewModel(Guest2 guest2, Tour selectedTour) {

            _tourReservationService = new TourReservationService(Injector.CreateInstance<ITourReservationRepository>());
            _tourService = new TourService(Injector.CreateInstance<ITourRepository>());
            _locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            _languageService = new LanguageService(Injector.CreateInstance<ILanguageRepository>());
            _checkpointService = new CheckPointService(Injector.CreateInstance<ICheckPointRepository>());
            Tours = new ObservableCollection<Tour>(_tourService.GetAll());
            CurrentTours = new ObservableCollection<Tour>();
            Locations = new List<Location>(_locationService.GetAll());
            Languages = new List<Language>(_languageService.GetAll());
            CheckPoints = new List<CheckPoint>(_checkpointService.GetAll());
            Guest2 = guest2;
            SelectedTour = selectedTour;
            CurrentSelectedTour = _tourReservationService.FindTourInActiveTours(selectedTour,Tours.ToList(), Locations, Languages, CheckPoints);
            CurrentTours.Add(CurrentSelectedTour);
        }
    }
}
