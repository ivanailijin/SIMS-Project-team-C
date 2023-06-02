using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TravelService.Application.Utils;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.Repository;

namespace TravelService.Application.UseCases
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

        public void saveComplexRequest(Guest2 guest2,List<TourRequest> tourRequests)
        {
            ComplexTourRequest complexTourRequest = new ComplexTourRequest(tourRequests,APPROVAL.WAITING, guest2);
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

        public List<TourRequest> GetTourRequests(List<ComplexTourRequest> guestsComplexRequests)
        {
            List<TourRequest> requests = new List<TourRequest>();
            List<TourRequest> tourRequests = new List<TourRequest>(_tourRequestService.GetAll());
            foreach (ComplexTourRequest complexTourRequest in guestsComplexRequests)
            {
                foreach (TourRequest tourRequest in tourRequests)
                {
                    if (complexTourRequest.TourRequests.Contains(tourRequest))
                    {
                        TourRequest currentRequest = tourRequests.Find(request => request.Id == tourRequest.Id);
                        requests.Add(currentRequest);
                    }
                }
            }
            return requests;
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
    }
}
