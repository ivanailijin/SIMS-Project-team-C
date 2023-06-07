using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Documents;
using TravelService.Applications.UseCases;
using TravelService.Applications.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class AddComplexTourRequestViewModel : ViewModelBase
    {
        public Guest2 Guest2 { get; set; }
        public NewTourNotification SelectedNotification { get; set; }
        public Action CloseAction { get; set; }

        private ObservableCollection<TourRequest> tourRequests;
        public ObservableCollection<TourRequest> TourRequests
        {
            get { return tourRequests; }
            set
            {
                tourRequests = value;
                OnPropertyChanged(nameof(TourRequests));
            }
        }

        private readonly ComplexTourRequestService _complexTourRequestService;

        private bool _isForwarded = true;
        public bool IsForwarded
        {
            get { return _isForwarded; }
            set
            {
                _isForwarded = value;
            }
        }
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (value != _name)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }
        private RelayCommand _addRequestCommand;
        public RelayCommand AddRequestCommand
        {
            get => _addRequestCommand;
            set
            {
                if (value != _addRequestCommand)
                {
                    _addRequestCommand = value;
                    OnPropertyChanged();
                }
            }
        }
        private RelayCommand _submitCommand;
        public RelayCommand SubmitCommand
        {
            get => _submitCommand;
            set
            {
                if (value != _submitCommand)
                {
                    _submitCommand = value;
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
        public AddComplexTourRequestViewModel(Guest2 guest2, ObservableCollection<TourRequest> tourRequests)
        {
            _complexTourRequestService = new ComplexTourRequestService(Injector.CreateInstance<IComplexTourRequestRepository>());

            Guest2 = guest2;
            IsForwarded = true;
            TourRequests = new ObservableCollection<TourRequest>();
            AddRequestCommand = new RelayCommand(Execute_AddRequestCommand, CanExecute_Command);
            SubmitCommand = new RelayCommand(Execute_SubmitCommand, CanExecute_Command);
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
        private void Execute_AddRequestCommand(object sender)
        {
            AddTourRequestView addTourRequestView = new AddTourRequestView(Guest2, IsForwarded, TourRequests);
            addTourRequestView.IsForwarded = true;
            addTourRequestView.Show();
        }
        private void AddTourRequestView_Closed(object sender, EventArgs e)
        {
            if (sender is AddTourRequestView addTourRequestView && addTourRequestView.DataContext is AddTourRequestViewModel addTourRequestViewModel)
            {
                TourRequests.Add(addTourRequestViewModel.TourRequest);
            }
        }
        private void Execute_NotificationCommand(object sender)
        {
            SecondGuestNotificationsView secondGuestNotificationsView = new SecondGuestNotificationsView(SelectedNotification, Guest2);
            secondGuestNotificationsView.Show();
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
        private void Execute_ReportCommand(object sender)
        {
        }
        private void Execute_SubmitCommand(object sender)
        {
            _complexTourRequestService.saveComplexRequest(Name, Guest2, TourRequests.ToList());
            CloseAction();
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
