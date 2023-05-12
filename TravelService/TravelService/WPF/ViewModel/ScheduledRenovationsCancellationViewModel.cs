using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TravelService.Application.UseCases;
using TravelService.Application.Utils;
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

        public Action CloseAction { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand ScheduleRenovationCommand { get; set; }

        public Accommodation SelectedAccommodation { get; set; }
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

        public ScheduledRenovationsCancellationViewModel(Owner owner)
        {
            InitializeCommands();
            this.Owner = owner;
            _accommodationService = new AccommodationService(Injector.CreateInstance<IAccommodationRepository>());
            _renovationService = new AccommodationRenovationService(Injector.CreateInstance<IAccommodationRenovationRepository>());

            List<AccommodationRenovation> lastRenovations = _renovationService.GetLastRenovations();
            LastRenovations = new ObservableCollection<AccommodationRenovation>(_accommodationService.GetAccommodationData(lastRenovations));
            List<AccommodationRenovation> futureRenovations = _renovationService.GetFutureRenovations();
            FutureRenovations = new ObservableCollection<AccommodationRenovation>(_accommodationService.GetAccommodationData(futureRenovations));
        }
        private void InitializeCommands()
        {
            CancelCommand = new RelayCommand(Execute_CancelCommand, CanExecute_Command);
            ScheduleRenovationCommand = new RelayCommand(Execute_ScheduleRenovationCommand, CanExecute_Command);
        }
        private void Execute_ScheduleRenovationCommand(object obj)
        {
            if (SelectedAccommodation != null && Owner != null)
            {
                RenovationSchedulingView renovationSchedulingView = new RenovationSchedulingView(Owner, SelectedAccommodation);
                renovationSchedulingView.Show();
            }
            else
            {
                MessageBox.Show("Morate izabrati smestaj za koji zelite da zakazete renoviranje!");
            }
        }
        private void Execute_CancelCommand(object obj)
        {
            CloseAction();
        }

        private bool CanExecute_Command(object arg)
        {
            return true;
        }
    }
}
