using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace TravelService.Domain.Model
{
    public class Notification
    {
        public string UserName { get; set; } 
        public Location Location { get; set; } = new();
        public DateOnly Date { get; set; }
        public bool ForumNotification { get; set; }
        public string AccommodationName { get; set; }
        public string Content { get; set; }
        

        public Notification(string userName, Location location, DateOnly date, bool forumNotification, string accommodationName)
        {
            UserName = userName;
            Location = location;
            Date = date;
            ForumNotification = forumNotification;
            AccommodationName = accommodationName;
        }
    }
}
