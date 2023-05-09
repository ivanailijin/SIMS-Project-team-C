using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Media.Imaging;
using TravelService.Application.UseCases;
using TravelService.Application.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class AddTourViewModel : ViewModelBase
    {
        public Action CloseAction { get; set; }

        private readonly TourService _tourService;
        private readonly LocationService _locationService;
        private readonly LanguageService _languageService;
        private readonly CheckPointService _checkPointService;
        public RelayCommand FindPicturesCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand AddTourCommand { get; set; }    
        public RelayCommand AddCheckPointCommand { get; set; }  
        public RelayCommand AddLocationCommand { get; set; }    
        public RelayCommand AddLanguageCommand { get; set; }    
        public RelayCommand IncrementCommand { get; set; }
        public RelayCommand DecrementCommand { get; set; }

        public int TourId;
        public Guide Guide;


        private string _tourName;
        public string TourName
        {
            get => _tourName;
            set
            {
                if (value != _tourName)
                {
                    _tourName = value;
                    OnPropertyChanged();
                }
            }
        }

        private Location _selectedLocation;

        public Location SelectedLocation
        {
            get => _selectedLocation;
            set
            {
                if (value != _selectedLocation)
                {
                    _selectedLocation = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _description;

        public string Description
        {
            get => _description;
            set
            {
                if (value != _description)
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }
        }
        private Language _tourLanguage;

        public Language SelectedLanguge
        {
            get => _tourLanguage;
            set
            {
                if (value != _tourLanguage)
                {
                    _tourLanguage = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _maxguestnumber;

        public int MaxGuestNumber
        {
            get => _maxguestnumber;
            set
            {
                if (value != _maxguestnumber)
                {
                    _maxguestnumber = value;
                    OnPropertyChanged();
                }
            }
        }




        private DateTime _tourStart;

        public DateTime TourStart
        {
            get => _tourStart;
            set
            {
                if (value != _tourStart)
                {
                    _tourStart = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _duration;

        public int Duration
        {
            get => _duration;
            set
            {
                if (value != _duration)
                {
                    _duration = value;
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
        private bool _done;

        public bool Done
        {
            get => _done;
            set
            {
                if (value != _done)
                {
                    _done = value;
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

        public AddTourViewModel(Guide guide)
        {

            this.Guide = guide;
            _tourService = new TourService(Injector.CreateInstance<ITourRepository>());
            _locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            _checkPointService = new CheckPointService(Injector.CreateInstance<ICheckPointRepository>());
            _languageService = new LanguageService(Injector.CreateInstance<ILanguageRepository>());
            types = new ObservableCollection<string>();
         

            FindPicturesCommand = new RelayCommand(Execute_FindPicturesCommand, CanExecute_Command);
            CancelCommand = new RelayCommand(Execute_CancelCommand, CanExecute_Command);
            IncrementCommand = new RelayCommand(Execute_GuestsIncrement, CanExecute_Command);
            DecrementCommand = new RelayCommand(Execute_GuestsDecrement, CanExecute_Command);
            AddLocationCommand = new RelayCommand(Execute_AddLocation, CanExecute_Command);
            AddLanguageCommand = new RelayCommand(Execute_AddLanguage, CanExecute_Command);
            AddCheckPointCommand = new RelayCommand(Execute_AddCheckPoint, CanExecute_Command);
            AddTourCommand = new RelayCommand(Execute_AddTourCommand,CanExecute_Command);   
        }


        private void Execute_GuestsIncrement(object obj)
        {
            MaxGuestNumber++;
        }

        private void Execute_GuestsDecrement(object obj)
        {
            if (MaxGuestNumber > 0)
            {
                MaxGuestNumber--;
            }
        }

        private void Execute_AddLanguage(object obj)
        {
            AddLanguageView addLanguage = new AddLanguageView(TourId);
            addLanguage.Show();
        }

        private void Execute_AddCheckPoint(object obj)
        {
            EnterCheckPointView enterCheckPoint = new EnterCheckPointView(TourId);
            enterCheckPoint.Show();
        }

        private void Execute_AddLocation(object obj)
        {
            AddLocationView addLocationView = new AddLocationView(TourId);
            addLocationView.Show();
        }


        private void Execute_AddTourCommand(object obj)
        {

            Location location = SelectedLocation;
            Language language = SelectedLanguge;


            List<string> formattedPictures = new List<string>();

            string[] delimitedPictures = Pictures.Split(new char[] { '|' });

            foreach (string picture in delimitedPictures)
            {
                formattedPictures.Add(picture);
            }

            Tour tour = new Tour(Guide.Id, TourName,SelectedLocation, SelectedLocation.Id, Description, SelectedLanguge, SelectedLanguge.Id, MaxGuestNumber, TourStart, Duration, formattedPictures, Done);
            _tourService.Save(tour);
            CloseAction();
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
                    string destinationFilePath = System.IO.Path.Combine(destinationFolder, Path.GetFileName(file));
                    File.Copy(file, destinationFilePath);

                }

                Pictures = Pictures.Substring(0, Pictures.Length - 1);

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
