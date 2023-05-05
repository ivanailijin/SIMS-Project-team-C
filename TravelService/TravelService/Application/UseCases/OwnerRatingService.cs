using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Application.Utils;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.Serializer;

namespace TravelService.Application.UseCases
{
    public class OwnerRatingService
    {
        private readonly IOwnerRatingRepository _ownerRatingRepository;
        private readonly Guest1Service _guestService;

        public OwnerRatingService(IOwnerRatingRepository ownerRatingRepository)
        {
            _ownerRatingRepository = ownerRatingRepository;
            _guestService = new Guest1Service(Injector.CreateInstance<IGuest1Repository>());
        }

        public List<OwnerRating> GetAll()
        {
            return _ownerRatingRepository.GetAll();
        }

        public OwnerRating Save(OwnerRating ownerRating)
        {
            return _ownerRatingRepository.Save(ownerRating);
        }

        public int NextId()
        {
            return _ownerRatingRepository.NextId();
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
        public void Delete(OwnerRating ownerRating)
        {
            _ownerRatingRepository.Delete(ownerRating);
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
    }
}
