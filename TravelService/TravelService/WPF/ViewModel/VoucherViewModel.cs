using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Applications.UseCases;
using TravelService.Applications.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.Repository;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class VoucherViewModel : ViewModelBase
    {
        public GuestVoucher SelectedVoucher { get; set; }
        public Tour SelectedTour { get; set; }
        public Guest2 Guest2 { get; set; }
        public Action CloseAction { get; set; }
        public static ObservableCollection<GuestVoucher> Vouchers { get; set; }
        public List<GuestVoucher> GuestVouchers { get; set; }

        private readonly VoucherService _guestVoucherService;

        private RelayCommand _cancelCommand;
        public RelayCommand CancelCommand
        {
            get => _cancelCommand;
            set
            {
                if (value != _cancelCommand)
                {
                    _cancelCommand = value;
                    OnPropertyChanged();
                }
            }
        }
        private RelayCommand _useVoucherCommand;
        public RelayCommand UseCommand
        {
            get => _useVoucherCommand;
            set
            {
                if (value != _useVoucherCommand)
                {
                    _useVoucherCommand = value;
                    OnPropertyChanged();
                }
            }
        }
        public VoucherViewModel(GuestVoucher selectedVoucher, Tour selectedTour, Guest2 guest2) {
            Guest2 = guest2;
            SelectedTour = selectedTour;
            SelectedVoucher = selectedVoucher;
            _guestVoucherService = new VoucherService(Injector.CreateInstance<IVoucherRepository>());

            Vouchers = new ObservableCollection<GuestVoucher>(_guestVoucherService.GetAll());

            GuestVouchers = _guestVoucherService.showVoucherList(Vouchers.ToList(), Guest2);
            CancelCommand = new RelayCommand(Execute_CancelCommand, CanExecute_Command);
            UseCommand = new RelayCommand(Execute_UseCommand, CanExecute_Command);
        }
        private bool CanExecute_Command(object parameter)
        {
            return true;
        }
        private void Execute_CancelCommand(object sender)
        {
            CloseAction();
        }
        private void Execute_UseCommand(object sender)
        {
            CloseAction();
        }
    }
}
