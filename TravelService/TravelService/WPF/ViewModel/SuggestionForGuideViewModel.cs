using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using TravelService.Applications.UseCases;
using TravelService.Applications.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class SuggestionForGuideViewModel :ViewModelBase
    {
        public AddTourView AddTourView { get; set; }
        public NavigationService NavigationService { get; set; }
        private int selectedYear;
        private int selectedMonth;
        

        public Guide Guide { get; set; }

        public Action CloseAction { get; set; }

        private readonly TourRequestService _tourRequestService;

        private readonly LocationService _locationService;

        private readonly LanguageService _languageService;
        public RelayCommand AddTourLocationCommand { get; set; }
        public RelayCommand AddTourLangaugeCommand { get; set; }

        private ObservableCollection<TourRequest> _tourRequests;
        public ObservableCollection<TourRequest> TourRequests
        {
            get => _tourRequests;
            set
            {
                if (value != _tourRequests)
                {
                    _tourRequests = value;
                    OnPropertyChanged();
                    List<TourRequest> tourRequest = _tourRequests.ToList();
                }
            }
        }
   

            private string _location;
            public string Location
            {
                get => _location;
                set
                {
                    if (_location != value)
                    {
                        _location = value;
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
                    if (_language != value)
                    {
                        _language = value;
                        OnPropertyChanged();
                    }
                }
            }



        private string _mostRequestedLocation;
        public string MostRequestedLocation
        {
            get => _mostRequestedLocation;
            set
            {
                if (value != _mostRequestedLocation)
                {
                    _mostRequestedLocation = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _mostRequestedLanguage;
        public string MostRequestedLanguage
        {
            get => _mostRequestedLanguage;
            set
            {
                if (value != _mostRequestedLanguage)
                {
                    _mostRequestedLanguage = value;
                    OnPropertyChanged();
                }
            }
        }


        private bool _isMostRequestedLocationVisible;
        public bool IsMostRequestedLocationVisible
        {
            get => _isMostRequestedLocationVisible;
            set
            {
                if (_isMostRequestedLocationVisible != value)
                {
                    _isMostRequestedLocationVisible = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isMostRequestedLanguageVisible;
        public bool IsMostRequestedLanguageVisible
        {
            get => _isMostRequestedLanguageVisible;
            set
            {
                if (_isMostRequestedLanguageVisible != value)
                {
                    _isMostRequestedLanguageVisible = value;
                    OnPropertyChanged();
                }
            }
        }



        public SuggestionForGuideViewModel(Guide guide,NavigationService navigationService)
        {
            
            NavigationService = navigationService;  
            this.Guide = guide;
            selectedYear = DateTime.Now.Year - 1;
            selectedMonth = DateTime.Now.Month;
            _tourRequestService = new TourRequestService(Injector.CreateInstance<ITourRequestRepository>());
            _locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            _languageService = new LanguageService(Injector.CreateInstance<ILanguageRepository>());

            List<TourRequest> tourRequests = new List<TourRequest>(_tourRequestService.GetAll());
            TourRequests = new ObservableCollection<TourRequest>(tourRequests);

            MostRequestedLocation= _tourRequestService.GetMostRequestedLocationString(TourRequests, selectedYear, selectedMonth);
            MostRequestedLanguage = _tourRequestService.GetMostRequestedLanguageString(TourRequests, selectedYear, selectedMonth);
            

            Location = MostRequestedLocation;
            Language = MostRequestedLanguage;

            AddTourLocationCommand = new RelayCommand(Execute_AddTourLocationCommand, CanExecute_Command);
            AddTourLangaugeCommand = new RelayCommand(Execute_AddTourLanguageCommand, CanExecute_Command);

        }

        private void Execute_AddTourLocationCommand(object obj)
        {
            bool locationBool = true;
            bool languageBool = false;
            bool visibility = _tourRequestService.IsLocationOrLanguageVisible(locationBool,languageBool);

            AddTourViewModel addTourViewModel = new AddTourViewModel(AddTourView,Guide, visibility,NavigationService);
            addTourViewModel.Location = MostRequestedLocation;
            addTourViewModel.CloseAction = () => { /* Handle close action if needed */ };
            NavigationService.Navigate(new AddTourView(Guide, visibility, NavigationService));
        }

        private void Execute_AddTourLanguageCommand(object obj)
        {
            bool locationBool = false;
            bool languageBool = true;
            bool visibility = _tourRequestService.IsLocationOrLanguageVisible(locationBool, languageBool);

            AddTourViewModel addTourViewModel = new AddTourViewModel(AddTourView,Guide, visibility,NavigationService);
            addTourViewModel.Language = MostRequestedLanguage;
            addTourViewModel.CloseAction = () => { /* Handle close action if needed */ };

            NavigationService.Navigate(new AddTourView(Guide, visibility, NavigationService));
        }

        private bool CanExecute_Command(object arg)
        {
            return true;
        }


    }



}
