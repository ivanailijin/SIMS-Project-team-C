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
using TravelService.Domain.Model;
using TravelService.Repository;
using TravelService.WPF.ViewModel;

namespace TravelService.WPF.View
{
    /// <summary>
    /// Interaction logic for ReviewsSelectionView.xaml
    /// </summary>
    public partial class ReviewsSelectionView : Window, INotifyPropertyChanged
    {
        public ReviewsSelectionView(Owner owner)
        {
            InitializeComponent();
            ReviewsSelectionViewModel reviewsSelectionViewModel = new ReviewsSelectionViewModel(owner);
            DataContext = reviewsSelectionViewModel;
            if (reviewsSelectionViewModel.CloseAction == null)
                reviewsSelectionViewModel.CloseAction = new Action(this.Close);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

    }
}
