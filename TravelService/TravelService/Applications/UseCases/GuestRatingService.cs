using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Applications.Utils;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.Repository;

namespace TravelService.Applications.UseCases
{
    public class GuestRatingService
    {
        private readonly IGuestRatingRepository _repository;
        private readonly Guest1Service _guestService;
        private readonly OwnerRatingService _ownerRatingService;
        private readonly OwnerService _ownerService;
        private readonly LocationService _locationService;
        private readonly AccommodationService _accommodationService;
        private readonly AccommodationReservationService _reservationService;

        public GuestRatingService(IGuestRatingRepository guestRatingRepository)
        {
            _repository = guestRatingRepository;
            _guestService = new Guest1Service(Injector.CreateInstance<IGuest1Repository>());
            _ownerRatingService = new OwnerRatingService(Injector.CreateInstance<IOwnerRatingRepository>());
            _ownerService = new OwnerService(Injector.CreateInstance<IOwnerRepository>());
            _locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            _accommodationService = new AccommodationService(Injector.CreateInstance<IAccommodationRepository>());
            _reservationService = new AccommodationReservationService(Injector.CreateInstance<IAccommodationReservationRepository>());
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

        public List<GuestRating> FindRatingsByGuestId(int guestId)
        {
            List<GuestRating> guestRatings = new List<GuestRating>();
            List<GuestRating> allGuestRatings = GetAll();
            foreach(GuestRating guestRating in allGuestRatings)
            {
                if(guestRating.GuestId == guestId)
                {
                    guestRatings.Add(guestRating);
                }
            }
            return guestRatings;
        }

        public List<GuestRating> FindCommonGuestRatings(int guestId)
        {
            List<GuestRating> commonGuestRatings = new List<GuestRating>();
            List<GuestRating> guestRatings = FindRatingsByGuestId(guestId);
            List<OwnerRating> ownerRatings = _ownerRatingService.GetAll();
            foreach(GuestRating guestRating in guestRatings)
            {
                foreach(OwnerRating ownerRating in ownerRatings)
                {
                    if (guestRating.ReservationId == ownerRating.ReservationId)
                    {
                        commonGuestRatings.Add(guestRating);
                    }
                }
            }
            return commonGuestRatings;
        }

        public List<GuestRating> GetOwnerData(List<GuestRating> guestRatings)
        {
            List<Owner> owners = _ownerService.GetAll();
            foreach (GuestRating guestRating in guestRatings)
            {
                guestRating.Owner = owners.Find(o => o.Id == guestRating.OwnerId);
            }
            return guestRatings;
        }

        public List<GuestRating> GetReservationData(List<GuestRating> guestRatings)
        {
            List<AccommodationReservation> reservations = _reservationService.GetAll();
            foreach (GuestRating guestRating in guestRatings)
            {
                guestRating.Reservation = reservations.Find(r => r.Id == guestRating.ReservationId);
            }
            return guestRatings;
        }

        public List<GuestRating> GetLocationData(List<GuestRating> guestRatings)
        {
            List<Location> locations = _locationService.GetAll();
            foreach (GuestRating guestRating in guestRatings)
            {
                guestRating.Reservation.Location = locations.Find(l => l.Id == guestRating.Reservation.LocationId);
            }
            return guestRatings;
        }

        public List<GuestRating> GetAccommodationData(List<GuestRating> guestRatings)
        {
            List<Accommodation> accommodations = _accommodationService.GetAll();
            foreach (GuestRating guestRating in guestRatings)
            {
                guestRating.Reservation.Accommodation = accommodations.Find(a => a.Id == guestRating.Reservation.AccommodationId);
            }
            return guestRatings;
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
