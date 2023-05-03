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
using System.Xaml.Schema;
using TravelService.Domain.Model;
using TravelService.Repository;
using TravelService.WPF.ViewModel;

namespace TravelService.WPF.View
{
    /// <summary>
    /// Interaction logic for RatingView.xaml
    /// </summary>
    public partial class RatingView : Window
    {
        public RatingView(Guest1 guest1)
        {
            InitializeComponent();
            RatingViewModel ratingViewModel = new RatingViewModel(guest1);
            DataContext = ratingViewModel;
            if (ratingViewModel.CloseAction == null)
            {
                ratingViewModel.CloseAction = new Action(this.Close);
            }
        }
    }
}
