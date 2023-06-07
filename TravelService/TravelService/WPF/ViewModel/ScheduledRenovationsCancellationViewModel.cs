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
    public class ScheduledRenovationsCancellationViewModel : ViewModelBase
    {
        public AccommodationService _accommodationService;

        public AccommodationRenovationService _renovationService;

        public ScheduledRenovationsCancellationView ScheduledRenovationsCancellationView { get; set; }
        public Action CloseAction { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand CancelRenovationCommand { get; set; }

        public AccommodationRenovation SelectedFutureRenovation { get; set; }
        public static ObservableCollection<Accommodation> Accommodations { get; set; }
        public static List<Location> Locations { get; set; }
        public Owner Owner { get; set; }

        private ObservableCollection<AccommodationRenovation> _lastRenovations;
        public ObservableCollection<AccommodationRenovation> LastRenovations
        {
            get => _lastRenovations;
            set
            {
                if (value != _lastRenovations)
                {
                    _lastRenovations = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<AccommodationRenovation> _futureRenovations;
        public ObservableCollection<AccommodationRenovation> FutureRenovations
        {
            get => _futureRenovations;
            set
            {
                if (value != _futureRenovations)
                {
                    _futureRenovations = value;
                    OnPropertyChanged();
                }
            }
        }

        public ScheduledRenovationsCancellationViewModel(Owner owner, ScheduledRenovationsCancellationView scheduledRenovationsCancellationView)
        {
            InitializeCommands();
            this.Owner = owner;
            _accommodationService = new AccommodationService(Injector.CreateInstance<IAccommodationRepository>());
            _renovationService = new AccommodationRenovationService(Injector.CreateInstance<IAccommodationRenovationRepository>());

            List<AccommodationRenovation> lastRenovations = _renovationService.GetLastRenovations();
            LastRenovations = new ObservableCollection<AccommodationRenovation>(_accommodationService.GetAccommodationData(lastRenovations));
            List<AccommodationRenovation> futureRenovations = _renovationService.GetFutureRenovations();
            FutureRenovations = new ObservableCollection<AccommodationRenovation>(_accommodationService.GetAccommodationData(futureRenovations));
            ScheduledRenovationsCancellationView = scheduledRenovationsCancellationView;
        }
        private void InitializeCommands()
        {
            CancelCommand = new RelayCommand(Execute_CancelCommand, CanExecute_Command);
            CancelRenovationCommand = new RelayCommand(Execute_CancelRenovationCommand, CanExecute_Command);
        }
        private void Execute_CancelRenovationCommand(object obj)
        {
            if (SelectedFutureRenovation != null)
            {
                TimeSpan dayDifference = SelectedFutureRenovation.StartDate - DateTime.Today;
                if (dayDifference.Days > 5)
                {
                    CancelRenovationView cancelRenovationView = new CancelRenovationView(SelectedFutureRenovation, this);
                    cancelRenovationView.Show();
                }
                else
                {
                    MessageBoxResult result = MessageBox.Show("Otkazivanje nije moguce!\n Do pocetka renoviranja ima manje od 5 dana.", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Morate izabrati renoviranje koje zelite da otkazete!", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void Execute_CancelCommand(object obj)
        {
            ScheduledRenovationsCancellationView.GoBack();
        }

        private bool CanExecute_Command(object arg)
        {
            return true;
        }
    }
}
