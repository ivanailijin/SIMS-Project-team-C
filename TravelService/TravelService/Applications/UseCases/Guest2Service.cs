using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.Repository;
using TravelService.Serializer;

namespace TravelService.Applications.UseCases
{
    
    public class Guest2Service

    {
        private readonly IGuest2Repository _repository;

        public Guest2Service(IGuest2Repository repository)
        {
            _repository = repository;
        }

        public Guest2 GetByUsername(string username)
        {
            Guest2 guest = _repository.GetByUsername(username);
            return guest;
        }
        public Guest2 FindById(int id)
        {
            Guest2 guest = _repository.FindById(id);
            return guest;
        }
        public List<Guest2> GetAll()
        {
            return _repository.GetAll();
        }
    }

}

