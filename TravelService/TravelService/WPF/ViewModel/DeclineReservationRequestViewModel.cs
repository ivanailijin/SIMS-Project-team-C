using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TravelService.Applications.UseCases;
using TravelService.Commands;
using TravelService.Domain.Model;

namespace TravelService.WPF.ViewModel
{
    public class DeclineReservationRequestViewModel : ViewModelBase
    {
        public ReservationRequestService _reservationRequestService { get; set; }
        public ReservationRequest SelectedRequest { get; set; }
        public ObservableCollection<ReservationRequest> ReservationRequests { get; set; } 

        public ICommand CancelCommand { get; set; }
        public ICommand ConfirmCommand { get; set; } 
        public Action CloseAction { get; set; }


        private string _accommodationName;

        public string AccommodationName
        {
            get => _accommodationName;
            set
            {
                if (value != _accommodationName)
                {
                    _accommodationName = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _guestName;

        public string GuestName
        {
            get => _guestName;
            set
            {
                if (value != _guestName)
                {
                    _guestName = value;
                    OnPropertyChanged();
                }
            }
        }
        private DateTime _previousCheckIn;

        public DateTime PreviousCheckIn
        {
            get => _previousCheckIn;
            set
            {
                if (value != _previousCheckIn)
                {
                    _previousCheckIn = value;
                    OnPropertyChanged();
                }
            }
        }
        private DateTime _previousCheckOut;

        public DateTime PreviousCheckOut
        {
            get => _previousCheckOut;
            set
            {
                if (value != _previousCheckOut)
                {
                    _previousCheckOut = value;
                    OnPropertyChanged();
                }
            }
        }
        private DateTime _newCheckOut;

        public DateTime NewCheckOut
        {
            get => _newCheckOut;
            set
            {
                if (value != _newCheckOut)
                {
                    _newCheckOut = value;
                    OnPropertyChanged();
                }
            }
        }
        private DateTime _newCheckIn;

        public DateTime NewCheckIn
        {
            get => _newCheckIn;
            set
            {
                if (value != _newCheckIn)
                {
                    _newCheckIn = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _reasoning;

        public string Reasoning
        {
            get => _reasoning;
            set
            {
                if (value != _reasoning)
                {
                    _reasoning = value;
                    OnPropertyChanged();
                }
            }
        }
        public DeclineReservationRequestViewModel(ReservationRequest selectedRequest, ReservationRequestService reservationRequestService, ObservableCollection<ReservationRequest> reservationRequests)
        {
            InitializeCommands();
            _reservationRequestService = reservationRequestService;
            SelectedRequest = selectedRequest;
            ReservationRequests = reservationRequests;

            AccommodationName = SelectedRequest.Reservation.Accommodation.Name;
            GuestName = SelectedRequest.Guest.Username;
            PreviousCheckIn = SelectedRequest.Reservation.CheckInDate;
            PreviousCheckOut = SelectedRequest.Reservation.CheckOutDate;
            NewCheckIn = SelectedRequest.NewStartDate.Date;
            NewCheckOut = SelectedRequest.NewEndDate.Date;
        }

        private void InitializeCommands()
        {
            CancelCommand = new RelayCommand(Execute_CancelCommand, CanExecute_Command);
            ConfirmCommand = new RelayCommand(Execute_ConfirmCommand, CanExecute_Command);
        }

        private void Execute_ConfirmCommand(object obj)
        {
            SelectedRequest.Status = STATUS.Rejected;
            SelectedRequest.Comment = Reasoning;
            _reservationRequestService.Update(SelectedRequest);
            ReservationRequests.Remove(SelectedRequest);
            CloseAction();
        }
        private void Execute_CancelCommand(object obj)
        {
            CloseAction();
        }
        private bool CanExecute_Command(object arg)
        {
            return true;
        }
    }
}
