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
    public partial class JoinTourView : Window, INotifyPropertyChanged
    {
        public JoinTourView(Tour selectedTour, Guest2 guest2)
        {
            InitializeComponent();
            JoinTourViewModel joinTourViewModel = new JoinTourViewModel(selectedTour, guest2);
            DataContext = joinTourViewModel;
            if (joinTourViewModel.CloseAction == null)
            {
                joinTourViewModel.CloseAction = new Action(this.Close);
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
