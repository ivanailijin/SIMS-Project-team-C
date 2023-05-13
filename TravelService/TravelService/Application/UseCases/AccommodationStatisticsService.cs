using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Application.Utils;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;

namespace TravelService.Application.UseCases
{
    public class AccommodationStatisticsService
    {
        private readonly AccommodationReservationService _reservationService;

        private readonly ReservationRequestService _reservationRequestService;

        private readonly RenovationRecommendationService _recommendationService;

        public AccommodationStatisticsService()
        {
            _reservationService = new AccommodationReservationService(Injector.CreateInstance<IAccommodationReservationRepository>());
            _reservationRequestService = new ReservationRequestService(Injector.CreateInstance<IReservationRequestRepository>());
            _recommendationService = new RenovationRecommendationService(Injector.CreateInstance<IRenovationRecommendationRepository>());
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
