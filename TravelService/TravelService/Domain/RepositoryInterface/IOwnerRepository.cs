using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Domain.Model;
using TravelService.Repository;
using TravelService.Serializer;

namespace TravelService.Domain.RepositoryInterface
{
    public interface IOwnerRepository
    {
        public Owner GetByUsername(string username);
        public int NextId();
        public Owner FindById(int id);
        public Owner Update(Owner owner);
        public Owner Save(Owner owner);
        public List<Owner> GetAll();
    }
}
