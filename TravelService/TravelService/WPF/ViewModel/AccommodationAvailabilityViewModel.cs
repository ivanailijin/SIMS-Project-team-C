using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TravelService.Application.UseCases;
using TravelService.Application.Utils;
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
        public Action CloseAction { get; set; }

        private DateTime _checkInDate;
        public DateTime CheckInDate
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

        private DateTime _checkOutDate;
        public DateTime CheckOutDate
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

        public AccommodationAvailabilityViewModel(Accommodation selectedAccommodation, Guest1 guest1)
        {
            SelectedAccommodation = selectedAccommodation;
            Guest1 = guest1;
            _reservationService = new AccommodationReservationService(Injector.CreateInstance<IAccommodationReservationRepository>());

            SearchAvailableDatesCommand = new RelayCommand(Execute_SearchAvailableDates, CanExecute_Command);
        }

        private bool CanExecute_Command(object parameter)
        {
            return true;
        }

        private void Execute_SearchAvailableDates(object sender)
        {
            List<Tuple<DateTime, DateTime>> AvailableDateRange = new List<Tuple<DateTime, DateTime>>();
            List<Tuple<DateTime, DateTime>> AvailableDatesOutsideRange = new List<Tuple<DateTime, DateTime>>();

            if (CheckOutDate < CheckInDate)
            {
                MessageBox.Show("End date must be greater than start date. Please try again.");
                return;
            }

            if (LengthOfStay < SelectedAccommodation.MinReservationDays)
            {
                MessageBox.Show($"Minimum number of days for reservation is {SelectedAccommodation.MinReservationDays}");
                return;
            }

            AvailableDateRange = _reservationService.FindAvailableDates(SelectedAccommodation, CheckInDate, CheckOutDate, LengthOfStay);
            if(AvailableDateRange.Count == 0)
            {
                AvailableDatesOutsideRange = _reservationService.FindAvailableDatesOutsideRange(SelectedAccommodation, CheckInDate, CheckOutDate, LengthOfStay);
            }

            ReserveAccommodationView reserveAccommodationView = new ReserveAccommodationView(SelectedAccommodation, Guest1, AvailableDateRange, AvailableDatesOutsideRange, CheckInDate, CheckOutDate, LengthOfStay);
            reserveAccommodationView.Show();
            
        }
    }
}
