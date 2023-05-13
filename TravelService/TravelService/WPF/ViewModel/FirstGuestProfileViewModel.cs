using System.Security.Cryptography.Xml;
using System.Threading.Channels;
using System.Windows;
using System.Windows.Controls;
using TravelService.Application.UseCases;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.WPF.View;
using TravelService.Application.Utils;
using TravelService.Domain.RepositoryInterface;
using System.Collections.Generic;

namespace TravelService.WPF.ViewModel
{
    public class FirstGuestProfileViewModel : ViewModelBase
    {
        private Guest1Service _guest1Service;
        private AccommodationReservationService _reservationService;
        public Window FirstView { get; set; }
        public Guest1 Guest1 { get; set; }

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
        public FirstGuestProfileViewModel(Window firstView, Guest1 guest1)
        {
            _guest1Service = new Guest1Service(Injector.CreateInstance<IGuest1Repository>());
            _reservationService = new AccommodationReservationService(Injector.CreateInstance<IAccommodationReservationRepository>());
            Guest1 = guest1;
            FirstView = firstView;

            List<AccommodationReservation> reservationsInLastYear = _reservationService.GetReservationsInLastYear(Guest1);
            int reservationsCount = reservationsInLastYear.Count;
            Guest1 = _guest1Service.CheckSuperOwnerStatus(Guest1, reservationsCount);

            NumberOfReservations = reservationsCount;

            LogOutCommand = new RelayCommand(Execute_LogOut, CanExecute_Command);
        }

        private bool CanExecute_Command(object parameter)
        {
            return true;
        }

        private void Execute_LogOut(object sender)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            FirstView.Close();
        }
    }
}
