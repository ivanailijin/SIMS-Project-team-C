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
using TravelService.WPF.Services;
using TravelService.WPF.ViewModel;

namespace TravelService.WPF.View
{
    /// <summary>
    /// Interaction logic for AddForumView.xaml
    /// </summary>
    public partial class AddForumView : Page, INavigationInterface
    {
        public AddForumView(Guest1 guest)
        {
            InitializeComponent();
            AddForumViewModel addForumViewModel = new AddForumViewModel(this, guest);
            DataContext = addForumViewModel;
        }

        public void GoBack()
        {
            NavigationService?.GoBack();
        }
    }
}
