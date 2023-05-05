using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Domain.Model;
using TravelService.Serializer;

namespace TravelService.Domain.RepositoryInterface
{
    public interface IGuestRatingRepository
    {
        public List<GuestRating> GetAll();

        public GuestRating Save(GuestRating guestRating);

        public int NextId();
        public List<Guest1> FindRatedGuests(int ownerId);

        public void Delete(GuestRating guestRating);

        public GuestRating Update(GuestRating guestRating);
    }
}
