using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using TravelService.Applications.UseCases;
using TravelService.Applications.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.WPF.Services;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class AccommodationStatisticsViewModel : INavigationInterface
    {
        public AccommodationService _accommodationService;

        public LocationService _locationService;

        public AccommodationStatisticsService _statisticsService;
        public Location MostPopularLocation { get; set; }
        public Location LeastPopularLocation { get; set; }
        public AccommodationStatisticsView AccommodationStatisticsView { get; set; }
        public Action CloseAction { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand ShowReviewCommand { get; set; }
        public RelayCommand AddAccommodationCommand { get; set; }
        public RelayCommand DeleteAccommodationCommand { get; set; }

        public ObservableCollection<Location> MostPopularLocations { get; set; }
        public ObservableCollection<Location> LeastPopularLocations { get; set; }
        public Accommodation SelectedAccommodation { get; set; }
        public Location SelectedAddLocation { get; set; }
        public Location SelectedDeleteLocation { get; set; }
        public static ObservableCollection<Accommodation> Accommodations { get; set; }
        public static List<Location> Locations { get; set; }
        public Owner Owner { get; set; }

        public AccommodationStatisticsViewModel(Owner owner, AccommodationStatisticsView accommodationStatisticsView)
        {
            InitializeCommands();
            AccommodationStatisticsView = accommodationStatisticsView;
            this.Owner = owner;
            _accommodationService = new AccommodationService(Injector.CreateInstance<IAccommodationRepository>());
            _locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            Accommodations = new ObservableCollection<Accommodation>(_accommodationService.GetOwnersAccommodations(Owner.Id));
            Locations = new List<Location>(_locationService.GetAll());
            _statisticsService = new AccommodationStatisticsService();
            MostPopularLocation = _statisticsService.GetMostPopularLocation();
            LeastPopularLocation = _statisticsService.GetLeastPopularLocation();
            MostPopularLocations = new ObservableCollection<Location>(_statisticsService.GetLocationsWithHighestParameters());
            LeastPopularLocations = new ObservableCollection<Location>(_statisticsService.GetLocationsWithLowestParameters());

            foreach (Accommodation accommodation in Accommodations)
            {
                accommodation.Location = Locations.Find(l => l.Id == accommodation.LocationId);
            }
        }

        public void GoBack()
        {
            AccommodationStatisticsView.GoBack();
        }
        private void InitializeCommands()
        {
            CancelCommand = new RelayCommand(Execute_CancelCommand, CanExecute_Command);
            ShowReviewCommand = new RelayCommand(Execute_ShowReviewCommand, CanExecute_Command);
            AddAccommodationCommand = new RelayCommand(Execute_AddAccommodationCommand, CanExecute_Command);
            DeleteAccommodationCommand = new RelayCommand(Execute_DeleteAccommodationCommand, CanExecute_Command);
        }
        private void Execute_ShowReviewCommand(object obj)
        {
            if (SelectedAccommodation != null)
            {
                AccommodationYearStatisticsView accommodationYearStatisticsView = new AccommodationYearStatisticsView(SelectedAccommodation, Owner);
                OwnerWindow ownerWindow = Window.GetWindow(AccommodationStatisticsView) as OwnerWindow;
                ownerWindow?.SwitchToPage(accommodationYearStatisticsView);
            }
            else
            {
                MessageBox.Show("Niste izabrali smestaj za prikaz statistike!");
            }
        }
        private void Execute_DeleteAccommodationCommand(object obj)
        {
            ClosingAccommodationView closingAccommodationView = new ClosingAccommodationView(SelectedDeleteLocation);
            OwnerWindow ownerWindow = Window.GetWindow(AccommodationStatisticsView) as OwnerWindow;
            ownerWindow?.SwitchToPage(closingAccommodationView);
        }
        private void Execute_AddAccommodationCommand(object obj)
        {
            AddAccommodation addAccommodation = new AddAccommodation(Owner, SelectedAddLocation);
            OwnerWindow ownerWindow = Window.GetWindow(AccommodationStatisticsView) as OwnerWindow;
            ownerWindow?.SwitchToPage(addAccommodation);
        }
        private void Execute_CancelCommand(object obj)
        {
            GoBack();
        }

        private bool CanExecute_Command(object arg)
        {
            return true;
        }
    }
}
