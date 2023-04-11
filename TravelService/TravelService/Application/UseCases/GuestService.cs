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


        public List<Tuple<string, string>> GetGuestsOnTour(Guest guest, Tour selectedTour, List<CheckPoint> checkPoints)
        {
            List<Tuple<string, string>> filteredGuests = new List<Tuple<string, string>>();

            // Find guests on the same tour as the current guest
            List<Guest> guestsOnTour = FindByTourId(selectedTour.Id);

            // Find check points that are relevant to the current guest
            List<CheckPoint> checkPointsForGuest = FindGuestByCheckPointId(checkPoints, guest);

            foreach (Guest currentGuest in guestsOnTour)
            {
                // Skip the current guest because we don't want to include them in the list
                if (currentGuest.Id == guest.Id)
                {
                    continue;
                }

                // Find the check point for the current guest
                CheckPoint currentCP = checkPointsForGuest.Find(cp => cp.CheckPointId == currentGuest.CheckPointId);

                // Only add guest if they have not already been added
                if (!filteredGuests.Any(tuple => tuple.Item1 == currentGuest.Username))
                {
                    if (currentCP != null)
                    {
                        filteredGuests.Add(new Tuple<string, string>(currentGuest.Username, currentCP.Name));
                    }
                }
            }

            return filteredGuests;
        }

        public List<CheckPoint> FindGuestByCheckPointId(List<CheckPoint> checkPoints, Guest guest)
        {
            List<CheckPoint> checkPointList = new List<CheckPoint>();

            foreach (CheckPoint checkPoint in checkPoints)
            {
                if (guest.CheckPointId == checkPoint.CheckPointId )
                {
                    checkPointList.Add(checkPoint);
                }
            }

            return checkPointList;
        }
        public List<Guest> FindByTourId(int tourId)
        {
            List<Guest> guests = GetAll();
            List<Guest> guestsByTourId = new List<Guest>();
            foreach (Guest guest in guests)
            {
                if (guest.TourId == tourId)
                {
                    guestsByTourId.Add(guest);
                }
            }
            return guestsByTourId;
        }


    }
}
    