using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
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
using System.Collections.ObjectModel;

namespace TravelService.WPF.ViewModel
{
    public class ReserveAnywhereViewModel : ViewModelBase
    {
        private AccommodationReservationService _reservationService;
        private Guest1Service _guestService;
        public Guest1 Guest1 { get; set; }
        public ReserveAnywhereView ReserveAnywhereView { get; set; }
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public int GuestNumber { get; set; }
        public int LengthOfStay { get; set; }
        public Tuple<DateTime, DateTime> SelectedAvailableDatePair { get; set; }

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

        private ObservableCollection<Tuple<DateTime, DateTime>> _availableDatesPair;
        public ObservableCollection<Tuple<DateTime, DateTime>> AvailableDatesPair
        {
            get => _availableDatesPair;
            set
            {
                if (value != _availableDatesPair)
                {
                    _availableDatesPair = value;
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

        private RelayCommand _reserveCommand;
        public RelayCommand ReserveCommand
        {
            get => _reserveCommand;
            set
            {
                if (value != _reserveCommand)
                {
                    _reserveCommand = value;
                    OnPropertyChanged();
                }
            }
        }

        public ReserveAnywhereViewModel(ReserveAnywhereView reserveAnywhereView, Accommodation selectedAccommodation, Guest1 guest1, List<Tuple<DateTime, DateTime>> availableDates, DateTime? checkInDate, DateTime? checkOutDate, int guestNumber, int lengthOfStay)
        {
            _reservationService = new AccommodationReservationService(Injector.CreateInstance<IAccommodationReservationRepository>());
            _guestService = new Guest1Service(Injector.CreateInstance<IGuest1Repository>());
            ReserveAnywhereView = reserveAnywhereView;
            GuestNumber = guestNumber;
            LengthOfStay = lengthOfStay;
            CheckInDate = checkInDate;
            CheckOutDate = checkOutDate;
            AvailableDatesPair = new ObservableCollection<Tuple<DateTime, DateTime>>(availableDates);

            SelectedAccommodation = selectedAccommodation;
            Guest1 = guest1;
            _currentIndex = 0;
            CurrentImage = SelectedAccommodation.Pictures.First();


            PreviousImageCommand = new RelayCommand(Execute_PreviousImage, CanExecute_Command);
            NextImageCommand = new RelayCommand(Execute_NextImage, CanExecute_Command);
            PreviousPageCommand = new RelayCommand(Execute_PreviousPage, CanExecute_Command);
            ReserveCommand = new RelayCommand(Execute_ReserveCommand, CanExecute_Command);
        }

        private bool CanExecute_Command(object parameter)
        {
            return true;
        }

        public void GoBack()
        {
            ReserveAnywhereView.GoBack();
        }
        private void Execute_PreviousPage(object sender)
        {
            GoBack();
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

        private void Execute_ReserveCommand(object sender)
        {
            if (SelectedAvailableDatePair != null)
            {
                DateTime checkInDate = SelectedAvailableDatePair.Item1;
                DateTime checkOutDate = SelectedAvailableDatePair.Item2;
                AccommodationReservation reservation = new AccommodationReservation(SelectedAccommodation.Id, Guest1.Id, SelectedAccommodation.OwnerId, SelectedAccommodation.LocationId, checkInDate, checkOutDate, LengthOfStay, GuestNumber);
                _reservationService.Save(reservation);
                if (Guest1.BonusPoints > 0)
                {
                    Guest1.BonusPoints--;
                }
                _guestService.Update(Guest1);
                AvailableDatesPair.Remove(SelectedAvailableDatePair);
                MessageBox.Show("Uspešno ste izvršili rezervaciju!", "Rezervacija", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Odaberite opseg datuma za rezervaciju.");
            }
        }
    }
}
