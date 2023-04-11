using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Domain.Model;

namespace TravelService.Domain.RepositoryInterface
{
    public interface IAccommodationRepository
    {
        public List<Accommodation> GetAll();
        public List<Accommodation> GetOwnersAccommodations(int ownerId);
        public Accommodation Save(Accommodation accommodation);
        public int NextId();
        public void Delete(Accommodation accommodation);
        public Accommodation FindById(int id);
        public Accommodation Update(Accommodation accommodation);
    }
}
