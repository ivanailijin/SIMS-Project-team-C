using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Domain.Model;

namespace TravelService.Domain.RepositoryInterface
{
    public interface IGuideRepository
    {
        public Guide GetByUsername(string username);
        public Guide FindById(int id);
        public List<Guide> GetAll();
    }
}