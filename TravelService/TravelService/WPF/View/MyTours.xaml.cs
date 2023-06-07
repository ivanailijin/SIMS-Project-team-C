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
    /// Interaction logic for MyTours.xaml
    /// </summary>
    public partial class MyTours : Page
    {

        public NavigationService NavigationService;
        public readonly TourRepository _tourRepository;
        public readonly LocationRepository _locationRepository;
        public readonly LanguageRepository _languageRepository;
        public readonly CheckPointRepository _checkpointRepository;
        public readonly TourReservationRepository _tourReservationRepository;

        public static List<Location> Locations { get; set; }
        public static List<CheckPoint> CheckPoints { get; set; }
        public static List<Language> Languages { get; set; }
        public static List<Tour> FutureTours { get; set; }
        public static List<TourReservation> TourReservations { get; set; }

        public static ObservableCollection<Tour> Tours { get; set; }


        public Tour SelectedTour { get; set; }
        public Guide Guide { get; set; }

        private string _confirmationMessage;
        public string ConfirmationMessage
        {
            get { return _confirmationMessage; }
            set
            {
                _confirmationMessage = value;
                OnPropertyChanged(nameof(ConfirmationMessage));
            }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }


        public MyTours(Tour selectedTour,Guide guide,NavigationService navigationService)
        {
            NavigationService = navigationService;
            InitializeComponent();
            DataContext = this;
            this.Guide = guide;
            _tourRepository = new TourRepository();
            _locationRepository = new LocationRepository();
            _languageRepository = new LanguageRepository();
            _checkpointRepository = new CheckPointRepository();
            _tourReservationRepository = new TourReservationRepository();

            Tours = new ObservableCollection<Tour>(_tourRepository.GetAll());
            Locations = new List<Location>(_locationRepository.GetAll());
            Languages = new List<Language>(_languageRepository.GetAll());
            CheckPoints = new List<CheckPoint>(_checkpointRepository.GetAll());
            TourReservations = new List<TourReservation>(_tourReservationRepository.GetAll());
            FutureTours = new List<Tour>();
            List<Tour> TourList = _tourRepository.GetAll();

            SelectedTour = selectedTour;

            List<Tour> GuideTours = _tourRepository.FindGuidesTours(Guide.Id);

            FutureTours = _tourRepository.ShowFutureTourList(GuideTours, Locations, Languages, CheckPoints,FutureTours, Guide.Id,_tourRepository);

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

        private void CancelTour_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTour != null)
            {
                bool tourCancelled = _tourRepository.CancelTour(SelectedTour.Id);
                if (tourCancelled)
                {
                    Tours.Remove(SelectedTour);

                    // Display message box with option to send vouchers
                   ConfirmationMessage="Tour cancelled successfully! Vouchers Sent!" ;
                    
                        
                        _tourRepository.SendVouchers(SelectedTour);
                    
                }
                else
                {
                    ErrorMessage = "You cannot cancel this tour as it starts within 48 hours.";
                }
            }
        }
    }
}
