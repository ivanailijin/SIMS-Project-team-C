using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using TravelService.Applications.UseCases;
using TravelService.Applications.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.Repository;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class AddAccommodationViewModel : ViewModelBase
    {
        private readonly AccommodationService _accommodationService;

        private readonly LocationService _locationService;
        public Action CloseAction { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand AddAccommodationCommand { get; set; }
        public RelayCommand FindPicturesCommand { get; set; }
        public AddAccommodation AddAccommodationView { get; set; }
        public Owner Owner { get; set; }

        public ObservableCollection<string> types
        {
            get;
            set;
        }

        public ObservableCollection<BitmapImage> ImageList
        {
            get;
            set;
        }

        private string _accommodationName;

        public string AccommodationName
        {
            get => _accommodationName;
            set
            {
                if (value != _accommodationName)
                {
                    _accommodationName = value;
                    OnPropertyChanged();
                }
            }
        }

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

        private string _type;

        public string Type
        {
            get => _type;
            set
            {
                if (value != _type)
                {
                    _type = value;
                    OnPropertyChanged();
                }
            }
        }

        private TYPE _accommodationType;

        public TYPE AccommodationType
        {
            get { return _accommodationType; }
            set
            {
                if (value != _accommodationType)
                {
                    _accommodationType = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _maxGuestNumber;
        public int MaxGuestNumber
        {
            get => _maxGuestNumber;
            set
            {
                if (value != _maxGuestNumber)
                {
                    _maxGuestNumber = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _minReservationDays;
        public int MinReservationDays
        {
            get => _minReservationDays;
            set
            {
                if (value != _minReservationDays)
                {
                    _minReservationDays = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _daysBeforeCancellingReservation = 1;
        public int DaysBeforeCancellingReservation
        {
            get => _daysBeforeCancellingReservation;
            set
            {
                if (value != _daysBeforeCancellingReservation)
                {
                    _daysBeforeCancellingReservation = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _pictures;

        public string Pictures
        {
            get => _pictures;
            set
            {
                if (value != _pictures)
                {
                    _pictures = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<string> _listBoxPictures;

        public ObservableCollection<string> ListBoxPictures
        {
            get { return _listBoxPictures; }
            set
            {
                _listBoxPictures = value;
                OnPropertyChanged(nameof(ListBoxPictures));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public AddAccommodationViewModel(Owner owner, Location location, AddAccommodation addAccommodationView)
        {
            InitializeCommands();
            this.Owner = owner;
            AddAccommodationView = addAccommodationView;
            types = new ObservableCollection<string>();
            ListBoxPictures = new ObservableCollection<string>();
            _accommodationService = new AccommodationService(Injector.CreateInstance<IAccommodationRepository>());
            _locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());

            if (location != null)
            {
                Location = location.CityAndCountry;
            }
            else
            {
                Location = "";
            }

        }
        public string Error
        {
            get
            {
                return string.Empty;
            }
        }
        public string this[string columnName] => throw new NotImplementedException();

        private void InitializeCommands()
        {
            CancelCommand = new RelayCommand(Execute_CancelCommand, CanExecute_Command);
            AddAccommodationCommand = new RelayCommand(Execute_AddAccommodationCommand, CanExecute_Command);
            FindPicturesCommand = new RelayCommand(Execute_FindPicturesCommand, CanExecute_Command);
        }

        private void Execute_FindPicturesCommand(object obj)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.Filter = "Image files (*.jpg;*.jpeg;*.png;*.jfif)|*.jpg;*.jpeg;*.png;*.jfif";
            dlg.Multiselect = true;

            Nullable<bool> result = dlg.ShowDialog();


            if (result == true)
            {
                string[] selectedFiles = dlg.FileNames;

                string destinationFolder = @"../../../Resources/Images/";

                if (!Directory.Exists(destinationFolder))
                {
                    Directory.CreateDirectory(destinationFolder);
                }

                foreach (string file in selectedFiles)
                {
                    Pictures += file;
                    Pictures += "|";
                    string destinationFilePath = Path.Combine(destinationFolder, Path.GetFileName(file));
                    File.Copy(file, destinationFilePath);
                    ListBoxPictures.Add(file);
                }

                Pictures = Pictures.Substring(0, Pictures.Length - 1);
            }
        }
        private void Execute_AddAccommodationCommand(object obj)
        {
            if (string.IsNullOrWhiteSpace(AccommodationName) ||
                string.IsNullOrWhiteSpace(Location) ||
                string.IsNullOrWhiteSpace(Type) ||
                string.IsNullOrWhiteSpace(MaxGuestNumber.ToString()) ||
                string.IsNullOrWhiteSpace(MinReservationDays.ToString()) ||
                string.IsNullOrWhiteSpace(DaysBeforeCancellingReservation.ToString()))
            {
                MessageBox.Show("Niste popunili sva polja!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                string[] words = _location.Split(',');

                string country = words[1];
                string city = words[0];

                if (Type.Equals("Apartment"))
                {
                    AccommodationType = TYPE.APARTMENT;
                }
                else if (Type.Equals("House"))
                {
                    AccommodationType = TYPE.HOUSE;
                }
                else if (Type.Equals("Cottage"))
                {
                    AccommodationType = TYPE.COTTAGE;
                }

                Location location = new Location(country, city);

                Location savedLocation = _locationService.Save(location);
                List<string> formattedPictures = new List<string>();

                string[] delimitedPictures = Pictures.Split(new char[] { '|' });

                foreach (string picture in delimitedPictures)
                {
                    formattedPictures.Add(picture);
                }

                Accommodation accommodation = new Accommodation(Owner.Id, AccommodationName, savedLocation.Id, AccommodationType, MaxGuestNumber, MinReservationDays, DaysBeforeCancellingReservation, DateTime.Today, formattedPictures);
                _accommodationService.Save(accommodation);
                CloseAction();
            }
        }
        private void Execute_CancelCommand(object obj)
        {
            AddAccommodationView.GoBack();
        }

        private bool CanExecute_Command(object arg)
        {
            return true;
        }
    }
}
