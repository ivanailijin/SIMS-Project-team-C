using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TravelService.Domain.Model;
using System.Threading.Tasks;

namespace TravelService.Domain.RepositoryInterface
{
    public interface INewTourNotificationRepository
    {
        public List<NewTourNotification> GetAll();

        public NewTourNotification Save(NewTourNotification newTourNotification);

        public int NextId();

        public void Delete(NewTourNotification newTourNotification);

        public NewTourNotification Update(NewTourNotification newTourNotification);
    }
}