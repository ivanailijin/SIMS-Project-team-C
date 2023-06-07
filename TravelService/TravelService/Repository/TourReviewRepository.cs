using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.Serializer;

namespace TravelService.Repository
{
    public class TourReviewRepository : ITourReviewRepository
    {
        private const string FilePath = "../../../Resources/Data/tourReviews.csv";

        private readonly Serializer<TourReview> _serializer;
        private List<TourReview> _tourReviews;
        private TourRepository _tourRepository;
     
        public TourReviewRepository()
        {
          
            _serializer = new Serializer<TourReview>();
            _tourReviews = _serializer.FromCSV(FilePath);
            _tourRepository = new TourRepository();
        }

        public List<TourReview> GetAll()
        {
            return _tourReviews;
        }

        public TourReview Save(TourReview tourReview)
        {
            tourReview.Id = NextId();
            _tourReviews.Add(tourReview);
            _serializer.ToCSV(FilePath, _tourReviews);
            return tourReview;
        }

        public int NextId()
        {
            if (_tourReviews.Count < 1)
            {
                return 1;
            }
            return _tourReviews.Max(r => r.Id) + 1;
        }

        public void Delete(TourReview tourReview)
        {
            TourReview founded = _tourReviews.Find(r => r.Id == tourReview.Id);
            _tourReviews.Remove(founded);
            _serializer.ToCSV(FilePath, _tourReviews);
        }

        public TourReview Update(TourReview tourReview)
        {
            TourReview current = _tourReviews.Find(r => r.Id == tourReview.Id);
            int index = _tourReviews.IndexOf(current);
            _tourReviews.Remove(current);
            _tourReviews.Insert(index, tourReview);
            _serializer.ToCSV(FilePath, _tourReviews);
            return tourReview;
        }

        public bool CalculateSuperGuideStatus(Guide guide,List<TourReview> reviews, List<Tour> tours)
        {
            int minimumTours = 2;
            double minimumRating = 4.0;
            int languageId = 13; // Engleski jezik

            foreach (TourReview review in reviews)
            {
                foreach (Tour tour in tours)
                {
                    if (review.GuideId == guide.Id)
                    {
                        if (tour.GuideId == review.GuideId)
                        {
                            if (tour.LanguageId == languageId)
                            {
                                // Dodajte ovaj dio koda za dobivanje ture na temelju ID-ja lokacije
                                int locationId = tour.LocationId;
                                List<Tour> languageTours = _tourRepository.GetTourByLanguageId(languageId);

                                if (languageTours.Count >= minimumTours)
                                {
                                    var averageRating = CalculateAverageRatingForLanguage(reviews);

                                    if (averageRating >= minimumRating)
                                    {
                                        return true; // Vodič je super-vodič
                                    }
                                }

                                return false; // Vodič nije super-vodič
                            }
                        }
                    }
                }
            }

            return false; // Vodič nije super-vodič
        }



        private double CalculateAverageRatingForLanguage(List<TourReview> tourReviews)
        {
            if (tourReviews.Count == 0)
                return 0;

            double sum = 0;
            foreach (var review in tourReviews)
            {
                sum += review.GuideLanguage;
            }

            double averageRating = sum / tourReviews.Count;
            return averageRating;
        }
    }
}
