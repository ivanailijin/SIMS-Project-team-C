using System;
using System.Collections.Generic;
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
    public partial class AddTour : Window
    {

        private readonly TourRepository _repository;
        private readonly LocationRepository _locrepository;


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
        private string _language;

        public string Language
        {
            get => _language;
            set
            {
                if (value != _language)
                {
                    _language = value;
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
        private string _checkPointStart;

        public string CheckPointStart
        {
            get => _checkPointStart;
            set
            {
                if (value != _checkPointStart)
                {
                    _checkPointStart = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _checkPointEnd;

        public string CheckPointEnd
        {
            get => _checkPointEnd;
            set
            {
                if (value != _checkPointEnd)
                {
                    _checkPointEnd = value;
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
        

        public AddTour()
        {
            InitializeComponent();
            _repository = new TourRepository();
        }

        private void AddTour_Click(object sender, RoutedEventArgs e)
        {

            

            string[] words = _location.Split(',');

            string city= words[0];
            string country = words[1];
            Location location = new Location(country,city);
            Location savedLocation = _locrepository.Save(location);


            Tour tour = new Tour(Name,savedLocation,savedLocation.Id,Description,L);

           

            DateTime datRodj = DateTime.Parse(DatRodj);

            adr = _controllerAdr.Create(ulica, broj, grad, drzava);
            int ida = adr.Id;
            ProsOcena = 0;
            BrojESPB = 0;


            
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
