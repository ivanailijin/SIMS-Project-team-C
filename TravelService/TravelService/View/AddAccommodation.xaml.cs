using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TravelService.Repository;
using TravelService.Model;
using TravelService.Validation;
using System.Text.RegularExpressions;
using System.Globalization;
using Microsoft.Win32;
using System.IO;

namespace TravelService.View
{
    /// <summary>
    /// Interaction logic for AddAccommodation.xaml
    /// </summary>
    public partial class AddAccommodation : Window, IDataErrorInfo, INotifyPropertyChanged
    {

        private readonly AccommodationRepository _repositoryAccommodation;

        private readonly LocationRepository _repositoryLocation;

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


        private TYPE _accommodationType;

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

        private int _daysBeforeCancellingReservation;
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

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public AddAccommodation()
        {
            InitializeComponent();
            types = new ObservableCollection<string>();
            types.Add("House");
            types.Add("Cottage");
            types.Add("Apartment");
            _repositoryAccommodation = new AccommodationRepository();
            _repositoryLocation = new LocationRepository();

            DataContext = this;
        }

        public string Error
        {
            get
            {
                return string.Empty;
            }
        }

        public string this[string columnName] => throw new NotImplementedException();

        private void AddAccommodation_Click(object sender, RoutedEventArgs e)
        {
            string[] words = _location.Split(',');

            string country = words[0];
            string city = words[1];

            Location location = new Location(country, city);

            Location savedLocation = _repositoryLocation.Save(location);


            string typeAccommodation = typeAccommodationCombo.Text;

            if (typeAccommodation.Equals("Apartment"))
            {
                _accommodationType = TYPE.APARTMENT;
            }
            else if (typeAccommodation.Equals("House"))
            {
                _accommodationType = TYPE.HOUSE;
            }
            else if (typeAccommodation.Equals("Cottage"))
            {
                _accommodationType = TYPE.COTTAGE;
            }

            List<string> formattedPictures = new List<string>();

            string[] delimitedPictures = Pictures.Split(new char[] {'|'});

            foreach (string picture in delimitedPictures)
            {
                formattedPictures.Add(picture);
            }

            Accommodation accommodation = new Accommodation(AccommodationName, savedLocation, savedLocation.Id, _accommodationType, MaxGuestNumber, MinReservationDays, DaysBeforeCancellingReservation, formattedPictures);
            _repositoryAccommodation.Save(accommodation);
            Close();
        }

        private void CancelAccommodation_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void typeAccommodationCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void findPictures_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.Filter = "Image files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";
            dlg.Multiselect = true;

            Nullable<bool> result = dlg.ShowDialog();


            if (result == true)
            {
                string[] selectedFiles = dlg.FileNames;

                foreach(string file in selectedFiles)
                {
                    Pictures += file;
                    Pictures += "|";
                }

                Pictures = Pictures.Substring(0, Pictures.Length - 1);

                
            }
        }
    }
}
