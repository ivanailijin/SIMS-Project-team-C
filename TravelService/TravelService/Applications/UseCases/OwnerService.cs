using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Applications.Utils;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.Repository;
using TravelService.Serializer;

namespace TravelService.Applications.UseCases
{
    public class OwnerService
    {
        private readonly IOwnerRepository _ownerRepository;

        public OwnerService(IOwnerRepository ownerRepository)
        {
            _ownerRepository = ownerRepository;
        }
        public Owner GetByUsername(string username)
        {
            Owner owner = _ownerRepository.GetByUsername(username);
            owner.NumberOfRatings = 52;
            owner.AverageRating = 4.7;
            owner.SuperOwner = true;

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
        public int NextId()
        {
            return _ownerRepository.NextId();
        }
        public Owner Update(Owner owner)
        {
            return _ownerRepository.Update(owner);
        }
        public Owner Save(Owner owner)
        {
            return _ownerRepository.Save(owner);
        }
    }
}
