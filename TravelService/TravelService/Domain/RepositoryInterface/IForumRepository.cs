using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Domain.Model;
using TravelService.Serializer;

namespace TravelService.Domain.RepositoryInterface
{
    public interface IForumRepository
    {
        public List<Forum> GetAll();
        public Forum Save(Forum forum);
        public int NextId();
        public void Delete(Forum forum);
        public Forum Update(Forum forum);
        public Forum FindById(int id);
    }
}