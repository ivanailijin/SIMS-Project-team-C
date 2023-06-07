using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.Applications.Utils;

namespace TravelService.Applications.UseCases
{
    public class GuestService
    {

        private readonly IGuestRepository _guestRepository;
        private readonly TourService _tourService;

        public GuestService(IGuestRepository guestRepository)
        {
            _guestRepository = guestRepository;
            _tourService = new TourService(Injector.CreateInstance<ITourRepository>());
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

        public List<Guest> GetGuestsOnTour(Guest guest, Tour selectedTour, List<CheckPoint> checkPoints)
        {
            CheckPoint currentCheckPoint;
            List<Guest> filteredGuests = new List<Guest>();
            List<Guest> guestsOnTour = FindByTourId(selectedTour.Id);
            foreach (Guest currentGuest in guestsOnTour)
            {
                if (currentGuest.Id == guest.Id)
                {
                    continue;
                }

                currentCheckPoint = checkPoints.Find(checkPoint => checkPoint.CheckPointId == guest.CheckPointId);


                filteredGuests.Add(currentGuest);
            }

            return filteredGuests;
        }



        public List<string> FindCheckPointName(List<Guest> Guests, List<CheckPoint> CheckPoints)
        {
            CheckPoint currentCheckPoint;
            string checkPointName = null;
            List<string> checkPointList = new List<string>();
            foreach (Guest guest in Guests)
            {
                currentCheckPoint = CheckPoints.Find(checkPoint => checkPoint.CheckPointId == guest.CheckPointId);
                checkPointName = currentCheckPoint.Name;
                checkPointList.Add(checkPointName);

            }
            return checkPointList;
        }


        public List<CheckPoint> FindGuestByCheckPointId(List<CheckPoint> checkPoints, Guest guest)
        {
            List<CheckPoint> checkPointList = new List<CheckPoint>();

            foreach (CheckPoint checkPoint in checkPoints)
            {
                if (guest.CheckPointId == checkPoint.CheckPointId)
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

        public List<Guest> GetTourData(List<Guest> guestAttendences)
        {
            List<Tour> tours = _tourService.GetAll();
            foreach (Guest guest in guestAttendences)
            {
                guest.Tour = tours.Find(t => t.Id == guest.TourId);
            }
            return guestAttendences;
        }

        public bool CheckGuestsPresence(string username, Location location)
        {
            List<Guest> guestAttendences = GetAll();
            guestAttendences = GetTourData(guestAttendences);

            foreach (Guest guest in guestAttendences)
            {
                if (guest.Username == username && guest.Tour.LocationId == location.Id && guest.Attendence == true)
                {
                    return true;
                }
            }
            return false;
        }
    }
}

