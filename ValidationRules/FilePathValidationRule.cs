using System.Globalization;
using System.IO;
using System.Windows.Controls;

namespace ReportGenerator
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
