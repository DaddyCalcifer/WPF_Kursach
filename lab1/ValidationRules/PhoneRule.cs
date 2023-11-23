using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace lab1.ValidationRules
{
    public class PhoneRule : ValidationRule
    {
        public override ValidationResult Validate(object value,
       System.Globalization.CultureInfo cultureInfo)
        {
            if (value == null)
            {
                PageMain.canSave = false;
                return new ValidationResult(false, "Номер телефона введён некорректно! ");
            }
            string phone = value.ToString();
            Regex regex = new Regex("^[\\+]{0,1}[0-9]{11}$");
            if (regex.IsMatch(phone))
            {
                PageMain.canSave = true;
                PageProfile.canSave = true;
                return new ValidationResult(true, null);
            }
            else
            {
                PageMain.canSave = false;
                PageProfile.canSave = false;
                return new ValidationResult(false,
                   "Номер телефона должен иметь формат +7XXXXXXXXXX или 8XXXXXXXXXX");
            }
        }
    }
}
