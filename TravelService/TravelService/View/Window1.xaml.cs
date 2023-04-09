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

namespace TravelService.View
{
    /// <summary>
    /// Interaction logic for SecondGuestView.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Tour SelectedTour { get; set; }
        public Window1()
        {
            InitializeComponent();
        }


        private void AddTourView_CLick(object sender, RoutedEventArgs e)
        {
            AddTour addTour = new AddTour();
            addTour.Show();
        }
        private void TourOverview_Click(object sender, RoutedEventArgs e)
        {
            TourOverview overView = new TourOverview(SelectedTour);
            overView.Show();
            Close();
        }
    }
}