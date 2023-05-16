using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TravelService.Serializer;
public enum STATUS {Approved, Rejected, OnHold};
public enum AVAILABILITY {Available, Unavailable};

namespace TravelService.Domain.Model
{
    public class ReservationRequest : ISerializable
    {
        public int Id { get; set; }
        public int GuestId { get; set; }
        public Guest1 Guest { get; set; }
        public int ReservationId { get; set; }
        public AccommodationReservation Reservation { get; set; }
        public DateTime NewStartDate { get; set; }
        public DateTime NewEndDate { get; set; }
        public STATUS Status { get; set; }
        public String Comment { get; set; }
        public AVAILABILITY Availability { get; set; }

        public ReservationRequest() { }

        public ReservationRequest(int guestId, int reservationId, DateTime newStartDate, DateTime newEndDate )
        {
            GuestId = guestId;
            ReservationId = reservationId;
            NewStartDate = newStartDate;
            NewEndDate = newEndDate;
            Status = STATUS.OnHold;
            Availability = AVAILABILITY.Available;
            Comment = "";
        }

        public string StatusToCSV()
        {
            if (Status == STATUS.Approved)
                return "Approved";
            else if (Status == STATUS.Rejected)
                return "Rejected";
            else
                return "OnHold";
        }

        public STATUS StatusFromCSV(string status)
        {
            if (string.Equals(status, "Approved"))
                return STATUS.Approved;
            else if (string.Equals(status, "Rejected"))
                return STATUS.Rejected;
            else
                return STATUS.OnHold;
        }
        public string AvailabilityToCSV()
        {
            if (Availability == AVAILABILITY.Available)
                return "Available";
            else
                return "Unavailable";
        }
        public AVAILABILITY AvailabilityFromCSV(string availability)
        {
            if (string.Equals(availability, "Available"))
                return AVAILABILITY.Available;
            else
                return AVAILABILITY.Unavailable;
        }
        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                GuestId.ToString(),
                ReservationId.ToString(),
                NewStartDate.ToString(),
                NewEndDate.ToString(),
                StatusToCSV(),
                AvailabilityToCSV(),
                Comment,
            };
            return csvValues;
        }


        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            GuestId = Convert.ToInt32(values[1]);
            ReservationId = Convert.ToInt32(values[2]);
            NewStartDate = DateTime.Parse(values[3]);
            NewEndDate = DateTime.Parse(values[4]);
            Status = StatusFromCSV(values[5]);
            Availability = AvailabilityFromCSV(values[6]);
            Comment = values[7];
        }
    }
}