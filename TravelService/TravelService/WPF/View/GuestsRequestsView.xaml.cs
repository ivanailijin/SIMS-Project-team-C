﻿using System;
using System.Collections.Generic;
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
using TravelService.Domain.Model;
using TravelService.WPF.ViewModel;

namespace TravelService.WPF.View
{
    /// <summary>
    /// Interaction logic for GuestsRequestsView.xaml
    /// </summary>
    public partial class GuestsRequestsView : Window, INotifyPropertyChanged
    {
        public GuestsRequestsView(Guest2 guest2)
        {
            InitializeComponent();
            GuestsRequestsViewModel guestsRequestsViewModel = new GuestsRequestsViewModel(guest2);
            DataContext = guestsRequestsViewModel;
            if (guestsRequestsViewModel.CloseAction == null)
            {
                guestsRequestsViewModel.CloseAction = new Action(this.Close);
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;

    }
}
