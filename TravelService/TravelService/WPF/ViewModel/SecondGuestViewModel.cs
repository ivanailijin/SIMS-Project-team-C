using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class SecondGuestViewModel : ViewModelBase
    {
        public Tour SelectedTour { get; set; }
        public GuestVoucher SelectedVoucher { get; set; }
        public Guest2 Guest2 { get; set; }
        public Action CloseAction { get; set; }

        private RelayCommand _trackCommand;
        public RelayCommand TrackCommand
        {
            get => _trackCommand;
            set
            {
                if (value != _trackCommand)
                {
                    _trackCommand = value;
                    OnPropertyChanged();
                }
            }
        }
        private RelayCommand _tourViewCommand;
        public RelayCommand TourViewCommand
        {
            get => _tourViewCommand;
            set
            {
                if (value != _tourViewCommand)
                {
                    _tourViewCommand = value;
                    OnPropertyChanged();
                }
            }
        }
        private RelayCommand _reservationCommand;
        public RelayCommand ReservationCommand
        {
            get => _reservationCommand;
            set
            {
                if (value != _reservationCommand)
                {
                    _reservationCommand = value;
                    OnPropertyChanged();
                }
            }
        }
        private RelayCommand _voucherViewCommand;
        public RelayCommand VoucherViewCommand
        {
            get => _voucherViewCommand;
            set
            {
                if (value != _voucherViewCommand)
                {
                    _voucherViewCommand = value;
                    OnPropertyChanged();
                }
            }
        }
        private RelayCommand _homePageCommand;
        public RelayCommand HomePageCommand
        {
            get => _homePageCommand;
            set
            {
                if (value != _homePageCommand)
                {
                    _homePageCommand = value;
                    OnPropertyChanged();
                }
            }
        }
        private RelayCommand _rateTourCommand;
        public RelayCommand RateTourCommand
        {
            get => _rateTourCommand;
            set
            {
                if (value != _rateTourCommand)
                {
                    _rateTourCommand = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                if (value != _username)
                {
                    _username = value;
                    OnPropertyChanged();
                }
            }
        }

        public SecondGuestViewModel(Guest2 guest2) 
        {
            this.Guest2 = guest2;
            TrackCommand = new RelayCommand(Execute_TrackCommand, CanExecute_Command);
            TourViewCommand = new RelayCommand(Execute_TourViewCommand, CanExecute_Command);
            ReservationCommand = new RelayCommand(Execute_ReservationCommand, CanExecute_Command);
            VoucherViewCommand = new RelayCommand(Execute_VoucherViewCommand, CanExecute_Command);
            RateTourCommand = new RelayCommand(Execute_RateTourCommand, CanExecute_Command);
            HomePageCommand = new RelayCommand(Execute_HomePageCommand, CanExecute_Command);
            Username = guest2.Username;
        }
        private bool CanExecute_Command(object parameter)
        {
            return true;
        }

        private void Execute_TrackCommand(object sender) 
        {
            TourTrackingView tourTrackingView = new TourTrackingView(SelectedTour, Guest2);
            tourTrackingView.Show();
        }
        private void Execute_TourViewCommand(object sender)
        {
            TourView tourView = new TourView(Guest2);
            tourView.Show();
        }
        private void Execute_HomePageCommand(object sender)
        {
            SecondGuestView secondGuestView = new SecondGuestView(Guest2);
            secondGuestView.Show();
            CloseAction();
        }
        private void Execute_ReservationCommand(object sender)
        {
            TourReservationView tourReservationView = new TourReservationView(SelectedTour, SelectedVoucher, Guest2);
            tourReservationView.Show();
        }
        private void Execute_VoucherViewCommand(object sender)
        {
            GuestsVouchersView guestsVouchersView = new GuestsVouchersView(Guest2);
            guestsVouchersView.Show();
        }
        private void Execute_RateTourCommand(object sender)
        {
            GuestsToursView guestsToursView = new GuestsToursView(SelectedTour, Guest2);
            guestsToursView.Show();
        }
        public event EventHandler<string> NotificationReceived;

        private void Notify(string message)
        {
            NotificationReceived?.Invoke(this, message);
        }
    }
}
