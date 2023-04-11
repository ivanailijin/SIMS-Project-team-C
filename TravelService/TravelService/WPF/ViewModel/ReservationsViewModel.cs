using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Windows;
using System.Windows.Threading;
using TravelService.Application.UseCases;
using TravelService.Application.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class ReservationsViewModel : ViewModelBase
    {
        private readonly AccommodationReservationService _accommodationReservationService;
        private readonly LocationService _locationService;
        private readonly AccommodationService _accommodationService;
        private readonly ReservationRequestService _reservationRequestService;
        public AccommodationReservation SelectedActiveReservation { get; set; }
        public Guest1 Guest1 { get; set; }
        public Action CloseAction { get; set; }
        //public static ObservableCollection<ReservationRequest> RequestsForDelaying { get; set; }


        private ObservableCollection<AccommodationReservation> _reservations;
        public ObservableCollection<AccommodationReservation> ActiveReservations
        {
            get { return _reservations; }
            set
            {
                _reservations = value;
                OnPropertyChanged(nameof(ActiveReservations));
            }
        }

        private ObservableCollection<ReservationRequest> _requests;
        public ObservableCollection<ReservationRequest> RequestsForDelaying
        {
            get { return _requests; }
            set
            {
                _requests = value;
                OnPropertyChanged(nameof(RequestsForDelaying));
            }
        }

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
            _accommodationReservationService = new AccommodationReservationService(Injector.CreateInstance<IAccommodationReservationRepository>());
            _locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            _accommodationService = new AccommodationService(Injector.CreateInstance<IAccommodationRepository>());

            ActiveReservations = new ObservableCollection<AccommodationReservation>(_accommodationReservationService.FindByGuestId(Guest1.Id));
            List<Location> locations = new List<Location>(_locationService.GetAll());
            _accommodationReservationService.SetLocation(locations);

            _reservationRequestService = new ReservationRequestService(Injector.CreateInstance<IReservationRequestRepository>());
            RequestsForDelaying = new ObservableCollection<ReservationRequest>(_reservationRequestService.FindRequestsByGuestId(Guest1.Id));
            _reservationRequestService.SetStatus();

            SendRequestCommand = new RelayCommand(Execute_SendRequest, CanExecute_Command);
            CancelReservationCommand = new RelayCommand(Execute_CancelReservation, CanExecute_Command);
        }

        private bool CanExecute_Command(object parameter)
        {
            return true;
        }

        private void Execute_SendRequest(object sender)
        {
            if (SelectedActiveReservation != null)
            {
                SendReservationRequestView sendReservationRequestView = new SendReservationRequestView(SelectedActiveReservation, Guest1);
                sendReservationRequestView.Show();
            }
            else
            {
                MessageBox.Show("Please select reservation for sending moving request.");
            }
        }

        private void Execute_CancelReservation(object sender)
        {
            bool IsCancellingRequestFulfilled = _accommodationReservationService.IsCancellingLimitFulfilled(SelectedActiveReservation.Id);
            bool IsMinimumCancellingDaysFulfilled = _accommodationReservationService.IsMinimumDaysForCancellingFulfilled(SelectedActiveReservation);
            Accommodation accommodation = _accommodationService.FindById(SelectedActiveReservation.AccommodationId);

            if (!IsMinimumCancellingDaysFulfilled)
            {
                MessageBox.Show($"Cancellation for {accommodation.Name} is possible {accommodation.DaysBeforeCancellingReservation} days before check-in date!");
                return;
            }
            else if (!IsCancellingRequestFulfilled)
            {
                MessageBox.Show("Cancellation is possible 24 hours before check-in date!");
                return;
            }
            if (SelectedActiveReservation != null)
            {
                CancelReservationConfirmView cancelReservation = new CancelReservationConfirmView(ActiveReservations, SelectedActiveReservation, Guest1);
                cancelReservation.Show();
            }
            else
            {
                MessageBox.Show("Please select reservation for cancelling.");
            }
        }
    }
}

