using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
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
    public class ReserveAnywhereViewModel : ViewModelBase
    {
        private AccommodationReservationService _reservationService;
        public Guest1 Guest1 { get; set; }
        public ReserveAnywhereView ReserveAnywhereView { get; set; }

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

        public ReserveAnywhereViewModel(ReserveAnywhereView reserveAnywhereView, Accommodation selectedAccommodation, Guest1 guest1)
        {
            _reservationService = new AccommodationReservationService(Injector.CreateInstance<IAccommodationReservationRepository>());
            ReserveAnywhereView = reserveAnywhereView;

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
            //message box za uspesno rezervisanje
        }
    }
}
