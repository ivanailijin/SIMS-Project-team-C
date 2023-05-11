using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Domain.Model;

namespace TravelService.Domain.RepositoryInterface
{
    public interface IVoucherRepository
    {
        public List<GuestVoucher> GetAll();

        public GuestVoucher Save(GuestVoucher guestVoucher);

        public int NextId();

        public void Delete(GuestVoucher guestVoucher);

        public GuestVoucher Update(GuestVoucher guestVoucher);
    }
}
