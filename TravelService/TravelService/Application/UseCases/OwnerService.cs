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
            return _ownerRepository.GetByUsername(username);
        }

        public Owner FindById(int id)
        {
            return _ownerRepository.FindById(id);
        }

        public List<Owner> GetAll()
        {
            return _ownerRepository.GetAll();
        }
    }
}
