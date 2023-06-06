using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Repository;
using TravelService.Serializer;

namespace TravelService.Domain.Model
{
    public class Guest : ISerializable
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public int CheckPointId { get; set; }
      public string CheckPointName { get; set; }
        public string TourName { get; set; }    
        public int TourId { get; set; }
        public Tour Tour { get; set; }
        public bool Attendence { get; set; }
        public int Age { get; set; }
        public List<GuestVoucher> VoucherList { get; set; }
        public Guest()
        {
            VoucherList = new List<GuestVoucher>();
        }
        public Guest(string username, int checkPointId, int tourId, bool attendence, int age)
        {
            Username = username;
            CheckPointId = checkPointId;
            TourId = tourId;
            Attendence = attendence;
            Age = age;
        }
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Username, CheckPointId.ToString(), CheckPointName, TourId.ToString(),TourName, Attendence.ToString(), Age.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Username = values[1];
            CheckPointId = int.Parse(values[2]);
            CheckPointName = values[3];
            
            TourId = int.Parse(values[4]);
            TourName = values[5];   
            Attendence = bool.Parse(values[6]);
            Age = int.Parse(values[7]);
        }
    }
}
