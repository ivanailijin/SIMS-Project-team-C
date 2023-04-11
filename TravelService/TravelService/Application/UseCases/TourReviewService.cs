using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;


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

        public List<TourReview> FindTourReviewsByGuestId(int guestId)
        {
            List<TourReview> tourReviews = GetAll();

            List<TourReview> matchingTourReviews = new List<TourReview>();

            foreach (TourReview review in tourReviews)
            {
                if (review.GuestId == guestId)
                {
                    matchingTourReviews.Add(review);
                }
            }

            return matchingTourReviews;
        }


    }
}