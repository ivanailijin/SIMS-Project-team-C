using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
    public class ActiveToursViewModel :ViewModelBase
    {

        public readonly TourService _tourService;
        public readonly LocationService _locationService;
        public readonly LanguageService _languageService;
        public readonly CheckPointService _checkPointService;
        public Action CloseAction { get; set; } 
        public static List<Location> Locations { get; set; }
        public static List<CheckPoint> CheckPoints { get; set; }
        public static List<Tour> ActiveTours { get; set; }
        public static List<Language> Languages { get; set; }

        public static ObservableCollection<Tour> Tours { get; set; }
        private ObservableCollection<Tour> _activeTours;

        public Tour SelectedTour { get; set; }
        public CheckPoint SelectedCheckPoint { get; set; }
        public RelayCommand StartCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }

        public ActiveToursViewModel(Tour selectedTour)
        {
            
            _tourService = new TourService(Injector.CreateInstance<ITourRepository>());
            _locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            _checkPointService = new CheckPointService(Injector.CreateInstance<ICheckPointRepository>());
            _languageService = new LanguageService(Injector.CreateInstance<ILanguageRepository>());

            Tours = new ObservableCollection<Tour>(_tourService.GetAll());
            Locations = new List<Location>(_locationService.GetAll());
            Languages = new List<Language>(_languageService.GetAll());
            CheckPoints = new List<CheckPoint>(_checkPointService.GetAll());
            ActiveTours = new List<Tour>();

            SelectedTour = selectedTour;           
            ActiveTours = _tourService.showAllActiveTours(convertTourList(Tours), Locations, Languages, CheckPoints, ActiveTours);
            StartCommand = new RelayCommand(Execute_StartCommand, CanExecute_Command);
            CancelCommand = new RelayCommand(Execute_CancelCommand, CanExecute_Command);
        }



        private List<Tour> convertTourList(ObservableCollection<Tour> observableCollection)
        {
            List<Tour> convertedList = observableCollection.ToList();
            return convertedList;
        }
        private void Execute_StartCommand(object obj)
        {

            if (SelectedTour != null)
            {
                CheckPointView checkPointView = new CheckPointView(SelectedTour);

                checkPointView.Show();
                CloseAction();
            }
        }
        private bool CanExecute_Command(object arg)
        {
            return true;
        }
        private void Execute_CancelCommand(object obj)
        {
            CloseAction();
        }


    }
}
