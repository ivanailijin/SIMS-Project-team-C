using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class ChooseRequestListViewModel : ViewModelBase
    {
        public Guest2 Guest2 { get; set; }
        public ComplexTourRequest SelectedComplexRequest { get; set; }
        public Action CloseAction { get; set; }

        private RelayCommand _singleRequestListCommand;
        public RelayCommand SingleRequestListCommand
        {
            get => _singleRequestListCommand;
            set
            {
                if (value != _singleRequestListCommand)
                {
                    _singleRequestListCommand = value;
                    OnPropertyChanged();
                }
            }
        }
        private RelayCommand _complexRequestListCommand;
        public RelayCommand ComplexRequestListCommand
        {
            get => _complexRequestListCommand;
            set
            {
                if (value != _complexRequestListCommand)
                {
                    _complexRequestListCommand = value;
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
        private RelayCommand _guestsRequestsCommand;
        public RelayCommand GuestsRequestsCommand
        {
            get => _guestsRequestsCommand;
            set
            {
                if (value != _guestsRequestsCommand)
                {
                    _guestsRequestsCommand = value;
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
        private RelayCommand _statisticsCommand;
        public RelayCommand StatisticsCommand
        {
            get => _statisticsCommand;
            set
            {
                if (value != _statisticsCommand)
                {
                    _statisticsCommand = value;
                    OnPropertyChanged();
                }
            }
        }
        private RelayCommand _reportCommand;
        public RelayCommand ReportCommand
        {
            get => _reportCommand;
            set
            {
                if (value != _reportCommand)
                {
                    _reportCommand = value;
                    OnPropertyChanged();
                }
            }
        }
        public NewTourNotification SelectedNotification { get; set; }
        public ChooseRequestListViewModel(Guest2 guest2)
        {
            Guest2 = guest2;
            SingleRequestListCommand = new RelayCommand(Execute_SingleRequestListCommand, CanExecute_Command);
            ComplexRequestListCommand = new RelayCommand(Execute_ComplexRequestListCommand, CanExecute_Command);
            HomePageCommand = new RelayCommand(Execute_HomePageCommand, CanExecute_Command);
            VoucherViewCommand = new RelayCommand(Execute_VoucherViewCommand, CanExecute_Command);
            GuestsRequestsCommand = new RelayCommand(Execute_GuestsRequestsCommand, CanExecute_Command);
            StatisticsCommand = new RelayCommand(Execute_StatisticsCommand, CanExecute_Command);
            ReportCommand = new RelayCommand(Execute_ReportCommand, CanExecute_Command);
            NotificationCommand = new RelayCommand(Execute_NotificationCommand, CanExecute_Command);
        }
        private bool CanExecute_Command(object parameter)
        {
            return true;
        }
        private void Execute_SingleRequestListCommand(object sender)
        {
            GuestsRequestsView guestsRequestsView = new GuestsRequestsView(Guest2);
            guestsRequestsView.Show();
        }
        private void Execute_NotificationCommand(object sender)
        {
            SecondGuestNotificationsView secondGuestNotificationsView = new SecondGuestNotificationsView(SelectedNotification, Guest2);
            secondGuestNotificationsView.Show();
            CloseAction();
        }
        private void Execute_ComplexRequestListCommand(object sender)
        {
            GuestsComplexRequestsView guestsComplexRequestsView = new GuestsComplexRequestsView(Guest2, SelectedComplexRequest);
            guestsComplexRequestsView.Show();
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
            CloseAction();
        }
        private void Execute_GuestsRequestsCommand(object sender)
        {
            ChoooseRequestListView choooseRequestListView = new ChoooseRequestListView(Guest2);
            choooseRequestListView.Show();
            CloseAction();
        }
        private void Execute_StatisticsCommand(object sender)
        {
            GuestsRequestsStatisticsView guestsRequestsStatisticsView = new GuestsRequestsStatisticsView(Guest2);
            guestsRequestsStatisticsView.Show();
        }
        private void Execute_ReportCommand(object sender)
        {
        }
    }
}
