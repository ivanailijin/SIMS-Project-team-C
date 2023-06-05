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
    /// Interaction logic for OwnerWindow.xaml
    /// </summary>
    public partial class OwnerWindow : Window
    {
        public OwnerWindow(Owner owner)
        {
            InitializeComponent();
            DataContext = this;

            MainFrame.Navigate(new OwnerView(owner));
        }

        public void SwitchToPage(Page page)
        {
            MainFrame.Navigate(page);
        }
    }
}
