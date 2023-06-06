using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using TravelService.Applications.UseCases;
using TravelService.Applications.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.WPF.View;
using System.Windows.Controls;
using System.Windows;

namespace TravelService.WPF.ViewModel
{
    public class ActiveToursViewModel :ViewModelBase
    {
        public ContentControl PopupContent;
        public Frame PopupFrame { get; set; }
        public NavigationService NavigationService { get; set; }
        public ActiveToursView ActiveToursView { get; set; }    
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
        public Guide Guide;

        public ActiveToursViewModel(ActiveToursView activeToursView,Tour selectedTour, NavigationService navigationService)
        {
            ActiveToursView = activeToursView;
            NavigationService = navigationService;
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
           // CancelCommand = new RelayCommand(Execute_CancelCommand, CanExecute_Command);
            PopupFrame = activeToursView.MyPopupFrame;

        }



        private List<Tour> convertTourList(ObservableCollection<Tour> observableCollection)
        {
            List<Tour> convertedList = observableCollection.ToList();
            return convertedList;
        }
        private void OpenPopupPage(Page CheckPointView)
        {
            PopupFrame.Navigate(CheckPointView);
            PopupFrame.Visibility = Visibility.Visible;

        }
        private void Execute_StartCommand(object obj)
        {

            if (SelectedTour != null)
            {
                var checkPointView = new CheckPointView(SelectedTour, NavigationService);
                OpenPopupPage(checkPointView);
            }
        }
        private bool CanExecute_Command(object arg)
        {
            return true;
        }
        private void ShowPopupOnActiveToursView(Page suggestionForGuideView)
        {
            PopupContent.Content = suggestionForGuideView;
        }



    }
}
