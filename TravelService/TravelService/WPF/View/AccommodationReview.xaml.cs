using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using TravelService.WPF.Services;
using TravelService.WPF.ViewModel;

namespace TravelService.WPF.View
{
    /// <summary>
    /// Interaction logic for AccommodationReview.xaml
    /// </summary>
    public partial class AccommodationReview : Page, INotifyPropertyChanged, INavigationInterface
    {
        public AccommodationReview(Accommodation selectedAccommodation, Owner owner)
        {
            InitializeComponent();
            AccommodationReviewViewModel accommodationReviewViewModel = new AccommodationReviewViewModel(selectedAccommodation, owner, this);
            DataContext = accommodationReviewViewModel;
        }
        public void GoBack()
        {
            NavigationService?.GoBack();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
