using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.WPF.View;
using TravelService.Serializer;

namespace TravelService.Domain.Model
{
    public class NewTourNotification : ISerializable
    {
        public int Id { get; set; }
        public int TourId { get; set; }
        public int GuestId { get; set; }
        public NewTourNotification() { }

        public NewTourNotification(int tourId, int guestId) 
        { 
            TourId = tourId;
            GuestId= guestId;
        }
        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                TourId.ToString(),
                GuestId.ToString(),
            };
            return csvValues;
        }
        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            TourId = int.Parse(values[1]);
            GuestId = int.Parse(values[2]);
        }
    }
}
