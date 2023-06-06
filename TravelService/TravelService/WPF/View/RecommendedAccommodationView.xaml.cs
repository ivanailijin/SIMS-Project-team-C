using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for RecommendedAccommodationView.xaml
    /// </summary>
    public partial class RecommendedAccommodationView : Page, INavigationInterface
    {
        public RecommendedAccommodationView(Guest1 guest, List<Accommodation> accommodations,DateTime? checkInDate, DateTime? checkOutDate, int guestNumber, int lengthOfStay)
        {
            InitializeComponent();
            RecommendedAccommodationViewModel recommendedAccommodationViewModel = new RecommendedAccommodationViewModel(this, accommodations, guest, checkInDate, checkOutDate, guestNumber, lengthOfStay);
            DataContext = recommendedAccommodationViewModel;
        }

        public void GoBack()
        {
            NavigationService?.GoBack();
        }
    }
}
