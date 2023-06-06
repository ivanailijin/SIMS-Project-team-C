using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace TravelService.Validation
{
    public class NotEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (string.IsNullOrEmpty(value?.ToString()))
            {
                return new ValidationResult(false, "Polje je obavezno!");
            }

            return ValidationResult.ValidResult;
        }
    }

    public class NumberValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            try
            {
                var s = value as string;
                double r;
                if (string.IsNullOrEmpty(s))
                {
                    return new ValidationResult(false, "Polje je obavezno!");
                }
                if (double.TryParse(s, out r))
                {
                    return new ValidationResult(true, null);
                }

                return new ValidationResult(false, "Dozvoljen unos samo brojeva!");
            }
            catch
            {
                return new ValidationResult(false, "Unknown error occured.");
            }
        }
    }

    public class LetterValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            try
            {
                var s = value as string;
                if (string.IsNullOrEmpty(s))
                {
                    return new ValidationResult(false, "Polje je obavezno!");
                }
                if (Regex.IsMatch(s, "^[a-zA-Z ]*$"))
                {
                    return new ValidationResult(true, null);
                }
                return new ValidationResult(false, "Dozvoljen unos samo slova!");
            }
            catch
            {
                return new ValidationResult(false, "Unknown error occured.");
            }
        }
    }

    public class PictureValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            try
            {
                var s = value as string;
                Regex r = new Regex("^[^,]+(,[^,]+)*$");
                if (r.IsMatch(s))
                {
                    return new ValidationResult(true, null);
                }
                return new ValidationResult(false, "Pogresan format unosa!");
            }
            catch
            {
                return new ValidationResult(false, "Unknown error occured.");
            }
        }
    }
    public class LocationValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            try
            {
                var s = value as string;
                Regex r = new Regex("^[a-zA-Z]+(\\s+[a-zA-Z]+)*,\\s?[a-zA-Z]+(\\s+[a-zA-Z]+)*$");
                if (r.IsMatch(s))
                {
                    return new ValidationResult(true, null);
                }
                return new ValidationResult(false, "Zahtevana forma: 'grad, drzava' ");
            }
            catch
            {
                return new ValidationResult(false, "Unknown error occured.");
            }
        }
    }
    public class DateTimeValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            try
            {
                var s = value as string;
                DateTime dt;
                if (!DateTime.TryParseExact(s, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                {
                    return new ValidationResult(false, "Invalid date format. The expected format is 'M/d/yyyy h:mm:ss tt'.");
                }
                if (dt < DateTime.Now)
                {
                    return new ValidationResult(false, "Date must be in the future.");
                }
                return ValidationResult.ValidResult;
            }
            catch
            {
                return new ValidationResult(false, "Unknown error occurred.");
            }
        }
    }

    public class DateValidation : ValidationRule
    {
        private const string DateFormat = "dd.MM.yyyy.";

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var dateString = value as string;

            if (string.IsNullOrEmpty(dateString))
            {
                return new ValidationResult(false, "Polje ne sme biti prazno!");
            }

            DateOnly parsedDate;
            if (DateOnly.TryParseExact(dateString, DateFormat, cultureInfo, DateTimeStyles.None, out parsedDate))
            {
                return new ValidationResult(true, null);
            }
            return new ValidationResult(false, $"Datum mora biti u formatu {DateFormat}");
        }
    }

    public class EmailValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null || !(value is string text) || string.IsNullOrEmpty(text.Trim()))
                return new ValidationResult(false, "Polje je obavezno");

            Regex regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (!regex.IsMatch(text))
                return new ValidationResult(false, "Neispravan format email-a");

            return ValidationResult.ValidResult;
        }
    }
}
