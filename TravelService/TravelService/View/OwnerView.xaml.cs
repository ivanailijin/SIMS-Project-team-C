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
using TravelService.Model;

namespace TravelService.View
{
    /// <summary>
    /// Interaction logic for OwnerView.xaml
    /// </summary>
    public partial class OwnerView : Window
    {
        public Owner Owner { get; set; }

        public OwnerView(Owner owner)
        {
            this.Owner = owner;
            InitializeComponent();
        }

        private void AddAccommodation_Click(object sender, RoutedEventArgs e)
        {
            AddAccommodation addAccommodation = new AddAccommodation(Owner);
            addAccommodation.Show();
        }

        private void GuestRating_Click(object sender, RoutedEventArgs e)
        {
            GuestRatingOverview ratingOverview = new GuestRatingOverview(Owner);
            ratingOverview.Show();
        }
    }
}
