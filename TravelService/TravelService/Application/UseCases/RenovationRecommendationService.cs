using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;

namespace TravelService.Application.UseCases
{
    public class RenovationRecommendationService
    {
        private readonly IRenovationRecommendationRepository _renovationRecommendationRepository;
        public RenovationRecommendationService(IRenovationRecommendationRepository renovationRecommendationRepository) 
        {
            _renovationRecommendationRepository = renovationRecommendationRepository;
        }

        public List<RenovationRecommendation> GetAll()
        {
            return _renovationRecommendationRepository.GetAll();
        }

        public RenovationRecommendation Save(RenovationRecommendation renovation)
        {
            return _renovationRecommendationRepository.Save(renovation);
        }

        public void Delete(RenovationRecommendation renovation)
        {
            _renovationRecommendationRepository.Delete(renovation);
        }

        public RenovationRecommendation Update(RenovationRecommendation renovation)
        {
            return _renovationRecommendationRepository.Update(renovation);
        }
    }
}
