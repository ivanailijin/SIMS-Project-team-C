using System;
using System.Collections.Generic;
using TravelService.Serializer;

namespace TravelService.Model
{
    public class Guest : ISerializable
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public int CheckPointId { get; set; }
        public int TourId { get; set; }
        public bool Attendence { get; set; }
        public List<GuestVoucher> VoucherList { get; set; }
        public Guest() { }
        public Guest(string username, int checkPointId, int tourId, bool attendence)
        {
            Username = username;
            CheckPointId = checkPointId;
            TourId = tourId;
            Attendence = attendence; 
        }
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Username, CheckPointId.ToString(), TourId.ToString(), Attendence.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Username = values[1];
            CheckPointId = int.Parse(values[2]);
            TourId = int.Parse(values[3]);
            Attendence = Boolean.Parse(values[4]);
        }
    }
}