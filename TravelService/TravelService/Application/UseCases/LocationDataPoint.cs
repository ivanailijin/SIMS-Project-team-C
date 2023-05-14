using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelService.Application.UseCases
{
    public class LocationDataPoint
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public string LocationName { get; set; }
        public double RequestNumber { get; set; }
        public double LocationRequestNumber { get; set; }
        public LocationDataPoint(string location, double requestNumber)
        {
            Location = location;
            LocationName = location;
            RequestNumber = requestNumber;
            LocationRequestNumber = requestNumber;
        }
    }
}
