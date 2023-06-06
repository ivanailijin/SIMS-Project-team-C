using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using TravelService.Domain.Model;
using TravelService.Repository;
using TravelService.WPF.ViewModel;

namespace TravelService.WPF.View
{
    /// <summary>
    /// Interaction logic for TourReservationView.xaml
    /// </summary>
    public partial class TourReservationView : Window, INotifyPropertyChanged
    {

        public TourReservationView(Tour selectedTour,Guest2 guest2)
        {
            InitializeComponent();
            TourReservationViewModel secondGuestViewModel = new TourReservationViewModel(guest2, selectedTour);
            DataContext = secondGuestViewModel;
            if (secondGuestViewModel.CloseAction == null)
            {
                secondGuestViewModel.CloseAction = new Action(this.Close);
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;        
    }
}