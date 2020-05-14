using ReportGenerator.Presentation;
using ReportGenerator.View;
using ReportItemReader.XML;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ReportGenerator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var reader = new XMLReportItemReader();
            var viewModel = new ReportGeneratorViewModel(reader);
            Application.Current.MainWindow = new ReportGeneratorWindow(viewModel);
            Application.Current.MainWindow.Show();
        }
    }
}
