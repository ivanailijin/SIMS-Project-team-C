using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Serializer;

namespace TravelService.Model
{
    public class CheckPoint :  ISerializable
    {

        public int CheckPointId { get; set; }
        public string Name { get; set; }
        public int TourId { get; set; }




        public CheckPoint() { }
        public CheckPoint(int checkPointId, string name, int tourId)
        {
            CheckPointId = checkPointId;
            Name = name;
            TourId = tourId;

        }

        public override string ToString()
        {
            return CheckPointId + ";" + Name;

        }



        public string[] ToCSV()
        {
            string[] csvValues = { CheckPointId.ToString(), Name, TourId.ToString() };
            return csvValues;
        }



        public void FromCSV(string[] values)
        {
            CheckPointId = Convert.ToInt32(values[0]);
            Name = values[1];
            TourId = Convert.ToInt32(values[2]);

        }
    }
}