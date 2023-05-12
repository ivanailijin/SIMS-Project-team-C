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
using TravelService.Application.UseCases;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.WPF.ViewModel;

namespace TravelService.WPF.View
{
    /// <summary>
    /// Interaction logic for RenovationRecommendationView.xaml
    /// </summary>
    public partial class RenovationRecommendationView : Window
    {
        public RenovationRecommendationView(AccommodationReservation selectedAccommodationReservation)
        {
            InitializeComponent();
            RenovationRecommendationViewModel renovationRecommendationViewModel = new RenovationRecommendationViewModel(selectedAccommodationReservation);
            DataContext = renovationRecommendationViewModel;
            if (renovationRecommendationViewModel.CloseAction == null)
            {
                renovationRecommendationViewModel.CloseAction = new Action(this.Close);
            }
        }
    }
}
