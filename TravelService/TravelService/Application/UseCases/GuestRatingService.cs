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
    public class GuestRatingService
    {
        private readonly IGuestRatingRepository _repository;
        private readonly Guest1Service _guestService;

        public GuestRatingService(IGuestRatingRepository guestRatingRepository)
        {
            _repository = guestRatingRepository;
            _guestService = new Guest1Service(Injector.CreateInstance<IGuest1Repository>());
        }
        public List<GuestRating> GetAll()
        {
            return _repository.GetAll();
        }

        public List<Guest1> FindRatedGuests(int ownerId)
        {
            List<Guest1> ratedGuests = new List<Guest1>();
            List<GuestRating> guestRatings = _repository.GetAll();

            foreach (GuestRating guestRating in guestRatings)
            {
                if (guestRating.OwnerId == ownerId)
                {
                    Guest1 ratedGuest = _guestService.FindById(guestRating.GuestId);
                    ratedGuests.Add(ratedGuest);
                }
            }

            return ratedGuests;
        }

        public GuestRating Save(GuestRating guestRating)
        {
            return _repository.Save(guestRating);
        }

        public int NextId()
        {
            return _repository.NextId();
        }

        public void Delete(GuestRating guestRating)
        {
            _repository.Delete(guestRating);
        }

        public GuestRating Update(GuestRating guestRating)
        {
            return _repository.Update(guestRating);
        }
    }
}
