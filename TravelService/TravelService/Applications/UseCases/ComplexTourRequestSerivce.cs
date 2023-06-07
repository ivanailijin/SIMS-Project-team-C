using System;
using System.Collections.Generic;
using System.Linq;
using TravelService.Applications.Utils;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;

namespace TravelService.Applications.UseCases
{
    public class ComplexTourRequestService
    {
        private readonly IComplexTourRequestRepository _complexTourRequestRepository;

        private readonly ITourRepository _tourRepository;
        private readonly ITourRequestRepository _tourRequestRepository;

        private readonly TourService _tourService;
        private readonly TourRequestService _tourRequestService;


        public ComplexTourRequestService(IComplexTourRequestRepository complexTourRequestRepository)
        {
            _complexTourRequestRepository = complexTourRequestRepository;
            _tourService = new TourService(Injector.CreateInstance<ITourRepository>());
            _tourRequestService = new TourRequestService(Injector.CreateInstance<ITourRequestRepository>());
        }
        public void Delete(ComplexTourRequest tourRequest)
        {
            _complexTourRequestRepository.Delete(tourRequest);
        }
        public List<ComplexTourRequest> GetAll()
        {
            return _complexTourRequestRepository.GetAll();
        }
        public ComplexTourRequest Save(ComplexTourRequest tourRequest)
        {
            ComplexTourRequest savedTourRequest = _complexTourRequestRepository.Save(tourRequest);
            return savedTourRequest;
        }
        public void Update(ComplexTourRequest tourRequest)
        {
            _complexTourRequestRepository.Update(tourRequest);
        }

        public void saveComplexRequest(string name, Guest2 guest2, List<TourRequest> tourRequests)
        {
            ComplexTourRequest complexTourRequest = new ComplexTourRequest(tourRequests, name, APPROVAL.WAITING, guest2);
            Save(complexTourRequest);
        }

        public List<ComplexTourRequest> GetGuestsComplexRequests(int guestId, List<ComplexTourRequest> complexRequests)
        {
            List<ComplexTourRequest> guestsComplexRequests = new List<ComplexTourRequest>();
            foreach (ComplexTourRequest complexTourRequest in complexRequests)
            {
                if (guestId == complexTourRequest.Guest2.Id)
                {
                    guestsComplexRequests.Add(complexTourRequest);
                }
            }
            return guestsComplexRequests;
        }

        public List<ComplexTourRequest> GetComplexRequests(int guestId, List<ComplexTourRequest> complexRequests)
        {
            List<ComplexTourRequest> guestsComplexRequests = new List<ComplexTourRequest>(GetGuestsComplexRequests(guestId, complexRequests));
            List<TourRequest> allTourRequests = new List<TourRequest>(_tourRequestService.GetAll());

            foreach (ComplexTourRequest complexTourRequest in guestsComplexRequests)
            {
                List<int> requestIds = getTourRequestIds(complexTourRequest.TourRequests);
                complexTourRequest.TourRequests.Clear();
                foreach (int id in requestIds)
                {
                    TourRequest foundRequest = allTourRequests.FirstOrDefault(request => request.Id == id);
                    if (foundRequest != null)
                    {
                        complexTourRequest.TourRequests.Add(foundRequest);
                    }
                }
            }
            return guestsComplexRequests;
        }

        public List<TourRequest> FindTourRequests(ComplexTourRequest selectedComplexRequest, int guestId, List<TourRequest> tourRequests)
        {
            List<TourRequest> requests = new List<TourRequest>();
            List<ComplexTourRequest> complexRequests = new List<ComplexTourRequest>(GetAll());
            List<ComplexTourRequest> guestsComplexRequests = new List<ComplexTourRequest>(GetGuestsComplexRequests(guestId, complexRequests));
            foreach (ComplexTourRequest complexTourRequest in guestsComplexRequests)
            {
                if (selectedComplexRequest.Id == complexTourRequest.Id)
                {
                    foreach (TourRequest tourRequest in tourRequests)
                    {
                        if (complexTourRequest.TourRequests.Contains(tourRequest))
                        {
                            requests.Add(tourRequest);
                        }
                    }
                }
            }
            return requests;
        }
        public List<int> getTourRequestIds(List<TourRequest> tourRequests)
        {
            List<int> ids = new List<int>();
            foreach (TourRequest request in tourRequests)
            {
                ids.Add(request.Id);
            }
            return ids;
        }
        public List<ComplexTourRequest> FindValidComplexRequests(int guestId, List<ComplexTourRequest> complexRequests)
        {
            List<ComplexTourRequest> allComplexRequests = new List<ComplexTourRequest>(GetComplexRequests(guestId, complexRequests));
            foreach (ComplexTourRequest complexRequest in allComplexRequests)
            {
                TourRequest firstRequest = complexRequest.TourRequests.FirstOrDefault();
                if (IsFirstRequestValid(firstRequest))
                {
                    bool complexRequestAccepted = CheckAcceptance(complexRequest.TourRequests);
                    if (complexRequestAccepted)
                    {
                        complexRequest.Acceptance = APPROVAL.ACCEPTED;
                        Update(complexRequest);
                    }
                }
                else
                {
                    complexRequest.Acceptance = APPROVAL.INVALID;
                    Update(complexRequest);
                }
            }
            return allComplexRequests;
        }

        private bool IsFirstRequestValid(TourRequest firstRequest)
        {
            TimeSpan timeSpan = firstRequest.TourStart - DateTime.Now;
            if (timeSpan.TotalHours < 48)
            {
                firstRequest.RequestApproved = APPROVAL.INVALID;
                _tourRequestService.Update(firstRequest);
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool CheckAcceptance(List<TourRequest> tourRequests)
        {
            foreach (TourRequest request in tourRequests)
            {
                if (request.RequestApproved == APPROVAL.WAITING || request.RequestApproved == APPROVAL.WAITING)
                {
                    return false;
                }
            }
            return true;
        }

        public void saveComplexRequest(Guest2 guest2, List<TourRequest> tourRequests, string name)
        {
            ComplexTourRequest complexTourRequest = new ComplexTourRequest(tourRequests, name, APPROVAL.WAITING, guest2);
            Save(complexTourRequest);
        }

        public List<ComplexTourRequest> GetComplex(List<ComplexTourRequest> complexRequests)
        {
            List<ComplexTourRequest> guestsComplexRequests = new List<ComplexTourRequest>();
            foreach (ComplexTourRequest complexTourRequest in complexRequests)
            {
                guestsComplexRequests.Add(complexTourRequest);

            }
            return guestsComplexRequests;
        }

      
        public List<ComplexTourRequest> GetGuidesComplexRequests(List<ComplexTourRequest> complexRequests)
        {
            List<ComplexTourRequest> guestsComplexRequests = new List<ComplexTourRequest>(GetComplex(complexRequests));
            List<TourRequest> allTourRequests = new List<TourRequest>(_tourRequestService.GetAll());

            foreach (ComplexTourRequest complexTourRequest in guestsComplexRequests)
            {
                List<int> requestIds = getTourRequestIds(complexTourRequest.TourRequests);
                complexTourRequest.TourRequests.Clear();
                foreach (int id in requestIds)
                {
                    TourRequest foundRequest = allTourRequests.FirstOrDefault(request => request.Id == id);
                    if (foundRequest != null)
                    {
                        complexTourRequest.TourRequests.Add(foundRequest);
                    }
                }
            }
            return guestsComplexRequests;
        }

        public List<TourRequest> FindTourRequests(ComplexTourRequest selectedComplexRequest, List<ComplexTourRequest> complexTourRequests)
        {
            List<TourRequest> requests = new List<TourRequest>();
            foreach (ComplexTourRequest complexTourRequest in complexTourRequests)
            {
                if (selectedComplexRequest.Id == complexTourRequest.Id)
                {
                    foreach (TourRequest tourRequest in complexTourRequest.TourRequests)
                    {
                        if (complexTourRequest.TourRequests.Contains(tourRequest))
                        {
                            requests.Add(tourRequest);
                        }
                    }
                }
            }
            return requests;
        }
       
       


    }
}