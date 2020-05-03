using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using TestReportItemRepository.Interface;

namespace ReportGenerator
{
    [ValueConversion(typeof(TestReportItemType),typeof(string))]
    public class TestReportItemTypeToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            var itemType = (TestReportItemType)value;

            switch (itemType)
            {
                case TestReportItemType.ProductStandard:
                    return "/Content/Images/Icon_ProductStandard.png";
                case TestReportItemType.TestStandard:
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
