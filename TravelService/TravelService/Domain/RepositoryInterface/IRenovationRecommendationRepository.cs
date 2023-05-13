using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Domain.Model;

namespace TravelService.Domain.RepositoryInterface
{
    public interface IRenovationRecommendationRepository
    {
        public List<RenovationRecommendation> GetAll();
        public RenovationRecommendation Save(RenovationRecommendation renovationRecommendation);
        public int NextId();
        public void Delete(RenovationRecommendation renovationRecommendation);
        public RenovationRecommendation Update(RenovationRecommendation renovationRecommendation);
    }
}
