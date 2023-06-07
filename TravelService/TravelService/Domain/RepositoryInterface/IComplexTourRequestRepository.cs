using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TravelService.Domain.Model;
using System.Threading.Tasks;

namespace TravelService.Domain.RepositoryInterface
{
    public interface IComplexTourRequestRepository
    {
        public List<ComplexTourRequest> GetAll();

        public ComplexTourRequest Save(ComplexTourRequest tourRequest);

        public int NextId();

        public void Delete(ComplexTourRequest tourRequest);

        public ComplexTourRequest Update(ComplexTourRequest tourRequest);
    }
}