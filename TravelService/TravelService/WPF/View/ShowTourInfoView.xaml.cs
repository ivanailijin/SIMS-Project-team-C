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
    /// <summary>
    /// Interaction logic for ShowTourInfoView.xaml
    /// </summary>
    public partial class ShowTourInfoView : Window, INotifyPropertyChanged
    {
        GuestVoucher selectedVoucher { get; set; }
        public ShowTourInfoView(Guest2 guest2, Tour selectedTour)
        {
            InitializeComponent();
            ShowTourInfoViewModel showTourInfoViewModel = new ShowTourInfoViewModel(guest2, selectedTour, selectedVoucher);
            DataContext = showTourInfoViewModel;
            if (showTourInfoViewModel.CloseAction == null)
            {
                showTourInfoViewModel.CloseAction = new Action(this.Close);
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
