using System;
using System.Collections.Generic;
using TravelService.Applications.Utils;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;

namespace TravelService.Applications.UseCases
{
    public class Guest1Service
    {
        private readonly IGuest1Repository _repository;
        private readonly IAccommodationReservationRepository _reservationRepository;

        public Guest1Service(IGuest1Repository repository)
        {
            _repository = repository;
            _reservationRepository = Injector.CreateInstance<IAccommodationReservationRepository>();
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
        public bool CheckCommentsOrigin(int userId)
        {
            List<Guest1> guests = _repository.GetAll();
            foreach(Guest1 guest in guests)
            {
                if (guest.Id == userId)
                    return false;
            }
            return true;
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
        public bool CheckGuestsPresence(int userId, Location location)
        {
            List<AccommodationReservation> reservations = _reservationRepository.GetAll();

            foreach(AccommodationReservation reservation in reservations)
            {
                if (reservation.GuestId == userId && reservation.LocationId == location.Id)
                {
                    return true;
                }
            }
            return false;
        }
        public List<Guest1> FindCommonGuests(List<Guest1> firstList, List<Guest1> secondList)
        {
            List<Guest1> commonGuests = new List<Guest1>();

            foreach (Guest1 guest in firstList)
            {
                foreach (Guest1 guest2 in secondList)
                {
                    if (guest.Id == guest2.Id)
                    {
                        commonGuests.Add(guest);
                        break;
                    }
                }
            }
            return commonGuests;
        }

        public List<AccommodationReservation> GetReservationsInLastYear(Guest1 guest)
        {
            DateTime oneYearAgo = DateTime.Today.AddYears(-1);

            List<AccommodationReservation> guestReservations = _reservationRepository.GetAll();
            List<AccommodationReservation> reservationsInLastYear = new List<AccommodationReservation>();
            foreach (AccommodationReservation reservation in guestReservations)
            {
                if (IsReservationInLastYear(reservation, oneYearAgo, guest.Id))
                {
                    reservationsInLastYear.Add(reservation);
                }
            }
            return reservationsInLastYear;
        }

        public bool IsReservationInLastYear(AccommodationReservation reservation, DateTime oneYearAgo, int guestId)
        {
            return reservation.GuestId == guestId && reservation.IsCancelled == false && reservation.CheckInDate >= oneYearAgo && reservation.CheckOutDate <= DateTime.Today;
        }

        public Guest1 CheckSuperOwnerStatus(Guest1 guest)
        {
            List<AccommodationReservation> reservationsInLastYear = GetReservationsInLastYear(guest);
            int reservationsCount = reservationsInLastYear.Count;

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
                if (DateTime.Now > guest.SuperGuestExpirationDate)
                {
                    if (reservationsCount >= 10)
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
