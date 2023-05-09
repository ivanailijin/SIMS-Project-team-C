using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Domain.Model;

namespace TravelService.Domain.RepositoryInterface
{
    public interface IGuestVoucherRepository
    {

        public List<GuestVoucher> GetAll();

        public GuestVoucher Save(GuestVoucher voucher);

        public int NextId();

        public void Delete(GuestVoucher voucher);

        public GuestVoucher Update(GuestVoucher voucher);

    }
}

