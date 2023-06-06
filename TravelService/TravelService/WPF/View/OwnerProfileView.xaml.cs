using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using TravelService.WPF.Services;
using TravelService.WPF.ViewModel;

namespace TravelService.WPF.View
{
    /// <summary>
    /// Interaction logic for OwnerProfileView.xaml
    /// </summary>
    public partial class OwnerProfileView : Page, INavigationInterface
    {
        public OwnerProfileView(Owner owner)
        {
            InitializeComponent();
            OwnerProfileViewModel ownerProfileViewModel = new OwnerProfileViewModel(owner, this);
            DataContext = ownerProfileViewModel;
        }
        public void GoBack()
        {
            NavigationService?.GoBack();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
