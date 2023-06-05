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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TravelService.WPF.ViewModel;

namespace TravelService.WPF.View
{
    /// <summary>
    /// Interaction logic for RequestsStatsView.xaml
    /// </summary>
    public partial class RequestsStatsView : Page,INotifyPropertyChanged
    {
        public RequestsStatsView(NavigationService navigationService)
        {
            InitializeComponent();
           this.DataContext = new RequestsStatsViewModel(this, navigationService);
           
        }

    
    public event PropertyChangedEventHandler? PropertyChanged;
    }
    }
