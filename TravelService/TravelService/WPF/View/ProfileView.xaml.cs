using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using TravelService.Domain.Model;
using TravelService.Repository;

namespace TravelService.WPF.View
{
    /// <summary>
    /// Interaction logic for ProfileView.xaml
    /// </summary>
    public partial class ProfileView : Page
    {

        private bool _isSuperGuide;

        public bool IsSuperGuide
        {
            get { return _isSuperGuide; }
            set
            {
                _isSuperGuide = value;
                OnPropertyChanged(); // Ovdje pozovite OnPropertyChanged da biste obavijestili sustav o promjeni vrijednosti svojstva
            }
        }



        public NavigationService NavigationService;
        public readonly TourRepository _tourRepository;
      

        public readonly TourReviewRepository _tourReviewRepository; 
        
        public static List<Location> Locations { get; set; }
        public static List<CheckPoint> CheckPoints { get; set; }
        public static List<Language> Languages { get; set; }
        public static List<Tour> FutureTours { get; set; }
        public static List<TourReservation> TourReservations { get; set; }
        public static List<TourReview> TourReviews { get; set; }
        public static List<Tour> Tourlist { get; set; }

        public static ObservableCollection<Tour> Tours { get; set; }


        public Tour Tour { get; set; }
        public Guide Guide { get; set; }
        public Language Language { get; set; }

        public ProfileView( Guide guide, NavigationService navigationService)
        {
           
            NavigationService = navigationService;
         
         
            this.Guide = guide;
            _tourRepository = new TourRepository();
           
           
            _tourReviewRepository = new TourReviewRepository();

            Tours = new ObservableCollection<Tour>(_tourRepository.GetAll());
        
            TourReviews = new List<TourReview>(_tourReviewRepository.GetAll());
            Tourlist = new List<Tour>(_tourRepository.GetAll());
            FutureTours = new List<Tour>();
            List<Tour> TourList = _tourRepository.GetAll();


            List<Tour> GuideTours = _tourRepository.FindGuidesTours(Guide.Id);
          
            IsSuperGuide = _tourReviewRepository.CalculateSuperGuideStatus(Guide, TourReviews, TourList);
            InitializeComponent();
            DataContext = this;
        }

        private List<Tour> convertTourList(ObservableCollection<Tour> observableCollection)
        {
            List<Tour> convertedList = observableCollection.ToList();
            return convertedList;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OtkazTour_Click(object sender, RoutedEventArgs e)
        {
           
                bool tourCancelled = _tourRepository.Otkaz(Guide.Id);
                if (tourCancelled)
                {
                    Tours.Remove(Tour);

                    // Display message box with option to send vouchers
                    var result = MessageBox.Show("Tour cancelled successfully! Do you want to send vouchers to guests?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {

                        _tourRepository.PoslatiVaucer();
                    }
                }
                else
                {
                    MessageBox.Show("You cannot cancel this tour as it starts within 48 hours.");
                }
            
        }
    }
}
