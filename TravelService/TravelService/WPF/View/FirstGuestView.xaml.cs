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
    /// Interaction logic for FirstGuestView.xaml
    /// </summary>
    public partial class FirstGuestView : Window
    {
        public FirstGuestViewModel firstGuestViewModel { get; set; }
        public FirstGuestView(Guest1 guest1)
        {
            InitializeComponent();
            var accommodationView = new AccommodationView(guest1);
            frame.Content = accommodationView;
            this.firstGuestViewModel = new FirstGuestViewModel(frame, guest1);
            this.DataContext = this.firstGuestViewModel;
        }
    }
}
