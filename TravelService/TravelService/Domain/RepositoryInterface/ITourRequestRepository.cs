using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TravelService.Domain.Model;
using System.Threading.Tasks;

namespace TravelService.Domain.RepositoryInterface
{
    public interface ITourRequestRepository
    {
        public List<TourRequest> GetAll();

        public TourRequest Save(TourRequest tourRequest);

        public int NextId();

        public void Delete(TourRequest tourRequest);

        public TourRequest Update(TourRequest tourRequest);
    }
}