using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using TravelService.Application.UseCases;
using TravelService.Application.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class FindAvailableAccommodationsViewModel : ViewModelBase
    {
        private AccommodationService accommodationService;
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
            accommodationService = new AccommodationService(Injector.CreateInstance<IAccommodationRepository>());
            Guest1 = guest1;
            FindAvailableAccommodationsView = findAvailableAccommodationsView;

            SearchAvailableAccommodationsCommand = new RelayCommand(Execute_SearchAvailableAccommodations, CanExecute_Command);
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

        private void Execute_SearchAvailableAccommodations(object sender)
        {
            //logika za pretragu
            List<Accommodation> accommodations = accommodationService.GetAll();
            FirstGuestView firstGuestView = new FirstGuestView(Guest1);
            firstGuestView.frame.Navigate(new RecommendedAccommodationView(Guest1, accommodations, GuestNumber, LengthOfStay));
            FirstGuestWindow firstGuestWindow = Window.GetWindow(FindAvailableAccommodationsView) as FirstGuestWindow ?? new(Guest1);
            firstGuestWindow?.SwitchToPage(firstGuestView);
        }

        private void Execute_PreviousPage(object sender)
        {
            GoBack();
        }
    }
}
