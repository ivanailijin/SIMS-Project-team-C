using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Applications.UseCases;
using TravelService.Applications.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class ShowRequestListViewModel : ViewModelBase
    {
        public Action CloseAction { get; set; }
        public ComplexTourRequest SelectedComplexRequest { get; set; }
        public Guest2 Guest2 { get; set; }
        public ObservableCollection<TourRequest> TourRequests { get; set; }

        private readonly ComplexTourRequestService _complexTourRequestService;

        private readonly TourRequestService _tourRequestService;
        private RelayCommand _cancelCommand;
        public RelayCommand CancelCommand
        {
            get => _cancelCommand;
            set
            {
                if (value != _cancelCommand)
                {
                    _cancelCommand = value;
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
        public ShowRequestListViewModel(Guest2 guest2, ComplexTourRequest selectedComplexRequest)
        {
            _complexTourRequestService = new ComplexTourRequestService(Injector.CreateInstance<IComplexTourRequestRepository>());
            _tourRequestService = new  TourRequestService(Injector.CreateInstance<ITourRequestRepository>());

            Guest2 = guest2;
            SelectedComplexRequest = selectedComplexRequest;
            List<TourRequest> allTourRequests= new List<TourRequest>(_tourRequestService.GetAll());
            List<ComplexTourRequest> complexRequests = new List<ComplexTourRequest>(_complexTourRequestService.GetAll());
            List<ComplexTourRequest> guestsComplexRequests = new List<ComplexTourRequest>(_complexTourRequestService.FindValidComplexRequests(Guest2.Id, complexRequests));
            List<TourRequest> tourRequests= new List<TourRequest>(_complexTourRequestService.FindTourRequests(SelectedComplexRequest, guestsComplexRequests));
            TourRequests = new ObservableCollection<TourRequest>(tourRequests);
            CancelCommand = new RelayCommand(Execute_CancelCommand, CanExecute_Command);

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
        private void Execute_CancelCommand(object sender)
        {
            CloseAction();
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
        private void Execute_NotificationCommand(object sender)
        {
            SecondGuestNotificationsView secondGuestNotificationsView = new SecondGuestNotificationsView(SelectedNotification, Guest2);
            secondGuestNotificationsView.Show();
            CloseAction();
        }
        private void Execute_ReportCommand(object sender)
        {
        }
    }
}
