using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Domain.Model;

namespace TravelService.Domain.RepositoryInterface
{
    public interface IInvitationRepository
    {
        public List<Invitation> GetAll();

        public Invitation Save(Invitation invitation);

        public int NextId();

        public void Delete(Invitation invitation);

        public Invitation Update(Invitation invitation);

    }
}
