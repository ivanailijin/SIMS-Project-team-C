using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Domain.Model;
using TravelService.Repository;
using TravelService.Serializer;

namespace TravelService.Application.UseCases
{
    public class Guest1Service
    {
        private readonly Guest1Repository _repository;

        public Guest1Service(Guest1Repository repository)
        {
            _repository = repository;
        }

        public Guest1 GetByUsername(string username)
        {
            Guest1 guest = _repository.GetByUsername(username);
            return guest;
        }
        public Guest1 FindById(int id)
        {
            Guest1 guest = _repository.FindById(id);
            return guest;
        }
        public List<Guest1> GetAll()
        {
            return _repository.GetAll();
        }
    }
}
