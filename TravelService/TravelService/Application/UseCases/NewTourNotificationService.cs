using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Application.Utils;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.Repository;
using TravelService.WPF.View;

namespace TravelService.Application.UseCases
{
    public class NewTourNotificationService
    {

        private readonly INewTourNotificationRepository _newTourNotificationRepository;

        public readonly Guest2Service _guest2Service;
        public readonly TourRequestService _tourRequestService;
        public NewTourNotificationService(INewTourNotificationRepository newTourNotificationRepository)
        {
            _newTourNotificationRepository = newTourNotificationRepository;
            _guest2Service = new Guest2Service(Injector.CreateInstance<IGuest2Repository>());
            _tourRequestService = new TourRequestService(Injector.CreateInstance<ITourRequestRepository>());
        }
        public void Delete(NewTourNotification newTourNotification)
        {
            _newTourNotificationRepository.Delete(newTourNotification);
        }
        public List<NewTourNotification> GetAll()
        {
            return _newTourNotificationRepository.GetAll();
        }
        public NewTourNotification Save(NewTourNotification newTourNotification)
        {
            NewTourNotification savedNotification = _newTourNotificationRepository.Save(newTourNotification);
            return savedNotification;
        }
        public void Update(NewTourNotification newTourNotification)
        {
            _newTourNotificationRepository.Update(newTourNotification);
        }
        public void SendNotification(int tourId, List<Tour> tours)
        { 
            List<Guest2> guests = new List<Guest2>(_guest2Service.GetAll());
            
            Tour currentTour = tours.Find(tour => tour.Id == tourId);
            foreach(Guest2 guest2 in guests) 
            {
                List<TourRequest> tourRequests = new List<TourRequest>(_tourRequestService.GetAll());
                List<TourRequest> guestsRequests = new List<TourRequest>(_tourRequestService.FindGuestsRequests(tourRequests, guest2.Id));
                List<TourRequest> invalidRequests = new List<TourRequest>(_tourRequestService.GetInvalidRequests(guestsRequests));
                foreach (TourRequest tourRequest in invalidRequests)
                {
                    if (currentTour.LanguageId == tourRequest.LanguageId || currentTour.LocationId == tourRequest.LocationId)
                    {
                        NewTourNotification newTourNotification = new NewTourNotification(tourId, guest2.Id);
                        Save(newTourNotification);
                    }
                }
            }
        }
    }
}
