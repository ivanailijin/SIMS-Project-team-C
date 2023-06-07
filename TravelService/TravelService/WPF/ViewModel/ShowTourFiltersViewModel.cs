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

namespace TravelService.WPF.ViewModel
{
    public class ShowTourFiltersViewModel : ViewModelBase
    {
        public Action CloseAction { get; set; }

        private readonly TourService _tourService;

        private readonly LocationService _locationService;

        private readonly LanguageService _languageService;

        private readonly CheckPointService _checkpointService;
        public ObservableCollection<string> LocationsComboBox { get; set; }
        public ObservableCollection<string> LanguageComboBox { get; set; }
        private ObservableCollection<Tour> _tours;
        public ObservableCollection<Tour> Tours
        {
            get => _tours;
            set
            {
                if (value != _tours)
                {
                    _tours = value;
                    OnPropertyChanged();
                    List<Tour> tours = _tours.ToList();
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
        private RelayCommand _applyCommand;
        public RelayCommand ApplyCommand
        {
            get => _applyCommand;
            set
            {
                if (value != _applyCommand)
                {
                    _applyCommand = value;
                    OnPropertyChanged();
                }
            }
        }
        public ShowTourFiltersViewModel() {
            _tourService = new TourService(Injector.CreateInstance<ITourRepository>());
            _locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            _languageService = new LanguageService(Injector.CreateInstance<ILanguageRepository>());
            _checkpointService = new CheckPointService(Injector.CreateInstance<ICheckPointRepository>());

            List<Tour> tours = new List<Tour>(_tourService.GetAll());
            List<Location> locations = new List<Location>(_locationService.GetAll());
            List<Language> languauges = new List<Language>(_languageService.GetAll());
            Tours = new ObservableCollection<Tour>(tours);
            LocationsComboBox = new ObservableCollection<string>();
            LanguageComboBox = new ObservableCollection<string>();

            foreach (Tour tour in Tours)
            {
                Location location = locations.Find(loc => loc.Id == tour.LocationId);
                Language language = languauges.Find(loc => loc.Id == tour.LanguageId);
                LocationsComboBox.Add(location.CityAndCountry);
                LanguageComboBox.Add(language.Name);
            }
            LocationsComboBox.Insert(0, "");
            CancelCommand = new RelayCommand(Execute_CancelCommand, CanExecute_Command);
            ApplyCommand = new RelayCommand(Execute_ApplyCommand, CanExecute_Command);
        }
        private bool CanExecute_Command(object parameter)
        {
            return true;
        }
        private void Execute_CancelCommand(object sender)
        {
            CloseAction();
        }
        private void Execute_ApplyCommand(object sender)
        {
            CloseAction();
        }
    }
}
