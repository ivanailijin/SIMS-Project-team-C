﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Application.Utils;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.Repository;
using TravelService.WPF.ViewModel;
using static TravelService.WPF.ViewModel.GuestsRequestsStatisticsViewModel;

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
            if (timeSpan.TotalHours < 48 && tourRequest.RequestApproved!=APPROVAL.ACCEPTED)
            {
                tourRequest.RequestApproved = APPROVAL.INVALID;
                Update(tourRequest);
            }
        }

        public List<TourRequest> GetApprovedRequests(ObservableCollection<TourRequest> guestsRequests)
        {
            List<TourRequest> approvedRequests = new List<TourRequest>();
            foreach (TourRequest tourRequest in guestsRequests)
            { 
                if(tourRequest.RequestApproved == APPROVAL.ACCEPTED) 
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
        public List<TourRequest> GetInvalidRequests(ObservableCollection<TourRequest> guestsRequests)
        {
            List<TourRequest>  invalidRequests = new List<TourRequest>();
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
            List<TourRequest> invalidRequests = GetInvalidRequests(guestsRequests);
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
            List<TourRequest> invalidRequests = GetInvalidRequests(guestsRequests);
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
            return Math.Round(percentageByYear,2);
        }

        public List<LanguageDataPoint> GetLanguageDataPoints(List<Language> languages, double requestNumber, ObservableCollection<TourRequest> GuestsRequests)
        {
            List<LanguageDataPoint> DataPoints = new List<LanguageDataPoint>();
            foreach (Language language in languages)
            {
                LanguageDataPoint dataPoint = new LanguageDataPoint(language.Name, FindGuestsRequestsPerLanguage(language,languages,GuestsRequests));
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
            double maxY = 50;

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
                LocationDataPoint dataPoint = new LocationDataPoint(location.CityAndCountry, FindGuestsRequestsPerLocation(location, locations, guestsRequests));
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
            double maxY = 50;

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
            averageGuestNumber = (double)guestNumberSum/(double)totalRequestNumber;
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
            double roundedGuestNumber =Math.Round(guestNumberByYear, 2);
            if(double.IsNaN(roundedGuestNumber))
                roundedGuestNumber = 0;
            return roundedGuestNumber;
        }
    }
}
