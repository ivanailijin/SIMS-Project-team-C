using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Application.Utils;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;

namespace TravelService.Application.UseCases
{
    public class TourRequestService
    {
        private readonly ITourRequestRepository _tourRequestRepository;
        private readonly ITourRepository _tourRepository;
        private readonly TourService _tourService;
        private readonly LocationService _locationService;
        private readonly LanguageService _languageService;

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

        public void ShowTourRequests(List<TourRequest> Requests, List<Location> Locations, List<Language> Languages)
        { 
            foreach (TourRequest tourRequests in Requests)
            {
                tourRequests.Location = Locations.Find(loc => loc.Id == tourRequests.LocationId);
                tourRequests.Language = Languages.Find(lan => lan.Id == tourRequests.LanguageId);

            }

        }
        /*  public List<DateTime> FindReservedDates(TourRequest tourRequest)
          {
              List<Tour> tours = _tourService.GetAll();
              List<DateTime> reservedDates = new List<DateTime>();

              foreach (Tour tour in tours)
              {
                  if (tourRequest.Id == tour.Id )
                  {
                      DateTime checkIn = tour.TourStart;
                      DateTime checkOut = tour.TourEnd;

                      for (DateTime currentDate = checkIn; currentDate <= checkOut; currentDate = currentDate.AddDays(1))
                      {
                          reservedDates.Add(currentDate);
                      }
                  }
              }
              return reservedDates;
          }*/

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



        /*  public List<Tuple<DateTime, DateTime>> FindAvailableDates(TourRequest selectedTourRequest, DateTime startDate, DateTime endDate,  int guideId)
          {
             // List<DateTime> reservedDates = FindReservedDates(selectedTourRequest);
              List<DateTime> availableDates = new List<DateTime>();
              List<Tuple<DateTime, DateTime>> availableDatesPair = new List<Tuple<DateTime, DateTime>>();

              for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
              {
                 /* if (!reservedDates.Contains(date))
                  {
                      availableDates.Add(date);
                  }
                  else
                  {
                      availableDates.Clear();
                  }

                  if ( IsGuideAvailable(availableDates[0].Date, availableDates[availableDates.Count - 1].Date, guideId))
                  {
                      availableDatesPair.Add(Tuple.Create(availableDates[0].Date, availableDates[availableDates.Count - 1].Date));
                      availableDates.RemoveAt(0);
                  }
              }
              return availableDatesPair;
          }


          public bool IsGuideAvailable(DateTime startDate, DateTime endDate, int guideId)
          {
              List<Tour> tours = _tourService.GetAll();
              foreach (Tour tour in tours)
              {
                  if (tour.GuideId == guideId && tour.TourStart <= endDate && tour.TourStart >= startDate)
                  {
                      return false;
                  }
              }
              return true;
          }
        */
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