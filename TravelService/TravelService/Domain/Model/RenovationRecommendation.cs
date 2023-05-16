using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using TravelService.Serializer;

namespace TravelService.Domain.Model
{
    public class RenovationRecommendation : ISerializable
    {
        public int Id { get; set; }
        public int AccommodationId { get; set; }
        public string Comment { get; set; }
        public int UrgencyLevel { get; set; }
        public DateTime DateCreated { get; set; }
        public RenovationRecommendation() { }
        public RenovationRecommendation(int accommodationId, string comment, int urgencyLevel)
        {
            AccommodationId = accommodationId;
            Comment = comment;
            UrgencyLevel = urgencyLevel;
            DateCreated = DateTime.Today;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                AccommodationId.ToString(),
                Comment,
                UrgencyLevel.ToString(),
                DateCreated.ToString(),
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            AccommodationId = Convert.ToInt32(values[1]);
            Comment = values[2];
            UrgencyLevel = Convert.ToInt32(values[3]);
            DateCreated = DateTime.Parse(values[4]);
        }
    }
}
