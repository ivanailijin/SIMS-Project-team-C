﻿using System;
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
using TravelService.Model;

namespace TravelService.View
{
    /// <summary>
    /// Interaction logic for SecondGuestView.xaml
    /// </summary>
    public partial class SecondGuestView : Window
    {
        public Tour SelectedTour { get; set; }
        public SecondGuestView()
        {
            InitializeComponent();
        }

        private void TourViewButton_CLick(object sender, RoutedEventArgs e)
        {
            TourView tourView = new TourView();
            tourView.Show();
        }
        private void TourReservationButton_Click(object sender, RoutedEventArgs e)
        {
            TourReservationView tourReservationView = new TourReservationView(SelectedTour);
            tourReservationView.Show();
        }
    }
}
