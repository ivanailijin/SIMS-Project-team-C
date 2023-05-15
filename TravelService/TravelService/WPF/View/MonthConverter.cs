using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TravelService.WPF.View
{
    class MonthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int monthNumber)
            {
                CultureInfo serbianCulture = new CultureInfo("sr-Latn-RS");
                string monthName = serbianCulture.DateTimeFormat.GetMonthName(monthNumber);

                string formattedMonthName = char.ToUpper(monthName[0]) + monthName.Substring(1);

                return formattedMonthName;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
