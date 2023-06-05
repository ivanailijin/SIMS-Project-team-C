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
using TravelService.Domain.Model;
using TravelService.WPF.ViewModel;

namespace TravelService.WPF.View
{
    /// <summary>
    /// Interaction logic for ClosingForumView.xaml
    /// </summary>
    public partial class ClosingForumView : Window
    {
        public ClosingForumView(Forum selectedForum, SelectedForumViewModel selectedForumViewModel)
        {
            InitializeComponent();
            ClosingForumViewModel closingForumViewModel = new ClosingForumViewModel(selectedForum, selectedForumViewModel);
            DataContext = closingForumViewModel;
            if (closingForumViewModel.CloseAction == null)
            {
                closingForumViewModel.CloseAction = new Action(this.Close);
            }
        }
    }
}
