using System;
using TravelService.Serializer;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace TravelService.Domain.Model
{
    public class User : ISerializable
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }
        public Uri Picture { get; set; }

        public User() { }

        public User(string username, string password, string userType)
        {
            Username = username;
            Password = password;
            UserType = userType;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Username, Password, UserType, Picture.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Username = values[1];
            Password = values[2];
            UserType = values[3];
            string profilePicture = values[4];
            Picture = new Uri(profilePicture);
        }
    }
}
