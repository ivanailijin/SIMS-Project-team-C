using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelService.Domain.Model
{
    public class LocationDataPoint
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public string LocationCity { get; set; }
        public string LocationCountry { get; set; }
        public double RequestNumber { get; set; }
        public double LocationRequestNumber { get; set; }
        public LocationDataPoint(string location,string city, string country, double requestNumber)
        {
            Location = location;
            LocationCity = city;
            LocationCountry = country;
            RequestNumber = requestNumber;
            LocationRequestNumber = requestNumber;
        }
    }
}
