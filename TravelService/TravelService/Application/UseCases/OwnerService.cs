using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;

namespace TravelService.Application.UseCases
{
    public class OwnerService
    {
        private readonly IOwnerRepository _repository;

        public OwnerService(IOwnerRepository repository)
        {
            _repository = repository;
        }

        public Owner GetByUsername(string username)
        {
            Owner owner = _repository.GetByUsername(username);
            return owner;
        }
        public Owner FindById(int id)
        {
            Owner owner = _repository.FindById(id);
            return owner;
        }
        public List<Owner> GetAll()
        {
            List<Owner> owners = _repository.GetAll();
            return owners;
        }
    }
}
