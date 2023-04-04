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
    public partial class TourTrackingView : Window
    {

        public Guest2 Guest2 { get; set; }
        public TourTrackingView(Guest2 guest2)
        {
            InitializeComponent();
            this.Guest2 = guest2;
        }
    }
}
