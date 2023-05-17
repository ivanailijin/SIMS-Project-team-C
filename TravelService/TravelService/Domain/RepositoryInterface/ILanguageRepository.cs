using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Domain.Model;

namespace TravelService.Domain.RepositoryInterface
{
    public interface ILanguageRepository
    {
        public List<Language> GetAll();

        public Language Save(Language language);

        public int NextId();

        public void Delete(Language language);

        public Language Update(Language language);
        public Language GetById(int id);
        public Language GetLanguageByName(string name);
    }
}
