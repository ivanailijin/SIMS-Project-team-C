using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.Repository;
using TravelService.Serializer;

namespace TravelService.Application.UseCases
{
    public class AccommodationService
    {
        private readonly IAccommodationRepository _accommodationRepository;

        public AccommodationService(IAccommodationRepository accommodationRepository)
        {
            _accommodationRepository = accommodationRepository;
        }

        public List<Accommodation> GetAll()
        {
            return _accommodationRepository.GetAll();
        }
        public Accommodation Save(Accommodation accommodation)
        {
            Accommodation savedAccommodation = _accommodationRepository.Save(accommodation);
            return savedAccommodation;
        }

        public void Delete(Accommodation accommodation)
        {
            _accommodationRepository.Delete(accommodation);
        }
        public Accommodation FindById(int id)
        {
            Accommodation accommodation = _accommodationRepository.FindById(id);
            return accommodation;
        }

        public Accommodation Update(Accommodation accommodation)
        {
            Accommodation updatedAccommodation = _accommodationRepository.Update(accommodation);
            return updatedAccommodation;
        }
    }
}
