using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using TravelService.Domain.Model;
using TravelService.Repository;
using TravelService.WPF.ViewModel;

namespace TravelService.WPF.View
{
    /// <summary>
    /// Interaction logic for TourOverview.xaml
    /// </summary>
    public partial class ActiveToursView : Page
    {
        public ActiveToursView(Tour selectedTour, NavigationService navigationService)
        {
            InitializeComponent();
            this.DataContext = new ActiveToursViewModel(this, selectedTour, navigationService);



        }
    }
}