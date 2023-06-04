﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using TravelService.Serializer;

public enum FORUMSTATUS { Open, Closed }

namespace TravelService.Domain.Model
{
    public class Forum : ISerializable
    {
        public int Id { get; set; }
        public User User { get; set; } = new();
        public string Name { get; set; }
        public int NumberOfComments { get; set; }
        public Location Location { get; set; } = new();
        public DateTime DateCreated { get; set; }
        public FORUMSTATUS Status { get; set; }
        public List<Comment> Comments { get; set; }

        public Forum(User user, string name, Location location, DateTime dateCreated, FORUMSTATUS status, List<Comment> comments)
        {
            User = user;
            Name = name;
            Location = location;
            DateCreated = dateCreated;
            Status = status;
            Comments = comments;
        }

        public Forum()  
        {
            Comments = new List<Comment>();
        }

        public string StatusToCSV()
        {
            if (Status == FORUMSTATUS.Open)
                return "Open";
            else
                return "Closed";
        }

        public FORUMSTATUS StatusFromCSV(string type)
        {
            if (string.Equals(type, "Open"))
                return FORUMSTATUS.Open;
            else
                return FORUMSTATUS.Closed;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                User.Id.ToString(),
                Name,
                Location.Id.ToString(),
                DateCreated.ToString(),
                StatusToCSV(),
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            User.Id = Convert.ToInt32(values[1]);
            Name = values[2];
            Location.Id = Convert.ToInt32(values[3]);
            DateCreated = Convert.ToDateTime(values[4]);
            Status = StatusFromCSV(values[5]);
        }
    }
}
