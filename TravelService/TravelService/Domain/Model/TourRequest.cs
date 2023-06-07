using System;
using TravelService.Serializer;

public enum APPROVAL { WAITING, ACCEPTED, INVALID };

namespace TravelService.Domain.Model
{
    public class TourRequest : ISerializable
    {
        public int Id { get; set; }
        public Location Location { get; set; }
        public int LocationId { get; set; }
        public string Description { get; set; }
        public Language Language { get; set; }
        public int LanguageId { get; set; }
        public int GuestNumber { get; set; }
        public DateTime TourStart { get; set; }
        public DateTime TourEnd { get; set; }
        public APPROVAL RequestApproved { get; set; }
        public int GuestId { get; set; }
        


        public TourRequest() { }

        public TourRequest(Location location, int locationId, string description, Language language, int languageId, int guestNumber, DateTime tourStart, DateTime tourEnd, APPROVAL requestApproved, int guestId)
        {
            Location = location;
            LocationId = locationId;
            Description = description;
            Language = language;
            LanguageId = languageId;
            GuestNumber = guestNumber;
            TourStart = tourStart;
            TourEnd = tourEnd;
            RequestApproved = requestApproved;
            GuestId = guestId;
        }
        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                LocationId.ToString(),
                Description,
                LanguageId.ToString(),
                GuestNumber.ToString(),
                TourStart.ToString(),
                TourEnd.ToString(),
                RequestApprovedToCSV(),
                GuestId.ToString()
            };
            return csvValues;
        }
        public string RequestApprovedToCSV()
        {
            if (RequestApproved == APPROVAL.WAITING)
                return "Waiting";
            else if (RequestApproved == APPROVAL.ACCEPTED)
                return "Accepted";
            else
                return "Invalid";
        }
        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            LocationId = int.Parse(values[1]);
            Description = values[2];
            LanguageId = int.Parse(values[3]);
            GuestNumber = int.Parse(values[4]);
            TourStart = DateTime.Parse(values[5]);
            TourEnd = DateTime.Parse(values[6]);
            RequestApproved = RequestApprovedFromCSV(values[7]);
            GuestId = int.Parse(values[8]);
        }
        public APPROVAL RequestApprovedFromCSV(string requestApproved)
        {
            if (string.Equals(requestApproved, "Waiting"))
                return APPROVAL.WAITING;
            else if (string.Equals(requestApproved, "Accepted"))
                return APPROVAL.ACCEPTED;
            else
                return APPROVAL.INVALID;
        }
        public static TourRequest CreateFromCSV(string[] values)
        {
            TourRequest tourRequest = new TourRequest();
            tourRequest.FromCSV(values);
            return tourRequest;
        }
    }

}


