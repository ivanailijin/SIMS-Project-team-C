using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using TravelService.Serializer;
using static System.Net.Mime.MediaTypeNames;

public enum TYPE { APARTMENT, HOUSE, COTTAGE };

namespace TravelService.Domain.Model
{
    public class Accommodation : ISerializable
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public Owner Owner { get; set; } 
        public string Name { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public TYPE Type { get; set; }
        public int MaxGuestNumber { get; set; }
        public int MinReservationDays { get; set; }
        public int DaysBeforeCancellingReservation { get; set; }
        public bool RecentlyRenovated { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastRenovation { get; set; }
        public List<Uri> Pictures { get; set; }


        public Accommodation()
        {
            Pictures = new List<Uri>();
        }

        public Accommodation(int ownerId, string name, int locationId, TYPE type, int maxGuestNumber, int minReservationDays, int daysBeforeCancellingReservation,DateTime dateCreated, List<string> pictures)
        {
            OwnerId = ownerId;
            Name = name;
            LocationId = locationId;
            Type = type;
            MaxGuestNumber = maxGuestNumber;
            MinReservationDays = minReservationDays;
            DaysBeforeCancellingReservation = daysBeforeCancellingReservation;
            DateCreated = dateCreated;
            Pictures = new List<Uri>();

            foreach (string picture in pictures)
            {
                Uri file = new Uri(picture);
                Pictures.Add(file);
            }
        }

        public string TypeToCSV()
        {
            if (Type == TYPE.APARTMENT)
                return "Apartment";
            else if (Type == TYPE.HOUSE)
                return "House";
            else
                return "Cottage";
        }

        public TYPE TypeFromCSV(string type)
        {
            if (string.Equals(type, "Apartment"))
                return TYPE.APARTMENT;
            else if (string.Equals(type, "Cottage"))
                return TYPE.COTTAGE;
            else
                return TYPE.HOUSE;
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
                OwnerId.ToString(),
                Name,
                LocationId.ToString(),
                TypeToCSV(),
                MaxGuestNumber.ToString(),
                MinReservationDays.ToString(),
                DaysBeforeCancellingReservation.ToString(),
                RecentlyRenovated.ToString(),
                DateCreated.ToString(),
                pictureList.ToString(),
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            OwnerId = Convert.ToInt32(values[1]);
            Name = values[2];
            LocationId = Convert.ToInt32(values[3]);
            Type = TypeFromCSV(values[4]);
            MaxGuestNumber = Convert.ToInt32(values[5]);
            MinReservationDays = Convert.ToInt32(values[6]);
            DaysBeforeCancellingReservation = Convert.ToInt32(values[7]);
            RecentlyRenovated = bool.Parse(values[8]);
            DateCreated = DateTime.Parse(values[9]);

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