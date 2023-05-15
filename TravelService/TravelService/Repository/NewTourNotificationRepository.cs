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
    public class NewTourNotificationRepository : INewTourNotificationRepository
    {
        private const string FilePath = "../../../Resources/Data/newTourNotification.csv";

        private readonly Serializer<NewTourNotification> _serializer;

        private List<NewTourNotification> _newTourNotifications;

        public NewTourNotificationRepository()
        {
            _serializer = new Serializer<NewTourNotification>();
            _newTourNotifications = _serializer.FromCSV(FilePath);
        }
        public List<NewTourNotification> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public NewTourNotification Save(NewTourNotification newTourNotification)
        {
            newTourNotification.Id = NextId();
            _newTourNotifications = _serializer.FromCSV(FilePath);
            _newTourNotifications.Add(newTourNotification);
            _serializer.ToCSV(FilePath, _newTourNotifications);
            return newTourNotification;
        }

        public int NextId()
        {
            _newTourNotifications = _serializer.FromCSV(FilePath);
            if (_newTourNotifications.Count < 1)
            {
                return 1;
            }
            return _newTourNotifications.Max(r => r.Id) + 1;
        }

        public void Delete(NewTourNotification newTourNotification)
        {
            _newTourNotifications = _serializer.FromCSV(FilePath);
            NewTourNotification found = _newTourNotifications.Find(r => r.Id == newTourNotification.Id);
            _newTourNotifications.Remove(found);
            _serializer.ToCSV(FilePath, _newTourNotifications);
        }

        public NewTourNotification Update(NewTourNotification newTourNotification)
        {
            _newTourNotifications = _serializer.FromCSV(FilePath);
            NewTourNotification current = _newTourNotifications.Find(r => r.Id == newTourNotification.Id);
            int index = _newTourNotifications.IndexOf(current);
            _newTourNotifications.Remove(current);
            _newTourNotifications.Insert(index, newTourNotification);
            _serializer.ToCSV(FilePath, _newTourNotifications);
            return newTourNotification;
        }
    }

}