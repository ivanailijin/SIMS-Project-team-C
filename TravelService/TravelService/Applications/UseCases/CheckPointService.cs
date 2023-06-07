using System.Collections.Generic;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.Repository;

namespace TravelService.Applications.UseCases
{
    public class CheckPointService
    {

        public readonly CheckPointRepository _checkPointRepository;
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

        public CheckPoint GetById(int id)
        {
            CheckPoint checkpoint = _checkPointRepository.GetById(id);
            return checkpoint;
        }
        public void FirstCheckPointActive(List<CheckPoint> FilteredCheckPoint)
        {
            if (FilteredCheckPoint.Count > 0)
            {
                FilteredCheckPoint[0].Active = true;
            }
        }

    }
}
