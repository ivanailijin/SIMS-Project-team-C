using LiveCharts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using TravelService.Application.UseCases;
using TravelService.Application.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class SelectedAccommodationViewModel : ViewModelBase
    {
        private AccommodationReservationService _reservationService;
        public Guest1 Guest1 { get; set; }
        public Action CloseAction { get; set; }

        private Dictionary<string, int> _reservationsByMonth;
        public Dictionary<string, int> ReservationsByMonth
        {
            get { return _reservationsByMonth; }
            set { _reservationsByMonth = value; OnPropertyChanged(); }
        }

        private ObservableCollection<KeyValuePair<string, int>> _reservationsByMonthCollection;
        public ObservableCollection<KeyValuePair<string, int>> ReservationsByMonthCollection
        {
            get => _reservationsByMonthCollection;
            set
            {
                if (value != _reservationsByMonthCollection)
                {
                    _reservationsByMonthCollection = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<string> _months;
        public ObservableCollection<string> Months
        {
            get => _months;
            set
            {
                if (value != _months)
                {
                    _months = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<int> _counts;
        public ObservableCollection<int> Counts
        {
            get => _counts;
            set
            {
                if (value != _counts)
                {
                    _counts = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _currentIndex;
        public int CurrentIndex
        {
            get { return _currentIndex; }
            set
            {
                if (_currentIndex != value && value >= 0 && value < SelectedAccommodation.Pictures.Count)
                {
                    _currentIndex = value;

                    OnPropertyChanged(nameof(CurrentImage));
                    OnPropertyChanged(nameof(CurrentIndex));
                }
            }
        }

        private Uri _currentImage;
        public Uri CurrentImage
        {
            get => _currentImage;
            set
            {
                if (value != _currentImage)
                {
                    _currentImage = value;
                    OnPropertyChanged(nameof(CurrentImage));
                }
            }
        }

        private Accommodation _selectedAccommodation;
        public Accommodation SelectedAccommodation
        {
            get => _selectedAccommodation;
            set
            {
                if (value != _selectedAccommodation)
                {
                    _selectedAccommodation = value;
                    OnPropertyChanged();
                }
            }
        }

        private RelayCommand _previousPageCommand;
        public RelayCommand PreviousPageCommand
        {
            get => _previousPageCommand;
            set
            {
                if (value != _previousPageCommand)
                {
                    _previousPageCommand = value;
                    OnPropertyChanged();
                }
            }
        }

        private RelayCommand _previousImageCommand;
        public RelayCommand PreviousImageCommand
        {
            get => _previousImageCommand;
            set
            {
                if (value != _previousImageCommand)
                {
                    _previousImageCommand = value;
                    OnPropertyChanged();
                }
            }
        }

        private RelayCommand _nextImageCommand;
        public RelayCommand NextImageCommand
        {
            get => _nextImageCommand;
            set
            {
                if (value != _nextImageCommand)
                {
                    _nextImageCommand = value;
                    OnPropertyChanged();
                }
            }
        }

        private RelayCommand _accommodationAvailabilityWindowCommand;
        public RelayCommand AccommodationAvailabilityWindowCommand
        {
            get => _accommodationAvailabilityWindowCommand;
            set
            {
                if (value != _accommodationAvailabilityWindowCommand)
                {
                    _accommodationAvailabilityWindowCommand = value;
                    OnPropertyChanged();
                }
            }
        }

        public SelectedAccommodationViewModel(Accommodation selectedAccommodation, Guest1 guest1)
        {
            _reservationService = new AccommodationReservationService(Injector.CreateInstance<IAccommodationReservationRepository>());
            SelectedAccommodation = selectedAccommodation;
            Guest1 = guest1;
            _currentIndex = 0;
            CurrentImage = SelectedAccommodation.Pictures.First();

            List<AccommodationReservation> Reservations = new List<AccommodationReservation>(_reservationService.FindReservationsByAccommodation(SelectedAccommodation.Id));
            ReservationsByMonth = new Dictionary<string, int>(_reservationService.CalculateReservationsByMonth(Reservations));

            Months = new ObservableCollection<string>(ReservationsByMonth.Keys.ToList());
            Counts = new ObservableCollection<int>(ReservationsByMonth.Values.ToList());

            ReservationsByMonthCollection = new ObservableCollection<KeyValuePair<string, int>>(
            ReservationsByMonth.Select(kv => new KeyValuePair<string, int>(kv.Key, kv.Value)));

            PreviousImageCommand = new RelayCommand(Execute_PreviousImage, CanExecute_Command);
            NextImageCommand = new RelayCommand(Execute_NextImage, CanExecute_Command);
            PreviousPageCommand = new RelayCommand(Execute_PreviousPage, CanExecute_Command);
            AccommodationAvailabilityWindowCommand = new RelayCommand(Execute_AccommodationAvailabilityWindow, CanExecute_Command);
        }

        private bool CanExecute_Command(object parameter)
        {
            return true;
        }

        private void Execute_PreviousPage(object sender)
        {
            CloseAction();
        }

        private void Execute_PreviousImage(object sender)
        {
            if (_currentIndex > 0)
            {
                _currentIndex--;
                CurrentImage = SelectedAccommodation.Pictures[_currentIndex];
            }
        }

        private void Execute_NextImage(object sender)
        {
            if (_currentIndex < SelectedAccommodation.Pictures.Count - 1)
            {
                _currentIndex++;
                CurrentImage = SelectedAccommodation.Pictures[_currentIndex];
            }
        }

        private void Execute_AccommodationAvailabilityWindow(object sender)
        {
            AccommodationAvailabilityView accommodationAvailabilityView = new AccommodationAvailabilityView(SelectedAccommodation, Guest1);
            accommodationAvailabilityView.Show();
        }
    }
}
