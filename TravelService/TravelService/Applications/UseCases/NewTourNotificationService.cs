using System;
using System.Collections.Generic;
using TravelService.Applications.Utils;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;

namespace TravelService.Applications.UseCases
{
    public class NewTourNotificationService
    {

        private readonly INewTourNotificationRepository _newTourNotificationRepository;

        public readonly Guest2Service _guest2Service;
        public readonly TourRequestService _tourRequestService;
        public readonly TourService _tourService;
        public NewTourNotificationService(INewTourNotificationRepository newTourNotificationRepository)
        {
            _newTourNotificationRepository = newTourNotificationRepository;
            _guest2Service = new Guest2Service(Injector.CreateInstance<IGuest2Repository>());
            _tourRequestService = new TourRequestService(Injector.CreateInstance<ITourRequestRepository>());
            _tourService = new TourService(Injector.CreateInstance<ITourRepository>());
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
        public void SendNotification(int tourId)
        {
            List<Guest2> guests = new List<Guest2>(_guest2Service.GetAll());
            List<Tour> Tours = new List<Tour>(_tourService.GetAll());
            Tour currentTour = Tours.Find(tour => tour.Id == tourId);
            foreach(Guest2 guest2 in guests)
            {
                List<TourRequest> tourRequests = new List<TourRequest>(_tourRequestService.GetAll());
                List<TourRequest> guestsRequests = new List<TourRequest>(_tourRequestService.FindGuestsRequests(tourRequests, guest2.Id));
                List<TourRequest> invalidRequests = new List<TourRequest>(_tourRequestService.GetInvalidRequests(guestsRequests));
                foreach (TourRequest tourRequest in invalidRequests)
                {
                    if (currentTour.LanguageId == tourRequest.LanguageId || currentTour.LocationId == tourRequest.LocationId)
                    {
                        string description = "Nova tura je kreirana";


                        NewTourNotification newTourNotification = new NewTourNotification(tourId, guest2.Id, description, DateTime.Now);

                        Save(newTourNotification);
                    }
                }
            }

        }
        public List<NewTourNotification> GetGuestsNotifications(List<NewTourNotification> notifications, Guest2 guest2)
        {
            List<NewTourNotification> guestsNotifications = new List<NewTourNotification>();
            foreach (NewTourNotification notification in notifications)
            {
                if (guest2.Id == notification.GuestId)
                    guestsNotifications.Add(notification);
            }
            return guestsNotifications;
        }
        public Tour FindTourById(int tourId, List<Tour> Tours)
        {
            Tour tour = Tours.Find(tour => tour.Id == tourId);
            return tour;
        }


        public List<Tour> ShowTourList(List<Location> locations, List<Language> languages, List<CheckPoint> checkPoints, List<Tour> Tours, Tour tour)

        {
            List<Tour> tours = new List<Tour>();
            tour.Location = locations.Find(loc => loc.Id == tour.LocationId);
            tour.Language = languages.Find(lan => lan.Id == tour.LanguageId);
            _tourService.ShowCheckPointList(tour.Id, Tours, checkPoints);
            tours.Add(tour);
            return tours;
        }


        public void TourRequestAcceptedNotification(TourRequest selectedTourRequest)
        {
            List<Guest2> guests = new List<Guest2>(_guest2Service.GetAll());

            foreach (Guest2 guest2 in guests)
            {
                if (selectedTourRequest.GuestId == guest2.Id)
                {
                    string description = "Tura je prihvaćena";
                    NewTourNotification newTourNotification = new NewTourNotification(0, guest2.Id, description, selectedTourRequest.TourStart);
                    Save(newTourNotification);
                    break;
                }
            }
        }



    }

}


