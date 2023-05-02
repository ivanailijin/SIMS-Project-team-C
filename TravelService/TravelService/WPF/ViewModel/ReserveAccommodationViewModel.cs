using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Application.UseCases;
using TravelService.Application.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.Repository;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class ReserveAccommodationViewModel : ViewModelBase
    {
        private readonly AccommodationReservationService _accommodationReservationService;
        public Accommodation SelectedAccommodation { get; set; }
        public Guest1 LoggedInGuest1 { get; set; }
        public ObservableCollection<AccommodationReservation> Reservations { get; set; }
        public Tuple<DateTime, DateTime> SelectedAvailableDatePair { get; set; }
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

        public ReserveAccommodationViewModel(Accommodation selectedAccommodation, Guest1 guest1, List<Tuple<DateTime, DateTime>> availableDateRange, List<Tuple<DateTime, DateTime>> availableDateOutsideRange, DateTime checkInDate, DateTime checkOutDate, int lengthOfStay)
        {
            AvailableDatesPair = new ObservableCollection<Tuple<DateTime, DateTime>>(availableDateRange);
            if(availableDateRange.Count == 0)
            {
                foreach(var availableDate in availableDateOutsideRange)
                {
                    AvailableDatesPair.Add(availableDate);
                }
            }
        }

        private bool CanExecute_Command(object parameter)
        {
            return true;
        }
    }
}
