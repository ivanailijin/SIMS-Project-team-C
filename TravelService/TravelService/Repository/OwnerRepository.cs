using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Model;
using TravelService.Serializer;

namespace TravelService.Repository
{
    public class OwnerRepository
    {
        private const string FilePath = "../../../Resources/Data/owners.csv";

        private readonly Serializer<Owner> _serializer;

        private List<Owner> _owners;

        public OwnerRepository()
        {
            _serializer = new Serializer<Owner>();
            _owners = _serializer.FromCSV(FilePath);
        }

        public Owner GetByUsername(string username)
        {
            _owners = _serializer.FromCSV(FilePath);
            return _owners.FirstOrDefault(u => u.Username == username);
        }
        
    }
}
