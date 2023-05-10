using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;

using TravelService.Serializer;

namespace TravelService.Repository
{
    public class AccommodationRepository : IAccommodationRepository
    {
        private const string FilePath = "../../../Resources/Data/accommodations.csv";

        private readonly Serializer<Accommodation> _serializer;

        private List<Accommodation> _accommodations;

        public AccommodationRepository()
        {
            _serializer = new Serializer<Accommodation>();
            _accommodations = _serializer.FromCSV(FilePath);
        }

        public List<Accommodation> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public List<Accommodation> GetOwnersAccommodations(int ownerId)
        {
            List<Accommodation> ownersAccommodations = new List<Accommodation>();
            foreach(Accommodation accommodation in _accommodations)
            {
                if(accommodation.OwnerId == ownerId)
                {
                    ownersAccommodations.Add(accommodation);
                }
            }

            return ownersAccommodations;
        }

        public Accommodation Save(Accommodation accommodation)
        {
            accommodation.Id = NextId();
            _accommodations = _serializer.FromCSV(FilePath);
            _accommodations.Add(accommodation);
            _serializer.ToCSV(FilePath, _accommodations);
            return accommodation;
        }

        public int NextId()
        {
            _accommodations = _serializer.FromCSV(FilePath);
            if (_accommodations.Count < 1)
            {
                return 1;
            }
            return _accommodations.Max(c => c.Id) + 1;
        }

        public void Delete(Accommodation accommodation)
        {
            _accommodations = _serializer.FromCSV(FilePath);
            Accommodation founded = _accommodations.Find(c => c.Id == accommodation.Id);
            _accommodations.Remove(founded);
            _serializer.ToCSV(FilePath, _accommodations);
        }
        public Accommodation FindById(int id)
        {
            _accommodations = _serializer.FromCSV(FilePath);
            foreach (Accommodation accommodation in _accommodations)
            {
                if (accommodation.Id == id)
                {
                    return accommodation;
                }
            }
            return null;
        }

        public Accommodation Update(Accommodation accommodation)
        {
            _accommodations = _serializer.FromCSV(FilePath);
            Accommodation current = _accommodations.Find(c => c.Id == accommodation.Id);
            int index = _accommodations.IndexOf(current);
            _accommodations.Remove(current);
            _accommodations.Insert(index, accommodation);      
            _serializer.ToCSV(FilePath, _accommodations);
            return accommodation;
        }
    }
}
