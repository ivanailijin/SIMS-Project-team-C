using System.Collections.Generic;
using System.Text;
using TravelService.Serializer;


namespace TravelService.Domain.Model
{
    public class ComplexTourRequest : ISerializable
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public List<TourRequest> TourRequests { get; set; }
        public APPROVAL Acceptance { get; set; }
        public Guest2 Guest2 { get; set; }


        public ComplexTourRequest()
        {
            TourRequests = new List<TourRequest>();
        }

        public ComplexTourRequest(List<TourRequest> tourRequests, string name, APPROVAL acceptance, Guest2 guest2)


        {
            Name = name;
            TourRequests = new List<TourRequest>(tourRequests);
            Acceptance = acceptance;
            Guest2 = guest2;
        }
        public string[] ToCSV()
        {
            StringBuilder requestList = new StringBuilder();
            foreach (TourRequest tourRequest in TourRequests)
            {
                requestList.Append(tourRequest.Id.ToString());
                requestList.Append(" ,");
            }
            if (requestList.Length > 0)
            {
                requestList.Remove(requestList.Length - 1, 1);
            }
            string[] csvValues =
            {
                Id.ToString(),
                Name,
                requestList.ToString(),
                RequestApprovedToCSV(),
                Guest2.Id.ToString()
            };
            return csvValues;
        }
        public string RequestApprovedToCSV()
        {
            if (Acceptance == APPROVAL.WAITING)
                return "Waiting";
            else if (Acceptance == APPROVAL.ACCEPTED)
                return "Accepted";
            else
                return "Invalid";
        }
        public void FromCSV(string[] values)
        {

            Id = int.Parse(values[0]);
            Name = values[1];

            string[] tourRequestIds = values[2].Split(",");
            TourRequests = new List<TourRequest>();

            foreach (string tourRequestId in tourRequestIds)
            {
                int requestId = int.Parse(tourRequestId.Trim());
                TourRequest tourRequest = new TourRequest { Id = requestId };
                TourRequests.Add(tourRequest);
            }

            Acceptance = RequestApprovedFromCSV(values[3]);
            int guestId = int.Parse(values[4]);
            Guest2 = new Guest2 { Id = guestId };
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
    }
}