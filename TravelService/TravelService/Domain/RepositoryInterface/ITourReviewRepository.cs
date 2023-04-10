using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TravelService.Domain.Model;
using System.Threading.Tasks;

namespace TravelService.Domain.RepositoryInterface
{
    public interface  ITourReviewRepository
    {
        public List<TourReview> GetAll();

        public TourReview Save(TourReview tourReview);

        public int NextId();

        public void Delete(TourReview tourReview);

        public TourReview Update(TourReview tourReview);
    }
}
