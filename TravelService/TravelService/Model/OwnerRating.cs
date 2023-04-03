using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TravelService.Serializer;

namespace TravelService.Model
{
    public class OwnerRating : ISerializable
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public int GuestId { get; set; }
        public Guest1 Guest { get; set; }
        public int OwnerId { get; set; }
        public Owner Owner { get; set; }
        public int Correctness { get; set; }
        public int Cleanliness { get; set; }
        public int Location  { get; set; }
        public int Comfort { get; set; }
        public int Content { get; set; }
        public string Comment { get; set; }
        public List<Uri> Pictures { get; set; }

        public OwnerRating() 
        {
            Pictures = new List<Uri>();
        }

        public OwnerRating(int reservationId, int guestId, int ownerId, int correctness, int cleanliness, int location, int comfort, int content, string comment, List<string> pictures)
        {
            ReservationId = reservationId;
            GuestId = guestId;
            OwnerId = ownerId;
            Correctness = correctness;
            Cleanliness = cleanliness;
            Location = location;
            Comfort = comfort;
            Content = content;
            Comment = comment;
            Pictures = new List<Uri>();

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

            pictureList.Remove(pictureList.Length - 1, 1);

            string[] csvValues =
            {
                Id.ToString(),
                ReservationId.ToString(),
                GuestId.ToString(),
                OwnerId.ToString(), 
                Correctness.ToString(),
                Cleanliness.ToString(),
                Location.ToString(),
                Comfort.ToString(),
                Content.ToString(),
                Comment,
                pictureList.ToString(),
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            ReservationId = Convert.ToInt32(values[1]);
            GuestId = Convert.ToInt32(values[2]);
            OwnerId = Convert.ToInt32(values[3]);
            Correctness = Convert.ToInt32(values[4]);
            Cleanliness = Convert.ToInt32(values[5]);
            Location = Convert.ToInt32(values[6]);
            Comfort = Convert.ToInt32(values[7]);
            Content = Convert.ToInt32(values[8]);
            Comment = values[9];

            string pictures = values[10];

            string[] delimitedPictures = pictures.Split(" ,");

            if (Pictures == null)
            {
                Pictures = new List<Uri>();
            }

            foreach (string picture in delimitedPictures)
            {
                Uri file = new Uri(picture);
                Pictures.Add(file);
            }
        }


    }
}
