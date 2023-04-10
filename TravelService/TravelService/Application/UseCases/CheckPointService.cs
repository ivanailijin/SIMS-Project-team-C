using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;

namespace TravelService.Application.UseCases
{
    public class CheckPointService 
    {

        private readonly ICheckPointRepository _checkPointRepsitory;

        public CheckPointService(ICheckPointRepository checkPointRepository)
        {
            _checkPointRepsitory = checkPointRepository;
        }
        public void Delete(CheckPoint checkPoint)
        {
            _checkPointRepsitory.Delete(checkPoint);
        }

        public List<CheckPoint> GetAll()
        {
            return _checkPointRepsitory.GetAll();
        }

        public CheckPoint Save(CheckPoint checkPoint)
        {
            CheckPoint savedCheckPoint = _checkPointRepsitory.Save(checkPoint);
            return savedCheckPoint;
        }

        public void Update(CheckPoint checkPoint)
        {
            _checkPointRepsitory.Update(checkPoint);
        }
    }
}
