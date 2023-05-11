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
    public class AccommodationRenovationRepository : IAccommodationRenovationRepository
    {
        private const string FilePath = "../../../Resources/Data/accommodationRenovations.csv";

        private readonly Serializer<AccommodationRenovation> _serializer;

        private List<AccommodationRenovation> _accommodationRenovations;

        public AccommodationRenovationRepository()
        {
            _serializer = new Serializer<AccommodationRenovation>();
            _accommodationRenovations = _serializer.FromCSV(FilePath);
        }

        public List<AccommodationRenovation> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public AccommodationRenovation Save(AccommodationRenovation accommodationRenovation)
        {
            accommodationRenovation.Id = NextId();
            _accommodationRenovations = _serializer.FromCSV(FilePath);
            _accommodationRenovations.Add(accommodationRenovation);
            _serializer.ToCSV(FilePath, _accommodationRenovations);
            return accommodationRenovation;
        }

        public int NextId()
        {
            _accommodationRenovations = _serializer.FromCSV(FilePath);
            if (_accommodationRenovations.Count < 1)
            {
                return 1;
            }
            return _accommodationRenovations.Max(c => c.Id) + 1;
        }

        public void Delete(AccommodationRenovation accommodationRenovation)
        {
            _accommodationRenovations = _serializer.FromCSV(FilePath);
            AccommodationRenovation founded = _accommodationRenovations.Find(c => c.Id == accommodationRenovation.Id);
            _accommodationRenovations.Remove(founded);
            _serializer.ToCSV(FilePath, _accommodationRenovations);
        }

        public AccommodationRenovation FindById(int id)
        {
            _accommodationRenovations = _serializer.FromCSV(FilePath);
            foreach (AccommodationRenovation renovation in _accommodationRenovations)
            {
                if (renovation.Id == id)
                {
                    return renovation;
                }
            }
            return null;
        }
        public AccommodationRenovation Update(AccommodationRenovation accommodationRenovation)
        {
            _accommodationRenovations = _serializer.FromCSV(FilePath);
            AccommodationRenovation current = _accommodationRenovations.Find(c => c.Id == accommodationRenovation.Id);
            int index = _accommodationRenovations.IndexOf(current);
            _accommodationRenovations.Remove(current);
            _accommodationRenovations.Insert(index, accommodationRenovation);
            _serializer.ToCSV(FilePath, _accommodationRenovations);
            return accommodationRenovation;
        }
    }
}
