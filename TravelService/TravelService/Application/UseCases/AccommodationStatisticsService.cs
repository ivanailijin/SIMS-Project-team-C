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

        public AccommodationStatisticsService()
        {
            _reservationService = new AccommodationReservationService(Injector.CreateInstance<IAccommodationReservationRepository>());
            _reservationRequestService = new ReservationRequestService(Injector.CreateInstance<IReservationRequestRepository>());
        }

        public List<AccommodationYearStatistics> GetAccommodationYearStatistics(int accommodationId)
        {
            List<AccommodationYearStatistics> statistics = new List<AccommodationYearStatistics>();

            for(int year = 2019; year <= 2023; year++)
            {
                int reservationsNumber = _reservationService.GetReservationYearNumber(year, accommodationId);
                int cancelledReservationsNumber = _reservationService.GetCancelledReservationYearNumber(year, accommodationId);
                int requestsNumber = _reservationRequestService.GetRequestsYearNumber(year, accommodationId);

                AccommodationYearStatistics yearStatistics = new AccommodationYearStatistics(year, reservationsNumber, cancelledReservationsNumber, requestsNumber, 3);
                statistics.Add(yearStatistics);
            }

            return statistics;
        }
    }
}
