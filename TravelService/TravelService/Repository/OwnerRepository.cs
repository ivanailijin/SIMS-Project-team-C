using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.Serializer;

namespace TravelService.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private const string FilePath = "../../../Resources/Data/owners.csv";

        private readonly Serializer<Owner> _serializer;

        private List<Owner> _owners;

        public OwnerRatingRepository _ownerRatingRepository;

        public OwnerRepository()
        {
            _serializer = new Serializer<Owner>();
            _owners = _serializer.FromCSV(FilePath);
            _ownerRatingRepository = new OwnerRatingRepository();
        }

        public Owner GetByUsername(string username)
        {
            Owner owner = new Owner();
            _owners = _serializer.FromCSV(FilePath);
            owner = _owners.FirstOrDefault(u => u.Username == username);

            return owner;
        }

        public int NextId()
        {
            _owners = _serializer.FromCSV(FilePath);
            if (_owners.Count < 1)
            {
                return 1;
            }
            return _owners.Max(o => o.Id) + 1;
        }
        public Owner FindById(int id)
        {
            _owners = _serializer.FromCSV(FilePath);
            foreach (Owner owner in _owners)
            {
                if (owner.Id == id)
                {
                    return owner;
                }
            }
            return null;
        }
        public Owner Update(Owner owner)
        {
            _owners = _serializer.FromCSV(FilePath);
            Owner current = _owners.Find(c => c.Id == owner.Id);
            int index = _owners.IndexOf(current);
            _owners.Remove(current);
            _owners.Insert(index, owner);
            _serializer.ToCSV(FilePath, _owners);
            return owner;
        }
        public Owner Save(Owner owner)
        {
            owner.Id = NextId();
            _owners = _serializer.FromCSV(FilePath);
            _owners.Add(owner);
            _serializer.ToCSV(FilePath, _owners);
            return owner;
        }
        public List<Owner> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }
        
    }
}
