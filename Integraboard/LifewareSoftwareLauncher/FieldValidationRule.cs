using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LifewareSoftwareLauncher
{
    public class FieldValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {


            string sValue = (value ?? "").ToString();

            if (string.IsNullOrWhiteSpace(sValue))
            {
                return new ValidationResult(false, "Campo requerido.");
            }
            else if (sValue.Length < 4)
            {
                return new ValidationResult(false, "Debe contener al menos 4 caracteres.");
            }

            return ValidationResult.ValidResult;
        }
    }
}
