using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TravelService.Domain.Model;
using TravelService.Repository;
using TravelService.WPF.ViewModel;

namespace TravelService.WPF.View
{
    public partial class TourView : Window, INotifyPropertyChanged
    {
        public TourView(Guest2 guest2, Tour selectedTour)
        {
            InitializeComponent();
            TourViewViewModel tourViewViewModel = new TourViewViewModel(guest2, selectedTour);
            DataContext = tourViewViewModel;
            if (tourViewViewModel.CloseAction == null)
            {
                tourViewViewModel.CloseAction = new Action(this.Close);
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}