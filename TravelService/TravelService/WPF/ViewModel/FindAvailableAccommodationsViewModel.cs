using System;
using System.Collections.Generic;
using System.Windows;
using TravelService.Applications.UseCases;
using TravelService.Applications.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class FindAvailableAccommodationsViewModel : ViewModelBase
    {
        private AccommodationReservationService _reservationService;
        public Guest1 Guest1 { get; set; }
        public FindAvailableAccommodationsView FindAvailableAccommodationsView { get; set; }

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

        private DateTime? _checkInDate;
        public DateTime? CheckInDate
        {
            get => _checkInDate;
            set
            {
                if (value != _checkInDate)
                {
                    _checkInDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime? _checkOutDate;
        public DateTime? CheckOutDate
        {
            get => _checkOutDate;
            set
            {
                if (value != _checkOutDate)
                {
                    _checkOutDate = value;
                    OnPropertyChanged();
                }
            }
        }

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

        private RelayCommand _decreaseLengthOfStayCommand;
        public RelayCommand DecreaseLengthOfStayCommand
        {
            get => _decreaseLengthOfStayCommand;
            set
            {
                if (value != _decreaseLengthOfStayCommand)
                {
                    _decreaseLengthOfStayCommand = value;
                    OnPropertyChanged();
                }
            }
        }

        private RelayCommand _increaseLengthOfStayCommand;
        public RelayCommand IncreaseLengthOfStayCommand
        {
            get => _increaseLengthOfStayCommand;
            set
            {
                if (value != _increaseLengthOfStayCommand)
                {
                    _increaseLengthOfStayCommand = value;
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

        public FindAvailableAccommodationsViewModel(FindAvailableAccommodationsView findAvailableAccommodationsView, Guest1 guest1)
        {
            _reservationService = new AccommodationReservationService(Injector.CreateInstance<IAccommodationReservationRepository>());
            Guest1 = guest1;
            FindAvailableAccommodationsView = findAvailableAccommodationsView;

            SearchAvailableAccommodationsCommand = new RelayCommand(Execute_SearchAvailableAccommodations, CanExecute_Command);
            DecreaseGuestNumberCommand = new RelayCommand(Execute_DecreaseGuestNumber, CanExecute_Command);
            IncreaseGuestNumberCommand = new RelayCommand(Execute_IncreaseGuestNumber, CanExecute_Command);
            DecreaseLengthOfStayCommand = new RelayCommand(Execute_DecreaseLengthOfStay, CanExecute_Command);
            IncreaseLengthOfStayCommand = new RelayCommand(Execute_IncreaseLengthOfStay, CanExecute_Command);
            PreviousPageCommand = new RelayCommand(Execute_PreviousPage, CanExecute_Command);
        }

        public void GoBack()
        {
            FindAvailableAccommodationsView.GoBack();
        }

        private bool CanExecute_Command(object parameter)
        {
            return true;
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
            GuestNumber++;
        }

        private void Execute_DecreaseLengthOfStay(object sender)
        {
            if (LengthOfStay > 0)
            {
                LengthOfStay--;

            }
        }

        private void Execute_IncreaseLengthOfStay(object sender)
        {
            LengthOfStay++;
        }

        private void Execute_SearchAvailableAccommodations(object sender)
        {
            List<Accommodation> accommodations = new List<Accommodation>();
            if (CheckInDate != null && CheckOutDate != null)
            {
                DateTime checkInDate = CheckInDate.Value;
                DateTime checkOutDate = CheckOutDate.Value;
                accommodations = _reservationService.FilterAvailableAccommodations(checkInDate, checkOutDate, LengthOfStay, GuestNumber);
            }
            else
            {
                DateTime checkInDate = DateTime.Today;
                DateTime checkOutDate = checkInDate.AddYears(1);
                accommodations = _reservationService.FilterAvailableAccommodations(checkInDate, checkOutDate, LengthOfStay, GuestNumber);
            }
            FirstGuestView firstGuestView = new FirstGuestView(Guest1);
            firstGuestView.frame.Navigate(new RecommendedAccommodationView(Guest1, accommodations, CheckInDate, CheckOutDate, GuestNumber, LengthOfStay));
            FirstGuestWindow firstGuestWindow = Window.GetWindow(FindAvailableAccommodationsView) as FirstGuestWindow ?? new(Guest1);
            firstGuestWindow?.SwitchToPage(firstGuestView);
        }

        private void Execute_PreviousPage(object sender)
        {
            GoBack();
        }
    }
}
