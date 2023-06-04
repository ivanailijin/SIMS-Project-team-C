using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Application.Utils;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.Repository;
using TravelService.Serializer;

namespace TravelService.Application.UseCases
{
    public class OwnerService
    {
        private readonly IOwnerRepository _ownerRepository;

        //private readonly OwnerRatingService _ownerRatingService;

        public OwnerService(IOwnerRepository ownerRepository)
        {
            _ownerRepository = ownerRepository;
            //_ownerRatingService = new OwnerRatingService(Injector.CreateInstance<IOwnerRatingRepository>());
        }
        public Owner GetByUsername(string username)
        {
            Owner owner = _ownerRepository.GetByUsername(username);
            //owner = CheckIsSuperOwner(owner);
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
        /*public Owner CheckIsSuperOwner(Owner owner)
        {
            owner.NumberOfRatings = _ownerRatingService.GetNumberOfRatings(owner.Id);
            owner.AverageRating = _ownerRatingService.GetAverageRating(owner.Id);

            if (owner.NumberOfRatings >= 3 && owner.AverageRating > 4.5)
            {
                owner.SuperOwner = true;
            }
            else
            {
                owner.SuperOwner = false;
            }
            Update(owner);

            return owner;
        }*/
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
