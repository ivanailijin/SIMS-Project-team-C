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
        public Owner FindById(int id);
        public List<Owner> GetAll();
    }
}
