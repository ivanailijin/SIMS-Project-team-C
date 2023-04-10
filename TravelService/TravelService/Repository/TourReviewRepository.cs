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

        public TourReviewRepository()
        {
            _serializer = new Serializer<TourReview>();
            _tourReviews = _serializer.FromCSV(FilePath);
        }
        public List<TourReview> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public TourReview Save(TourReview tourReview)
        {
            tourReview.Id = NextId();
            _tourReviews = _serializer.FromCSV(FilePath);
            _tourReviews.Add(tourReview);
            _serializer.ToCSV(FilePath, _tourReviews);
            return tourReview;
        }

        public int NextId()
        {
            _tourReviews = _serializer.FromCSV(FilePath);
            if (_tourReviews.Count < 1)
            {
                return 1;
            }
            return _tourReviews.Max(r => r.Id) + 1;
        }

        public void Delete(TourReview tourReview)
        {
            _tourReviews = _serializer.FromCSV(FilePath);
            TourReview founded = _tourReviews.Find(r => r.Id == tourReview.Id);
            _tourReviews.Remove(founded);
            _serializer.ToCSV(FilePath, _tourReviews);
        }

        public TourReview Update(TourReview tourReview)
        {
            _tourReviews = _serializer.FromCSV(FilePath);
            TourReview current = _tourReviews.Find(r => r.Id == tourReview.Id);
            int index = _tourReviews.IndexOf(current);
            _tourReviews.Remove(current);
            _tourReviews.Insert(index, tourReview);
            _serializer.ToCSV(FilePath, _tourReviews);
            return tourReview;
        }
    }
}