using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;

namespace TravelService.Applications.UseCases
{
    public class GuideService
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
        public bool IsSuperGuide(Guide guide, List<Tour> tours, List<TourReview> tourReviews, Language language, int minimumTours, double minimumRating)
        {
            var guideTours = tours.Where(tour => tour.GuideId == guide.Id).ToList();
            var languageTours = guideTours.Where(tour => tour.Language.Name == language.Name).ToList();

            if (languageTours.Count < minimumTours)
                return false;

            var languageTourReviews = tourReviews.Where(review => languageTours.Contains(review.Tour)).ToList();
            var averageRating = CalculateAverageRatingForLanguage(languageTourReviews, language);

            return averageRating >= minimumRating;
        }
        public double CalculateAverageRatingForLanguage(List<TourReview> tourReviews, Language language)
        {
            var filteredReviews = tourReviews.Where(review => review.Tour.Language.Name == language.Name).ToList();

            if (filteredReviews.Count == 0)
                return 0;

            double sum = 0;
            foreach (var review in filteredReviews)
            {
                sum += review.GuideLanguage;
            }

            double averageRating = sum / filteredReviews.Count;
            return averageRating;
        }


    }

}

