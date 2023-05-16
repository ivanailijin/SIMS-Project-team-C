using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class FirstGuestViewModel : ViewModelBase
    {
        public Frame Frame { get; set; }
        public Window FirstWindow { get; set; }
        public Guest1 Guest1 { get; set; }
        public RelayCommand NavigateToAccommodationViewCommand { get; set; }
        public RelayCommand NavigateToRatingViewCommand { get; set; }
        public RelayCommand NavigateToReservationsViewCommand { get; set; }
        public RelayCommand NavigateToForumsViewCommand { get; set; }
        public RelayCommand NavigateToProfileViewCommand { get; set; }

        private void Execute_NavigateToAccommodationViewCommand(object obj)
        {
            AccommodationView accommodationView = new AccommodationView(Guest1);
            this.Frame.NavigationService.Navigate(accommodationView);
        }

        private bool CanExecute_NavigateCommand(object obj)
        {
            return true;
        }

        private void Execute_NavigateToRatingViewCommand(object obj)
        {
            RatingView ratingView = new RatingView(Guest1, Frame);
            this.Frame.NavigationService.Navigate(ratingView);
        }

        private void Execute_NavigateToReservationsViewCommand(object obj)
        {
            ReservationsView reservationsView = new ReservationsView(Guest1);
            this.Frame.NavigationService.Navigate(reservationsView);
        }

        private void Execute_NavigateToProfileViewCommand(object obj)
        {
            FirstGuestProfileView firstGuestProfileView = new FirstGuestProfileView(FirstWindow, Guest1);
            this.Frame.NavigationService.Navigate(firstGuestProfileView);
        }

        public FirstGuestViewModel(Window firstWindow,Frame frame, Guest1 guest1)
        {
            NavigateToAccommodationViewCommand = new RelayCommand(Execute_NavigateToAccommodationViewCommand, CanExecute_NavigateCommand);
            NavigateToRatingViewCommand = new RelayCommand(Execute_NavigateToRatingViewCommand, CanExecute_NavigateCommand);
            NavigateToReservationsViewCommand = new RelayCommand(Execute_NavigateToReservationsViewCommand, CanExecute_NavigateCommand);
            NavigateToProfileViewCommand = new RelayCommand(Execute_NavigateToProfileViewCommand, CanExecute_NavigateCommand);

            FirstWindow = firstWindow;
            Frame = frame;
            Guest1 = guest1;
        } 
    }
}
