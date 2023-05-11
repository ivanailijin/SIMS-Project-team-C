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
using System.Windows.Shapes;
using TravelService.Domain.Model;
using TravelService.Repository;
using TravelService.WPF.ViewModel;

namespace TravelService.WPF.View
{
    public partial class TourTrackingView : Window
    {
        public TourTrackingView(Tour selectedTour,Guest2 guest2)
        {
            InitializeComponent();
            TourTrackingViewModel tourTrackingViewModel = new TourTrackingViewModel(selectedTour,guest2);
            DataContext = tourTrackingViewModel;
            if (tourTrackingViewModel.CloseAction == null)
            {
                tourTrackingViewModel.CloseAction = new Action(this.Close);
            }
        }
    }
}
