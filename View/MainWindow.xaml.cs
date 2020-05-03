using System.Windows;

namespace ReportGenerator.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ViewModelReportGenerator _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new ViewModelReportGenerator();
            this.DataContext = _viewModel;
        }

    }
}
