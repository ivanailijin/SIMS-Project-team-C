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
        private readonly TourService _tourService;


        public ComplexTourRequestService(IComplexTourRequestRepository complexTourRequestRepository)
        {
            _complexTourRequestRepository = complexTourRequestRepository;
            _tourService = new TourService(Injector.CreateInstance<ITourRepository>());
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
    }
}
