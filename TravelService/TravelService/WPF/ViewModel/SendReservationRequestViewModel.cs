using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using TravelService.Application.UseCases;
using TravelService.Application.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;

namespace TravelService.WPF.ViewModel
{
    public class SendReservationRequestViewModel : ViewModelBase
    {
        private readonly ReservationRequestService _reservationRequestService;
        public AccommodationReservation SelectedReservation { get; set; }

        public ObservableCollection<ReservationRequest> RequestsForDelaying { get; set; }
        public Guest1 Guest1 { get; set; }
        public Action CloseAction { get; set; }

        private DateTime? _newCheckInDate;
        public DateTime? NewCheckInDate
        {
            get => _newCheckInDate;
            set
            {
                if (value != _newCheckInDate)
                {
                    _newCheckInDate = value;
                    OnPropertyChanged(nameof(NewCheckInDate));
                }
            }
        }

        private DateTime? _newCheckoOutDate;
        public DateTime? NewCheckOutDate
        {
            get => _newCheckoOutDate;
            set
            {
                if (value != _newCheckoOutDate)
                {
                    _newCheckoOutDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private RelayCommand sendCommand;
        public RelayCommand SendCommand
        {
            get => sendCommand;
            set
            {
                if (value != sendCommand)
                {
                    sendCommand = value;
                    OnPropertyChanged();
                }
            }
        }

        private RelayCommand cancelCommand;
        public RelayCommand CancelCommand
        {
            get => cancelCommand;
            set
            {
                if (value != cancelCommand)
                {
                    cancelCommand = value;
                    OnPropertyChanged();
                }
            }
        }

        public SendReservationRequestViewModel(ObservableCollection<ReservationRequest> requestsForDelaying, AccommodationReservation accommodationReservation, Guest1 guest1)
        {
            SelectedReservation = accommodationReservation;
            Guest1 = guest1;
            RequestsForDelaying = requestsForDelaying;

            _reservationRequestService = new ReservationRequestService(Injector.CreateInstance<IReservationRequestRepository>());

            SendCommand = new RelayCommand(Execute_SendCommand, CanExecute_Command);
            CancelCommand = new RelayCommand(Execute_CancelCommand, CanExecute_Command);
        }

        private bool CanExecute_Command(object parameter)
        {
            return true;
        }

        private void Execute_SendCommand(object sender)
        {
            if (NewCheckInDate != null && NewCheckOutDate != null)
            {
                DateTime newCheckInDate = NewCheckInDate.Value;
                DateTime newCheckOutDate = NewCheckOutDate.Value;
                ReservationRequest reservationRequest = new ReservationRequest(Guest1.Id, SelectedReservation.Id, newCheckInDate, newCheckOutDate);
                _reservationRequestService.Save(reservationRequest);
                List<ReservationRequest> Requests = new List<ReservationRequest>(_reservationRequestService.FindRequestsByGuestId(Guest1.Id));
                UpdateRequestData();
                CloseAction();
            }
            else
            {
                MessageBox.Show("Niste popunili sva polja\n za pomeranje rezervacije", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void UpdateRequestData()
        {
            List<ReservationRequest> Requests = new List<ReservationRequest>(_reservationRequestService.FindRequestsByGuestId(Guest1.Id));
            Requests = _reservationRequestService.GetReservationData(Requests);
            Requests = _reservationRequestService.GetLocationData(Requests);
            Requests = _reservationRequestService.GetAccommodationData(Requests);
            foreach (ReservationRequest reservationRequest in Requests)
            {
                RequestsForDelaying.Add(reservationRequest);
            }
        }

        private void Execute_CancelCommand(object sender)
        {
            CloseAction();
        }
    }
}
