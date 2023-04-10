using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Repository;
using TravelService.Serializer;

namespace TravelService.Model
{
    public class Guest : ISerializable
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int CheckPointId { get; set; }
        public int TourId { get; set; }
        public bool Attendence { get; set; }
        public int Age { get; set; }    
        public List<GuestVoucher> VoucherList { get; set; }
        



        public Guest() 
        {
            VoucherList = new List<GuestVoucher>();



        }

        public Guest(string firstName, string lastName, int checkPointId,int tourId, bool attendence, int age)

        {
            FirstName = firstName;
            LastName = lastName;
            CheckPointId = checkPointId;
            TourId = tourId;
            Attendence = attendence;
            Age = age; 
           
            

        }
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), FirstName, LastName, CheckPointId.ToString(),TourId.ToString(), Attendence.ToString(),Age.ToString()};
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            FirstName = values[1];
            LastName = values[2];
            CheckPointId = int.Parse(values[3]);    
            TourId = int.Parse(values[4]);
            Attendence = Boolean.Parse(values[5]);
            Age = int.Parse(values[6]);
        }

    }

}



