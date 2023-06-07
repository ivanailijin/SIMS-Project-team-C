using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.Serializer;

namespace TravelService.Repository
{
    public class TourRequestRepository : ITourRequestRepository
    {
        private const string FilePath = "../../../Resources/Data/tourRequests.csv";

        private readonly Serializer<TourRequest> _serializer;

        private List<TourRequest> _tourRequests;

        public TourRequestRepository()
        {
            _serializer = new Serializer<TourRequest>();
            _tourRequests = _serializer.FromCSV(FilePath);
        }
        public List<TourRequest> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public TourRequest Save(TourRequest tourRequest)
        {
            tourRequest.Id = NextId();
            _tourRequests = _serializer.FromCSV(FilePath);
            _tourRequests.Add(tourRequest);
            _serializer.ToCSV(FilePath, _tourRequests);
            return tourRequest;
        }

        public int NextId()
        {
            _tourRequests = _serializer.FromCSV(FilePath);
            if (_tourRequests.Count < 1)
            {
                return 1;
            }
            return _tourRequests.Max(r => r.Id) + 1;
        }

        public void Delete(TourRequest tourRequest)
        {
            _tourRequests = _serializer.FromCSV(FilePath);
            TourRequest found = _tourRequests.Find(r => r.Id == tourRequest.Id);
            _tourRequests.Remove(found);
            _serializer.ToCSV(FilePath, _tourRequests);
        }

        public TourRequest Update(TourRequest tourRequest)
        {
            _tourRequests = _serializer.FromCSV(FilePath);
            TourRequest current = _tourRequests.Find(r => r.Id == tourRequest.Id);
            int index = _tourRequests.IndexOf(current);
            _tourRequests.Remove(current);
            _tourRequests.Insert(index, tourRequest);
            _serializer.ToCSV(FilePath, _tourRequests);
            return tourRequest;
        }
        

    }

}