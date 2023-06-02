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
using TravelService.WPF.ViewModel;

namespace TravelService.WPF.View
{
    public partial class ChoooseRequestListView : Window
    {
        public ChoooseRequestListView(Guest2 guest2)
        {
            InitializeComponent();
            ChooseRequestListViewModel chooseRequestListViewModel = new ChooseRequestListViewModel(guest2);
            DataContext = chooseRequestListViewModel;
            if (chooseRequestListViewModel.CloseAction == null)
            {
                chooseRequestListViewModel.CloseAction = new Action(this.Close);
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
