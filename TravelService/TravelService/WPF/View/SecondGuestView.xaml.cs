using System.Windows;
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TravelService.Domain.Model;
using TravelService.WPF.View;

namespace TravelService.WPF.View
{
    public partial class SecondGuestView : Window
    {
        public Tour SelectedTour { get; set; }
        public GuestVoucher SelectedVoucher { get; set; }
        public Guest2 Guest2 { get; set; }
        public SecondGuestView(Guest2 guest2)
        {
            InitializeComponent();
            this.Guest2 = guest2;
        }
        private void TourTrackingViewButton_CLick(object sender, RoutedEventArgs e)
        {
            TourTrackingView tourTrackingView = new TourTrackingView(SelectedTour, Guest2);
            tourTrackingView.Show();
        }
        private void TourViewButton_CLick(object sender, RoutedEventArgs e)
        {
            TourView tourView = new TourView(Guest2);

            tourView.Show();
        }
        private void TourReservationButton_Click(object sender, RoutedEventArgs e)
        {
            TourReservationView tourReservationView = new TourReservationView(SelectedTour,SelectedVoucher, Guest2);
            tourReservationView.Show();
        }

        private void VoucherViewButton_CLick(object sender, RoutedEventArgs e)
        {
            TourReservationView tourReservationView = new TourReservationView(SelectedTour, SelectedVoucher, Guest2);
            VoucherView voucherView = new VoucherView(tourReservationView,SelectedVoucher,SelectedTour,Guest2);
            voucherView.ResetItemSource(voucherView.GuestVouchers);
            voucherView.Show();
        }
        
        private void RateToursViewButton_CLick(object sender, RoutedEventArgs e)
        {
            GuestsToursView guestsToursView = new GuestsToursView(SelectedTour, Guest2);
            guestsToursView.Show();
        }
    }
}
