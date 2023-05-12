﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.Serializer;

namespace TravelService.Repository
{
    public class RenovationRecommendationRepository : IRenovationRecommendationRepository
    {
        private const string FilePath = "../../../Resources/Data/renovationRecommendations.csv";

        private readonly Serializer<RenovationRecommendation> _serializer;

        private List<RenovationRecommendation> _renovationRecommendations;

        public RenovationRecommendationRepository()
        {
            _serializer = new Serializer<RenovationRecommendation>();
            _renovationRecommendations = _serializer.FromCSV(FilePath);
        }

        public List<RenovationRecommendation> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public RenovationRecommendation Save(RenovationRecommendation renovationRecommendation)
        {
            renovationRecommendation.Id = NextId();
            _renovationRecommendations = _serializer.FromCSV(FilePath);
            _renovationRecommendations.Add(renovationRecommendation);
            _serializer.ToCSV(FilePath, _renovationRecommendations);
            return renovationRecommendation;
        }

        public int NextId()
        {
            _renovationRecommendations = _serializer.FromCSV(FilePath);
            if (_renovationRecommendations.Count < 1)
            {
                return 1;
            }
            return _renovationRecommendations.Max(r => r.Id) + 1;
        }

        public void Delete(RenovationRecommendation renovationRecommendation)
        {
            _renovationRecommendations = _serializer.FromCSV(FilePath);
            RenovationRecommendation founded = _renovationRecommendations.Find(r => r.Id == renovationRecommendation.Id);
            _renovationRecommendations.Remove(founded);
            _serializer.ToCSV(FilePath, _renovationRecommendations);
        }

        public RenovationRecommendation Update(RenovationRecommendation renovationRecommendation)
        {
            _renovationRecommendations = _serializer.FromCSV(FilePath);
            RenovationRecommendation current = _renovationRecommendations.Find(r => r.Id == renovationRecommendation.Id);
            int index = _renovationRecommendations.IndexOf(current);
            _renovationRecommendations.Remove(current);
            _renovationRecommendations.Insert(index, renovationRecommendation);
            _serializer.ToCSV(FilePath, _renovationRecommendations);
            return renovationRecommendation;
        }
    }
}
