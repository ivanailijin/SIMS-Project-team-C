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
        public int AccommodationId { get; set; }
        public string AccommodationName { get; set; }
        public string GuestName { get; set; }
        public string GuestSurname { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int LengthOfStay { get; set; }
        public int GuestNumber { get; set; }
        public bool IsRated { get; set; }
        public AccommodationReservation() { }

        public AccommodationReservation(int accommodationId,string accommodationName, string guestName, string guestSurname, DateTime checkInDate, DateTime checkOutDate,int lengthOfStay, int guestNumber, bool isRated)
        {
            AccommodationId = accommodationId;
            AccommodationName = accommodationName;
            GuestName = guestName;
            GuestSurname = guestSurname;
            CheckInDate = checkInDate;
            CheckOutDate = checkOutDate;
            LengthOfStay = lengthOfStay;
            GuestNumber = guestNumber;
            IsRated = isRated;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                AccommodationId.ToString(),
                AccommodationName,
                GuestName,
                GuestSurname,
                CheckInDate.ToString(),
                CheckOutDate.ToString(),
                LengthOfStay.ToString(),
                GuestNumber.ToString(),
                IsRated.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            AccommodationId = Convert.ToInt32(values[1]);
            AccommodationName = values[2];
            GuestName = values[3];
            GuestSurname = values[4];
            CheckInDate = DateTime.Parse(values[5]);
            CheckOutDate = DateTime.Parse(values[6]);
            LengthOfStay = Convert.ToInt32(values[7]);
            GuestNumber = Convert.ToInt32(values[8]);
            IsRated = bool.Parse(values[9]);
        }
    }
}
