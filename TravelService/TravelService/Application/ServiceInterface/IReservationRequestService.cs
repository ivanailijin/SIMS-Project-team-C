using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Domain.Model;

namespace TravelService.Application.ServiceInterface
{
    public interface IReservationRequestService
    {
        public List<ReservationRequest> GetAll();
        public ReservationRequest Save(ReservationRequest reservationRequest);
        public void Delete(ReservationRequest reservationRequest);
        public void Update(ReservationRequest reservationRequest);
    }
}
