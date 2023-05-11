using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TravelService.Application.UseCases;
using TravelService.Application.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;

namespace TravelService.WPF.ViewModel
{
    public class AddTourRequestViewModel : ViewModelBase
    {
        private readonly TourRequestService _tourRequestService; 
        private readonly LocationService _locationService;
        private readonly LanguageService _languageService;
        public Guest2 Guest2 { get; set; }
        public Action CloseAction { get; set; }

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

        public AddTourRequestViewModel(Guest2 guest2)
        {
            _tourRequestService = new TourRequestService(Injector.CreateInstance<ITourRequestRepository>());
            _locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            _languageService = new LanguageService(Injector.CreateInstance<ILanguageRepository>());

            Guest2 = guest2;
            AddRequestCommand = new RelayCommand(Execute_AddRequestCommand, CanExecute_Command);

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

            _tourRequestService.addRequest(savedLocation, savedLocation.Id, Description, savedLanguage, savedLanguage.Id, GuestNumber, TourStart, TourEnd, Guest2.Id);
            CloseAction();
        }
    }
}
