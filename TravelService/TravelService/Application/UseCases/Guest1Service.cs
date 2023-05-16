using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.Repository;
using TravelService.Serializer;
using TravelService.Application.Utils;
using TravelService.WPF.View;

namespace TravelService.Application.UseCases
{
    public class Guest1Service
    {
        private readonly IGuest1Repository _repository;

        public Guest1Service(IGuest1Repository repository)
        {
            _repository = repository;
        }

        public Guest1 GetByUsername(string username)
        {
            Guest1 guest = _repository.GetByUsername(username);
            return guest;
        }
        public Guest1 FindById(int id)
        {
            Guest1 guest = _repository.FindById(id);
            return guest;
        }
        public List<Guest1> GetAll()
        {
            return _repository.GetAll();
        }

        public Guest1 Update(Guest1 guest1)
        {
            return _repository.Update(guest1);
        }

        public List<AccommodationReservation> FindReservationGuest(List<AccommodationReservation> UnratedReservations)
        {
            List<Guest1> guests = GetAll();
            foreach (AccommodationReservation unratedReservation in UnratedReservations)
            {
                unratedReservation.Guest1 = guests.Find(g => g.Id == unratedReservation.GuestId);
            }
            return UnratedReservations;
        }

        public Guest1 CheckSuperOwnerStatus(Guest1 guest, int reservationsCount)
        {
            if (!guest.SuperGuest)
            {
                if (reservationsCount >= 10)
                {
                    guest.SuperGuest = true;
                    guest.BonusPoints = 5;
                    guest.SuperGuestExpirationDate = DateTime.Now.AddYears(1);
                    guest = _repository.Update(guest);
                }
            }
            else
            {
                if(DateTime.Now > guest.SuperGuestExpirationDate)
                {
                    if(reservationsCount >= 10)
                    {
                        guest.BonusPoints = 5;
                        guest.SuperGuestExpirationDate = DateTime.Now.AddYears(1);
                        guest = _repository.Update(guest);
                    }
                    else
                    {
                        guest.SuperGuest = false;
                        guest.SuperGuestExpirationDate = DateTime.MinValue;
                        guest.BonusPoints = 0;
                        guest = _repository.Update(guest);
                    }
                }
            }
            return guest;
        }
    }
}
