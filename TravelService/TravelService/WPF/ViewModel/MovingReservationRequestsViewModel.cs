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
using TravelService.Applications.UseCases;
using TravelService.Applications.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.View;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class MovingReservationRequestsViewModel : ViewModelBase
    {
        private readonly ReservationRequestService _reservationRequestService;
        public ObservableCollection<ReservationRequest> ReservationRequests { get; set; }

        public MovingReservationRequestsView MovingReservationRequestsView { get; set; }
        public ReservationRequest SelectedRequest { get; set; }
        public Action CloseAction { get; set; }
        public RelayCommand DeclineRequestCommand { get; set; }
        public RelayCommand ApproveRequestCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }


        public MovingReservationRequestsViewModel(MovingReservationRequestsView movingReservationRequestsView)
        {
            InitializeCommands();

            MovingReservationRequestsView = movingReservationRequestsView;
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
            CancelCommand = new RelayCommand(Execute_CancelCommand, CanExecute_Command);
        }

        private void Execute_CancelCommand(object obj)
        {
            MovingReservationRequestsView.GoBack();
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
                MessageBox.Show("Niste izabrali zahtev!", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                MessageBox.Show("Niste izabrali zahtev!", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private bool CanExecute_Command(object arg)
        {
            return true;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
