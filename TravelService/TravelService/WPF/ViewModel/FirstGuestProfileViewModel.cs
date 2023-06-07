using System.Security.Cryptography.Xml;
using System.Threading.Channels;
using System.Windows;
using System.Windows.Controls;
using TravelService.Applications.UseCases;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.WPF.View;
using TravelService.Applications.Utils;
using TravelService.Domain.RepositoryInterface;
using System.Collections.Generic;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Defaults;
using System.Reflection.Emit;
using System;

namespace TravelService.WPF.ViewModel
{
    public class FirstGuestProfileViewModel : ViewModelBase
    {
        private Guest1Service _guest1Service;
        private AccommodationReservationService _reservationService;
        public Guest1 Guest1 { get; set; }
        public FirstGuestProfileView FirstGuestProfileView { get; set; }
        public SeriesCollection ReservationSeries { get; set; }
        public List<string> MonthLabels { get; set; }

        private int _numberOfReservations;
        public int NumberOfReservations
        {
            get => _numberOfReservations;
            set
            {
                if (value != _numberOfReservations)
                {
                    _numberOfReservations = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _bonusPoints;
        public int BonusPoints
        {
            get => _bonusPoints;
            set
            {
                if (value != _bonusPoints)
                {
                    _bonusPoints = value;
                    OnPropertyChanged();
                }
            }
        }

        private RelayCommand _logOutCommand;
        public RelayCommand LogOutCommand
        {
            get => _logOutCommand;
            set
            {
                if (value != _logOutCommand)
                {
                    _logOutCommand = value;
                    OnPropertyChanged();
                }
            }
        }
        public FirstGuestProfileViewModel(FirstGuestProfileView firstGuestProfileView, Guest1 guest1)
        {
            _guest1Service = new Guest1Service(Injector.CreateInstance<IGuest1Repository>());
            _reservationService = new AccommodationReservationService(Injector.CreateInstance<IAccommodationReservationRepository>());
            Guest1 = guest1;
            FirstGuestProfileView = firstGuestProfileView;

            List<AccommodationReservation> reservationsInLastYear = _guest1Service.GetReservationsInLastYear(Guest1);
            int reservationsCount = reservationsInLastYear.Count;
            Guest1 = _guest1Service.CheckSuperOwnerStatus(Guest1);

            NumberOfReservations = reservationsCount;

            LogOutCommand = new RelayCommand(Execute_LogOut, CanExecute_Command);

            MonthLabels = new List<string>();

            Dictionary<string, int> reservationsByMonth = new Dictionary<string, int>(_reservationService.CalculateReservationCountByMonthInPreviousYear(Guest1));
            ChartValues<ObservableValue> reservationsData = new ChartValues<ObservableValue>();

            foreach (var item in reservationsByMonth)
            {
                reservationsData.Add(new ObservableValue(item.Value));
                MonthLabels.Add(item.Key);
            }

            ColumnSeries reservationsSeries = new ColumnSeries
            {
                Title = "Reservations",
                Values = reservationsData
            };

            ReservationSeries = new SeriesCollection { reservationsSeries };
        }

        private bool CanExecute_Command(object parameter)
        {
            return true;
        }

        private void Execute_LogOut(object sender)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            FirstGuestWindow firstGuestWindow = Window.GetWindow(FirstGuestProfileView) as FirstGuestWindow ?? new(Guest1);
            firstGuestWindow.Close();
        }
    }
}
