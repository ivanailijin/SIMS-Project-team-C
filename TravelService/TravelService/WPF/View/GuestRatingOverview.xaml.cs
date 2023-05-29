using System;
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
using TravelService.WPF.Services;
using TravelService.WPF.ViewModel;

namespace TravelService.WPF.View
{
    /// <summary>
    /// Interaction logic for GuestRatingOverview.xaml
    /// </summary>
    public partial class GuestRatingOverview : Page, INotifyPropertyChanged, INavigationInterface
    {
        public GuestRatingOverview(Owner owner)
        {
            InitializeComponent();
            GuestRatingOverviewViewModel guestRatingOverviewViewModel = new GuestRatingOverviewViewModel(owner, this);
            DataContext = guestRatingOverviewViewModel;
            //if (guestRatingOverviewViewModel.CloseAction == null)
                //guestRatingOverviewViewModel.CloseAction = new Action(this.Close);
        }
        public void GoBack()
        {
            NavigationService?.GoBack();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
