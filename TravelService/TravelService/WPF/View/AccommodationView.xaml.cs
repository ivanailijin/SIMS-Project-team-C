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
using TravelService.Observer;
using TravelService.Repository;
using System.Collections.ObjectModel;
using TravelService.Domain.Model;
using TravelService.WPF.View;
using System.Printing;
using System.Diagnostics.Metrics;
using System.ComponentModel;
using TravelService.WPF.ViewModel;

namespace TravelService.WPF.View
{
    /// <summary>
    /// Interaction logic for AccommodationView.xaml
    /// </summary>
    public partial class AccommodationView : Page
    {
        public AccommodationView(Guest1 guest1)
        {
            InitializeComponent();
            AccommodationViewModel accommodationViewModel = new AccommodationViewModel(this, guest1);
            DataContext = accommodationViewModel;
        }
    }
}

       

