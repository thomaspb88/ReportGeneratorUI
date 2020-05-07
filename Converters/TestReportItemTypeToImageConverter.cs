using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using TestReport.Components;
using TestReportItemReader.Interface;

namespace ReportGenerator
{
    [ValueConversion(typeof(ReportItemType),typeof(string))]
    public class TestReportItemTypeToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            var itemType = (ReportItemType)value;

            switch (itemType)
            {
                case ReportItemType.ProductStandard:
                    return "/Content/Images/Icon_ProductStandard.png";
                case ReportItemType.TestStandard:
                    return "/Content/Images/Icon_TestStandard.png";
                default:
                    return "/Content/Images/Icon_QuestionMark.png";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
