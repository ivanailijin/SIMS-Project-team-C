using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.Serializer;
using TravelService.Domain.RepositoryInterface;

namespace TravelService.Repository
{
    public class GuestRepository : IGuestRepository
    {
        private const string FilePath = "../../../Resources/Data/guest.csv";

        private readonly Serializer<Guest> _serializer;
        public int Under18Count { get; set; }
        public int Between18And50Count { get; set; }
        public int Over50Count { get; set; }
        public double WithVoucherPercentage { get; set; }
        public double WithoutVoucherPercentage { get; set; }
        private List<Guest> guests = new List<Guest>();

        private List<Guest> _guest;
        public static GuestVoucherRepository _guestVoucherRepository { get; set; }

        public GuestRepository()
        {
            _serializer = new Serializer<Guest>();
            _guest = _serializer.FromCSV(FilePath);
            guests = new List<Guest>();
            _guestVoucherRepository = new GuestVoucherRepository();
        }

        public List<Guest> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public Guest Save(Guest guest)
        {
            guest.Id = NextId();
            _guest = _serializer.FromCSV(FilePath);
            _guest.Add(guest);
            _serializer.ToCSV(FilePath, _guest);
            return guest;
        }

        public int NextId()
        {
            _guest = _serializer.FromCSV(FilePath);
            if (_guest.Count < 1)
            {
                return 1;
            }
            return _guest.Max(c => c.Id) + 1;
        }

        public void Delete(Guest guest)
        {
            _guest = _serializer.FromCSV(FilePath);
            Guest founded = _guest.Find(c => c.Id == guest.Id);
            _guest.Remove(founded);
            _serializer.ToCSV(FilePath, _guest);
        }

        public Guest Update(Guest guest)
        {
            _guest = _serializer.FromCSV(FilePath);
            Guest current = _guest.Find(c => c.Id == guest.Id);
            int index = _guest.IndexOf(current);
            _guest.Remove(current);
            _guest.Insert(index, guest);
            _serializer.ToCSV(FilePath, _guest);
            return guest;
        }


        public List<Guest> filterGuestsByCheckpointAndTour(List<Guest> guests, CheckPoint selectedCheckPoint, Tour selectedTour)
        {
            List<Guest> filteredGuests = new List<Guest>();
            foreach (Guest guest in guests)
            {
                if (guest.CheckPointId == selectedCheckPoint.CheckPointId && guest.TourId == selectedTour.Id)
                {
                    guest.CheckPointName = selectedCheckPoint.Name;
                    guest.TourName = selectedTour.Name;
                    filteredGuests.Add(guest);
                }
            }
            return filteredGuests;
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

        public TourStatistics ShowTourStatistics(Tour tour)
        {
            List<Guest> guests = GetAllGuestsWithVouchers();
            List<Guest> selectedTourGuests = guests.Where(g => g.TourId == tour.Id).ToList();
          

            int under18Count = selectedTourGuests.Count(g => g.Age < 18);
            int between18And50Count = selectedTourGuests.Count(g => g.Age >= 18 && g.Age <= 50);
            int over50Count = selectedTourGuests.Count(g => g.Age > 50);
            int voucherUsedCount = selectedTourGuests.Count(g => g.VoucherList?.Any(v => v.GuestId == g.Id && v.Used) ?? false);
            int voucherNotUsedCount = selectedTourGuests.Count(g => g.VoucherList?.Any(v => v.GuestId == g.Id && !v.Used) ?? false);
            int totalGuests = selectedTourGuests.Count;
            double withVoucherPercentage = (double)voucherUsedCount / (double)totalGuests * 100;
            double withoutVoucherPercentage = (double)voucherNotUsedCount / (double)totalGuests * 100;

            TourStatistics stats = new TourStatistics
            {
                TourId = tour.Id,
                Under18Count = under18Count,
                Between18And50Count = between18And50Count,
                Over50Count = over50Count,
                WithVoucherPercentage = withVoucherPercentage,
                WithoutVoucherPercentage = withoutVoucherPercentage
            };

            return stats;
        }



        public List<Guest> GetAllGuestsWithVouchers()
        {
            List<Guest> guests = GetAll();

            foreach (Guest guest in guests)
            {
                guest.VoucherList = _guestVoucherRepository.GetVouchersForGuest(guest.Id);
            }

            return guests;
        }



    }
}



