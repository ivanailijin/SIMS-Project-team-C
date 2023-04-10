using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Domain.Model;
using TravelService.Repository;

namespace TravelService.Domain.RepositoryInterface
{
    public interface IAccommodationReservationRepository
    {
        public List<AccommodationReservation> GetAll();
        public AccommodationReservation Save(AccommodationReservation accommodationReservation);
        public int NextId();
        public void Delete(AccommodationReservation accommodationReservation);
        public AccommodationReservation FindById(int id);
        public ObservableCollection<AccommodationReservation> FindUnratedReservations(AccommodationRepository _accommodationRepository, int OwnerId);
        public AccommodationReservation Update(AccommodationReservation accommodationReservation);
        public List<Tuple<DateTime, DateTime>> FindAvailableDates(Accommodation selectedAccommodation, DateTime startDate, DateTime endDate, int daysOfStaying);
        public List<Tuple<DateTime, DateTime>> FindAvailableDatesOutsideRange(Accommodation selectedAccommodation, DateTime startDate, DateTime endDate, int daysOfStaying);
        public List<DateTime> FindReservedDates(Accommodation selectedAccommodation);
        public List<AccommodationReservation> FindUnratedOwners(int guestId);
        public List<AccommodationReservation> FindReservationsByGuestId(int guestId);
        public void SetAccommodationForUnratedOwners(List<Accommodation> accomodations);
        public void SetLocationForUnratedOwners(List<Location> locations);
        public void SetNameForUnratedOwners(List<Owner> owners);


    }
}
