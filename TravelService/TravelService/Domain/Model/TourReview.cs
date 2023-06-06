using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using TravelService.Serializer;
using TravelService.View;

namespace TravelService.Domain.Model
{
    public class TourReview : ISerializable
    {

        public int Id { get; set; }
        public int GuideKnowledge { get; set; }
        public int GuideLanguage { get; set; }
        public int TourEntertainment { get; set; }
        public string Comment { get; set; }
        public List<Uri> Pictures { get; set; }
        public int GuideId { get; set; }
        public int GuestId { get; set; }
        public bool Valid { get; set; }
        public Tour Tour { get; set; }
        public TourReview()
        {
            Pictures = new List<Uri>();
        }
        public TourReview(int guideKnowledge, int guideLanguage, int tourEntertainment, string comment, List<string> pictures, int guideId, int guestId, bool valid)
        {
            GuideKnowledge = guideKnowledge;
            GuideLanguage = guideLanguage;
            TourEntertainment = tourEntertainment;
            Comment = comment;
            Pictures = new List<Uri>();
            GuideId = guideId;
            GuestId = guestId;
            Valid = valid;

            foreach (string picture in pictures)
            {
                Uri file = new Uri(picture);
                Pictures.Add(file);
            }
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
                GuideKnowledge.ToString(),
                GuideLanguage.ToString(),
                TourEntertainment.ToString(),
                Comment.ToString(),
                pictureList.ToString(),
                GuideId.ToString(),
                GuestId.ToString(),
                Valid.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            GuideKnowledge = Convert.ToInt32(values[1]);
            GuideLanguage = Convert.ToInt32(values[2]);
            TourEntertainment = Convert.ToInt32(values[3]);
            Comment = values[4];
            string pictures = values[5];
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
            GuideId = Convert.ToInt32(values[6]);
            GuestId = Convert.ToInt32(values[7]);
            Valid = bool.Parse(values[8]);
        }
    }
}