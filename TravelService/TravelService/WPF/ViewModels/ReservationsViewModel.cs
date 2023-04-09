using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using TravelService.Domain.Model;
using System.Collections.ObjectModel;
using TravelService.Commands;
using TravelService.WPF.Views;
using TravelService.Repository;
using TravelService.Application.UseCases;
using TravelService.Application.Utils;
using TravelService.Domain.RepositoryInterfaces;

namespace TravelService.WPF.ViewModels
{
    public class ReservationsViewModel : ViewModelBase
    {
        private readonly AccommodationReservationService _accommodationReservationService;
        private readonly LocationService _locationService;
        private readonly AccommodationService _accommodationService;
        public AccommodationReservation SelectedActiveReservation { get; set; }
        public Guest1 Guest1 { get; set; }
        public Action CloseAction { get; set; }
        public static ObservableCollection<AccommodationReservation> ActiveReservations { get; set; }
        public static ObservableCollection<ReservationRequest> RequestsForDelaying { get; set; }

        private RelayCommand sendRequest;
        public RelayCommand SendRequestCommand
        {
            get => sendRequest;
            set
            {
                if (value != sendRequest)
                {
                    sendRequest = value;
                    OnPropertyChanged();
                }

            }
        }

        private RelayCommand cancelReservation;
        public RelayCommand CancelReservationCommand
        {
            get => cancelReservation;
            set
            {
                if (value != cancelReservation)
                {
                    cancelReservation = value;
                    OnPropertyChanged();
                }

            }
        }

        public ReservationsViewModel(Guest1 guest1)
        {
            Guest1 = guest1;
            List<Location> Locations = new List<Location>();
            _accommodationReservationService = new AccommodationReservationService(Injector.CreateInstance<IAccommodationReservationRepository>());
            ActiveReservations = new ObservableCollection<AccommodationReservation>(_accommodationReservationService.GetAll());

            SendRequestCommand = new RelayCommand(Execute_SendRequest, CanExecute_Command);
            CancelReservationCommand = new RelayCommand(Execute_CancelReservation, CanExecute_Command);
        }
        
        private bool CanExecute_Command(object parameter)
        {
            return true;
        }

        private void Execute_SendRequest(object sender)
        {
            ReservationRequestView reservationRequestView = new ReservationRequestView();
            reservationRequestView.Show();
        }

        private void Execute_CancelReservation(object sender)
        {
            CancelReservationConfirmView cancelReservation = new CancelReservationConfirmView();
            cancelReservation.Show();
        }
    }
}

