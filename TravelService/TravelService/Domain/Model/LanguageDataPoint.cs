using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelService.Domain.Model
{
    public class LanguageDataPoint
    {
        public int Id { get; set; }
        public string Language { get; set; }
        public string LanguageName { get; set; }
        public double RequestNumber { get; set; }
        public double LanguageRequestNumber { get; set; }
        public LanguageDataPoint(string language, double requestNumber)
        {
            Language = language;
            LanguageName = language;
            RequestNumber = requestNumber;
            LanguageRequestNumber = requestNumber;
        }
    }
}
