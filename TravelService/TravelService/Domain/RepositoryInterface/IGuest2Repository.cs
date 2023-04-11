using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Domain.Model;

namespace TravelService.Domain.RepositoryInterface
{
    public interface IGuest2Repository
    {
        public Guest2 GetByUsername(string username);
        public Guest2 FindById(int id);
        public List<Guest2> GetAll();
    }
}
