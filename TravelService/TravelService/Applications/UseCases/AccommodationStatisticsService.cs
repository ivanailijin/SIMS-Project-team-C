using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Applications.Utils;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.WPF.ViewModel;

namespace TravelService.Applications.UseCases
{
    public class AccommodationStatisticsService
    {
        private readonly AccommodationReservationService _reservationService;

        private readonly ReservationRequestService _reservationRequestService;

        private readonly RenovationRecommendationService _recommendationService;

        private readonly LocationService _locationService;

        public AccommodationStatisticsService()
        {
            _reservationService = new AccommodationReservationService(Injector.CreateInstance<IAccommodationReservationRepository>());
            _reservationRequestService = new ReservationRequestService(Injector.CreateInstance<IReservationRequestRepository>());
            _recommendationService = new RenovationRecommendationService(Injector.CreateInstance<IRenovationRecommendationRepository>());
            _locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
        }

        public List<AccommodationYearStatistics> GetAccommodationYearStatistics(Accommodation accommodation)
        {
            List<AccommodationYearStatistics> statistics = new List<AccommodationYearStatistics>();

            for(int year = accommodation.DateCreated.Year; year <= DateTime.Today.Year; year++)
            {
                int reservationsNumber = _reservationService.GetReservationYearNumber(year, accommodation.Id);
                int cancelledReservationsNumber = _reservationService.GetCancelledReservationYearNumber(year, accommodation.Id);
                int requestsNumber = _reservationRequestService.GetRequestsYearNumber(year, accommodation.Id);
                int recommendationsNumber = _recommendationService.GetRecommendationYearNumber(year, accommodation.Id);

                AccommodationYearStatistics yearStatistics = new AccommodationYearStatistics(year, reservationsNumber, cancelledReservationsNumber, requestsNumber, recommendationsNumber);
                statistics.Add(yearStatistics);
            }

            return statistics;
        }
        public int GetBusiestYear(Accommodation accommodation)
        {
            int busiestYear = accommodation.DateCreated.Year;
            double highestBusyness = _reservationService.GetBusynessPerYear(accommodation, accommodation.DateCreated.Year);

            for (int year = accommodation.DateCreated.Year; year <= DateTime.Today.Year; year++)
            {
                if(_reservationService.GetBusynessPerYear(accommodation, year) > highestBusyness)
                {
                    highestBusyness = _reservationService.GetBusynessPerYear(accommodation, year);
                    busiestYear = year;
                }
            }

            return busiestYear;
        }
        public int GetBusiestMonth(Accommodation accommodation, int year)
        {
            int busiestMonth = 1;
            double highestBusyness = _reservationService.GetBusynessPerMonth(accommodation, year, 1);

            for (int month = 1; month <= 12; month++)
            {
                double tempBusyness = _reservationService.GetBusynessPerMonth(accommodation, year, month);
                if (_reservationService.GetBusynessPerMonth(accommodation, year, month) > highestBusyness)
                {
                    highestBusyness = _reservationService.GetBusynessPerMonth(accommodation, year, month);
                    busiestMonth = month;
                }
            }

            return busiestMonth;
        }
        public List<Location> GetThreeLocationsByHighestBusyness()
        {
            List<Location> allLocations = _locationService.GetAll();
            List<Location> topThreeLocations = new List<Location>();

            List<Location> sortedLocations = allLocations.OrderByDescending(l => _reservationService.GetBusynessByLocation(l)).ToList();
            topThreeLocations = sortedLocations.Take(3).ToList();

            return topThreeLocations;
        }
        public List<Location> GetThreeLocationsByLowestBusyness()
        {
            List<Location> allLocations = _locationService.GetAll();
            List<Location> topThreeLocations = new List<Location>();

            List<Location> sortedLocations = allLocations.OrderByDescending(l => _reservationService.GetBusynessByLocation(l)).ToList();
            sortedLocations.Reverse();
            topThreeLocations = sortedLocations.Take(3).ToList();

            return topThreeLocations;
        }
        public List<Location> GetThreeLocationsByHighestReservations()
        {
            List<Location> allLocations = _locationService.GetAll();
            List<Location> topThreeLocations = new List<Location>();

            List<Location> sortedLocations = allLocations.OrderByDescending(l => _reservationService.GetReservationsNumberByLocation(l)).ToList();
            topThreeLocations = sortedLocations.Take(3).ToList();

            return topThreeLocations;
        }
        public List<Location> GetThreeLocationsByLowestReservations()
        {
            List<Location> allLocations = _locationService.GetAll();
            List<Location> topThreeLocations = new List<Location>();

            List<Location> sortedLocations = allLocations.OrderByDescending(l => _reservationService.GetReservationsNumberByLocation(l)).ToList();
            sortedLocations.Reverse();
            topThreeLocations = sortedLocations.Take(3).ToList();

            return topThreeLocations;
        }
        public List<Location> GetLocationsWithHighestParameters()
        {
            List<Location> locationsByReservations = GetThreeLocationsByHighestReservations();
            List<Location> locationsByBusyness = GetThreeLocationsByHighestBusyness();

            List<Location> mergedLocations = locationsByReservations.Concat(locationsByBusyness).ToList();

            List<Location> sortedLocations = mergedLocations.OrderByDescending(l => _reservationService.GetReservationsNumberByLocation(l)).ThenByDescending(l => _reservationService.GetBusynessByLocation(l)).ToList();

            List<Location> distinctLocations = new List<Location>();

            foreach (Location location in sortedLocations)
            {
                if (!ContainLocation(location, distinctLocations))
                {
                    distinctLocations.Add(location);
                }
            }

            List<Location> topThreeLocations = distinctLocations.Take(3).ToList();

            return topThreeLocations;
        }
        public List<Location> GetLocationsWithLowestParameters()
        {
            List<Location> locationsByReservations = GetThreeLocationsByLowestReservations();
            List<Location> locationsByBusyness = GetThreeLocationsByLowestBusyness();

            List<Location> mergedLocations = locationsByReservations.Concat(locationsByBusyness).ToList();

            List<Location> sortedLocations = mergedLocations.OrderByDescending(l => _reservationService.GetReservationsNumberByLocation(l)).ThenByDescending(l => _reservationService.GetBusynessByLocation(l)).ToList();

            List<Location> distinctLocations = new List<Location>();

            foreach (Location location in sortedLocations)
            {
                if (!ContainLocation(location, distinctLocations))
                {
                    distinctLocations.Add(location);
                }
            }

            List<Location> topThreeLocations = distinctLocations.Take(3).ToList();

            return topThreeLocations;
        }
        private bool ContainLocation(Location location, List<Location> locations)
        {
            foreach (Location location2 in locations)
            {
                if (location2.Id == location.Id)
                {
                    return true;
                }
            }
            return false;
        }
        public Location GetMostPopularLocation()
        {
            List<Location> locations = _locationService.GetAll();

            Location MostPopularLocation = locations[0];

            foreach(Location location in locations)
            {
                if(_reservationService.GetReservationsNumberByLocation(location) > _reservationService.GetReservationsNumberByLocation(MostPopularLocation))
                    MostPopularLocation = location;
            }

            return MostPopularLocation;
        }

        public Location GetLeastPopularLocation()
        {
            List<Location> locations = _locationService.GetAll();

            Location LeastPopularLocation = locations[0];

            foreach (Location location in locations)
            {
                if (_reservationService.GetReservationsNumberByLocation(location) < _reservationService.GetReservationsNumberByLocation(LeastPopularLocation))
                    LeastPopularLocation = location;
            }

            return LeastPopularLocation;
        }
        public List<AccommodationMonthStatistics> GetAccommodationMonthStatistics(Accommodation accommodation, int year)
        {
            List<AccommodationMonthStatistics> statistics = new List<AccommodationMonthStatistics>();

            for (int month = 1; month <= 12; month++)
            {
                int reservationsNumber = _reservationService.GetReservationMonthNumber(month, year, accommodation.Id);
                int cancelledReservationsNumber = _reservationService.GetCancelledReservationMonthNumber(month,year, accommodation.Id);
                int requestsNumber = _reservationRequestService.GetRequestsMonthNumber(month, year, accommodation.Id);
                int recommendationsNumber = _recommendationService.GetRecommendationMonthNumber(month,year, accommodation.Id);

                AccommodationMonthStatistics monthStatistics = new AccommodationMonthStatistics(month, reservationsNumber, cancelledReservationsNumber, requestsNumber, recommendationsNumber);
                statistics.Add(monthStatistics);
            }

            return statistics;
        }
    }
}
