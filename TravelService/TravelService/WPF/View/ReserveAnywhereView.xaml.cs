using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TravelService.Domain.Model;
using TravelService.WPF.Services;
using TravelService.WPF.ViewModel;

namespace TravelService.WPF.View
{
    /// <summary>
    /// Interaction logic for ReserveAnywhereView.xaml
    /// </summary>
    public partial class ReserveAnywhereView : Page, INavigationInterface
    {
        public ReserveAnywhereView(Accommodation selectedAccommodation, Guest1 guest, List<Tuple<DateTime, DateTime>> availableDates, DateTime? checkInDate, DateTime? checkOutDate, int guestNumber, int lengthOfStay)
        {
            InitializeComponent();
            ReserveAnywhereViewModel reserveAnywhereViewModel = new ReserveAnywhereViewModel(this, selectedAccommodation, guest, availableDates, checkInDate, checkOutDate, guestNumber, lengthOfStay);
            DataContext = reserveAnywhereViewModel;
        }

        public void GoBack()
        {
            NavigationService?.GoBack();
        }
    }
}
