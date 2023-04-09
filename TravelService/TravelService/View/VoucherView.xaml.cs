using System;
using System.Collections;
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
using TravelService.Model;
using TravelService.Repository;

namespace TravelService.View
{
    public partial class VoucherView : Window
    {
        public readonly GuestVoucherRepository _guestVoucherRepository;
        public static ObservableCollection<GuestVoucher> Vouchers { get; set; }
        public List<GuestVoucher> GuestVouchers { get; set; }
        public Guest2 Guest2 { get; set; }
        public VoucherView(Guest2 guest2)
        {
            InitializeComponent();
            DataContext = this;
            this.Guest2 = guest2;

            _guestVoucherRepository = new GuestVoucherRepository();
            Vouchers = new ObservableCollection<GuestVoucher>(_guestVoucherRepository.GetAll());
            GuestVouchers = new List<GuestVoucher>();

            GuestVouchers = _guestVoucherRepository.showVoucherList(convertVoucherList(Vouchers), Guest2, GuestVouchers);
        }

        public List<GuestVoucher> convertVoucherList(ObservableCollection<GuestVoucher> observableCollection)
        {
            List<GuestVoucher> convertedList = observableCollection.ToList();
            return convertedList;
        }
        public void ResetItemSource(IEnumerable newItemsSource)
        {
            allVouchers.ItemsSource = newItemsSource;
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
