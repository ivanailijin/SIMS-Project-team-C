using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Domain.Model;

namespace TravelService.Domain.RepositoryInterface
{
    public interface ICheckPointRepository
    {

        public List<CheckPoint> GetAll();

        public CheckPoint Save(CheckPoint checkPoint);

        public int NextId();

        public void Delete(CheckPoint checkPoint);

        public CheckPoint Update(CheckPoint checkPoint);
        public CheckPoint GetById(int id);
    }

}
