using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Serializer;

namespace TravelService.Domain.Model
{
    public class AccommodationRenovation : ISerializable
    {
        public int Id { get; set; }
        public int AccommodationId { get; set; }
        public Accommodation Accommodation { get; set; }
        public int OwnerId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public String Description { get; set; }

        public AccommodationRenovation() { }

        public AccommodationRenovation(int accommodationId, int ownerId, DateTime startDate, DateTime endDate, String description)
        {
            AccommodationId = accommodationId;
            OwnerId = ownerId;
            StartDate = startDate;  
            EndDate = endDate;
            Description = description;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                AccommodationId.ToString(),
                OwnerId.ToString(),
                StartDate.ToString(),
                EndDate.ToString(),
                Description,
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            AccommodationId = Convert.ToInt32(values[1]);
            OwnerId = Convert.ToInt32(values[2]);
            StartDate = DateTime.Parse(values[3]);
            EndDate = DateTime.Parse(values[4]);
            Description = values[5];
        }
    }
}
