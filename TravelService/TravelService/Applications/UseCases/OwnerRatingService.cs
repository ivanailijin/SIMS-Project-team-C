using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Applications.Utils;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.Serializer;
using System.Security.RightsManagement;

namespace TravelService.Applications.UseCases
{
    public class OwnerRatingService
    {
        private readonly IOwnerRatingRepository _ownerRatingRepository;
        private readonly Guest1Service _guestService;
        private readonly AccommodationService _accommodationService;

        public OwnerRatingService(IOwnerRatingRepository ownerRatingRepository)
        {
            _ownerRatingRepository = ownerRatingRepository;
            _guestService = new Guest1Service(Injector.CreateInstance<IGuest1Repository>());
            _accommodationService = new AccommodationService(Injector.CreateInstance<IAccommodationRepository>());
        }

        public List<OwnerRating> GetAll()
        {
            List<OwnerRating> ownerRatings = _ownerRatingRepository.GetAll();
            ownerRatings = GetGuestData(ownerRatings);
            return GetAccommodationData(ownerRatings);
        }

        public OwnerRating Save(OwnerRating ownerRating)
        {
            return _ownerRatingRepository.Save(ownerRating);
        }

        public int NextId()
        {
            return _ownerRatingRepository.NextId();
        }
        public List<OwnerRating> GetGuestData(List<OwnerRating> ownerRatings)
        {
            List<Guest1> guests = _guestService.GetAll();

            foreach (OwnerRating ownerRating in ownerRatings)
            {
                ownerRating.Guest = guests.Find(g => g.Id == ownerRating.GuestId);
            }

            return ownerRatings;
        }
        public List<OwnerRating> GetAccommodationData(List<OwnerRating> ownerRatings)
        {
            List<Accommodation> accommodations = _accommodationService.GetAll();

            foreach(OwnerRating ownerRating in ownerRatings)
            {
                ownerRating.Accommodation = accommodations.Find(a => a.Id == ownerRating.AccommodationId);
            }

            return ownerRatings;
        }
        public int GetNumberOfRatings(int ownerId)
        {
            int ratingCount = 0;
            List<OwnerRating> ownerRatings = GetAll();

            foreach (OwnerRating rating in ownerRatings)
            {
                if (rating.OwnerId == ownerId)
                {
                    ratingCount++;
                }
            }
            return ratingCount;
        }
        public int GetNumberOfAccommodationRatings(Accommodation accommodation)
        {
            int ratingCount = 0;
            List<OwnerRating> ownerRatings = GetAll();

            foreach (OwnerRating rating in ownerRatings)
            {
                if (rating.AccommodationId == accommodation.Id)
                {
                    ratingCount++;
                }
            }
            return ratingCount;
        }
        public double GetAverageCleanliness(Accommodation accommodation)
        {
            double sum = 0;
            double count = 0;

            List<OwnerRating> ownerRatings = GetAll();

            foreach(OwnerRating rating in ownerRatings)
            {
                if(rating.AccommodationId == accommodation.Id)
                {
                    sum += rating.Cleanliness;
                    count++;
                }
            }
            return sum / count;
        }
        public double GetAverageCorrectness(Accommodation accommodation)
        {
            double sum = 0;
            double count = 0;

            List<OwnerRating> ownerRatings = GetAll();

            foreach (OwnerRating rating in ownerRatings)
            {
                if (rating.AccommodationId == accommodation.Id)
                {
                    sum += rating.Correctness;
                    count++;
                }
            }
            return sum / count;
        }
        public double GetAverageLocation(Accommodation accommodation)
        {
            double sum = 0;
            double count = 0;

            List<OwnerRating> ownerRatings = GetAll();

            foreach (OwnerRating rating in ownerRatings)
            {
                if (rating.AccommodationId == accommodation.Id)
                {
                    sum += rating.Location;
                    count++;
                }
            }
            return sum / count;
        }
        public double GetAverageComfort(Accommodation accommodation)
        {
            double sum = 0;
            double count = 0;

            List<OwnerRating> ownerRatings = GetAll();

            foreach (OwnerRating rating in ownerRatings)
            {
                if (rating.AccommodationId == accommodation.Id)
                {
                    sum += rating.Comfort;
                    count++;
                }
            }
            return sum / count;
        }
        public double GetAverageContent(Accommodation accommodation)
        {
            double sum = 0;
            double count = 0;

            List<OwnerRating> ownerRatings = GetAll();

            foreach (OwnerRating rating in ownerRatings)
            {
                if (rating.AccommodationId == accommodation.Id)
                {
                    sum += rating.Content;
                    count++;
                }
            }
            return sum / count;
        }
        public List<Uri> GetRatingImages(Accommodation accommodation)
        {
            List<Uri> ratingImages = new List<Uri>();
            List<OwnerRating> ownerRatings = GetAll();

            foreach (OwnerRating rating in ownerRatings)
            {
                if (rating.AccommodationId == accommodation.Id)
                {
                    foreach(Uri picture in rating.Pictures)
                    {
                        ratingImages.Add(picture);
                    }
                }
            }
            return ratingImages;
        }
        public double GetAverageAccommodationRating(Accommodation accommodation)
        {
            return (double)(GetAverageCleanliness(accommodation) + GetAverageComfort(accommodation) + GetAverageCorrectness(accommodation)
                            + GetAverageLocation(accommodation) + GetAverageContent(accommodation)) / 5;
        }
        public OwnerRating FindByGuestId(Guest1 guest)
        {
            List<OwnerRating> ownerRatings = GetAll();
            foreach(OwnerRating rating in ownerRatings)
            {
                if(rating.GuestId == guest.Id)
                {
                    return rating;
                }
            }
            return null;
        }
        public List<OwnerRating> GetRatingsByGuests(List<Guest1> guests)
        {
            List<OwnerRating> ownerRatings = new List<OwnerRating>();

            foreach(Guest1 guest in guests)
            {
                OwnerRating ownerRating = FindByGuestId(guest);
                ownerRating.AverageRating = (double)(ownerRating.Cleanliness + ownerRating.Correctness +
                                            ownerRating.Comfort + ownerRating.Content + ownerRating.Location) / 5;
                ownerRatings.Add(ownerRating);
            }

            return ownerRatings;
        }
        public List<OwnerRating> GetFirstTenRatings()
        {
            List<OwnerRating> ratings = _ownerRatingRepository.GetAll();
            List<OwnerRating> firstRatings = new List<OwnerRating>();
            for(int i = 0; i < 10; i++)
            {
                ratings[i].Guest = _guestService.FindById(ratings[i].GuestId);
                ratings[i].AverageRating = (double)(ratings[i].Cleanliness + ratings[i].Correctness +
                                            ratings[i].Comfort + ratings[i].Content + ratings[i].Location) / 5;
                firstRatings.Add(ratings[i]);
            }
            return firstRatings;
        }
        public double GetAverageRating(int ownerId)
        {
            int ratingCount = 0;
            double sumRatings = 0;

            List<OwnerRating> ownerRatings = GetAll();

            foreach (OwnerRating rating in ownerRatings)
            {
                double averageRating = 0;
                if (rating.Id == ownerId)
                {
                    ratingCount++;
                    averageRating =(double)(rating.Cleanliness + rating.Comfort + rating.Correctness + rating.Content + rating.Location) / (double)5;
                    sumRatings += averageRating;
                }
            }
            return (double)sumRatings / ratingCount;
        }
        public List<Guest1> FindGuestsByAccommodation(Accommodation selectedAccommodation)
        {
            List<Guest1> guests = new List<Guest1>();
            List<OwnerRating> ownerRatings = GetAll();

            foreach (OwnerRating ownerRating in ownerRatings)
            {
                if (ownerRating.AccommodationId == selectedAccommodation.Id)
                {
                    Guest1 guest = _guestService.FindById(ownerRating.GuestId);
                    guests.Add(guest);
                }
            }

            return guests;
        }

        public OwnerRating FindById(int id)
        {
           return _ownerRatingRepository.FindById(id);
        }

        public OwnerRating FindByGuestOwnerIds(int guestId, int ownerId, int accommodationId)
        {
            List<OwnerRating> ownerRatings = GetAll();

            foreach (OwnerRating ownerRating in ownerRatings)
            {
                if (ownerRating.GuestId == guestId && ownerRating.OwnerId == ownerId && ownerRating.AccommodationId == accommodationId)
                {
                    return ownerRating;
                }
            }
            return null;
        }

        public OwnerRating Update(OwnerRating ownerRating)
        {
            return _ownerRatingRepository.Update(ownerRating);
        }

        public void Delete(OwnerRating ownerRating)
        {
            _ownerRatingRepository.Delete(ownerRating);
        }
    }
}
