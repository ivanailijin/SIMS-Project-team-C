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
        public IOwnerRepository _ownerRepository;

        public OwnerService(IOwnerRepository ownerRepository)
        {
            _ownerRepository = ownerRepository;
        }

        public Owner GetByUsername(string username)
        {
            Owner owner = _ownerRepository.GetByUsername(username);
            return owner;
        }
        public Owner FindById(int id)
        {
            Owner owner = _ownerRepository.FindById(id);
            return owner;
        }
        public List<Owner> GetAll()
        {
            List<Owner> owners = _ownerRepository.GetAll();
            return owners;
        }
    }
}
