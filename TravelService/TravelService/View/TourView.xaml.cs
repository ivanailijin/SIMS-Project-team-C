using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
using System.Xml.Linq;
using TravelService.Repository;
using TravelService.Model;

namespace TravelService.View
{

    public partial class TourView : Window
    {
        public readonly TourRepository _tourRepository;

        public readonly LocationRepository _locationRepository;

        public readonly LanguageRepository _languageRepository;

        public static List<Location> Locations { get; set; }
        public static ObservableCollection<Tour> Tours { get; set; }

        public static List<Language> Languages { get; set; }
        public TourView()
        {
            InitializeComponent();
            DataContext = this;
            _tourRepository = new TourRepository();
            _locationRepository = new LocationRepository();
            _languageRepository = new LanguageRepository();
            Tours = new ObservableCollection<Tour>(_tourRepository.GetAll());
            Locations = new List<Location>(_locationRepository.GetAll());
            Languages = new List<Language>(_languageRepository.GetAll());

            foreach (Tour tour in Tours)
            {
                tour.Location = Locations.Find(loc => loc.Id == tour.LocationId);
                tour.Language = Languages.Find(lan => lan.Id == tour.LanguageId);
            }
        }

       private void findByEntity(object sender, RoutedEventArgs e)
        {
            /*string[] input = findByEntityBox.Text;

            foreach (Tour tour in Tours) { 
                if(tour.Duration)
            }*/
        }
    }
}
