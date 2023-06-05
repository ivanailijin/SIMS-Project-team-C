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
    public class ForumRepository : IForumRepository
    {
        private const string FilePath = "../../../Resources/Data/forums.csv";

        private readonly Serializer<Forum> _serializer;

        private List<Forum> _forums;

        public ForumRepository()
        {
            _serializer = new Serializer<Forum>();
            _forums = _serializer.FromCSV(FilePath);
        }

        public List<Forum> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public Forum Save(Forum forum)
        {
            forum.Id = NextId();
            _forums = _serializer.FromCSV(FilePath);
            _forums.Add(forum);
            _serializer.ToCSV(FilePath, _forums);
            return forum;
        }

        public int NextId()
        {
            _forums = _serializer.FromCSV(FilePath);
            if (_forums.Count < 1)
            {
                return 1;
            }
            return _forums.Max(c => c.Id) + 1;
        }

        public void Delete(Forum forum)
        {
            _forums = _serializer.FromCSV(FilePath);
            Forum founded = _forums.Find(c => c.Id == forum.Id);
            _forums.Remove(founded);
            _serializer.ToCSV(FilePath, _forums);
        }
        public Forum FindById(int id)
        {
            _forums = _serializer.FromCSV(FilePath);
            foreach (Forum forum in _forums)
            {
                if (forum.Id == id)
                {
                    return forum;
                }
            }
            return null;
        }

        public Forum Update(Forum forum)
        {
            _forums = _serializer.FromCSV(FilePath);
            Forum current = _forums.Find(c => c.Id == forum.Id);
            int index = _forums.IndexOf(current);
            _forums.Remove(current);
            _forums.Insert(index, forum);
            _serializer.ToCSV(FilePath, _forums);
            return forum;
        }
    }
}
