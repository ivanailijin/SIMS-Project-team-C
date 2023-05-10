using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Application.UseCases;
using TravelService.Application.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.Repository;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class TourTrackingViewModel : ViewModelBase
    {
        public Action CloseAction { get; set; }

        private readonly TourService _tourService;

        private readonly LocationService _locationService;

        private readonly LanguageService _languageService;

        private readonly CheckPointService _checkpointService;

        private RelayCommand trackTourCommand;
        public RelayCommand TrackTourCommand
        {
            get => trackTourCommand;
            set
            {
                if (value != trackTourCommand)
                {
                    trackTourCommand = value;
                    OnPropertyChanged();
                }
            }
        }
        public Tour SelectedTour { get; set; }
        public Guest2 Guest2 { get; set; }
        public TourTrackingViewModel(Tour selectedTour,Guest2 guest2)
        {
            _tourService = new TourService(Injector.CreateInstance<ITourRepository>());
            _locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            _languageService = new LanguageService(Injector.CreateInstance<ILanguageRepository>());
            _checkpointService = new CheckPointService(Injector.CreateInstance<ICheckPointRepository>());

            //Tours = _tourService.GetAll();
            List<Location> Locations = _locationService.GetAll();
            List<Language> Languages = _languageService.GetAll();
            List<CheckPoint> CheckPoints = _checkpointService.GetAll();
            SelectedTour = selectedTour;
            Guest2 = guest2;
            //ActiveTours = _tourReservationRepository.showGuestsTours(convertTourList(Tours), Locations, Languages, CheckPoints, ActiveTours, convertTourReservationList(TourReservations), Guest2);


            TrackTourCommand = new RelayCommand(Execute_TrackTourCommand, CanExecute_Command);

        }
        private bool CanExecute_Command(object parameter)
        {
            return true;
        }
        private void Execute_TrackTourCommand(object sender)
        {
            if (SelectedTour != null)
            {
                JoinTourView joinTourView = new JoinTourView(SelectedTour, Guest2);
                joinTourView.Show();
            }
        }
    }
}
