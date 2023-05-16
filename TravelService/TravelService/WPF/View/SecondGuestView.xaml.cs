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
using TravelService.WPF.ViewModel;
using System.ComponentModel;

namespace TravelService.WPF.View
{
    public partial class SecondGuestView : Window, INotifyPropertyChanged
    {
        public SecondGuestView(Guest2 guest2)
        {
            InitializeComponent();
            SecondGuestViewModel secondGuestViewModel = new SecondGuestViewModel(guest2);
            DataContext = secondGuestViewModel;
            if (secondGuestViewModel.CloseAction == null)
            {
                secondGuestViewModel.CloseAction = new Action(this.Close);
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        public event EventHandler<string> NotificationReceived;

        public void Notify(string message)
        {
            NotificationReceived?.Invoke(this, message);
        }

    }

}
