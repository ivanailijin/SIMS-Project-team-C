using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Application.UseCases;
using TravelService.Domain.Model;
using TravelService.Application.Utils;
using TravelService.Domain.RepositoryInterface;
using TravelService.Commands;

namespace TravelService.WPF.ViewModel
{
    public class SendReservationRequestViewModel : ViewModelBase
    {
        private readonly ReservationRequestService _reservationRequestService;
        public AccommodationReservation SelectedReservation { get; set; }
        public Guest1 Guest1 { get; set; }
        public Action CloseAction { get; set; }

        private DateTime _newCheckInDate;
        public DateTime NewCheckInDate
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

        private DateTime _newCheckoOutDate;
        public DateTime NewCheckOutDate
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

        public SendReservationRequestViewModel(AccommodationReservation accommodationReservation, Guest1 guest1)
        {
            SelectedReservation = accommodationReservation;
            Guest1 = guest1;

            _reservationRequestService = new ReservationRequestService(Injector.CreateInstance<IReservationRequestRepository>());
            _newCheckInDate = DateTime.Today;
            _newCheckoOutDate = DateTime.Today;

            SendCommand = new RelayCommand(Execute_SendCommand, CanExecute_Command);
            CancelCommand = new RelayCommand(Execute_CancelCommand, CanExecute_Command);
        }

        private bool CanExecute_Command(object parameter)
        {
            return true;
        }

        private void Execute_SendCommand(object sender)
        {
            ReservationRequest reservationRequest = new ReservationRequest(Guest1.Id, SelectedReservation.Id, NewCheckInDate, NewCheckOutDate);
            _reservationRequestService.Save(reservationRequest);
            CloseAction();
        }

        private void Execute_CancelCommand(object sender)
        {
            CloseAction();
        }
    }
}
