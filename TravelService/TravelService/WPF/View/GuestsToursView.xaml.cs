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
    /// Interaction logic for GuestsToursView.xaml
    /// </summary>
    public partial class GuestsToursView : Window
    {
        public GuestsToursView(Tour selectedTour, Guest2 guest2)
        {
            InitializeComponent();
            GuestsToursViewModel guestsToursViewModel = new GuestsToursViewModel(selectedTour, guest2);
            DataContext = guestsToursViewModel;
            if (guestsToursViewModel.CloseAction == null)
            {
                guestsToursViewModel.CloseAction = new Action(this.Close);
            }
        }
    }
}
