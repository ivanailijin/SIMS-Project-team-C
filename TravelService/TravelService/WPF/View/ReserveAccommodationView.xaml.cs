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
using TravelService.WPF.ViewModel;

namespace TravelService.WPF.View
{
    /// <summary>
    /// Interaction logic for ReserveAccommodationView.xaml
    /// </summary>
    public partial class ReserveAccommodationView : Window
    {
        public ReserveAccommodationView(Accommodation selectedAccommodation, Guest1 guest1, List<Tuple<DateTime, DateTime>> availableDateRange, List<Tuple<DateTime, DateTime>> availableDateOutsideRange, int lengthOfStay)
        {
            InitializeComponent();
            ReserveAccommodationViewModel reserveAccommodationViewModel = new ReserveAccommodationViewModel(selectedAccommodation, guest1, availableDateRange, availableDateOutsideRange, lengthOfStay);
            DataContext = reserveAccommodationViewModel;
            if (reserveAccommodationViewModel.CloseAction == null)
            {
                reserveAccommodationViewModel.CloseAction = new Action(this.Close);
            }
        }
    }
}
