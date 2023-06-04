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
    /// Interaction logic for ForumsView.xaml
    /// </summary>
    public partial class ForumsView : Page
    {
        public ForumsView(Guest1 guest)
        {
            InitializeComponent();
            ForumsViewModel forumsViewModel = new ForumsViewModel(this, guest);
            DataContext = forumsViewModel;
        }
    }
}
