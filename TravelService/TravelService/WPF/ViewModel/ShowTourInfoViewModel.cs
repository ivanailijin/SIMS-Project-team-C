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
    public class ShowTourInfoViewModel : ViewModelBase
    {
        public string EnteredNumberOfGuests { get; set; } = string.Empty;
        public Tour SelectedTour { get; set; }
        public Tour CurrentSelectedTour { get; set; }
        public GuestVoucher SelectedVoucher { get; set; } = null;
        public Guest2 Guest2 { get; set; }
        public Action CloseAction { get; set; }

        private readonly TourReservationService _tourReservationService;

        private readonly TourService _tourService;

        private readonly LocationService _locationService;

        private readonly LanguageService _languageService;

        private readonly CheckPointService _checkpointService;

        private readonly VoucherService _guestVoucherService;
        public ObservableCollection<Tour> Tours { get; set; }
        public ObservableCollection<Tour> CurrentTours { get; set; }
        public List<Location> Locations { get; set; }
        public List<Language> Languages { get; set; }
        public List<CheckPoint> CheckPoints { get; set; }
        public List<GuestVoucher> ValidVouchers { get; set; }
        public static ObservableCollection<TourReservation> TourReservations { get; set; }
        public static ObservableCollection<GuestVoucher> Vouchers { get; set; }

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
        public RelayCommand UseVoucherCommand
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
        private RelayCommand _reserveCommand;
        public RelayCommand ReserveCommand
        {
            get => _reserveCommand;
            set
            {
                if (value != _reserveCommand)
                {
                    _reserveCommand = value;
                    OnPropertyChanged();
                }
            }
        }
        public ShowTourInfoViewModel(Guest2 guest2, Tour selectedTour, GuestVoucher selectedVoucher) {

            _tourReservationService = new TourReservationService(Injector.CreateInstance<ITourReservationRepository>());
            _tourService = new TourService(Injector.CreateInstance<ITourRepository>());
            _locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            _languageService = new LanguageService(Injector.CreateInstance<ILanguageRepository>());
            _checkpointService = new CheckPointService(Injector.CreateInstance<ICheckPointRepository>());
            _guestVoucherService = new VoucherService(Injector.CreateInstance<IVoucherRepository>());

            TourReservations = new ObservableCollection<TourReservation>(_tourReservationService.GetAll());
            Vouchers = new ObservableCollection<GuestVoucher>(_guestVoucherService.GetAll());
            Tours = new ObservableCollection<Tour>(_tourService.GetAll());
            CurrentTours = new ObservableCollection<Tour>();
            Locations = new List<Location>(_locationService.GetAll());
            Languages = new List<Language>(_languageService.GetAll());
            CheckPoints = new List<CheckPoint>(_checkpointService.GetAll());
            Guest2 = guest2;
            SelectedTour = selectedTour;
            SelectedVoucher = selectedVoucher;
            CurrentSelectedTour = _tourReservationService.FindTourInActiveTours(selectedTour,Tours.ToList(), Locations, Languages, CheckPoints);
            CurrentTours.Add(CurrentSelectedTour);
            ValidVouchers = _guestVoucherService.showValidVouchers(Vouchers.ToList(), Guest2);

            CancelCommand = new RelayCommand(Execute_CancelCommand, CanExecute_Command);
            UseVoucherCommand = new RelayCommand(Execute_UseVoucherCommand, CanExecute_Command);
            ReserveCommand = new RelayCommand(Execute_ReserveCommand, CanExecute_Command);

        }
        private bool CanExecute_Command(object parameter)
        {
            return true;
        }
        private void Execute_CancelCommand(object sender)
        {
            CloseAction();
        }
        private void Execute_UseVoucherCommand(object sender)
        {
            VoucherView voucherView = new VoucherView(SelectedVoucher, SelectedTour, Guest2);
            voucherView.Show();
        }
        private void Execute_ReserveCommand(object sender)
        {
            SecondGuestView secondGuestView = new SecondGuestView(Guest2);
            secondGuestView.Show();
        }
    }
}
