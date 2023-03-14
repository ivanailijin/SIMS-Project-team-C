using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Model;
using TravelService.Serializer;

namespace TravelService.Repository
{
    public class LanguageRepository
    {

        private const string FilePath = "../../../Resources/Data/language.csv";

        private readonly Serializer<Language> _serializer;

        private List<Language> _languages;

        public LanguageRepository()
        {
            _serializer = new Serializer<Language>();
            _languages = _serializer.FromCSV(FilePath);
        }
        public List<Language> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }
    }
}
