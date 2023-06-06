using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using TravelService.Applications.UseCases;
using TravelService.Applications.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.WPF.Services;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class SelectedAccommodationViewModel : ViewModelBase
    {
        private AccommodationReservationService _reservationService;
        public Guest1 Guest1 { get; set; }
        public SelectedAccommodationView SelectedAccommodationView { get; set; }
        public SeriesCollection ReservationSeries { get; set; }
        public List<string> MonthLabels { get; set; }

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

        public SelectedAccommodationViewModel(SelectedAccommodationView selectedAccommodationView, Accommodation selectedAccommodation, Guest1 guest1)
        {
            _reservationService = new AccommodationReservationService(Injector.CreateInstance<IAccommodationReservationRepository>());
            SelectedAccommodationView = selectedAccommodationView;

            SelectedAccommodation = selectedAccommodation;
            Guest1 = guest1;
            _currentIndex = 0;
            CurrentImage = SelectedAccommodation.Pictures.First();

            ReservationSeries = new SeriesCollection();
            MonthLabels = new List<string>();

            Dictionary<string, int> reservationsByMonth = new Dictionary<string, int>(_reservationService.CalculateReservationCountByMonth(SelectedAccommodation));

            var lineSeries = new LineSeries { Title = "Broj rezervacija", Values = new ChartValues<int>(reservationsByMonth.Values) };
            ReservationSeries.Add(lineSeries);

            foreach (var monthYear in reservationsByMonth.Keys)
            {
                MonthLabels.Add(monthYear);
            }

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
            FirstGuestView firstGuestView = new FirstGuestView(Guest1);
            firstGuestView.frame.Navigate(new AccommodationView(Guest1));
            FirstGuestWindow firstGuestWindow = Window.GetWindow(SelectedAccommodationView) as FirstGuestWindow ?? new(Guest1);
            firstGuestWindow?.SwitchToPage(firstGuestView);
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
            FirstGuestWindow firstGuestWindow = Window.GetWindow(SelectedAccommodationView) as FirstGuestWindow ?? new(Guest1);
            firstGuestWindow?.SwitchToPage(accommodationAvailabilityView);
        }
    }
}
