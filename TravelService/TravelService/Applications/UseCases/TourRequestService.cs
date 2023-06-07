using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TravelService.Applications.Utils;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;

namespace TravelService.Applications.UseCases
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
        public TourRequest addRequest(Location location, int locationId, string description, Language language, int languageId, int guestNumber, DateTime tourStart, DateTime tourEnd, int guestId)
        {
            TourRequest tourRequest = new TourRequest(location, locationId, description, language, languageId, guestNumber, tourStart, tourEnd, APPROVAL.WAITING, guestId);
            _tourRequestRepository.Save(tourRequest);
            return tourRequest;
        }
        public List<int> FindYears(ObservableCollection<TourRequest> guestsRequests)
        {
            List<int> years = new List<int>();
            foreach (TourRequest tourRequest in guestsRequests)
            {
                if (!years.Contains(tourRequest.TourStart.Year))
                    years.Add(tourRequest.TourStart.Year);
            }
            return years;
        }
        public List<TourRequest> FindGuestsRequests(List<TourRequest> tourRequests, int guestId)
        {
            List<TourRequest> guestsRequests = new List<TourRequest>();
            List<Location> Locations = new List<Location>(_locationService.GetAll());
            List<Language> Languages = new List<Language>(_languageService.GetAll());

            foreach (TourRequest tourRequest in tourRequests)
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
            if (timeSpan.TotalHours < 48 && tourRequest.RequestApproved != APPROVAL.ACCEPTED)
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
        public bool Availability(List<Tour> existingTours, DateTime tourStart, DateTime tourEnd)
        {
            for (DateTime date = tourStart.Date; date <= tourEnd.Date; date = date.AddDays(1))
            {
                foreach (var tour in existingTours)
                {
                    if (tour.TourStart.Date <= date && tour.TourEnd.Date >= date)
                    {
                        return false;
                    }
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
                    (IsGuestNumberLessThanMax(tourRequest, guestNumber) || string.IsNullOrEmpty(guestNumber)))

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
            if (string.IsNullOrEmpty(language))
            {
                return false;
            }

            return tourRequest.Language.Name.Equals(language, StringComparison.OrdinalIgnoreCase);
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

        public int GetRequestCountForLocation(string selectedLocation, ObservableCollection<TourRequest> guestsRequests, int selectedYear, int selectedMonth)
        {
            List<TourRequest> tourRequests = new List<TourRequest>(_tourRequestRepository.GetAll());

            tourRequests = GetLocationData(tourRequests);
            int requestCount = guestsRequests.Count(r =>
                r.Location != null &&
                r.Location.CityAndCountry?.Replace(",", "").Replace(" ", "").Equals(selectedLocation, StringComparison.OrdinalIgnoreCase) == true &&
                r.TourStart.Year == selectedYear &&
                (selectedMonth == 0 || r.TourStart.Month == selectedMonth)
            );
            return requestCount;
        }




        public int GetRequestCountForLanguage(Language selectedLanguage, ObservableCollection<TourRequest> guestsRequests, int selectedYear, int selectedMonth)
        {
            List<TourRequest> tourRequests = new List<TourRequest>(_tourRequestRepository.GetAll());

            tourRequests = GetLanguageData(tourRequests);
            int requestCount = guestsRequests.Count(r =>
                r.Language == selectedLanguage &&
                r.TourStart.Year == selectedYear &&
                (selectedMonth == 0 || r.TourStart.Month == selectedMonth)
            );
            return requestCount;

        }
        public List<TourRequest> GetApprovedRequests(ObservableCollection<TourRequest> guestsRequests)
        {
            List<TourRequest> approvedRequests = new List<TourRequest>();
            foreach (TourRequest tourRequest in guestsRequests)
            {
                if (tourRequest.RequestApproved == APPROVAL.ACCEPTED)
                {
                    approvedRequests.Add(tourRequest);
                }
            }
            return approvedRequests;
        }
        public double GetApprovedRequestsPercentage(ObservableCollection<TourRequest> guestsRequests)
        {
            List<TourRequest> approvedRequests = GetApprovedRequests(guestsRequests);
            double approvedRequestNumber = approvedRequests.Count();
            double totalRequestNumber = guestsRequests.Count();
            double approvedRequestsPercentage = (double)approvedRequestNumber / (double)totalRequestNumber * 100;
            return Math.Round(approvedRequestsPercentage, 2);
        }
        public List<TourRequest> GetInvalidRequests(List<TourRequest> guestsRequests)
        {
            List<TourRequest> invalidRequests = new List<TourRequest>();
            foreach (TourRequest tourRequest in guestsRequests)
            {
                if (tourRequest.RequestApproved == APPROVAL.INVALID)
                {
                    invalidRequests.Add(tourRequest);
                }
            }
            return invalidRequests;
        }
        public double GetInvalidRequestsPercentage(ObservableCollection<TourRequest> guestsRequests)
        {
            List<TourRequest> invalidRequests = GetInvalidRequests(guestsRequests.ToList());
            double invalidRequestsNumber = invalidRequests.Count();
            double totalRequestNumber = guestsRequests.Count();
            double invalidRequestsPercentage = (double)invalidRequestsNumber / (double)totalRequestNumber * 100;
            return Math.Round(invalidRequestsPercentage, 2);
        }
        public double GetApprovedRequestsPercentageByYear(ObservableCollection<TourRequest> guestsRequests, int selectedYear)
        {
            List<TourRequest> approvedRequests = GetApprovedRequests(guestsRequests);
            double tourRequestYear = 0;
            double tourRequestCount = 0;
            double percentageByYear = 0;
            double totalRequestCount = guestsRequests.Count();
            foreach (TourRequest tourRequest in approvedRequests)
            {
                tourRequestYear = tourRequest.TourStart.Year;
                if (selectedYear == tourRequestYear)
                    tourRequestCount++;
            }
            percentageByYear = (double)tourRequestCount / (double)totalRequestCount * 100;
            return Math.Round(percentageByYear, 2);
        }
        public double GetInvalidRequestsPercentageByYear(ObservableCollection<TourRequest> guestsRequests, int selectedYear)
        {
            List<TourRequest> invalidRequests = GetInvalidRequests(guestsRequests.ToList());
            double tourRequestYear = 0;
            double tourRequestCount = 0;
            double percentageByYear = 0;
            double totalRequestCount = guestsRequests.Count();
            foreach (TourRequest tourRequest in invalidRequests)
            {
                tourRequestYear = tourRequest.TourStart.Year;
                if (selectedYear == tourRequestYear)
                    tourRequestCount++;
            }
            percentageByYear = (double)tourRequestCount / (double)totalRequestCount * 100;
            return Math.Round(percentageByYear, 2);
        }

        public List<LanguageDataPoint> GetLanguageDataPoints(List<Language> languages, double requestNumber, ObservableCollection<TourRequest> GuestsRequests)
        {
            List<LanguageDataPoint> DataPoints = new List<LanguageDataPoint>();
            foreach (Language language in languages)
            {
                LanguageDataPoint dataPoint = new LanguageDataPoint(language.Name, FindGuestsRequestsPerLanguage(language, languages, GuestsRequests));
                DataPoints.Add(dataPoint);
            }
            return DataPoints;
        }
        public double FindGuestsRequestsPerLanguage(Language Language, List<Language> languages, ObservableCollection<TourRequest> GuestsRequests)
        {
            double requestNumber = 0;
            Language currentLanguage = languages.Find(lan => lan.Id == Language.Id);
            foreach (TourRequest tourRequest in GuestsRequests)
            {
                if (currentLanguage.Id == tourRequest.LanguageId)
                    requestNumber++;
            }
            return requestNumber;
        }
        public List<LanguageDataPoint> CalculateLanguageDataPointPositions(List<LanguageDataPoint> languageDataPoints, ObservableCollection<Language> Languages, ObservableCollection<TourRequest> GuestsRequests)
        {
            List<LanguageDataPoint> DataPoints = new List<LanguageDataPoint>();
            double maxX = 300;
            double maxY = 60;

            for (int i = 0; i < languageDataPoints.Count; i++)
            {
                var dataPoint = languageDataPoints[i];
                double x = CalculateXPosition(dataPoint.Language, maxX, Languages);
                double y = CalculateYPosition(dataPoint.RequestNumber, maxY, GuestsRequests);

                dataPoint.Language = x.ToString();
                dataPoint.RequestNumber = y;
                dataPoint.Id = i;
                DataPoints.Add(dataPoint);
            }
            return DataPoints;
        }
        public double CalculateXPosition(string language, double maxX, ObservableCollection<Language> Languages)
        {
            int languageIndex = Array.IndexOf(Languages.Select(l => l.Name).ToArray(), language);
            double interval = maxX / (Languages.Count - 1);
            double x = (languageIndex) * interval;
            return Math.Round(x, 2);
        }
        public double CalculateYPosition(double requests, double maxY, ObservableCollection<TourRequest> GuestsRequests)
        {
            double y = (double)requests / GuestsRequests.Count * maxY;
            return Math.Round(y, 2);
        }
        public List<LocationDataPoint> GetLocationDataPoints(List<Location> locations, double requestNumber, ObservableCollection<TourRequest> guestsRequests)
        {
            List<LocationDataPoint> DataPoints = new List<LocationDataPoint>();
            foreach (Location location in locations)
            {
                LocationDataPoint dataPoint = new LocationDataPoint(location.CityAndCountry, location.City, location.Country, FindGuestsRequestsPerLocation(location, locations, guestsRequests));
                DataPoints.Add(dataPoint);
            }
            return DataPoints;
        }
        public double FindGuestsRequestsPerLocation(Location Location, List<Location> Locations, ObservableCollection<TourRequest> GuestsRequests)
        {
            double requestNumber = 0;
            Location currentLocation = Locations.Find(loc => loc.Id == Location.Id);
            foreach (TourRequest tourRequest in GuestsRequests)
            {
                if (currentLocation.Id == tourRequest.LocationId)
                    requestNumber++;
            }
            return requestNumber;
        }
        public List<LocationDataPoint> CalculateLocationDataPointPositions(List<LocationDataPoint> locationDataPoints, ObservableCollection<Location> Locations, ObservableCollection<TourRequest> GuestsRequests)
        {
            List<LocationDataPoint> DataPoints = new List<LocationDataPoint>();
            double maxX = 300;
            double maxY = 60;

            for (int i = 0; i < locationDataPoints.Count; i++)
            {
                var dataPoint = locationDataPoints[i];
                double x = CalculateXPositionLocation(dataPoint.Location, maxX, Locations);
                double y = CalculateYPositionLocation(dataPoint.RequestNumber, maxY, GuestsRequests);

                dataPoint.Location = x.ToString();
                dataPoint.RequestNumber = y;
                dataPoint.Id = i;
                DataPoints.Add(dataPoint);
            }
            return DataPoints;
        }
        public double CalculateXPositionLocation(string location, double maxX, ObservableCollection<Location> locations)
        {
            int locationIndex = Array.IndexOf(locations.Select(l => l.CityAndCountry).ToArray(), location);
            double interval = maxX / (locations.Count - 1);
            double x = (locationIndex) * interval;
            return Math.Round(x, 2);
        }

        public double CalculateYPositionLocation(double requests, double maxY, ObservableCollection<TourRequest> GuestsRequests)
        {
            double y = (double)requests / GuestsRequests.Count * maxY;
            return Math.Round(y, 2);
        }

        public double FindAverageGuestNumber(ObservableCollection<TourRequest> guestsRequests)
        {
            List<TourRequest> approvedRequests = GetApprovedRequests(guestsRequests);
            double guestNumberSum = 0;
            double totalRequestNumber = approvedRequests.Count();
            double averageGuestNumber = 0;
            foreach (TourRequest tourRequest in approvedRequests)
            {
                guestNumberSum += tourRequest.GuestNumber;
            }
            averageGuestNumber = (double)guestNumberSum / (double)totalRequestNumber;
            return Math.Round(averageGuestNumber, 2);
        }

        public double GetGuestNumberByYear(ObservableCollection<TourRequest> guestsRequests, int selectedYearGuest)
        {
            List<TourRequest> approvedRequests = GetApprovedRequests(guestsRequests);
            double tourRequestYear = 0;
            double tourRequestCount = 0;
            double guestNumberSum = 0;
            double guestNumberByYear = 0;
            foreach (TourRequest tourRequest in approvedRequests)
            {
                tourRequestYear = tourRequest.TourStart.Year;
                if (selectedYearGuest == tourRequestYear)
                {
                    tourRequestCount++;
                    guestNumberSum += tourRequest.GuestNumber;
                }
            }
            guestNumberByYear = (double)guestNumberSum / (double)tourRequestCount;
            double roundedGuestNumber = Math.Round(guestNumberByYear, 2);
            if (double.IsNaN(roundedGuestNumber))
                roundedGuestNumber = 0;
            return roundedGuestNumber;
        }
        public string GetMostRequestedLocationString(ObservableCollection<TourRequest> guestsRequests, int selectedYear, int selectedMonth)
        {
            Dictionary<int, int> locationCounts = new Dictionary<int, int>();

            foreach (TourRequest request in guestsRequests)
            {
                if (request.TourStart.Year == selectedYear && (selectedMonth == 0 || request.TourStart.Month == selectedMonth))
                {
                    int locationId = request.LocationId;

                    // Fetch the location based on the location ID
                    Location location = _locationService.GetLocationById(locationId);

                    if (location != null)
                    {
                        if (locationCounts.ContainsKey(locationId))
                        {
                            locationCounts[locationId]++;
                        }
                        else
                        {
                            locationCounts[locationId] = 1;
                        }
                    }
                }
            }

            int mostRequestedLocationId = 0;
            int maxRequestCount = 0;

            foreach (KeyValuePair<int, int> kvp in locationCounts)
            {
                if (kvp.Value > maxRequestCount)
                {
                    maxRequestCount = kvp.Value;
                    mostRequestedLocationId = kvp.Key;
                }
            }

            // Fetch the location name based on the most requested location ID
            Location mostRequestedLocation = _locationService.GetLocationById(mostRequestedLocationId);
            string mostRequestedLocationName = mostRequestedLocation?.CityAndCountry;

            return mostRequestedLocationName;
        }

        public string GetMostRequestedLanguageString(ObservableCollection<TourRequest> guestsRequests, int selectedYear, int selectedMonth)
        {
            Dictionary<int, int> languageCounts = new Dictionary<int, int>();

            foreach (TourRequest request in guestsRequests)
            {
                if (request.TourStart.Year == selectedYear && (selectedMonth == 0 || request.TourStart.Month == selectedMonth))
                {
                    int languageId = request.LanguageId;

                    Language language = _languageService.GetById(languageId);

                    if (language != null)
                    {
                        if (languageCounts.ContainsKey(languageId))
                        {
                            languageCounts[languageId]++;
                        }
                        else
                        {
                            languageCounts[languageId] = 1;
                        }
                    }
                }
            }

            int mostRequestedLanguageId = 0;
            int maxRequestCount = 0;

            foreach (KeyValuePair<int, int> kvp in languageCounts)
            {
                if (kvp.Value > maxRequestCount)
                {
                    maxRequestCount = kvp.Value;
                    mostRequestedLanguageId = kvp.Key;
                }
            }

            // Fetch the location name based on the most requested location ID
            Language mostRequestedLanguage = _languageService.GetById(mostRequestedLanguageId);
            string mostRequestedLanguageName = mostRequestedLanguage?.Name;

            return mostRequestedLanguageName;
        }
        public bool IsLocationOrLanguageVisible(bool locationBool, bool languageBool)
        {
            if (locationBool == true || languageBool == true)
            {
                return true;
            }
            return false;
        }
    }
}
