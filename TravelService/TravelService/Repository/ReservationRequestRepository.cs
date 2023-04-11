using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.Serializer;

namespace TravelService.Repository
{
    public class ReservationRequestRepository : IReservationRequestRepository
    {
        private const string FilePath = "../../../Resources/Data/reservationRequests.csv";


        private readonly Serializer<ReservationRequest> _serializer;

        private List<ReservationRequest> _reservationRequests;

        public ReservationRequestRepository()
        {
            _serializer = new Serializer<ReservationRequest>();
            _reservationRequests = _serializer.FromCSV(FilePath);
        }
        public List<ReservationRequest> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public ReservationRequest Save(ReservationRequest reservationRequest)
        {
            reservationRequest.Id = NextId();
            _reservationRequests = _serializer.FromCSV(FilePath);
            _reservationRequests.Add(reservationRequest);
            _serializer.ToCSV(FilePath, _reservationRequests);
            return reservationRequest;
        }

        public int NextId()
        {
            _reservationRequests = _serializer.FromCSV(FilePath);
            if (_reservationRequests.Count < 1)
            {
                return 1;
            }
            return _reservationRequests.Max(r => r.Id) + 1;
        }

        public void Delete(ReservationRequest reservationRequest)
        {
            _reservationRequests = _serializer.FromCSV(FilePath);
            ReservationRequest founded = _reservationRequests.Find(r => r.Id == reservationRequest.Id);
            _reservationRequests.Remove(founded);
            _serializer.ToCSV(FilePath, _reservationRequests);
        }

        public ReservationRequest Update(ReservationRequest reservationRequest)
        {
            _reservationRequests = _serializer.FromCSV(FilePath);
            ReservationRequest current = _reservationRequests.Find(r => r.Id == reservationRequest.Id);
            int index = _reservationRequests.IndexOf(current);
            _reservationRequests.Remove(current);
            _reservationRequests.Insert(index, reservationRequest);
            _serializer.ToCSV(FilePath, _reservationRequests);
            return reservationRequest;
        }

        public List<ReservationRequest> FindRequestsByGuestId(int guestId)
        {
            List<ReservationRequest> Requests = new List<ReservationRequest>();

            foreach (ReservationRequest request in _reservationRequests)
            {
                if (request.GuestId == guestId)
                {
                    Requests.Add(request);
                }
            }
            return Requests;
        }
    }
}
