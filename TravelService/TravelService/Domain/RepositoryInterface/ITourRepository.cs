using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TravelService.Domain.Model;
using System.Threading.Tasks;

namespace TravelService.Domain.RepositoryInterface
{
    public interface ITourRepository
    {
        public List<Tour> GetAll();

        public Tour Save(Tour tour);

        public int NextId();

        public void Delete(Tour tour);

        public Tour Update(Tour tour);
    }
}