using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows;
using System.Windows.Threading;
using TravelService.Applications.UseCases;
using TravelService.Applications.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class ReservationsViewModel : ViewModelBase
    {
        private readonly AccommodationReservationService _accommodationReservationService;
        private readonly AccommodationService _accommodationService;
        private readonly ReservationRequestService _reservationRequestService;
        public AccommodationReservation SelectedActiveReservation { get; set; }
        public ReservationsView ReservationsView { get; set; }
        public Guest1 Guest1 { get; set; }

        public event PropertyChangedEventHandler RequestStatusChanged;

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

        public ReservationsViewModel(ReservationsView reservationsView, Guest1 guest1)
        {
            Guest1 = guest1;
            ReservationsView = reservationsView;
            _accommodationReservationService = new AccommodationReservationService(Injector.CreateInstance<IAccommodationReservationRepository>());
            _reservationRequestService = new ReservationRequestService(Injector.CreateInstance<IReservationRequestRepository>());
            _accommodationService = new AccommodationService(Injector.CreateInstance<IAccommodationRepository>());

            List<AccommodationReservation> Reservations = new List<AccommodationReservation>(_accommodationReservationService.FindByGuestId(Guest1.Id));
            _accommodationReservationService.GetAccommodationData(Reservations);
            _accommodationReservationService.GetLocationData(Reservations);
            ActiveReservations = new ObservableCollection<AccommodationReservation>(Reservations);

            List<ReservationRequest> Requests = new List<ReservationRequest>(_reservationRequestService.FindRequestsByGuestId(Guest1.Id));
            Requests = _reservationRequestService.GetReservationData(Requests);
            Requests = _reservationRequestService.GetLocationData(Requests);
            Requests = _reservationRequestService.GetAccommodationData(Requests);
            RequestsForDelaying = new ObservableCollection<ReservationRequest>(Requests);

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
                SendReservationRequestView sendReservationRequestView = new SendReservationRequestView(RequestsForDelaying, SelectedActiveReservation, Guest1);
                sendReservationRequestView.Show();
            }
            else
            {
                MessageBox.Show("Odaberite smestaj za slanje zahteva za pomeranje rezervacije", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Execute_CancelReservation(object sender)
        {
            bool IsCancellingRequestFulfilled = _accommodationReservationService.IsCancellingLimitFulfilled(SelectedActiveReservation.Id);
            bool IsMinimumCancellingDaysFulfilled = _accommodationReservationService.IsMinimumDaysForCancellingFulfilled(SelectedActiveReservation);
            Accommodation accommodation = _accommodationService.FindById(SelectedActiveReservation.AccommodationId);

            if (!IsMinimumCancellingDaysFulfilled)
            {
                MessageBox.Show($"Otkazivanje {accommodation.Name} je moguce {accommodation.DaysBeforeCancellingReservation} dana pre check-in datuma!");
                return;
            }
            else if (!IsCancellingRequestFulfilled)
            {
                MessageBox.Show("Otkazivanje je moguce 24h pre check-in datuma!");
                return;
            }
            if (SelectedActiveReservation != null)
            {
                CancelReservationConfirmView cancelReservation = new CancelReservationConfirmView(ActiveReservations, SelectedActiveReservation, Guest1);
                cancelReservation.Show();
            }
            else
            {
                MessageBox.Show("Odaberite smestaj za otkazivanje rezervacije.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}

