using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Serializer;

namespace TravelService.Domain.Model
{
    public class Guide : User, ISerializable
    {
        public bool SuperGuide { get; set; }
        public List<Tour> Tours { get; set; }
        public Uri ProfilePicture { get; set; }
        public Guide()
        {
            Tours = new List<Tour>();
        }

        public Guide(string username, string password, string userType)
        {
            Username = username;
            Password = password;
            UserType = userType;
            Tours = new List<Tour>();

        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Username, Password, UserType,SuperGuide.ToString(), ProfilePicture.ToString()};
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Username = values[1];
            Password = values[2];
            UserType = values[3];
            SuperGuide = Boolean.Parse(values[4]);
            string profilePicture = values[5];
            ProfilePicture = new Uri(profilePicture);
        }
    }
}
