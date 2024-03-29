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
using TravelService.Domain.Model;
using TravelService.WPF.ViewModel;

namespace TravelService.WPF.View
{
    /// <summary>
    /// Interaction logic for AccommodationAvailabilityView.xaml
    /// </summary>
    public partial class AccommodationAvailabilityView : Window
    {
        public AccommodationAvailabilityView(Accommodation selectedAccommodation, Guest1 guest1)
        {
            InitializeComponent();
            AccommodationAvailabilityViewModel accommodationAvailabilityViewModel = new AccommodationAvailabilityViewModel(selectedAccommodation, guest1);
            DataContext = accommodationAvailabilityViewModel;
            if (accommodationAvailabilityViewModel.CloseAction == null)
            {
                accommodationAvailabilityViewModel.CloseAction = new Action(this.Close);
            }
        }
    }
}
