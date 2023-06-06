using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.Repository;
using TravelService.Serializer;
using TravelService.Applications.Utils;
using System.Security.AccessControl;

namespace TravelService.Applications.UseCases
{
    public class AccommodationService
    {
        private readonly IAccommodationRepository _accommodationRepository;
        private readonly LocationService _locationService;
        private readonly OwnerService _ownerService;
        private readonly AccommodationRenovationService _renovationService;

        public AccommodationService(IAccommodationRepository accommodationRepository)
        {
            _accommodationRepository = accommodationRepository;
            _locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            _ownerService = new OwnerService(Injector.CreateInstance<IOwnerRepository>());
        }
        public List<Accommodation> GetAll()
        {
            List<Accommodation> accommodations = _accommodationRepository.GetAll();
            accommodations = GetLocationData(accommodations);
            return accommodations;
        }
        public Accommodation Save(Accommodation accommodation)
        {
            Accommodation savedAccommodation = _accommodationRepository.Save(accommodation);
            return savedAccommodation;
        }

        public void Delete(Accommodation accommodation)
        {
            _accommodationRepository.Delete(accommodation);
        }
        public Accommodation FindById(int id)
        {
            Accommodation accommodation = _accommodationRepository.FindById(id);
            return accommodation;
        }

        public Accommodation Update(Accommodation accommodation)
        {
            Accommodation updatedAccommodation = _accommodationRepository.Update(accommodation);
            return updatedAccommodation;
        }

        public List<Accommodation> GetOwnersAccommodations(int ownerId)
        {
            List<Accommodation> ownersAccommodations = new List<Accommodation>();
            List<Accommodation> accommodations = GetAll();

            foreach (Accommodation accommodation in accommodations)
            {
                if (accommodation.OwnerId == ownerId)
                {
                    ownersAccommodations.Add(accommodation);
                }
            }

            return ownersAccommodations;
        }
        public List<Accommodation> GetOwnerData(List<Accommodation> accommodations)
        {
            List<Owner> owners = _ownerService.GetAll();
            foreach (Accommodation accommodation in accommodations)
            {
                accommodation.Owner = owners.Find(o => o.Id == accommodation.OwnerId);
            }
            return accommodations;
        }

        public List<Accommodation> GetLocationData(List<Accommodation> accommodations)
        {
            List<Location> locations = _locationService.GetAll();
            foreach (Accommodation accommodation in accommodations)
            { 
                accommodation.Location = locations.Find(l => l.Id == accommodation.LocationId);
            }
            return accommodations;
        }

        public List<Accommodation> SortBySuperowner(List<Accommodation> accommodations)
        {
            return accommodations.OrderByDescending(a => a.Owner.SuperOwner).ToList();
        }

        public List<Accommodation> Search(string name, string[] nameWords, string location, string type, string guestNumber, string daysForReservation)
        {
            List<Accommodation> accommodations = new List<Accommodation>(_accommodationRepository.GetAll());
            accommodations = GetLocationData(accommodations);

            List<Accommodation> filteredAccommodations = new List<Accommodation>();

            foreach (Accommodation accommodation in accommodations)
            {
                if ((IsContainingNameWords(accommodation, nameWords) || string.IsNullOrEmpty(name)) &&
                    (HasMatchingLocation(accommodation, location) || string.IsNullOrEmpty(location)) &&
                    (HasMatchingAccommodationType(accommodation, type) || string.IsNullOrEmpty(type)) &&
                    (IsGuestNumberLessThanMaximum(accommodation, guestNumber) || string.IsNullOrEmpty(guestNumber)) &&
                    (IsReservationGreaterThanMinimum(accommodation, daysForReservation) || string.IsNullOrEmpty(daysForReservation)))
                {
                    if (!filteredAccommodations.Contains(accommodation))
                        filteredAccommodations.Add(accommodation);
                }
            }
            return filteredAccommodations;
        }

        public bool IsContainingNameWords(Accommodation accommodation, string[] nameWords)
        {
            bool containsAllWords = false;
            if (nameWords != null)
            {
                containsAllWords = true;
                foreach (string word in nameWords)
                {
                    if (!accommodation.Name.ToLower().Contains(word))
                    {
                        containsAllWords = false;
                        break;
                    }
                }
            }
            return containsAllWords;
        }

        public bool HasMatchingLocation(Accommodation accommodation, string location)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(location))
            {
                result = accommodation.Location.CityAndCountry.Replace(",", "").Replace(" ", "").Contains(location);
            }
            return result;
        }

        public bool HasMatchingAccommodationType(Accommodation accommodation, string type)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(type))
            {
                result = accommodation.Type.ToString().ToLower().Contains(type.ToLower());
            }
            return result;
        }

        public bool IsGuestNumberLessThanMaximum(Accommodation accommodation, string guestNumber)
        {
            bool isLess = false;
            if (int.TryParse(guestNumber, out int parsedGuestNumber) && parsedGuestNumber <= accommodation.MaxGuestNumber)
            {
                isLess = true;
            }
            return isLess;
        }

        public bool IsReservationGreaterThanMinimum(Accommodation accommodation, string daysForReservation)
        {
            bool isGreater = false;
            if (int.TryParse(daysForReservation, out int parsedDaysForReservation) && parsedDaysForReservation >= accommodation.MinReservationDays)
            {
                isGreater = true;
            }
            return isGreater;
        }

        public int GetNumberOfAccommodations(int ownerId)
        {
            List<Accommodation> accommodations = GetAll();
            int numberOfAccommodations = 0;

            foreach(Accommodation a in accommodations)
            {
                if(a.OwnerId == ownerId)
                    numberOfAccommodations++;
            }

            return numberOfAccommodations;
        }

        public List<Accommodation> GetAccommodationsByLocation(Location location)
        {
            List<Accommodation> accommodations = GetAll();
            accommodations = GetLocationData(accommodations);
            List<Accommodation> locationAccommodations = new List<Accommodation>();

            foreach(Accommodation accommodation in accommodations)
            {
                if(accommodation.Location.Country == location.Country && accommodation.Location.City == location.City)
                {
                    locationAccommodations.Add(accommodation);
                }
            }

            return locationAccommodations;
        }

        public List<AccommodationRenovation> GetAccommodationData(List <AccommodationRenovation> renovations)
        {
            List<Accommodation> accommodations = GetAll();

            foreach(AccommodationRenovation renovation in renovations)
            {
                renovation.Accommodation = accommodations.Find(a => a.Id == renovation.AccommodationId);
            }

            return renovations;
        }
    }
}
