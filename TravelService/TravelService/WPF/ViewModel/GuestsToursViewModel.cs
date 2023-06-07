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
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class GuestsToursViewModel : ViewModelBase
    {
        public Guide Guide { get; set; }
        private readonly TourService _tourService;

        private readonly TourReviewService _tourReviewService;

        private readonly LocationService _locationService;

        private readonly LanguageService _languageService;

        private readonly CheckPointService _checkpointService;

        private readonly GuestService _guestService;
        public List<Tour> PastTours { get; set; }
        public List<Tour> GuestsTours { get; set; }
        public Tour SelectedTour { get; set; }
        public Guest2 Guest2 { get; set; }
        public Action CloseAction { get; set; }

        private RelayCommand _rateTour;
        public RelayCommand RateTourCommand
        {
            get => _rateTour;
            set
            {
                if (value != _rateTour)
                {
                    _rateTour = value;
                    OnPropertyChanged();
                }
            }
        }
        private RelayCommand _voucherViewCommand;
        public RelayCommand VoucherViewCommand
        {
            get => _voucherViewCommand;
            set
            {
                if (value != _voucherViewCommand)
                {
                    _voucherViewCommand = value;
                    OnPropertyChanged();
                }
            }
        }
        private RelayCommand _homePageCommand;
        public RelayCommand HomePageCommand
        {
            get => _homePageCommand;
            set
            {
                if (value != _homePageCommand)
                {
                    _homePageCommand = value;
                    OnPropertyChanged();
                }
            }
        }
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
        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                if (value != _username)
                {
                    _username = value;
                    OnPropertyChanged();
                }
            }
        }
        public GuestsToursViewModel(Tour selectedTour, Guest2 guest2) 
        {
            
            _tourService = new TourService(Injector.CreateInstance<ITourRepository>());
            _tourReviewService = new TourReviewService(Injector.CreateInstance<ITourReviewRepository>());
            _locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            _languageService = new LanguageService(Injector.CreateInstance<ILanguageRepository>());
            _checkpointService = new CheckPointService(Injector.CreateInstance<ICheckPointRepository>());
            _guestService = new GuestService(Injector.CreateInstance<IGuestRepository>());
            List<Tour> Tours = _tourService.GetAll();
            List<Location> Locations = _locationService.GetAll();
            List<Language> Languages = _languageService.GetAll();
            List<CheckPoint> CheckPoints = _checkpointService.GetAll();
            List<Guest> Guests = _guestService.GetAll();
            List<TourReview> TourReviews = _tourReviewService.GetAll();
            SelectedTour = selectedTour;
            Guest2 = guest2;
            Username = guest2.Username;
            GuestsTours = _tourService.ShowGuestTourList(Tours, Locations, Languages, CheckPoints, Guests, Guest2);
            HomePageCommand = new RelayCommand(Execute_HomePageCommand, CanExecute_Command);
            RateTourCommand = new RelayCommand(Execute_RateTour, CanExecute_Command);
            CancelCommand = new RelayCommand(Execute_Cancel, CanExecute_Command);
            VoucherViewCommand = new RelayCommand(Execute_VoucherViewCommand, CanExecute_Command);
        }
        private bool CanExecute_Command(object parameter)
        {
            return true;
        }
        private void Execute_RateTour(object sender)
        {
            if (SelectedTour != null)
            {
                AddReviewView addReviewView = new AddReviewView(SelectedTour, Guest2);
                addReviewView.Show();
            }
            else MessageBox.Show("Choose the tour you want to rate!");
            
        }
        private void Execute_HomePageCommand(object sender)
        {
            SecondGuestView secondGuestView = new SecondGuestView(Guest2);
            secondGuestView.Show();
            CloseAction();
        }
        private void Execute_VoucherViewCommand(object sender)
        {
            GuestsVouchersView guestsVouchersView = new GuestsVouchersView(Guest2);
            guestsVouchersView.Show();
        }
        private void Execute_Cancel(object sender)
        {
            CloseAction();
        }
    }
}
