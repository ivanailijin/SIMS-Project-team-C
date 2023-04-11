using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.WPF.View;

namespace TravelService.Application.UseCases
{
    public class TourReviewService 
    {
        private readonly ITourReviewRepository _tourReviewRepository;

        public TourReviewService(ITourReviewRepository tourReviewRepository)
        {
            _tourReviewRepository = tourReviewRepository;
        }
        public void Delete(TourReview tourReview)
        {
            _tourReviewRepository.Delete(tourReview);
        }

        public List<TourReview> GetAll()
        {
            return _tourReviewRepository.GetAll();
        }

        public TourReview Save(TourReview tourReview)
        {
            TourReview savedTourReview = _tourReviewRepository.Save(tourReview);
            return savedTourReview;
        }

        public void Update(TourReview tourReview)
        {
            _tourReviewRepository.Update(tourReview);
        }
    }
}
