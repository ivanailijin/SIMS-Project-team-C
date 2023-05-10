using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Domain.Model;
using TravelService.Serializer;

namespace TravelService.Domain.RepositoryInterface
{
    public interface IOwnerRatingRepository
    {
        public List<OwnerRating> GetAll();
        public OwnerRating Save(OwnerRating ownerRating);

        public int NextId();

        public void Delete(OwnerRating ownerRating);

        public OwnerRating FindById(int id);

        public OwnerRating Update(OwnerRating ownerRating);
    }
}
