using ReportGenerator.Presentation;
using System.Windows;

namespace ReportGenerator.View
{
    public partial class ReportGeneratorWindow : Window
    {
        ReportGeneratorViewModel viewModel;

        public ReportGeneratorWindow(ReportGeneratorViewModel reportgeneratorViewModel)
        {
            InitializeComponent();
            viewModel = reportgeneratorViewModel;
            this.DataContext = viewModel;
        }

    }
}
