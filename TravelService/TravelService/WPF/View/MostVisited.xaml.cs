using System;
using System.Collections.Generic;
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
    public partial class MostVisited : Page, INotifyPropertyChanged
    {
        public NavigationService NavigationService;
        public int SelectedYear { get; set; }
        private readonly TourRepository _tourRepository;
        public List<Tour> MostVisitedTours { get; set; }
        public List<Tour> MostVisitedToursInYear { get; set; }
        public static List<Location> Locations { get; set; }
        public readonly LocationRepository _locationRepository;

        public Guide Guide { get; set; }
        public List<int> AvailableYears { get; set; }

        public MostVisited(Guide guide,NavigationService navigationService)
        {
            NavigationService = navigationService;  
            InitializeComponent();
            DataContext = this;
            _locationRepository = new LocationRepository();
            _tourRepository = new TourRepository();
            Guide = guide;

            var startYear = _tourRepository.GetAll().Min(t => t.TourStart.Year);
            var currentYear = DateTime.Now.Year;
            AvailableYears = Enumerable.Range(startYear, currentYear - startYear + 1).ToList();

            if (AvailableYears.Count > 0)
            {
                SelectedYear = AvailableYears[0];
            }

            List<Guest> guests = new List<Guest>();
            Locations = new List<Location>(_locationRepository.GetAll());

            List<Tour> guideTours = _tourRepository.FindGuidesTours(guide.Id);
            MostVisitedTours = new List<Tour> { _tourRepository.GetMostVisitedTour(guideTours, guests, Locations) };
            MostVisitedToursInYear = new List<Tour>();

            UpdateMostVisitedToursInYear(guide);
            SelectYear.SelectionChanged += YearComboBox_SelectionChanged;

        }



        private void UpdateMostVisitedToursInYear(Guide guide)
        {
            if (guide == null)
            {
                return;
            }

            List<Guest> guests = new List<Guest>();
            List<Tour> guideTours = _tourRepository.FindGuidesTours(guide.Id);

            if (MostVisitedToursInYear == null)
            {
                MostVisitedToursInYear = new List<Tour>();
            }

            Tour mostVisitedTourInYear = _tourRepository.GetMostVisitedTour(guideTours, guests, Locations, SelectedYear);

            if (mostVisitedTourInYear != null)
            {
                MostVisitedToursInYear.Clear();
                MostVisitedToursInYear.Add(mostVisitedTourInYear);
                OnPropertyChanged(nameof(MostVisitedToursInYear));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private void YearComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedYear = (int)((ComboBox)sender).SelectedValue;
            UpdateMostVisitedToursInYear(Guide);
            MostVisitedToursInYear = MostVisitedTours.Where(t => t.TourStart.Year == SelectedYear).ToList();
            OnPropertyChanged(nameof(MostVisitedToursInYear));
        }






    }
}
