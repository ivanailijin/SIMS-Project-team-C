using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;

namespace TravelService.Application.UseCases
{
    internal class GuideService
    {
        private readonly IGuideRepository _guideRepository;

        public GuideService(IGuideRepository guideRepository)
        {
            _guideRepository = guideRepository;
        }

        public Guide GetByUsername(string username)
        {
            Guide guide = _guideRepository.GetByUsername(username);
            return guide;
        }
        public Guide FindById(int id)
        {
            Guide guide = _guideRepository.FindById(id);
            return guide;
        }
        public List<Guide> GetAll()
        {
            return _guideRepository.GetAll();
        }
    }
}