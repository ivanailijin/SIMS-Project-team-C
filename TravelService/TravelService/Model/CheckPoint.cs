using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelService.Model
{
    public class CheckPoint
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }
        public int LocationId { get; set; }

        public CheckPoint() { }
        public CheckPoint(int id, string name, Location location, int locationId)
        {
            Id = id;
            Name = name;
            Location = location;
            LocationId = locationId;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Name, LocationId.ToString()};
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            LocationId = Convert.ToInt32(values[2]);
        }
    }
}
