using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
    /// Interaction logic for GuestRatingView.xaml
    /// </summary>
    public partial class GuestRatingView : Page, INotifyPropertyChanged, INavigationInterface
    {
        public GuestRatingView(AccommodationReservation selectedReservation, Owner owner)
        {
            InitializeComponent();
            GuestRatingViewModel guestRatingViewModel = new GuestRatingViewModel(selectedReservation, owner, this);
            DataContext = guestRatingViewModel;
            //if (guestRatingViewModel.CloseAction == null)
              //  guestRatingViewModel.CloseAction = new Action(this.Close);
        }
        public void GoBack()
        {
            NavigationService?.GoBack();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
