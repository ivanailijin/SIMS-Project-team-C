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
        public Page FirstGuestView { get; set; }
        public Guest1 Guest1 { get; set; }
        public RelayCommand NavigateToAccommodationViewCommand { get; set; }
        public RelayCommand NavigateToRatingViewCommand { get; set; }
        public RelayCommand NavigateToReservationsViewCommand { get; set; }
        public RelayCommand NavigateToForumsViewCommand { get; set; }
        public RelayCommand NavigateToProfileViewCommand { get; set; }

        private void Execute_NavigateToAccommodationViewCommand(object obj)
        {
            AccommodationView accommodationView = new AccommodationView(Guest1);
            Frame.Navigate(accommodationView);
        }

        private bool CanExecute_NavigateCommand(object obj)
        {
            return true;
        }

        private void Execute_NavigateToRatingViewCommand(object obj)
        {
            RatingView ratingView = new RatingView(Guest1);
            Frame.Navigate(ratingView);
        }

        private void Execute_NavigateToReservationsViewCommand(object obj)
        {
            ReservationsView reservationsView = new ReservationsView(Guest1);
            Frame.Navigate(reservationsView);
        }

        private void Execute_NavigateToForumsViewCommand(object obj)
        {
            ForumsView forumsView = new ForumsView(Guest1);
            Frame.Navigate(forumsView);
        }

        private void Execute_NavigateToProfileViewCommand(object obj)
        {
            FirstGuestProfileView firstGuestProfileView = new FirstGuestProfileView(Guest1);
            Frame.Navigate(firstGuestProfileView);
        }

        public FirstGuestViewModel(Page firstGuestView,Frame frame, Guest1 guest1)
        {
            NavigateToAccommodationViewCommand = new RelayCommand(Execute_NavigateToAccommodationViewCommand, CanExecute_NavigateCommand);
            NavigateToRatingViewCommand = new RelayCommand(Execute_NavigateToRatingViewCommand, CanExecute_NavigateCommand);
            NavigateToReservationsViewCommand = new RelayCommand(Execute_NavigateToReservationsViewCommand, CanExecute_NavigateCommand);
            NavigateToForumsViewCommand = new RelayCommand(Execute_NavigateToForumsViewCommand, CanExecute_NavigateCommand);
            NavigateToProfileViewCommand = new RelayCommand(Execute_NavigateToProfileViewCommand, CanExecute_NavigateCommand);

            FirstGuestView = firstGuestView;
            Frame = frame;
            Guest1 = guest1;
            Frame.Navigate(new AccommodationView(Guest1));
        } 
    }
}
