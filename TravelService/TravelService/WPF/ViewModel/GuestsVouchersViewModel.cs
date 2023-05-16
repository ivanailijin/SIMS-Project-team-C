using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Application.UseCases;
using TravelService.Application.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.Repository;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class GuestsVouchersViewModel : ViewModelBase
    {
        public Guest2 Guest2 { get; set; }
        public Action CloseAction { get; set; }

        private readonly VoucherService _voucherService;
        private readonly NewTourNotificationService _notificationService;
        private readonly TourService _tourService;
        public ObservableCollection<GuestVoucher> Vouchers { get; set; }
        public ObservableCollection<GuestVoucher> GuestsVouchers { get; set; }
        public ObservableCollection<Tour> Tours { get; set; }

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
        private RelayCommand _notificationCommand;
        public RelayCommand NotificationCommand
        {
            get => _notificationCommand;
            set
            {
                if (value != _notificationCommand)
                {
                    _notificationCommand = value;
                    OnPropertyChanged();
                }
            }
        }
        
        private int _language;
        public int TourId
        {
            get => _language;
            set
            {
                if (value != _language)
                {
                    _language = value;
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
        public GuestsVouchersViewModel( Guest2 guest2) 
        {
            Guest2 = guest2;
            Username = guest2.Username;
            _voucherService = new VoucherService(Injector.CreateInstance<IVoucherRepository>());
            _tourService = new TourService(Injector.CreateInstance<ITourRepository>());
            _notificationService = new NewTourNotificationService(Injector.CreateInstance<INewTourNotificationRepository>());
            List<GuestVoucher> vouchers = new List<GuestVoucher>(_voucherService.GetAll());
            Vouchers = new ObservableCollection<GuestVoucher>(vouchers);
            List<GuestVoucher> guestsVouchers = new List<GuestVoucher>(_voucherService.showVoucherList(Vouchers.ToList(),guest2));
            GuestsVouchers = new ObservableCollection<GuestVoucher>(guestsVouchers);

            List<Tour> tours = new List<Tour>(_tourService.GetAll());
            Tours = new ObservableCollection<Tour>(tours);

            HomePageCommand = new RelayCommand(Execute_HomePageCommand, CanExecute_Command);
            VoucherViewCommand = new RelayCommand(Execute_VoucherViewCommand, CanExecute_Command);
            NotificationCommand = new RelayCommand(Execute_NotificationCommand, CanExecute_Command);
        }
        private bool CanExecute_Command(object parameter)
        {
            return true;
        }
        private void Execute_HomePageCommand(object sender)
        {
            SecondGuestView secondGuestView = new SecondGuestView(Guest2);
            secondGuestView.Show();
            CloseAction();
        }
        private void Execute_VoucherViewCommand(object sender)
        {
            GuestsVouchersView guestsVouchersView = new GuestsVouchersView(Guest2);
            guestsVouchersView.Show();
        }
        private void Execute_NotificationCommand(object sender)
        {
            _notificationService.SendNotification(TourId);
        }
    }
}
