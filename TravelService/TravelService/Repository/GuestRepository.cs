using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TravelService.Model;
using TravelService.Serializer;

namespace TravelService.Repository
{
    public class GuestRepository
    {
        private const string FilePath = "../../../Resources/Data/guest.csv";

        private readonly Serializer<Guest> _serializer;

        private List<Guest> _guest;

        public GuestRepository()
        {
            _serializer = new Serializer<Guest>();
            _guest = _serializer.FromCSV(FilePath);
        }

        public List<Guest> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public Guest Save(Guest guest)
        {
            guest.Id = NextId();
            _guest = _serializer.FromCSV(FilePath);
            _guest.Add(guest);
            _serializer.ToCSV(FilePath, _guest);
            return guest;
        }

        public int NextId()
        {
            _guest = _serializer.FromCSV(FilePath);
            if (_guest.Count < 1)
            {
                return 1;
            }
            return _guest.Max(c => c.Id) + 1;
        }

        public void Delete(Guest guest)
        {
            _guest = _serializer.FromCSV(FilePath);
            Guest founded = _guest.Find(c => c.Id == guest.Id);
            _guest.Remove(founded);
            _serializer.ToCSV(FilePath, _guest);
        }

        public Guest Update(Guest guest)
        {
            _guest = _serializer.FromCSV(FilePath);
            Guest current = _guest.Find(c => c.Id == guest.Id);
            int index = _guest.IndexOf(current);
            _guest.Remove(current);
            _guest.Insert(index, guest);       // keep ascending order of ids in file
            _serializer.ToCSV(FilePath, _guest);
            return guest;
        }

    }
}