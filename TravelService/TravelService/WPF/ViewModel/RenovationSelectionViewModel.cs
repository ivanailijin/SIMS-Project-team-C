using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TravelService.Applications.UseCases;
using TravelService.Applications.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.WPF.Services;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class RenovationSelectionViewModel : ViewModelBase, INavigationInterface
    {
        public AccommodationService _accommodationService;

        public AccommodationRenovationService _renovationService;

        public LocationService _locationService;

        public RenovationSelectionView RenovationSelectionView { get; set; }
        public Action CloseAction { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand ScheduleRenovationCommand { get; set; }
        public Accommodation SelectedAccommodation { get; set; }
        public static ObservableCollection<Accommodation> Accommodations { get; set; }
        public static List<Location> Locations { get; set; }
        public Owner Owner { get; set; }


        public RenovationSelectionViewModel(Owner owner, RenovationSelectionView renovationSelectionView)
        {
            InitializeCommands();
            this.Owner = owner;
            RenovationSelectionView = renovationSelectionView;
            _accommodationService = new AccommodationService(Injector.CreateInstance<IAccommodationRepository>());
            _locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            _renovationService = new AccommodationRenovationService(Injector.CreateInstance<IAccommodationRenovationRepository>());
            List<Accommodation> accommodations = _accommodationService.GetOwnersAccommodations(Owner.Id);
            Accommodations = new ObservableCollection<Accommodation>(_renovationService.GetRenovationData(accommodations));
            Locations = new List<Location>(_locationService.GetAll());

            foreach (Accommodation accommodation in Accommodations)
            {
                accommodation.Location = Locations.Find(l => l.Id == accommodation.LocationId);
            }
        }
        private void InitializeCommands()
        {
            CancelCommand = new RelayCommand(Execute_CancelCommand, CanExecute_Command);
            ScheduleRenovationCommand = new RelayCommand(Execute_ScheduleRenovationCommand, CanExecute_Command);
        }
        private void Execute_ScheduleRenovationCommand(object obj)
        {
            if (SelectedAccommodation != null && Owner != null)
            {
                RenovationSchedulingView renovationSchedulingView = new RenovationSchedulingView(Owner, SelectedAccommodation);
                OwnerWindow ownerWindow = Window.GetWindow(RenovationSelectionView) as OwnerWindow;
                ownerWindow?.SwitchToPage(renovationSchedulingView);
            }
            else
            {
                MessageBox.Show("Morate izabrati smestaj za koji zelite da zakazete renoviranje!");
            }
        }
        private void Execute_CancelCommand(object obj)
        {
            RenovationSelectionView.GoBack();
        }

        private bool CanExecute_Command(object arg)
        {
            return true;
        }

        public void GoBack()
        {
            RenovationSelectionView.GoBack();
        }
    }
}
