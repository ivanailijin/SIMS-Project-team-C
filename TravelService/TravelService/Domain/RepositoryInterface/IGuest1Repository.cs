using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Domain.Model;
using TravelService.Serializer;

namespace TravelService.Domain.RepositoryInterface
{
    public interface IGuest1Repository
    {
        public Guest1 GetByUsername(string username);
        public Guest1 FindById(int id);
        public List<Guest1> GetAll();
        public Guest1 Update(Guest1 guest1);
    }
}
