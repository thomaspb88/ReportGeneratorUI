using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ReportGenerator.ValidationRules
{
    public class FilePathValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            bool isValidFilePath = File.Exists((string)value);
            return new ValidationResult(isValidFilePath, "This file does not exist");
        }
    }
}
