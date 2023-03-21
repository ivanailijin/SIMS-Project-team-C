﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using TravelService.Repository;

namespace TravelService.View
{
    /// <summary>
    /// Interaction logic for GuestRatingOverview.xaml
    /// </summary>
    public partial class GuestRatingOverview : Window, INotifyPropertyChanged
    {
        private readonly AccommodationReservationRepository _reservationRepository;

        private readonly AccommodationRepository _accommodationRepository;
        public ObservableCollection<AccommodationReservation> UnratedReservations { get; set; }

        public AccommodationReservation SelectedReservation { get; set; }

        public Owner Owner { get; set; }
        public GuestRatingView Child { get; set;  }
        public GuestRatingOverview(Owner owner)
        {
            InitializeComponent();
            this.Owner = owner;
            _reservationRepository = new AccommodationReservationRepository();
            _accommodationRepository = new AccommodationRepository();
            UnratedReservations = new ObservableCollection<AccommodationReservation>();

            List<AccommodationReservation> reservationList = _reservationRepository.GetAll();

            foreach (AccommodationReservation reservation in reservationList)
            {
                Accommodation reservedAccommodation = _accommodationRepository.FindById(reservation.AccommodationId);
                TimeSpan dayDifference = DateTime.Today - reservation.CheckOutDate;
                if (!reservation.IsRated && dayDifference.Days < 5 && dayDifference.Days > 0 &&  reservedAccommodation.OwnerId == Owner.Id)
                {
                    UnratedReservations.Add(reservation);
                }
            }

            DataContext = this;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void Rating_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                GuestRatingView guestRatingView = new GuestRatingView(SelectedReservation.Id, SelectedReservation);
                guestRatingView.Parent = this;
                guestRatingView.ShowDialog();
            });
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}