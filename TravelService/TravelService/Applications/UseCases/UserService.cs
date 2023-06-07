using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Applications.Utils;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.Serializer;

namespace TravelService.Applications.UseCases
{
    public class UserService
    {
        private readonly IUserRepository _repository;
        private readonly AccommodationReservationService _reservationService;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
            _reservationService = new AccommodationReservationService(Injector.CreateInstance<IAccommodationReservationRepository>());
        }
        public List<User> GetAll()
        {
            return _repository.GetAll();
        }
        public User GetByUsername(string username)
        {
            User user = _repository.GetByUsername(username);
            return user;
        }
        public bool CheckGuestsPresence(int userId, Location location)
        {
            List<AccommodationReservation> reservations = _reservationService.GetAll();

            foreach (AccommodationReservation reservation in reservations)
            {
                if (reservation.GuestId == userId && reservation.LocationId == location.Id)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
