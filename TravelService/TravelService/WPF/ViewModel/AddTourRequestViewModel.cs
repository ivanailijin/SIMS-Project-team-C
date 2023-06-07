using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;

using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.WPF.View;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TravelService.Applications.UseCases;
using TravelService.Applications.Utils;

namespace TravelService.WPF.ViewModel
{
    public class AddTourRequestViewModel : ViewModelBase

    {
        private readonly TourRequestService _tourRequestService;

     
       

        private readonly LocationService _locationService;
        private readonly LanguageService _languageService;
        public Guest2 Guest2 { get; set; }
        public Action CloseAction { get; set; }
        public bool IsForwarded { get; set; }

        private ObservableCollection<TourRequest> tourRequests;
        public ObservableCollection<TourRequest> TourRequests
        {
            get { return tourRequests; }
            set
            {
                tourRequests = value;
                OnPropertyChanged(nameof(TourRequests));
            }
        }

        private string _location;
        public string Location
        {
            get => _location;
            set
            {
                if (value != _location)
                {
                    _location = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                if (value != _description)
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _language;
        public string Language
        {
            get => _language;
            set
            {
                if (value != _language)
                {
                    _language = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _guestNumber;
        public int GuestNumber
        {
            get => _guestNumber;
            set
            {
                if (value != _guestNumber)
                {
                    _guestNumber = value;
                    OnPropertyChanged();
                }
            }
        }
        private DateTime _tourStart;
        public DateTime TourStart
        {
            get => _tourStart;
            set
            {
                if (value != _tourStart)
                {
                    _tourStart = value;
                    OnPropertyChanged();
                }
            }
        }
        private DateTime _tourEnd;
        public DateTime TourEnd
        {
            get => _tourEnd;
            set
            {
                if (value != _tourEnd)
                {
                    _tourEnd = value;
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
        private RelayCommand _addRequestCommand;
        public RelayCommand AddRequestCommand
        {
            get => _addRequestCommand;
            set
            {
                if (value != _addRequestCommand)
                {
                    _addRequestCommand = value;
                    OnPropertyChanged();
                }
            }
        }
        private RelayCommand _requestCommand;
        public RelayCommand RequestCommand
        {
            get => _requestCommand;
            set
            {
                if (value != _requestCommand)
                {
                    _requestCommand = value;
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
        public TourRequest TourRequest { get; set; }

        public AddTourRequestViewModel(Guest2 guest2, bool isForwarded, ObservableCollection<TourRequest> tourRequests)
        {
        
            _tourRequestService = new TourRequestService(Injector.CreateInstance<ITourRequestRepository>());
            _locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            _languageService = new LanguageService(Injector.CreateInstance<ILanguageRepository>());

            Guest2 = guest2;
            IsForwarded = isForwarded;
            TourRequests = tourRequests;
            AddRequestCommand = new RelayCommand(Execute_AddRequestCommand, CanExecute_Command);
            CancelCommand = new RelayCommand(Execute_Cancel, CanExecute_Command);
            HomePageCommand = new RelayCommand(Execute_HomePageCommand, CanExecute_Command);
            VoucherViewCommand = new RelayCommand(Execute_VoucherViewCommand, CanExecute_Command);
            RequestCommand = new RelayCommand(Execute_RequestCommand, CanExecute_Command);

        }
        private bool CanExecute_Command(object parameter)
        {
            return true;
        }
        private void Execute_AddRequestCommand(object sender)
        {
            string[] words = _location.Split(',');
            string country = words[1];
            string city = words[0];
            string inputLanguage = _language;
            Location location = new Location(country, city);
            Location savedLocation = _locationService.Save(location);
            Language language = new Language(inputLanguage);
            Language savedLanguage = _languageService.Save(language);
            TourRequest = new TourRequest();
            if (IsForwarded)
            {
                TourRequest = _tourRequestService.addRequest(savedLocation, savedLocation.Id, Description, savedLanguage, savedLanguage.Id, GuestNumber, TourStart, TourEnd, Guest2.Id);
                TourRequests.Add(TourRequest);
                CloseAction?.Invoke();
            }
            else
            {
                TourRequest = _tourRequestService.addRequest(savedLocation, savedLocation.Id, Description, savedLanguage, savedLanguage.Id, GuestNumber, TourStart, TourEnd, Guest2.Id);
            }

            CloseAction();
        }
        private void Execute_Cancel(object sender)
        {
            CloseAction();
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
            CloseAction();
        }
        private void Execute_RequestCommand(object sender)
        {
            ChooseRequestTypeView chooseRequestTypeView = new ChooseRequestTypeView(Guest2);
            chooseRequestTypeView.Show();
            CloseAction();
        }
    }
}
