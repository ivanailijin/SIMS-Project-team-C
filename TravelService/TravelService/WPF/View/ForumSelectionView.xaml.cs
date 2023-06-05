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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TravelService.Domain.Model;
using TravelService.WPF.Services;
using TravelService.WPF.ViewModel;

namespace TravelService.WPF.View
{
    /// <summary>
    /// Interaction logic for ForumSelectionView.xaml
    /// </summary>
    public partial class ForumSelectionView : Page, INotifyPropertyChanged, INavigationInterface
    {
        public ForumSelectionView(Owner owner)
        {
            InitializeComponent();
            ForumSelectionViewModel forumSelectionViewModel = new ForumSelectionViewModel(owner, this);
            DataContext = forumSelectionViewModel;
        }
        public void GoBack()
        {
            NavigationService?.GoBack();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}

