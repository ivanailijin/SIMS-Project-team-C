using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Serializer;

namespace TravelService.Model
{
    public class AccommodationReservation : ISerializable
    {
        public int Id { get; set; }
        public Accommodation Accommodation { get; set; }
        public int AccommodationId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public bool IsRated { get; set; }
        public AccommodationReservation() { }

        public AccommodationReservation(Accommodation accommodation, int accommodationId, DateTime checkInDate, DateTime checkOutDate, bool isRated)
        {
            Accommodation = accommodation;
            AccommodationId = accommodationId;
            CheckInDate = checkInDate;
            CheckOutDate = checkOutDate;
            IsRated = isRated;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                AccommodationId.ToString(),
                CheckInDate.ToString(),
                CheckOutDate.ToString(),
                IsRated.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            AccommodationId = Convert.ToInt32(values[1]);
            CheckInDate = DateTime.Parse(values[2]);
            CheckOutDate = DateTime.Parse(values[3]);
            IsRated = bool.Parse(values[4]);
        }
    }
}
