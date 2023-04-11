using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using TravelService.Application.ServiceInterface;
using TravelService.Application.UseCases;
using TravelService.Application.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class MovingReservationRequestsViewModel : ViewModelBase
    {
        private readonly ReservationRequestService _reservationRequestService;
        public ObservableCollection<ReservationRequest> ReservationRequests { get; set; }

        public ReservationRequest SelectedRequest { get; set; }

        public Action CloseAction { get; set; }
        public ICommand DeclineRequestCommand { get; set; }
        public ICommand ApproveRequestCommand { get; set; }


        private ObservableCollection<string> _availabilities;

        public ObservableCollection<string> Availabilities
        {
            get => _availabilities;
            set
            {
                if (value != _availabilities)
                {
                    _availabilities = value;
                    OnPropertyChanged();
                }
            }
        }
        public MovingReservationRequestsViewModel()
        {
            InitializeCommands();

            _reservationRequestService = new ReservationRequestService(Injector.CreateInstance<IReservationRequestRepository>());
            List<ReservationRequest> reservationRequests = _reservationRequestService.GetAllUnsolvedRequests();
            reservationRequests = _reservationRequestService.GetReservationData(reservationRequests);
            reservationRequests = _reservationRequestService.GetAccommodationData(reservationRequests);
            reservationRequests = _reservationRequestService.GetGuestData(reservationRequests);
            reservationRequests = _reservationRequestService.GetAvailabilities(reservationRequests);
            ReservationRequests = new ObservableCollection<ReservationRequest>(reservationRequests);
        }

        private void InitializeCommands()
        {
            DeclineRequestCommand = new RelayCommand(Execute_DeclineRequestCommand, CanExecute_Command);
            ApproveRequestCommand = new RelayCommand(Execute_ApproveRequestCommand, CanExecute_Command);
        }

        private void Execute_DeclineRequestCommand(object obj)
        {
            if (SelectedRequest != null)
            {
                DeclineReservationRequestView declineReservationRequestView = new DeclineReservationRequestView(SelectedRequest, _reservationRequestService, ReservationRequests);
                declineReservationRequestView.Show();
            }
            else
            {
                MessageBox.Show("Choose reservation request!");
            }
        }
        private void Execute_ApproveRequestCommand(object obj)
        {
            if (SelectedRequest != null)
            {
                ApproveReservationRequestView approveReservationRequestView = new ApproveReservationRequestView(SelectedRequest, _reservationRequestService, ReservationRequests);
                approveReservationRequestView.Show();
            }
            else
            {
                MessageBox.Show("Choose reservation request!");
            }
        }
        private bool CanExecute_Command(object arg)
        {
            return true;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
