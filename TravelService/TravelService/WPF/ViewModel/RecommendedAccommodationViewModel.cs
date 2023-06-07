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
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class RecommendedAccommodationViewModel : ViewModelBase
    {
        private AccommodationReservationService _reservationService;
        public Guest1 Guest1 { get; set; }
        public Accommodation SelectedAccommodation { get; set; }
        public RecommendedAccommodationView RecommendedAccommodationView { get; set; }
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public int GuestNumber { get; set; }
        public int LengthOfStay { get; set; }

        private List<Accommodation> _accommodations;
        public List<Accommodation> Accommodations
        {
            get => _accommodations;
            set
            {
                if (value != _accommodations)
                {
                    _accommodations = value;
                    OnPropertyChanged();
                    List<Accommodation> accommodations = _accommodations.ToList();
                   // accommodations = _accommodationService.GetOwnerData(accommodations);
                   // accommodations = _accommodationService.SortBySuperowner(accommodations);
                }
            }
        }

        private RelayCommand _accommodationSelectedCommand;
        public RelayCommand AccommodationSelectedCommand
        {
            get => _accommodationSelectedCommand;
            set
            {
                if (value != _accommodationSelectedCommand)
                {
                    _accommodationSelectedCommand = value;
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
        public RecommendedAccommodationViewModel(RecommendedAccommodationView recommendedAccommodationView,List<Accommodation> accommodations, Guest1 guest1, DateTime? checkInDate, DateTime? checkOutDate, int guestNumber, int lengthOfStay)
        {
            _reservationService = new AccommodationReservationService(Injector.CreateInstance<IAccommodationReservationRepository>());
            Guest1 = guest1;
            RecommendedAccommodationView = recommendedAccommodationView;
            Accommodations = accommodations;
            GuestNumber = guestNumber;
            LengthOfStay = lengthOfStay;
            CheckInDate = checkInDate;
            CheckOutDate = checkOutDate;

            AccommodationSelectedCommand = new RelayCommand(Execute_OnItemSelected, CanExecute_Command);
            PreviousPageCommand = new RelayCommand(Execute_PreviousPage, CanExecute_Command);
        }

        public void GoBack()
        {
            RecommendedAccommodationView.GoBack();
        }

        private bool CanExecute_Command(object parameter)
        {
            return true;
        }

        private void Execute_OnItemSelected(object sender)
        {
            List<Tuple<DateTime, DateTime>> AvailableDateRange = new List<Tuple<DateTime, DateTime>>();

            if (CheckInDate != null && CheckOutDate != null)
            {
                DateTime checkInDate = CheckInDate.Value;
                DateTime checkOutDate = CheckOutDate.Value;
                AvailableDateRange = _reservationService.FindAvailableDates(SelectedAccommodation, checkInDate, checkOutDate, LengthOfStay);
            }
            else
            {
                DateTime checkInDate = DateTime.Today;
                DateTime checkOutDate = checkInDate.AddYears(1);
                AvailableDateRange = _reservationService.FindAvailableDates(SelectedAccommodation, checkInDate, checkOutDate, LengthOfStay);
            }
            ReserveAnywhereView reserveAnywhereView = new ReserveAnywhereView(SelectedAccommodation, Guest1, AvailableDateRange, CheckInDate, CheckOutDate, GuestNumber, LengthOfStay);
            FirstGuestWindow firstGuestWindow = Window.GetWindow(RecommendedAccommodationView) as FirstGuestWindow ?? new(Guest1);
            firstGuestWindow?.SwitchToPage(reserveAnywhereView);
        }

        private void Execute_PreviousPage(object sender)
        {
            GoBack();
        }
    }
}
