using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportGenerator.ViewModel
{
    public class ViewModelReportGenerator
    {
        public ObservableCollection<string> testStandardList;

        private string customer;

        public string Customer
        {
            get { return customer; }
            set 
            { 
                if(customer != value)
                {
                    customer = value; 
                }          
            }
        }

        public ViewModelReportGenerator()
        {
            testStandardList = new ObservableCollection<string>() { "Hello", "World"};
        }

    }
}
