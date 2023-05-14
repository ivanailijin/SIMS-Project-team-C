using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
using TravelService.Repository;
using TravelService.WPF.ViewModel;

namespace TravelService.WPF.View
{
    /// <summary>
    /// Interaction logic for OwnerRatingView.xaml
    /// </summary>
    public partial class OwnerRatingView : Window
    {
        public OwnerRatingView(Frame frame, RatingViewModel ratingViewModel, AccommodationReservation selectedUnratedOwner)
        {
            InitializeComponent();
            OwnerRatingViewModel ownerRatingViewModel = new OwnerRatingViewModel(frame, ratingViewModel, selectedUnratedOwner);
            DataContext = ownerRatingViewModel;
            if (ownerRatingViewModel.CloseAction == null)
            {
                ownerRatingViewModel.CloseAction = new Action(this.Close);
            }
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            e.Handled = true;
        }
    }
}
