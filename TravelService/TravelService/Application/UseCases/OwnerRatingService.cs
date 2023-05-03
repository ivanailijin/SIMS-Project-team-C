using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;

namespace TravelService.Application.UseCases
{
    public class OwnerRatingService
    {
        private IOwnerRatingRepository _ownerRatingRepository;

        public OwnerRatingService(IOwnerRatingRepository ownerRatingRepository)
        {
            _ownerRatingRepository = ownerRatingRepository;
        }

        public List<OwnerRating> GetAll()
        {
            return _ownerRatingRepository.GetAll();
        }

        public OwnerRating Save(OwnerRating ownerRating)
        {
            return _ownerRatingRepository.Save(ownerRating);
        }

        public OwnerRating FindById(int id)
        {
            return _ownerRatingRepository.FindById(id);
        }

        public OwnerRating Update(OwnerRating ownerRating)
        {
            return _ownerRatingRepository.Update(ownerRating);
        }

        public void Delete(OwnerRating ownerRating)
        {
            _ownerRatingRepository.Delete(ownerRating);
        }
    }
}
