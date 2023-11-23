using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace lab1.ValidationRules
{
    public class EmptyRule : ValidationRule
    {
        public override ValidationResult Validate(object value,
       System.Globalization.CultureInfo cultureInfo)
        {
            if(value == null)
                return new ValidationResult(false, "Поле не заполнено!");

            string email = value.ToString();
            if (email!=String.Empty)
            {
                PageProfile.canSave = true;
                return new ValidationResult(true, null);
            }
            else
            {
                PageProfile.canSave = false;
                return new ValidationResult(false,
                   "Поле должно содержать информацию!");
            }
        }
    }
}
