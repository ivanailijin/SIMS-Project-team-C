using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Domain.Model;

namespace TravelService.Domain.RepositoryInterface
{
    public interface IGuestRepository
    {
        public List<Guest> GetAll();

        public Guest Save(Guest Guest);

        public int NextId();

        public void Delete(Guest guest);

        public Guest Update(Guest guest);
        public List<Guest> FindByTourId(int tourId);
        public List<Guest> GetAllGuestsWithVouchers();
    }
}

