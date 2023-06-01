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
    public class ComplexTourRequestRepository : IComplexTourRequestRepository
    {
        private const string FilePath = "../../../Resources/Data/complexTourRequests.csv";

        private readonly Serializer<ComplexTourRequest> _serializer;

        private List<ComplexTourRequest> _tourRequests;

        public ComplexTourRequestRepository()
        {
            _serializer = new Serializer<ComplexTourRequest>();
            _tourRequests = _serializer.FromCSV(FilePath);
        }
        public List<ComplexTourRequest> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public ComplexTourRequest Save(ComplexTourRequest tourRequest)
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

        public void Delete(ComplexTourRequest tourRequest)
        {
            _tourRequests = _serializer.FromCSV(FilePath);
            ComplexTourRequest found = _tourRequests.Find(r => r.Id == tourRequest.Id);
            _tourRequests.Remove(found);
            _serializer.ToCSV(FilePath, _tourRequests);
        }

        public ComplexTourRequest Update(ComplexTourRequest tourRequest)
        {
            _tourRequests = _serializer.FromCSV(FilePath);
            ComplexTourRequest current = _tourRequests.Find(r => r.Id == tourRequest.Id);
            int index = _tourRequests.IndexOf(current);
            _tourRequests.Remove(current);
            _tourRequests.Insert(index, tourRequest);
            _serializer.ToCSV(FilePath, _tourRequests);
            return tourRequest;
        }
    }

}