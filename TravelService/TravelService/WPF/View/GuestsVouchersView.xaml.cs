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
    /// Interaction logic for GuestsVouchersView.xaml
    /// </summary>
    public partial class GuestsVouchersView : Window, INotifyPropertyChanged
    {
        public GuestsVouchersView(Guest2 guest2)
        {
            InitializeComponent();
            GuestsVouchersViewModel guestsVouchersViewModel = new GuestsVouchersViewModel(guest2);
            DataContext = guestsVouchersViewModel;
            if (guestsVouchersViewModel.CloseAction == null)
            {
                guestsVouchersViewModel.CloseAction = new Action(this.Close);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
