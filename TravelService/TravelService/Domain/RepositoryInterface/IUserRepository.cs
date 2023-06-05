using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Domain.Model;
using TravelService.Serializer;

namespace TravelService.Domain.RepositoryInterface
{
    public interface IUserRepository
    {
        public User GetByUsername(string username);
        public List<User> GetAll();
    }
}
