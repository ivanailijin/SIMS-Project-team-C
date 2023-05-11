using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Application.Utils;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.Repository;

namespace TravelService.Application.UseCases
{
    public class TourRequestService
    {
        private readonly ITourRequestRepository _tourRequestRepository;
        public readonly LocationService _locationService;
        public readonly LanguageService _languageService;

        public TourRequestService(ITourRequestRepository tourRequestRepository)
        {
            _tourRequestRepository = tourRequestRepository;
            _locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            _languageService = new LanguageService(Injector.CreateInstance<ILanguageRepository>());
        }
        public void Delete(TourRequest tourRequest)
        {
            _tourRequestRepository.Delete(tourRequest);
        }
        public List<TourRequest> GetAll()
        {
            return _tourRequestRepository.GetAll();
        }
        public TourRequest Save(TourRequest tourRequest)
        {
            TourRequest savedTourRequest = _tourRequestRepository.Save(tourRequest);
            return savedTourRequest;
        }
        public void Update(TourRequest tourRequest)
        {
            _tourRequestRepository.Update(tourRequest);
        }
        public void addRequest(Location location,int locationId, string description, Language language, int languageId, int guestNumber, DateTime tourStart, DateTime tourEnd, int guestId)
        { 
            TourRequest tourRequest = new TourRequest(location, locationId, description, language, languageId, guestNumber, tourStart, tourEnd, APPROVAL.WAITING, guestId);
            _tourRequestRepository.Save(tourRequest);
        }

        public List<TourRequest> FindGuestsRequests(List<TourRequest> tourRequests, int guestId)
        {
            List<TourRequest> guestsRequests = new List<TourRequest>();
            List<Location> Locations = new List<Location>(_locationService.GetAll());
            List<Language> Languages = new List<Language>(_languageService.GetAll());
            foreach(TourRequest tourRequest in tourRequests) 
            {
                tourRequest.Location = Locations.Find(loc => loc.Id == tourRequest.LocationId);
                tourRequest.Language = Languages.Find(lan => lan.Id == tourRequest.LanguageId);
                if (guestId == tourRequest.GuestId)
                    guestsRequests.Add(tourRequest);
            }
            return guestsRequests;
        }
    }
}
