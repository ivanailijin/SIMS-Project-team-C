using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using TravelService.Applications.UseCases;
using TravelService.Applications.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class AddTourViewModel : ViewModelBase
    {
        public Frame PopupFrame { get; set; }
        public AddTourView AddTourView { get; set; }
        public NavigationService NavigationService { get; set; }
        public Action CloseAction { get; set; }

        private readonly TourService _tourService;
        private readonly NewTourNotificationService _newTourNotificationService;
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
        public bool Visibility;

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
        private string _tourLanguage;

        public string Language
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




        private DateTime _tourStart=DateTime.Now;

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

        internal void Show()
        {
            throw new NotImplementedException();
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

        public AddTourViewModel(AddTourView addTourView,Guide guide,bool visibility,NavigationService navigationService)
        {
            ListBoxPictures = new ObservableCollection<string>();
            AddTourView = addTourView;  
            NavigationService = navigationService;
            this.Guide = guide;
            this.Visibility = visibility;
            _tourService = new TourService(Injector.CreateInstance<ITourRepository>());
            _newTourNotificationService = new NewTourNotificationService(Injector.CreateInstance<INewTourNotificationRepository>());
            _locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            _checkPointService = new CheckPointService(Injector.CreateInstance<ICheckPointRepository>());
            _languageService = new LanguageService(Injector.CreateInstance<ILanguageRepository>());
            types = new ObservableCollection<string>();

            SetInitialDateTime();
            FindPicturesCommand = new RelayCommand(Execute_FindPicturesCommand, CanExecute_Command);
            CancelCommand = new RelayCommand(Execute_CancelCommand, CanExecute_Command);
            IncrementCommand = new RelayCommand(Execute_GuestsIncrement, CanExecute_Command);
            DecrementCommand = new RelayCommand(Execute_GuestsDecrement, CanExecute_Command);
         
            AddCheckPointCommand = new RelayCommand(Execute_AddCheckPoint, CanExecute_Command);

            
            AddTourCommand = new RelayCommand(Execute_AddTourCommand,CanExecute_Command);

            PopupFrame = addTourView.MyPopupFrame;
        }

        private void SetInitialDateTime()
        {
            TourStart = DateTime.Now;
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

      

        private void Execute_AddCheckPoint(object obj)
        {
            var enterCheckPoint = new EnterCheckPointView(TourId,NavigationService);
            OpenPopupPage(enterCheckPoint);
        }

       


        private void Execute_AddTourCommand(object obj)
        {


            string[] words = _location.Split(',');

            string city = words[0];
            string country = words[1];
            Location savedLocation = _locationService.GetByCityAndCountry(_location);
            if (savedLocation == null)
            {
                Location location = new Location(country, city);
                savedLocation = _locationService.Save(location);
            }
            Language savedLanguage = _languageService.GetLanguageByName(Language);
            if (savedLanguage == null)
            {
                Language language = new Language(Language);
                savedLanguage = _languageService.Save(language);
            }
            
            List<string> formattedPictures = new List<string>();

            string[] delimitedPictures = Pictures.Split(new char[] { '|' });

            foreach (string picture in delimitedPictures)
            {
                formattedPictures.Add(picture);
            }

            Tour tour = new Tour(Guide.Id, TourName, savedLocation, savedLocation.Id, Description, savedLanguage, savedLanguage.Id, MaxGuestNumber, TourStart, Duration, formattedPictures, Done);
            

            List<CheckPoint> checkPoints = _checkPointService.GetAll();
            _tourService.Check(checkPoints, tour, TourId);
            if (Visibility)
                _newTourNotificationService.SendNotification(tour.Id);
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
                    ListBoxPictures.Add(file);

                }

                Pictures = Pictures.Substring(0, Pictures.Length - 1);

            }
        }
        private void OpenPopupPage(Page EnterCheckPointView)
        {
            PopupFrame.Navigate(EnterCheckPointView);
            PopupFrame.Visibility = System.Windows.Visibility.Visible;

        }

       private void Execute_CancelCommand(object obj)
        {
            // Check if the NavigationService is available
            if (NavigationService != null && NavigationService.CanGoBack)
            {
                // Navigate back to the previous page
                NavigationService.GoBack();
            }

        }

        private bool CanExecute_Command(object arg)
        {
            return true;
        }
    }
}
