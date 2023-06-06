using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TravelService.Serializer;

namespace TravelService.Domain.Model
{
    public class Comment : ISerializable
    {
        public int Id { get; set; }
        public User User { get; set; } = new();
        public Forum Forum { get; set; } = new();
        public string Content { get; set; }
        public int ReportsNumber { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsMarkedComment { get; set; }
        public bool IsOwnersAccommodationOnLocation { get; set; }

        public Comment(User user, Forum forum, string content, DateTime dateCreated, bool isMarkedComment)
        {
            User = user;
            Forum = forum;
            Content = content;
            DateCreated = dateCreated;
            ReportsNumber = 0;
            IsMarkedComment = isMarkedComment;
        }

        public Comment() { }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                User.Id.ToString(),
                Forum.Id.ToString(),
                Content,
                DateCreated.ToString(),
                ReportsNumber.ToString(),
                IsMarkedComment.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            User.Id = Convert.ToInt32(values[1]);
            Forum.Id = Convert.ToInt32(values[2]);
            Content = values[3];
            DateCreated = DateTime.Parse(values[4]);
            ReportsNumber = Convert.ToInt32(values[5]);
            IsMarkedComment = bool.Parse(values[6]);
        }
    }
}
