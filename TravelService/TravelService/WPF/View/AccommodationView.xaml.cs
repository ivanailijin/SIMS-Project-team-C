using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TravelService.Observer;
using TravelService.Repository;
using System.Collections.ObjectModel;
using TravelService.Domain.Model;
using TravelService.WPF.View;
using System.Printing;
using System.Diagnostics.Metrics;
using System.ComponentModel;
using TravelService.WPF.ViewModel;

namespace TravelService.WPF.View
{
    /// <summary>
    /// Interaction logic for AccommodationView.xaml
    /// </summary>
    public partial class AccommodationView : Window
    {
        public AccommodationView(Guest1 guest1)
        {
            InitializeComponent();
            AccommodationViewModel accommodationViewModel = new AccommodationViewModel(guest1);
            DataContext = accommodationViewModel;
            if (accommodationViewModel.CloseAction == null)
            {
                accommodationViewModel.CloseAction = new Action(this.Close);
            }
        }
        
        /*
        private void ReserveAccommodation_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedAccommodation != null)
            {
                ReserveAccommodationView reservationView = new ReserveAccommodationView(SelectedAccommodation, Guest1);
                reservationView.Show();
            }
            else
            {
                MessageBox.Show("Please select accommodation for reservation.");
            }
        }

        private void OwnerRating_Click(object sender, RoutedEventArgs e)
        {
            RatingView ratingView = new RatingView(Guest1);
            ratingView.Show();
;        }

        private void ShowReservations_Click(object sender, RoutedEventArgs e)
        {
            ReservationsView reservationsView = new ReservationsView(Guest1);
            reservationsView.Show();
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            SignInForm signIn = new SignInForm();
            signIn.Show();
            Close();
        }*/
    
    }
}

       

