using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;


namespace TravelService.Application.UseCases
{
    public class GuestService
    {

        private readonly IGuestRepository _guestRepository;

        public GuestService(IGuestRepository guestRepository)
        {
            _guestRepository = guestRepository;
        }
        public void Delete(Guest guest)
        {
            _guestRepository.Delete(guest);
        }

        public List<Guest> GetAll()
        {
            return _guestRepository.GetAll();
        }

        public Guest Save(Guest guest)
        {
            Guest savedGuest = _guestRepository.Save(guest);
            return savedGuest;
        }

        public void Update(Guest guest)
        {
            _guestRepository.Update(guest);
        }
    }
}