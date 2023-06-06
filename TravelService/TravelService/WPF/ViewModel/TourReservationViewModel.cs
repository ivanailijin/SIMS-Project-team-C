using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TravelService.Applications.UseCases;
using TravelService.Applications.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.Repository;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class TourReservationViewModel : ViewModelBase
    {
        public Guest2 Guest2 { get; set; }
        public Action CloseAction { get; set; }
        public Tour SelectedTour { get; set; }
        public ObservableCollection<Tour> ActiveTours { get; set; }

        private readonly TourReservationService _tourReservationService;

        private readonly TourService _tourService;

        private readonly LocationService _locationService;

        private readonly LanguageService _languageService;

        private readonly CheckPointService _checkpointService;
        public ObservableCollection<Tour> Tours { get; set; }
        public List<Location> Locations { get; set; }
        public List<Language> Languages { get; set; }
        public List<CheckPoint> CheckPoints { get; set; }
        public ObservableCollection<TourReservation> TourReservations { get; set; }
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
        private RelayCommand _showTourDetailsCommand;
        public RelayCommand ShowTourDetailsCommand
        {
            get => _showTourDetailsCommand;
            set
            {
                if (value != _showTourDetailsCommand)
                {
                    _showTourDetailsCommand = value;
                    OnPropertyChanged();
                }
            }
        }
        

        public TourReservationViewModel(Guest2 guest2, Tour selectedTour) {
            _tourReservationService = new TourReservationService(Injector.CreateInstance<ITourReservationRepository>());
            _tourService = new TourService(Injector.CreateInstance<ITourRepository>());
            _locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            _languageService = new LanguageService(Injector.CreateInstance<ILanguageRepository>());
            _checkpointService = new CheckPointService(Injector.CreateInstance<ICheckPointRepository>());

            Guest2 = guest2;
            SelectedTour = selectedTour;
            Tours = new ObservableCollection<Tour>(_tourService.GetAll());
            Locations = new List<Location>(_locationService.GetAll());
            Languages = new List<Language>(_languageService.GetAll());
            CheckPoints = new List<CheckPoint>(_checkpointService.GetAll());
            ActiveTours = new ObservableCollection<Tour>(_tourReservationService.showAllActiveToursNew(Tours.ToList(), Locations, Languages, CheckPoints));

            /*
            _guestVoucherRepository = new GuestVoucherRepository();

            TourReservations = new ObservableCollection<TourReservation>(_tourReservationRepository.GetAll());
            Vouchers = new ObservableCollection<GuestVoucher>(_guestVoucherRepository.GetAll());
            
            ReservationsByTour = new List<TourReservation>();
            ValidVouchers = new List<GuestVoucher>();
            GuestVouchers = new List<GuestVoucher>();
            this.Guest2 = guest2;
            

            OtherTours = new List<Tour>();
            OtherOtherTours = new List<Tour>();

            SelectedTour = selectedTour;
            SelectedVoucher = selectedVoucher;

            ActiveTours = ;
            ValidVouchers = _guestVoucherRepository.showValidVouchers(convertVoucherList(Vouchers), Guest2,GuestVouchers,ValidVouchers);
        
             */
            CancelCommand = new RelayCommand(Execute_CancelCommand, CanExecute_Command);
            ShowTourDetailsCommand = new RelayCommand(Execute_ShowTourDetailsCommand, CanExecute_Command);

        }
        private bool CanExecute_Command(object parameter)
        {
            return true;
        }
        private void Execute_CancelCommand(object sender)
        {
            CloseAction();
        }
        private void Execute_ShowTourDetailsCommand(object sender)
        {
            ShowTourInfoView showTourInfoView = new ShowTourInfoView(Guest2, SelectedTour);
            showTourInfoView.Show();
            CloseAction();
        }

        /*
         private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CheckTourButton_Click(object sender, RoutedEventArgs e)
        {
            bool reservationSuccess = false;
            reservationSuccess = _tourReservationRepository.TryReserving(SelectedTour, EnteredNumberOfGuests, convertTourReservationList(TourReservations), ReservationsByTour, OtherTours, this, Guest2);
            if(SelectedVoucher!=null)
                _guestVoucherRepository.UseVoucher(SelectedVoucher, reservationSuccess); 
        }

        private void UseVoucherButton_Click(object sender, RoutedEventArgs e)
        {
            VoucherView voucherView = new VoucherView(this,SelectedVoucher,SelectedTour,Guest2);
            voucherView.ResetItemSource(ValidVouchers);
            voucherView.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
         */
    }
}
