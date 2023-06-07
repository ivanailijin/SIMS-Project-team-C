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
    public class PastToursViewModel : ViewModelBase
    {
        public Frame PopupFrame { get; set; }
        public PastTours PastTours { get; set; }
        public NavigationService NavigationService { get; set; }

        public readonly TourService _tourService;
        public readonly LocationService _locationService;
        public readonly LanguageService _languageService;
        public readonly CheckPointService _checkPointService;
        public Action CloseAction { get; set; }
        public static List<Location> Locations { get; set; }
        public static List<CheckPoint> CheckPoints { get; set; }
        public static List<Tour> PastTour { get; set; }
        public static List<Language> Languages { get; set; }

        public static ObservableCollection<Tour> Tours { get; set; }


        public Tour SelectedTour { get; set; }
        public Guide Guide { get; set; }
        public Guest SelectedGuest { get; set; }
        public RelayCommand StatsCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand ReviewsCommand { get; set; }


        public PastToursViewModel(PastTours pastTours,Tour selectedTour, Guide guide,NavigationService navigationService)
        {
            PastTours = pastTours;
            NavigationService = navigationService;
            this.Guide = guide;
            _tourService = new TourService(Injector.CreateInstance<ITourRepository>());
            _locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            _checkPointService = new CheckPointService(Injector.CreateInstance<ICheckPointRepository>());
            _languageService = new LanguageService(Injector.CreateInstance<ILanguageRepository>());

            Tours = new ObservableCollection<Tour>(_tourService.GetAll());
            Locations = new List<Location>(_locationService.GetAll());
            Languages = new List<Language>(_languageService.GetAll());
            CheckPoints = new List<CheckPoint>(_checkPointService.GetAll());
            PastTour = new List<Tour>();

            SelectedTour = selectedTour;

            PastTour = _tourService.ShowPastTour(convertTourList(Tours), Locations, Languages, CheckPoints, PastTour, Guide.Id);
            StatsCommand = new RelayCommand(Execute_StatsCommand, CanExecute_Command);
            PopupFrame = pastTours.MyPopupFrame;
            ReviewsCommand = new RelayCommand(Execute_ReviewsCommand,CanExecute_Command);   
        }


        private List<Tour> convertTourList(ObservableCollection<Tour> observableCollection)
        {
            List<Tour> convertedList = observableCollection.ToList();
            return convertedList;
        }
        private void Execute_StatsCommand(object obj)
        {

            if (SelectedTour != null)
            {
               NavigationService.Navigate( new TourStats(SelectedTour, NavigationService));
                
            }

            

           
            

        }
        private bool CanExecute_Command(object arg)
        {
            return true;
        }
        
        private void Execute_ReviewsCommand(object obj)
        {
            if (SelectedTour != null)
            {
                NavigationService.Navigate(new ShowGuestsView(SelectedTour, SelectedGuest, NavigationService));
               
            }
            
        }
        private void OpenPopupPageGuests(Page ShowGuestsView)
        {
            PopupFrame.Navigate(ShowGuestsView);
            PopupFrame.Visibility = System.Windows.Visibility.Visible;

        }

        private void OpenPopupPageStats(Page TourStats)
        {
            PopupFrame.Navigate(TourStats);
            PopupFrame.Visibility = System.Windows.Visibility.Visible;

        }

    }
}
