using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TravelService.Applications.UseCases;
using TravelService.Applications.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.Repository;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class TourReservationViewModel : ViewModelBase
    {
        public Guest2 Guest2 { get; set; }
        public Action CloseAction { get; set; }
        public Tour SelectedTour { get; set; }
        public ObservableCollection<Tour> ActiveTours { get; set; }

        private readonly TourReservationService _tourReservationService;

        private readonly TourService _tourService;

        private readonly LocationService _locationService;

        private readonly LanguageService _languageService;

        private readonly CheckPointService _checkpointService;
        public ObservableCollection<Tour> Tours { get; set; }
        public List<Location> Locations { get; set; }
        public List<Language> Languages { get; set; }
        public List<CheckPoint> CheckPoints { get; set; }
        public ObservableCollection<TourReservation> TourReservations { get; set; }

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
        private RelayCommand _showTourDetailsCommand;
        public RelayCommand ShowTourDetailsCommand
        {
            get => _showTourDetailsCommand;
            set
            {
                if (value != _showTourDetailsCommand)
                {
                    _showTourDetailsCommand = value;
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

        public TourReservationViewModel(Guest2 guest2, Tour selectedTour) {
            _tourReservationService = new TourReservationService(Injector.CreateInstance<ITourReservationRepository>());
            _tourService = new TourService(Injector.CreateInstance<ITourRepository>());
            _locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            _languageService = new LanguageService(Injector.CreateInstance<ILanguageRepository>());
            _checkpointService = new CheckPointService(Injector.CreateInstance<ICheckPointRepository>());

            Guest2 = guest2;
            SelectedTour = selectedTour;
            Tours = new ObservableCollection<Tour>(_tourService.GetAll());
            Locations = new List<Location>(_locationService.GetAll());
            Languages = new List<Language>(_languageService.GetAll());
            CheckPoints = new List<CheckPoint>(_checkpointService.GetAll());
            ActiveTours = new ObservableCollection<Tour>(_tourReservationService.showAllActiveToursNew(Tours.ToList(), Locations, Languages, CheckPoints));

           
            CancelCommand = new RelayCommand(Execute_CancelCommand, CanExecute_Command);
            ShowTourDetailsCommand = new RelayCommand(Execute_ShowTourDetailsCommand, CanExecute_Command);
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
        private void Execute_NotificationCommand(object sender)
        {
            SecondGuestNotificationsView secondGuestNotificationsView = new SecondGuestNotificationsView(SelectedNotification, Guest2);
            secondGuestNotificationsView.Show();
            CloseAction();
        }
        private void Execute_CancelCommand(object sender)
        {
            CloseAction();
        }
        private void Execute_ShowTourDetailsCommand(object sender)
        {
            ShowTourInfoView showTourInfoView = new ShowTourInfoView(Guest2, SelectedTour);
            showTourInfoView.Show();
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
        private void Execute_RateTourCommand(object sender)
        {
            GuestsToursView guestsToursView = new GuestsToursView(SelectedTour, Guest2);
            guestsToursView.Show();
            CloseAction();
        }
        private void Execute_ReportCommand(object sender)
        {
        }
    }
}
