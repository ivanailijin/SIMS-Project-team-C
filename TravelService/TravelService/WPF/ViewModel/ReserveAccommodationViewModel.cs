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
    public class ReserveAccommodationViewModel : ViewModelBase
    {
        private AccommodationReservationService _accommodationReservationService;
        private Guest1Service _guest1Service;
        public Accommodation SelectedAccommodation { get; set; }
        public ReserveAccommodationView ReserveAccommodationView { get; set; }
        public Guest1 Guest1 { get; set; }
        public ObservableCollection<AccommodationReservation> Reservations { get; set; }
        public Tuple<DateTime, DateTime> SelectedAvailableDatePair { get; set; }


        private int _lengthOfStay;
        public int LengthOfStay
        {
            get => _lengthOfStay;
            set
            {
                if (value != _lengthOfStay)
                {
                    _lengthOfStay = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _guestNumber;
        public int GuestNumber
        {
            get => _guestNumber;
            set
            {
                if (value != _guestNumber)
                {
                    _guestNumber = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _notification;
        public string Notification
        {
            get => _notification;
            set
            {
                if (value != _notification)
                {
                    _notification = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<Tuple<DateTime, DateTime>> _availableDatesPair;
        public ObservableCollection<Tuple<DateTime, DateTime>> AvailableDatesPair
        {
            get => _availableDatesPair;
            set
            {
                if (value != _availableDatesPair)
                {
                    _availableDatesPair = value;
                    OnPropertyChanged();
                }
            }
        }

        private RelayCommand _searchAvailableAccommodations;
        public RelayCommand SearchAvailableAccommodationsCommand
        {
            get => _searchAvailableAccommodations;
            set
            {
                if (value != _searchAvailableAccommodations)
                {
                    _searchAvailableAccommodations = value;
                    OnPropertyChanged();
                }
            }
        }

        private RelayCommand _increaseGuestNumberCommand;
        public RelayCommand IncreaseGuestNumberCommand
        {
            get => _increaseGuestNumberCommand;
            set
            {
                if (value != _increaseGuestNumberCommand)
                {
                    _increaseGuestNumberCommand = value;
                    OnPropertyChanged();
                }
            }
        }

        private RelayCommand _decreaseGuestNumberCommand;
        public RelayCommand DecreaseGuestNumberCommand
        {
            get => _decreaseGuestNumberCommand;
            set
            {
                if (value != _decreaseGuestNumberCommand)
                {
                    _decreaseGuestNumberCommand = value;
                    OnPropertyChanged();
                }
            }
        }

        private RelayCommand _reserveAccommodationCommand;
        public RelayCommand ReserveAccommodationCommand
        {
            get => _reserveAccommodationCommand;
            set
            {
                if (value != _reserveAccommodationCommand)
                {
                    _reserveAccommodationCommand = value;
                    OnPropertyChanged();
                }
            }
        }

        private RelayCommand _previousPageCommand;
        public RelayCommand PreviousPageCommand
        {
            get => _previousPageCommand;
            set
            {
                if (value != _previousPageCommand)
                {
                    _previousPageCommand = value;
                    OnPropertyChanged();
                }
            }
        }

        public ReserveAccommodationViewModel(ReserveAccommodationView reserveAccommodationView, Accommodation selectedAccommodation, Guest1 guest1, List<Tuple<DateTime, DateTime>> availableDateRange, List<Tuple<DateTime, DateTime>> availableDateOutsideRange, int lengthOfStay)
        {
            _accommodationReservationService = new AccommodationReservationService(Injector.CreateInstance<IAccommodationReservationRepository>());
            _guest1Service = new Guest1Service(Injector.CreateInstance<IGuest1Repository>());
            ReserveAccommodationView = reserveAccommodationView;
            AvailableDatesPair = new ObservableCollection<Tuple<DateTime, DateTime>>(availableDateRange);
            if (availableDateRange.Count == 0)
            {
                Notification = "Datumi u zadatom opsegu su zauzeti.\nPreporucujemo sledece:";
                foreach (var availableDate in availableDateOutsideRange)
                {
                    AvailableDatesPair.Add(availableDate);
                }
            }
            SelectedAccommodation = selectedAccommodation;
            Guest1 = guest1;
            LengthOfStay = lengthOfStay;

            ReserveAccommodationCommand = new RelayCommand(Execute_ReserveAccommodation, CanExecute_Command);
            DecreaseGuestNumberCommand = new RelayCommand(Execute_DecreaseGuestNumber, CanExecute_Command);
            IncreaseGuestNumberCommand = new RelayCommand(Execute_IncreaseGuestNumber, CanExecute_Command);
            PreviousPageCommand = new RelayCommand(Execute_PreviousPage, CanExecute_Command);
        }

        private bool CanExecute_Command(object parameter)
        {
            return true;
        }

        private void Execute_ReserveAccommodation(object sender)
        {
            if (SelectedAvailableDatePair == null ||
                string.IsNullOrWhiteSpace(GuestNumber.ToString()))
            {
                MessageBox.Show("Niste uneli sva polja!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (GuestNumber > SelectedAccommodation.MaxGuestNumber)
                {
                    MessageBox.Show($"Maksimalan broj gostiju za {SelectedAccommodation.Name} smestaj je {SelectedAccommodation.MaxGuestNumber}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                DateTime checkInDate = SelectedAvailableDatePair.Item1;
                DateTime checkOutDate = SelectedAvailableDatePair.Item2;
                AccommodationReservation reservation = new AccommodationReservation(SelectedAccommodation.Id, Guest1.Id, SelectedAccommodation.OwnerId, SelectedAccommodation.LocationId, checkInDate, checkOutDate, LengthOfStay, GuestNumber);
                _accommodationReservationService.Save(reservation);
                if (Guest1.BonusPoints > 0)
                {
                    Guest1.BonusPoints--;
                }
                _guest1Service.Update(Guest1);
                SelectedAccommodationView selectedAccommodationView = new SelectedAccommodationView(SelectedAccommodation, Guest1);
                FirstGuestWindow firstGuestWindow = Window.GetWindow(ReserveAccommodationView) as FirstGuestWindow;
                firstGuestWindow?.SwitchToPage(selectedAccommodationView);
            }
        }

        private void Execute_DecreaseGuestNumber(object sender)
        {
            if (GuestNumber > 0)
            {
                GuestNumber--;

            }
        }

        private void Execute_IncreaseGuestNumber(object sender)
        {
            if (GuestNumber < SelectedAccommodation.MaxGuestNumber)
            {
                GuestNumber++;
            }
        }

        public void GoBack()
        {
            ReserveAccommodationView.GoBack();
        }

        private void Execute_PreviousPage(object sender)
        {
            GoBack();
        }
    }
}
