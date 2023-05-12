using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Xml.Linq;

namespace TravelService.Domain.Model
{
    public class AccommodationYearStatistics
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public int ReservationsNumber { get; set; }
        public int CancellationNumber { get; set; }
        public int MovingReservationsNumber { get; set; }
        public int RecommendationsNumber { get; set; }

        public AccommodationYearStatistics() { }

        public AccommodationYearStatistics(int year, int reservationsNumber, int cancellationNumber, int movingReservationsNumber, int recommendationsNumber)
        {
            Year = year;
            ReservationsNumber = reservationsNumber;
            CancellationNumber = cancellationNumber;
            MovingReservationsNumber = movingReservationsNumber;
            RecommendationsNumber = recommendationsNumber;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Year.ToString(),
                ReservationsNumber.ToString(),
                CancellationNumber.ToString(),
                MovingReservationsNumber.ToString(),
                RecommendationsNumber.ToString(),
            };
            return csvValues;
        }


        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Year = Convert.ToInt32(values[1]);
            ReservationsNumber = Convert.ToInt32(values[2]);
            CancellationNumber = Convert.ToInt32(values[3]);
            MovingReservationsNumber = Convert.ToInt32(values[4]);
            RecommendationsNumber = Convert.ToInt32(values[5]);
        }
    }
}
