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
    /// Interaction logic for SelectedForumView.xaml
    /// </summary>
    public partial class SelectedForumView : Page, INavigationInterface
    {
        public SelectedForumView(Guest1 guest, Forum selectedForum, ForumsViewModel forumsViewModel)
        {
            InitializeComponent();
            SelectedForumViewModel selectedForumViewModel = new SelectedForumViewModel(this, guest, selectedForum, forumsViewModel);
            DataContext = selectedForumViewModel;
        }

        public void GoBack()
        {
            NavigationService?.GoBack();
        }
    }
}
