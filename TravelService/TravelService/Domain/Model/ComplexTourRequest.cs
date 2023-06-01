using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Serializer;
using TravelService.WPF.View;


namespace TravelService.Domain.Model
{
    public class ComplexTourRequest : ISerializable
    {
        public int Id { get; set; }
        public List<TourRequest> TourRequests { get; set; }
        public APPROVAL Acceptance { get; set; }
        public ComplexTourRequest()
        {
            TourRequests = new List<TourRequest>();
        }

        public ComplexTourRequest(int id, APPROVAL acceptance)
        {
            Id = id;
            TourRequests = new List<TourRequest>();
            Acceptance = acceptance;
        }
        public string[] ToCSV()
        {
            StringBuilder requestList = new StringBuilder();
            foreach (TourRequest tourRequest in TourRequests)
            {
                requestList.Append(tourRequest.Id.ToString());
                requestList.Append(" ,");
            }
            string[] csvValues =
            {
                Id.ToString(),
                RequestApprovedToCSV(),
                requestList.ToString().TrimEnd(',')
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
            Acceptance = RequestApprovedFromCSV(values[1]);
            TourRequests = new List<TourRequest>();

            for (int i = 2; i < values.Length; i++)
            {
                string[] tourValues = values[i].Split(",");
                TourRequest tourRequest = new TourRequest();
                tourRequest.FromCSV(tourValues);
                TourRequests.Add(tourRequest);
            }
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
