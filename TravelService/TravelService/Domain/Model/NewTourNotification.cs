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
        public string Description { get; set; }
        public int GuestId { get; set; }
        public DateTime TourStart { get; set; }

        public NewTourNotification() { }

        public NewTourNotification(int tourId, int guestId, string description,DateTime tourStart)
        {
            TourId = tourId;
            Description = description;
            GuestId = guestId;
            TourStart = tourStart;

        }
        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                TourId.ToString(),
                Description,
                GuestId.ToString(),
                TourStart.ToString(),

            };
            return csvValues;
        }
        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            TourId = int.Parse(values[1]);
            Description = values[2];
            GuestId = int.Parse(values[3]);
            TourStart = DateTime.Parse(values[4]);

        }
    }
}

        