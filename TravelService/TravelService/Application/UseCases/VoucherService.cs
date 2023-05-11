using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;

namespace TravelService.Application.UseCases
{
    public class VoucherService
    {
        private readonly IVoucherRepository _voucherRepository;

        public VoucherService(IVoucherRepository voucherRepository)
        {
            _voucherRepository = voucherRepository;
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
    }
}
