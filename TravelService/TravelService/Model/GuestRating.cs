using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TravelService.Serializer;

namespace TravelService.Model
{
    public class GuestRating : ISerializable
    {
        public int Id { get; set; } 
        public int Cleanness { get; set; }
        public int RulesFollowing { get; set; }
        public int Communication { get; set; }
        public int NoiseLevel { get; set; }
        public int PropertyRespect { get; set; }
        public string Comment { get; set; }
        public int ReservationId { get; set; }

        public GuestRating() { }
        public GuestRating(int cleanness, int rulesFollowing, int communication, int noiseLevel, int propertyRespect, string comment, int reservationId)
        {
            Cleanness = cleanness;
            RulesFollowing = rulesFollowing;
            Communication = communication;
            NoiseLevel = noiseLevel;
            PropertyRespect = propertyRespect;
            Comment = comment;
            ReservationId = reservationId;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Cleanness = Convert.ToInt32(values[1]);
            RulesFollowing = Convert.ToInt32(values[2]);
            Communication = Convert.ToInt32(values[3]);
            NoiseLevel = Convert.ToInt32(values[4]);
            PropertyRespect = Convert.ToInt32(values[5]);
            Comment = values[6];
            ReservationId = Convert.ToInt32(values[7]);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
           {
                Id.ToString(),
                Cleanness.ToString(),
                Cleanness.ToString(),
                Cleanness.ToString(),
                Cleanness.ToString(),
                RulesFollowing.ToString(),
                Comment,
                ReservationId.ToString(),
            };
            return csvValues;
        }
    }
}
