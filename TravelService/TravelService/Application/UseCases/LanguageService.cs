using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.Repository;

namespace TravelService.Application.UseCases
{
    public class LanguageService
    {
        private readonly ILanguageRepository _languageRepository;

        public LanguageService(ILanguageRepository languageRepository)
        {
            _languageRepository = languageRepository;
        }

        public List<Language> GetAll()
        {
            return _languageRepository.GetAll();
        }

        public Language Save(Language language)
        {
            return _languageRepository.Save(language);
        }

        public void Delete(Language language)
        {
            _languageRepository.Delete(language);
        }

        public Language Update(Language language)
        {
            return _languageRepository.Update(language);
        }
    }
}
