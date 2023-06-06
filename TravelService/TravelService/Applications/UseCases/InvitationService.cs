using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.Repository;

namespace TravelService.Applications.UseCases
{
    public class InvitationService
    {
        public readonly IInvitationRepository _invitationRepository;

        public InvitationService(IInvitationRepository invitationRepository)
        {
            _invitationRepository = invitationRepository;
        }

        public List<Invitation> GetAll()
        {
            return _invitationRepository.GetAll();
        }

        public Invitation Save(Invitation invitation)
        {
            return _invitationRepository.Save(invitation);
        }

        public void Delete(Invitation invitation)
        {
            _invitationRepository.Delete(invitation);
        }

        public Invitation Update(Invitation invitation)
        {
            return _invitationRepository.Update(invitation);
        }

    }
}
