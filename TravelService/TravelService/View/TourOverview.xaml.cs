using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using TravelService.Model;
using TravelService.Repository;

namespace TravelService.View
{
    /// <summary>
    /// Interaction logic for TourOverview.xaml
    /// </summary>
    public partial class TourOverview : Window
    {

        public readonly TourRepository _tourRepository;
        private ObservableCollection<Tour> _activeTours;
        public readonly LocationRepository _locationRepository;

        public readonly LanguageRepository _languageRepository;

        public readonly CheckPointRepository _checkpointRepository;
        public static List<Location> Locations { get; set; }
        public static ObservableCollection<Tour> Tours { get; set; }
        public static List<CheckPoint> CheckPoints { get; set; }
        public List<Tour> ActiveTours { get; set; }

        public static List<Language> Languages { get; set; }
        public Tour SelectedTour { get; set; }


        public TourOverview(Tour selectedTour)
        {
            InitializeComponent();
            DataContext = this;
            _tourRepository = new TourRepository();
            _locationRepository = new LocationRepository();
            _languageRepository = new LanguageRepository();
            _checkpointRepository = new CheckPointRepository();

            Tours = new ObservableCollection<Tour>(_tourRepository.GetAll());
            Locations = new List<Location>(_locationRepository.GetAll());
            Languages = new List<Language>(_languageRepository.GetAll());
            CheckPoints = new List<CheckPoint>(_checkpointRepository.GetAll());
            //_activeTours = new ObservableCollection<Tour>(ActiveTours(_tourRepository.GetAll()));

            //   allActiveTours.ItemsSource = _activeTours;
            ActiveTours = new List<Tour>();
            SelectedTour = selectedTour;




            foreach (Tour tour in Tours)
            {

                List<CheckPoint> ListCheckPoints = new List<CheckPoint>();
                tour.Location = Locations.Find(loc => loc.Id == tour.LocationId);
                tour.Language = Languages.Find(lan => lan.Id == tour.LanguageId);


                tour.CheckPoints.Clear();
                ListCheckPoints.Clear();

                int currentId = tour.Id;


                foreach (CheckPoint c in CheckPoints)
                {
                    int currentCheckPointTourId = c.TourId;
                    if ((currentCheckPointTourId == currentId))
                    {
                        ListCheckPoints.Add(c);

                    }
                }

                tour.CheckPoints.AddRange(ListCheckPoints);

                if (IsInProgress(tour))
                {
                    ActiveTours.Add(tour);

                }


            }



        }




        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            
            if (SelectedTour != null)
            {
                CheckPointView checkPointView = new CheckPointView(SelectedTour);
               
                checkPointView.Show();
                Close();
                

            }




        }
        /*  public List<Tour> ActiveTours(List<Tour> tours)
          {
              List<Tour> activeTours = new List<Tour>();
              foreach (Tour tour in tours)
              {
                  if (IsInProgress(tour))
                  {
                      activeTours.Add(tour);
                  }
              }
              return activeTours;
          }
        */

        public bool IsInProgress(Tour tour)
        {
            DateTime currentDate = DateTime.Now.Date;
            if (tour.TourStart.Date == currentDate)
            {
                return true;
            }
            return false;
        }
    }
}