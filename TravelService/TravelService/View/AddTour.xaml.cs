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
using TravelService.Model;
using TravelService.Repository;

namespace TravelService.View
{
    /// <summary>
    /// Interaction logic for AddTour.xaml
    /// </summary>
    public partial class AddTour : Window,INotifyPropertyChanged
    {
       


        private readonly TourRepository _repositoryTour;
        private readonly LocationRepository _repositoryLocation;
        private readonly LanguageRepository _repositoryLanguage;
        private readonly CheckPointRepository _repositoryCheckPoint;
        public static ObservableCollection<Tour> Tours { get; set; }
        public ObservableCollection<string> CheckPoints{ get; set; }
        
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (value != _name)
                {
                    _name = value;
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

        public string TourLanguage
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

       
        public List<string> CheckPointList(List<CheckPoint> checkPoints)
        {
            List<string> strings = new List<string>();
            foreach(CheckPoint checkPoint in checkPoints)
            {
                strings.Add(checkPoint.Name); 
                
            }
            return strings; 
        }
       




        public AddTour()
        {
            InitializeComponent();
            DataContext = this;
            _repositoryTour = new TourRepository();
            _repositoryCheckPoint = new CheckPointRepository();
            _repositoryLanguage = new LanguageRepository();
            _repositoryLocation = new LocationRepository(); 
            CheckPoints = new ObservableCollection<string>(CheckPointList(_repositoryCheckPoint.GetAll()));


            CheckPointCombo.ItemsSource = CheckPoints;
        }



        private void AddTour_Click(object sender, RoutedEventArgs e)
        {

            InitializeComponent();
            DataContext = this;

            string[] words = _location.Split(',');

            string city= words[0];
            string country = words[1];

            Location location = new Location(country,city);
            Location savedLocation = _repositoryLocation.Save(location);

            
            Language language = new Language(TourLanguage);
            Language savedLanguage = _repositoryLanguage.Save(language);

            CheckPoint checkPoint = new CheckPoint();
            CheckPoint savedCheckPoint = _repositoryCheckPoint.Save(checkPoint);

            


            



            List<string> formattedPictures = new List<string>();

            string[] delimitedPictures = Pictures.Split(",");

            foreach (string picture in delimitedPictures)
            {
                formattedPictures.Add(picture);
            }


            List<CheckPoint> checkPoints = new List<CheckPoint>();

           



            Tour tour = new Tour(Name,savedLocation,Description,savedLanguage,savedLanguage.Id,MaxGuestNumber,checkPoints,TourStart, Duration,formattedPictures);

           
            _repositoryTour.Save(tour);
            Close();
           


            
        }

        private void findPictures_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            //dlg.DefaultExt = ".png";
            dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg";
            dlg.Multiselect = true;

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string[] selectedFiles = dlg.FileNames;

                foreach (string file in selectedFiles)
                {
                    Pictures += file;
                    Pictures += "|";
                }

                Pictures = Pictures.Substring(0, Pictures.Length - 1);

                //string filename = dlg.FileName;
                //ikonicaResursaBox.Text = filename;

                //slikaResursa.Source = new BitmapImage(new Uri(filename));
            }
        }


        
           
        
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
