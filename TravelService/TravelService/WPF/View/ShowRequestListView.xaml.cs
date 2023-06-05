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
    public partial class ShowRequestListView : Window, INotifyPropertyChanged
    {
        public ShowRequestListView(Guest2 guest2, ComplexTourRequest selectedComplexRequest)
        {
            InitializeComponent();
            ShowRequestListViewModel showRequestListViewModel = new ShowRequestListViewModel(guest2, selectedComplexRequest);
            DataContext = showRequestListViewModel;
            if (showRequestListViewModel.CloseAction == null)
            {
                showRequestListViewModel.CloseAction = new Action(this.Close);
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
