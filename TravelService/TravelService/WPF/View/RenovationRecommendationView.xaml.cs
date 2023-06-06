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
using TravelService.Applications.UseCases;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.WPF.ViewModel;

namespace TravelService.WPF.View
{
    /// <summary>
    /// Interaction logic for RenovationRecommendationView.xaml
    /// </summary>
    public partial class RenovationRecommendationView : Page
    {
        public RenovationRecommendationView(AccommodationReservation selectedAccommodationReservation, Guest1 guest, OwnerRating ownerRating)
        {
            InitializeComponent();
            RenovationRecommendationViewModel renovationRecommendationViewModel = new RenovationRecommendationViewModel(this, selectedAccommodationReservation, guest, ownerRating);
            DataContext = renovationRecommendationViewModel;
        }

        public void GoBack()
        {
            NavigationService?.GoBack();
        }
    }
}
