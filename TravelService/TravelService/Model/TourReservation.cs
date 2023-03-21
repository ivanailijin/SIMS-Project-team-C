using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using TravelService.Serializer;

namespace TravelService.Model
{
    public class TourReservation : ISerializable
    {
        public int Id { get; set; }
        public int TourId { get; set; }
        public int GuestNumber { get; set; }

        public TourReservation() { }
        public TourReservation(int id, int tourId, int guestNumber)
        {
            Id = id;
            TourId = tourId;
            GuestNumber = guestNumber;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                TourId.ToString(),
                GuestNumber.ToString(),
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            TourId = int.Parse(values[1]);
            GuestNumber = int.Parse(values[2]);
        }
    }
}
