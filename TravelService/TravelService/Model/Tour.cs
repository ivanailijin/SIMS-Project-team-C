using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Serializer;

namespace TravelService.Model
{
    public class Tour : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }
        public int LocationId { get; set; }
        public string Description { get; set; }
        public Language Language { get; set; }
        public int LanguageId { get; set; }
        public int MaxGuestNumber { get; set; }
        public List<CheckPoint> CheckPoints { get; set; }
        public DateTime TourStart { get; set; }
        public DateTime TourEnd { get; set; }
        public bool IsActive { get; set; }
        public int Duration { get; set; }
        public List<Uri> Pictures { get; set; }
        public bool Done { get; set; }

        public Tour()
        {
            Pictures = new List<Uri>();
            CheckPoints = new List<CheckPoint>();

        }
        public Tour(string name, Location location, string description, Language language, int languageId, int maxGuestNumber, DateTime tourStart, int duration, List<string> pictures, bool done)
        {

            Name = name;
            Location = location;
            Description = description;
            Language = language;
            LanguageId = languageId;
            MaxGuestNumber = maxGuestNumber;
            TourStart = tourStart;
            Duration = duration;
            Pictures = new List<Uri>();

            foreach (string picture in pictures)
            {
                Uri file = new Uri(picture);
                Pictures.Add(file);
            }
            Done = done;
        }

        public string[] ToCSV()
        {
            StringBuilder pictureList = new StringBuilder();

            foreach (Uri picture in Pictures)
            {
                string pictureString = picture.ToString();
                pictureList.Append(picture);
                pictureList.Append(" ,");
            }

            if (pictureList.Length > 0)
            {
                pictureList.Remove(pictureList.Length - 1, 1);
            }




            string[] csvValues =
            {
                Id.ToString(),
                Name,
                LocationId.ToString(),
                Description,
                LanguageId.ToString(),
                MaxGuestNumber.ToString(),
               // CheckPointId.ToString(),
                Duration.ToString(),
                pictureList.ToString(),
                TourStart.ToString(),
                Done.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            LocationId = int.Parse(values[2]);
            Description = values[3];
            LanguageId = int.Parse(values[4]);
            MaxGuestNumber = int.Parse(values[5]);
            // CheckPointId = int.Parse(values[6]);
            Duration = int.Parse(values[6]);


            string pictures = values[7];
            string[] delimitedPictures = pictures.Split(",");
            if (Pictures == null)
            {
                Pictures = new List<Uri>();
            }

            foreach (string picture in delimitedPictures)
            {
                Uri file = new Uri(picture);
                Pictures.Add(file);
            }



            TourStart = DateTime.Parse(values[8]);




        }



    }
}