using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using TravelService.Domain.Model;
using TravelService.Repository;
using TravelService.WPF.ViewModel;

namespace TravelService.WPF.View
{
    /// <summary>
    /// Interaction logic for TourOverview.xaml
    /// </summary>
    public partial class ActiveToursView : Window
    {
        public ActiveToursView(Tour selectedTour)
        {
            InitializeComponent();
            ActiveToursViewModel activeToursViewModel = new ActiveToursViewModel(selectedTour);
            DataContext = activeToursViewModel;
            if (activeToursViewModel.CloseAction == null)
                activeToursViewModel.CloseAction = new Action(this.Close);
        }

        
    }
}