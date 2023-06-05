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
using System.Windows.Shapes;
using TravelService.Domain.Model;
using TravelService.WPF.Services;
using TravelService.WPF.ViewModel;

namespace TravelService.WPF.View
{
    /// <summary>
    /// Interaction logic for SearchAccommodationView.xaml
    /// </summary>
    public partial class SearchAccommodationView : Page, INavigationInterface
    {
        public SearchAccommodationView(AccommodationViewModel accommodationViewModel)
        {
            InitializeComponent();
            SearchAccommodationViewModel searchAccommodationView = new SearchAccommodationViewModel(this, accommodationViewModel);
            DataContext = searchAccommodationView;
        }

        public void GoBack()
        {
            NavigationService?.GoBack();
        }
    }
}
