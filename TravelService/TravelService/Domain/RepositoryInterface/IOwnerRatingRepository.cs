using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Domain.Model;

namespace TravelService.Domain.RepositoryInterface
{
    public interface IOwnerRatingRepository
    {
        public List<OwnerRating> GetAll();
        public OwnerRating Save(OwnerRating ownerRating);
        public int NextId();
        public OwnerRating Update(OwnerRating ownerRating);
        public int GetNumberOfRatings(int ownerId);
        public double GetAverageRating(int ownerId);
        public void Delete(OwnerRating ownerRating);
        public List<Guest1> FindGuestsByAccommodation(Accommodation selectedAccommodation);
        public OwnerRating FindById(int id);
        public OwnerRating FindByGuestOwnerIds(int guestId, int ownerId, int accommodationId);

    }
}
