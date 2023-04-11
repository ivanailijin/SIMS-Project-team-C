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
    public class GuestsToursViewModel : ViewModelBase
    {
        private readonly TourService _tourService;

        private readonly LocationService _locationService;

        private readonly LanguageService _languageService;

        private readonly CheckPointService _checkpointService;

        private readonly GuestService _guestService;
        public List<Tour> PastTours { get; set; }
        public List<Tour> GuestsTours { get; set; }
        public Tour SelectedTour { get; set; }
        public Guest2 Guest2 { get; set; }
        public Action CloseAction { get; set; }

        private RelayCommand rateTour;
        public RelayCommand RateTourCommand
        {
            get => rateTour;
            set
            {
                if (value != rateTour)
                {
                    rateTour = value;
                    OnPropertyChanged();
                }
            }
        }
        public GuestsToursViewModel(Tour selectedTour, Guest2 guest2) 
        {
            _tourService = new TourService(Injector.CreateInstance<ITourRepository>());
            _locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            _languageService = new LanguageService(Injector.CreateInstance<ILanguageRepository>());
            _checkpointService = new CheckPointService(Injector.CreateInstance<ICheckPointRepository>());
            _guestService = new GuestService(Injector.CreateInstance<IGuestRepository>());
            List<Tour> Tours = _tourService.GetAll();
            List<Location> Locations = _locationService.GetAll();
            List<Language> Languages = _languageService.GetAll();
            List<CheckPoint> CheckPoints = _checkpointService.GetAll();
            List<Guest> Guests = _guestService.GetAll();
            //GuestsTours = new List<Tour>();
            SelectedTour = selectedTour;
            Guest2 = guest2;
            GuestsTours = _tourService.ShowGuestTourList(Tours, Locations, Languages, CheckPoints, Guests,Guest2);

            RateTourCommand = new RelayCommand(Execute_RateTour, CanExecute_Command);
        }
        private bool CanExecute_Command(object parameter)
        {
            return true;
        }
        private void Execute_RateTour(object sender)
        { 
            AddReviewView addReviewView = new AddReviewView(SelectedTour,Guest2);
            addReviewView.Show();
        }
    }
}
