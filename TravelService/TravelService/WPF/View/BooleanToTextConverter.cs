﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace TravelService.WPF.View
{
    public class BooleanToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is true)
            {
                return "Super Owner";
            }

            return "User is not the super owner";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
