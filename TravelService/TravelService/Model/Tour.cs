using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Serializer;

namespace TravelService.Model
{
    public class Tour: ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }
        public int LocationId { get; set; }
        public string Description { get; set; }
        public Language Language { get; set; }
        public int LanguageId { get; set; } 
        public int MaxGuestNumber { get; set; }
        public CheckPoint CheckPointStart { get; set; }
        public List<CheckPoint> CheckPointMiddle { get; set; }
        public CheckPoint CheckPointEnd { get; set; }
        public int CheckPointStartId { get; set; }
        public int CheckPointEndId { get; set; }
        public List<DateTime> TourStart { get; set; }
        public int Duration { get; set; }
        public List<string> Pictures { get; set; }

        public Tour()
        {
            List<string> Pictures = new List<string>();
            List<DateTime> TourStart = new List<DateTime>();
            List<CheckPoint> CheckPointMiddle = new List<CheckPoint>();
        }
        public Tour( string name, Location location,int locationId, string description, Language language, int languageId,int maxGuestNumber, CheckPoint checkPointStart, List<CheckPoint> checkPointMiddle, CheckPoint checkPointEnd, List<DateTime> tourStart, int duration, List<string> pictures)
        {
           
            Name = name;
            Location = location;
            LocationId = locationId;
            Description = description;
            Language = language;
            LanguageId = languageId;
            MaxGuestNumber = maxGuestNumber;
            CheckPointStart = checkPointStart;
            CheckPointEnd = checkPointEnd;
            TourStart = tourStart;
            Duration = duration;
            Pictures = pictures;
        }

        public string[] ToCSV()
        {
            string[] csvValues = 
            { 
                Id.ToString(),
                Name, 
                LocationId.ToString(),
                Description,
                LanguageId.ToString(),
                MaxGuestNumber.ToString(),
                CheckPointStartId.ToString(),
                CheckPointEndId.ToString(),
                Duration.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name= values[1];
            LocationId = Convert.ToInt32(values[2]);
            Description = values[3];
            LanguageId = Convert.ToInt32(values[4]);
            MaxGuestNumber = Convert.ToInt32(values[5]);
            CheckPointStartId = Convert.ToInt32(values[6]);
            CheckPointEndId = Convert.ToInt32(values[7]);
            Duration = Convert.ToInt32(values[8]);
        }
    }
}
