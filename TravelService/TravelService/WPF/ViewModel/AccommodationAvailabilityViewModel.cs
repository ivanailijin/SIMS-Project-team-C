using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TravelService.Applications.UseCases;
using TravelService.Applications.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class AccommodationAvailabilityViewModel : ViewModelBase
    {
        private readonly AccommodationReservationService _reservationService;
        public Accommodation SelectedAccommodation { get; set; }
        public Guest1 Guest1 { get; set; }
        public AccommodationAvailabilityView AccommodationAvailabilityView { get; set; }

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

        private RelayCommand _searchAvailableDates;
        public RelayCommand SearchAvailableDatesCommand
        {
            get => _searchAvailableDates;
            set
            {
                if (value != _searchAvailableDates)
                {
                    _searchAvailableDates = value;
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

        public AccommodationAvailabilityViewModel(AccommodationAvailabilityView accommodationAvailabillityView, Accommodation selectedAccommodation, Guest1 guest1)
        {
            SelectedAccommodation = selectedAccommodation;
            AccommodationAvailabilityView = accommodationAvailabillityView;
            Guest1 = guest1;
            _reservationService = new AccommodationReservationService(Injector.CreateInstance<IAccommodationReservationRepository>());
            LengthOfStay = SelectedAccommodation.MinReservationDays;

            SearchAvailableDatesCommand = new RelayCommand(Execute_SearchAvailableDates, CanExecute_Command);
            PreviousPageCommand = new RelayCommand(Execute_PreviousPage, CanExecute_Command);
            DecreaseLengthOfStayCommand = new RelayCommand(Execute_DecreaseLengthOfStay, CanExecute_Command);
            IncreaseLengthOfStayCommand = new RelayCommand(Execute_IncreaseLengthOfStay, CanExecute_Command);
        }

        public void GoBack()
        {
            AccommodationAvailabilityView.GoBack();
        }

        private bool CanExecute_Command(object parameter)
        {
            return true;
        }

        private void Execute_SearchAvailableDates(object sender)
        {
            if (string.IsNullOrWhiteSpace(CheckInDate.ToString()) ||
                string.IsNullOrWhiteSpace(CheckOutDate.ToString()) ||
                string.IsNullOrWhiteSpace(LengthOfStay.ToString()))
            {
                MessageBox.Show("Niste uneli sva polja!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (CheckInDate != null && CheckOutDate != null)
                {
                    DateTime checkInDate = CheckInDate.Value;
                    DateTime checkOutDate = CheckOutDate.Value;
                    List<Tuple<DateTime, DateTime>> AvailableDateRange = new List<Tuple<DateTime, DateTime>>();
                    List<Tuple<DateTime, DateTime>> AvailableDatesOutsideRange = new List<Tuple<DateTime, DateTime>>();

                    if (CheckOutDate < CheckInDate)
                    {
                        MessageBox.Show("Krajnji datum mora biti veci od pocetnog. Pokusajte ponovo.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (LengthOfStay < SelectedAccommodation.MinReservationDays)
                    {
                        MessageBox.Show($"Minimalan broj za rezervaciju je {SelectedAccommodation.MinReservationDays}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    AvailableDateRange = _reservationService.FindAvailableDates(SelectedAccommodation, checkInDate, checkOutDate, LengthOfStay);
                    if (AvailableDateRange.Count == 0)
                    {
                        AvailableDatesOutsideRange = _reservationService.FindAvailableDatesOutsideRange(SelectedAccommodation, checkInDate, checkOutDate, LengthOfStay);
                    }

                    ReserveAccommodationView reserveAccommodationView = new ReserveAccommodationView(SelectedAccommodation, Guest1, AvailableDateRange, AvailableDatesOutsideRange, LengthOfStay);
                    FirstGuestWindow firstGuestWindow = Window.GetWindow(AccommodationAvailabilityView) as FirstGuestWindow;
                    firstGuestWindow?.SwitchToPage(reserveAccommodationView);
                }
            }
        }

        private void Execute_DecreaseLengthOfStay(object sender)
        {
            if (LengthOfStay > SelectedAccommodation.MinReservationDays)
            {
                LengthOfStay--;

            }
        }

        private void Execute_IncreaseLengthOfStay(object sender)
        {
            LengthOfStay++;
        }

        private void Execute_PreviousPage(object sender)
        {
            GoBack();
        }
    }
}
