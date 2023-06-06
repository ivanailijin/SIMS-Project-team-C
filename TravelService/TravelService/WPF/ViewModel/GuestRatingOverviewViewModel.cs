using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using TravelService.Applications.UseCases;
using TravelService.Applications.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.Repository;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class GuestRatingOverviewViewModel : ViewModelBase
    {
        private readonly AccommodationReservationService _accommodationReservationService;

        private readonly AccommodationService _accommodationService;

        private readonly Guest1Service _guest1Service;

        public GuestRatingOverview GuestRatingOverview { get; set; }
        public ObservableCollection<AccommodationReservation> UnratedReservations { get; set; }
        public AccommodationReservation SelectedReservation { get; set; }
        public Action CloseAction { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand RatingCommand { get; set; }

        public Owner Owner { get; set; }
        public GuestRatingOverviewViewModel(Owner owner, GuestRatingOverview guestRatingOverview = null)
        {
            InitializeCommands();
            this.Owner = owner;
            _accommodationReservationService = new AccommodationReservationService(Injector.CreateInstance<IAccommodationReservationRepository>());
            _accommodationService = new AccommodationService(Injector.CreateInstance<IAccommodationRepository>());
            _guest1Service = new Guest1Service(Injector.CreateInstance<IGuest1Repository>());
            List<AccommodationReservation> unratedReservations = new List<AccommodationReservation>();

            List<AccommodationReservation> reservationList = _accommodationReservationService.GetAll();
            unratedReservations = _accommodationReservationService.FindUnratedReservations(Owner.Id);
            unratedReservations = _accommodationReservationService.GetAccommodationData(unratedReservations);
            unratedReservations = _guest1Service.FindReservationGuest(unratedReservations);
            UnratedReservations = new ObservableCollection<AccommodationReservation>(unratedReservations);
            GuestRatingOverview = guestRatingOverview;
        }

        private void InitializeCommands()
        {
            CancelCommand = new RelayCommand(Execute_CancelCommand, CanExecute_Command);
            RatingCommand = new RelayCommand(Execute_RatingCommand, CanExecute_Command);
        }

        private void Execute_RatingCommand(object obj)
        {
            if (SelectedReservation != null)
            {
                GuestRatingView guestRatingView = new GuestRatingView(SelectedReservation, Owner, UnratedReservations);
                OwnerWindow ownerWindow = Window.GetWindow(GuestRatingOverview) as OwnerWindow;
                ownerWindow?.SwitchToPage(guestRatingView);
            }
            else
            {
                MessageBox.Show("Niste izabrali gosta!", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void Execute_CancelCommand(object obj)
        {
            GuestRatingOverview.GoBack();
        }

        private bool CanExecute_Command(object arg)
        {
            return true;
        }
    }
}
