using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using TravelService.Repository;
using TravelService.WPF.ViewModel;

namespace TravelService.WPF.View
{
    public partial class VoucherView : Window, INotifyPropertyChanged
    {
        public VoucherView(GuestVoucher selectedVoucher, Tour selectedTour, Guest2 guest2)
        {
            InitializeComponent();
            VoucherViewModel voucherViewModel = new VoucherViewModel(selectedVoucher, selectedTour, guest2);
            DataContext = voucherViewModel;
            if (voucherViewModel.CloseAction == null)
            {
                voucherViewModel.CloseAction = new Action(this.Close);
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}