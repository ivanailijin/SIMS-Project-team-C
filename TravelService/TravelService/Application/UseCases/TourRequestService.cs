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

        private readonly ITourRepository _tourRepository;
        private readonly TourService _tourService;


        public TourRequestService(ITourRequestRepository tourRequestRepository)
        {
            _tourRequestRepository = tourRequestRepository;
            _locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());

            _tourService = new TourService(Injector.CreateInstance<ITourRepository>());

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
                isRequestValid(tourRequest);
                tourRequest.Location = Locations.Find(loc => loc.Id == tourRequest.LocationId);
                tourRequest.Language = Languages.Find(lan => lan.Id == tourRequest.LanguageId);
                if (guestId == tourRequest.GuestId)
                    guestsRequests.Add(tourRequest);
            }
            return guestsRequests;
        }
        public void isRequestValid(TourRequest tourRequest) 
        {
            TimeSpan timeSpan = tourRequest.TourStart - DateTime.Now;
            if (timeSpan.TotalHours < 48)
            {
                tourRequest.RequestApproved = APPROVAL.INVALID;
                Update(tourRequest);
            }
        }
    



        public void ShowTourRequests(List<TourRequest> Requests, List<Location> Locations, List<Language> Languages)
        { 
            foreach (TourRequest tourRequests in Requests)
            {
                tourRequests.Location = Locations.Find(loc => loc.Id == tourRequests.LocationId);
                tourRequests.Language = Languages.Find(lan => lan.Id == tourRequests.LanguageId);

            }

        }
      

        public List<TourRequest> FindTourRequestsByDate(DateTime startDate, DateTime endDate)
        {
            List<TourRequest> tourRequests = _tourRequestRepository.GetAll();
            List<TourRequest> filteredTourRequests = new List<TourRequest>();

            foreach (TourRequest tourRequest in tourRequests)
            {
                if (tourRequest.TourStart >= startDate && tourRequest.TourEnd <= endDate)
                {
                    filteredTourRequests.Add(tourRequest);
                }
            }

            return filteredTourRequests;
        }

        public bool AvailabilityDate(List<Tour> existingTours, DateTime selectedDate)
        {
            foreach (var tour in existingTours)
            {
                if (tour.TourStart.Date == selectedDate.Date)
                {
                    return false;
                }
            }
            return true;
        }



        public List<TourRequest> GetLocationData(List<TourRequest> tourRequests)
        {
            List<Location> locations = _locationService.GetAll();
            foreach (TourRequest tourRequest in tourRequests)
            {
                if (tourRequest.LocationId != null)
                {
                    tourRequest.Location = locations.Find(l => l.Id == tourRequest.LocationId);

                }
            }
            return tourRequests;
        }

        public List<TourRequest> GetLanguageData(List<TourRequest> tourRequests)
        {
            List<Language> languages = _languageService.GetAll();
            foreach (TourRequest tourRequest in tourRequests)
            {
                tourRequest.Language = languages.Find(l => l.Id == tourRequest.LanguageId);
            }
            return tourRequests;
        }

        public bool isTourSearchable(TourRequest tourRequest, string inputLocation, string inputLanguage, string inputGuestNumber)
        {
            if (((tourRequest.Location.CityAndCountry.Replace(",", "").Replace(" ", "")).Contains(inputLocation) || string.IsNullOrEmpty(inputLocation)) &&
                (tourRequest.Language.Name.Contains(inputLanguage) || string.IsNullOrEmpty(inputLanguage)) &&
                (IsGuestNumberLessThanMax(tourRequest, inputGuestNumber) || string.IsNullOrEmpty(inputGuestNumber))
                )
            {
                return true;
            }
            return false;
        }
        public List<TourRequest> Search(string location, string language, string guestNumber)
        {
            List<TourRequest> tourRequests = new List<TourRequest>(_tourRequestRepository.GetAll());
            tourRequests = GetLocationData(tourRequests);
            tourRequests = GetLanguageData(tourRequests);


            List<TourRequest> filteredTourRequests = new List<TourRequest>();

            foreach (TourRequest tourRequest in tourRequests)
            {
                if (
                    (HasMatchingLocation(tourRequest, location) || string.IsNullOrEmpty(location)) &&
                    (HasMatchingLanguage(tourRequest, language) || string.IsNullOrEmpty(language)) &&
                    (IsGuestNumberLessThanMax(tourRequest, guestNumber) || string.IsNullOrEmpty(guestNumber)) )
                    
                {
                    if (!filteredTourRequests.Contains(tourRequest))
                        filteredTourRequests.Add(tourRequest);
                }
            }
            return filteredTourRequests;
        }

        public bool HasMatchingLocation(TourRequest tourRequest, string location)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(location))
            {
                result = tourRequest.Location.CityAndCountry.Replace(",", "").Replace(" ", "").Contains(location);
            }
            return result;
        }
        public bool HasMatchingLanguage(TourRequest tourRequest, string language)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(language))
            {
                result = tourRequest.Language.Name.Replace(",", "").Replace(" ", "").Contains(language);
            }
            return result;
        }
        private bool IsGuestNumberLessThanMax(TourRequest tourRequest, string inputGuestNumber)
        {
            bool isLess = false;
            if (int.TryParse(inputGuestNumber, out int parsedGuestNumber) && parsedGuestNumber <= tourRequest.GuestNumber)
            {
                isLess = true;
            }
            return isLess;
        }

    
    }
}
