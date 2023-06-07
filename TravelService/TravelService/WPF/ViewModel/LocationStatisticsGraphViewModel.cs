using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Applications.UseCases;
using TravelService.Applications.Utils;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;

namespace TravelService.WPF.ViewModel
{
    public class LocationStatisticsGraphViewModel : ViewModelBase
    {
        private readonly TourRequestService _tourRequestService;
        private readonly LocationService _locationService;
        public ObservableCollection<TourRequest> GuestsRequests { get; set; }
        public ObservableCollection<Location> Locations { get; set; }
        public Guest2 Guest2 { get; set; }
        public Action CloseAction { get; set; }

        private ObservableCollection<LocationDataPoint> _locationDataPoints;
        public ObservableCollection<LocationDataPoint> LocationDataPoints
        {
            get => _locationDataPoints;
            set
            {
                if (value != _locationDataPoints)
                {
                    _locationDataPoints = value;
                    OnPropertyChanged();
                }
            }
        }
        public LocationStatisticsGraphViewModel(Guest2 guest2)
        {
            _tourRequestService = new TourRequestService(Injector.CreateInstance<ITourRequestRepository>());
            _locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            Guest2 = guest2;
            double RequestNumber = 0;
            List<TourRequest> tourRequests = new List<TourRequest>(_tourRequestService.GetAll());
            List<Location> locations = new List<Location>(_locationService.GetAll());
            List<TourRequest> guestsRequests = new List<TourRequest>(_tourRequestService.FindGuestsRequests(tourRequests, guest2.Id));

            Locations = new ObservableCollection<Location>(locations);
            GuestsRequests = new ObservableCollection<TourRequest>(guestsRequests);
            List<LocationDataPoint> locationDataPoints = new List<LocationDataPoint>(_tourRequestService.GetLocationDataPoints(locations, RequestNumber, GuestsRequests));
            LocationDataPoints = new ObservableCollection<LocationDataPoint>(_tourRequestService.CalculateLocationDataPointPositions(locationDataPoints, Locations, GuestsRequests));

        }
    }
}
