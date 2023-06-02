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

namespace TravelService.WPF.View
{
    /// <summary>
    /// Interaction logic for FirstGuestWindow.xaml
    /// </summary>
    public partial class FirstGuestWindow : Window
    {
        public FirstGuestWindow(Guest1 guest)
        {
            InitializeComponent();

            MainFrame.Navigate(new FirstGuestView(guest));
        }

        public void SwitchToPage(Page page)
        {
            MainFrame.Navigate(page);
        }
    }
}
