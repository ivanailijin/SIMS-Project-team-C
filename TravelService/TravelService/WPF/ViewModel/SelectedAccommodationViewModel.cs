using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class SelectedAccommodationViewModel : ViewModelBase
    { 
        public Guest1 Guest1 { get; set; }
        public Action CloseAction { get; set; }
       
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

        public SelectedAccommodationViewModel(Accommodation selectedAccommodation, Guest1 guest1)
        {
            SelectedAccommodation = selectedAccommodation;
            Guest1 = guest1;
            _currentIndex = 0;
            CurrentImage = SelectedAccommodation.Pictures.First();

            PreviousImageCommand = new RelayCommand(Execute_PreviousImage, CanExecute_Command);
            NextImageCommand = new RelayCommand(Execute_NextImage, CanExecute_Command);
            PreviousPageCommand = new RelayCommand(Execute_PreviousPage, CanExecute_Command);
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
    }
}
