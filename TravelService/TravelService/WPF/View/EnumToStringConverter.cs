using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TravelService.WPF.View
{
    public class EnumToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TYPE enumValue)
            {
                if (enumValue == TYPE.HOUSE)
                    return "Kuca";
                else if (enumValue == TYPE.APARTMENT)
                    return "Apartman";
                else if (enumValue == TYPE.COTTAGE)
                    return "Koliba";
            }
            else if (value is STATUS secondEnumValue)
            {
                if (secondEnumValue == STATUS.Approved)
                    return "Odobren";
                else if (secondEnumValue == STATUS.OnHold)
                    return "Na cekanju";
                else if (secondEnumValue == STATUS.Rejected)
                    return "Odbijen";
            }
            else if(value is AVAILABILITY thirdEnumValue)
            {
                if (thirdEnumValue == AVAILABILITY.Available)
                    return "Slobodan";
                else if (thirdEnumValue == AVAILABILITY.Unavailable)
                    return "Zauzet";
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
