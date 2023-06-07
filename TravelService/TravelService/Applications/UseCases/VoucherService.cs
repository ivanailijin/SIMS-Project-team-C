using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using TravelService.Applications.Utils;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;

namespace TravelService.Applications.UseCases
{
    public class VoucherService
    {
        private readonly IVoucherRepository _voucherRepository;
        public readonly GuestService _guestService;
        public VoucherService(IVoucherRepository voucherRepository)
        {
            _voucherRepository = voucherRepository;
            _guestService = new GuestService(Injector.CreateInstance<IGuestRepository>());
        }
        public void Delete(GuestVoucher guestVoucher)
        { 
            _voucherRepository.Delete(guestVoucher);
        }
        public List<GuestVoucher> GetAll()
        {
            return _voucherRepository.GetAll();
        }
        public GuestVoucher Save(GuestVoucher guestVoucher)
        {
            GuestVoucher savedGuestVoucher = _voucherRepository.Save(guestVoucher);
            return savedGuestVoucher;
        }
        public void Update(GuestVoucher guestVoucher)
        {
            _voucherRepository.Update(guestVoucher);
        }

        public List<GuestVoucher> showVoucherList(List<GuestVoucher> Vouchers, Guest2 guest2)
        {
            List<GuestVoucher> guestVouchers = new List<GuestVoucher>();
            foreach (GuestVoucher voucher in Vouchers)
            {
                if (guest2.Id == voucher.GuestId )
                {
                    guestVouchers.Add(voucher);
                }

            }
            return guestVouchers;
        }

        public List<GuestVoucher> CheckAllVouchers(List<GuestVoucher> vouchers, Guest2 guest2, List<Tour> tours)
        {
            List<int> guestsTourIds = new List<int>(GetGuestsTourIds(guest2));
            List<Tour> guestsTours = new List<Tour>(GetGuestsTours(guestsTourIds,tours));
            if (guestsTours.Count() >= 5) {
                if (GetNumberOfYears(guestsTours) == 1) {
                    GuestVoucher voucher = new GuestVoucher("Osvojili ste vaucer!", VOUCHERTYPE.BONUS,5000, "17k", false, guest2.Id,0,DateTime.Now.AddMonths(6));
                    Save(voucher);
                    vouchers.Add(voucher);
                }
            }
            return vouchers;
        }
        public List<int> GetGuestsTourIds(Guest2 guest2)
        {
            List<Guest> allGuests = new List<Guest>(_guestService.GetAll());
            List<int> allTourIds = new List<int>();
            foreach (Guest guest in allGuests)
            {
                if (guest.Id == guest2.Id)
                {
                    int currentTourId = guest.TourId;
                    if (!allTourIds.Contains(currentTourId))
                    {
                        allTourIds.Add(currentTourId);
                    }
                }
            }
            return allTourIds;
        }
        public List<Tour> GetGuestsTours(List<int> tourIds,List<Tour> tours) {
            List<Tour> guestsTours = new List<Tour>();
            foreach (int tourId in tourIds)
            {
                Tour currentTour = tours.FirstOrDefault(request => request.Id == tourId);
                if (currentTour != null)
                {
                    guestsTours.Add(currentTour);
                }
            }
            return guestsTours;
        }
        public int GetNumberOfYears(List<Tour> tours)
        {
            List<int> years = new List<int>();
            foreach (Tour tour in tours) {
                if(!years.Contains(tour.TourStart.Year))
                    years.Add(tour.TourStart.Year);
            }
            return years.Count();
        }
    }
}
