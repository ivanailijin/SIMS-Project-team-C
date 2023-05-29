using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TravelService.Application.UseCases;
using TravelService.Application.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class AccommodationStatisticsViewModel
    {
        public AccommodationService _accommodationService;

        public LocationService _locationService;

        public Action CloseAction { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand ShowReviewCommand { get; set; }

        public Accommodation SelectedAccommodation { get; set; }
        public static ObservableCollection<Accommodation> Accommodations { get; set; }
        public static List<Location> Locations { get; set; }
        public Owner Owner { get; set; }

        public AccommodationStatisticsViewModel(Owner owner)
        {
            InitializeCommands();
            this.Owner = owner;
            _accommodationService = new AccommodationService(Injector.CreateInstance<IAccommodationRepository>());
            _locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            Accommodations = new ObservableCollection<Accommodation>(_accommodationService.GetOwnersAccommodations(Owner.Id));
            Locations = new List<Location>(_locationService.GetAll());

            foreach (Accommodation accommodation in Accommodations)
            {
                accommodation.Location = Locations.Find(l => l.Id == accommodation.Location.Id);
            }
        }
        private void InitializeCommands()
        {
            CancelCommand = new RelayCommand(Execute_CancelCommand, CanExecute_Command);
            ShowReviewCommand = new RelayCommand(Execute_ShowReviewCommand, CanExecute_Command);
        }
        private void Execute_ShowReviewCommand(object obj)
        {
            if (SelectedAccommodation != null)
            {
                AccommodationYearStatisticsView accommodationYearStatisticsView = new AccommodationYearStatisticsView(SelectedAccommodation);
                accommodationYearStatisticsView.Show();
            }
            else
            {
                MessageBox.Show("Please select accommodation to show the review.");
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
