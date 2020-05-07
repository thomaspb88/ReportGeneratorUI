using GalaSoft.MvvmLight.CommandWpf;
using Report.Components;
using ReportItemReader.Factory;
using ReportItemReader.Interface;
using System.Collections.ObjectModel;
using ReportDocument;

namespace ReportGenerator
{
    public class ViewModelReportGenerator : BaseViewModel

    {
        IReportItemReader repo = ReportItemReaderFactory.GetRepository();

        public RelayCommand UpdateFileDirectoryPathCommand { get; private set; }
        public RelayCommand ShowPopUpCommand { get; private set; }
        public RelayCommand AddTestsCommand { get; private set; }
        public RelayCommand RemoveTestCommand { get; private set; }
        public RelayCommand CreateReportCommand { get; private set; }

        #region Property - Test List for Combobox

        public ObservableCollection<ReportComponentBody> TestList
        {
            get 
            {
                IReportItemReader repo = ReportItemReaderFactory.GetRepository();
                if(repo.Status != ReportItemReaderState.Loaded)
                {
                    repo.Load(FileDirectoryPath);
                    return new ObservableCollection<ReportComponentBody>(repo.GetAllTestreportItems());
                }

                return new ObservableCollection<ReportComponentBody>()
                {
                   new ReportComponentBody(){ Title = "Error - Something went wrong" }
                };
            }
        }

        #endregion

        #region Property - Chosen Test List
        private ObservableCollection<ReportComponentBody> _chosenTests;

        public ObservableCollection<ReportComponentBody> ChosenTests
        {
            get 
            {
                return _chosenTests; 
            }
            set
            {
                _chosenTests = value;
            }
        }
        #endregion

        #region Property - Selected Test on Combobox
        private ReportComponentBody _selectedTest;

        public ReportComponentBody SelectedTest
        {
            get { return _selectedTest; }
            set 
            { 
                _selectedTest = value;
                RaisePropertyChanged("SelectedTest");
            }
        }
        #endregion

        #region Property - Selected Test on ListBox
        private ReportComponentBody _selectedListItem;

        public ReportComponentBody SelectedListItem
        {
            get { return _selectedListItem; }
            set
            {
                _selectedListItem = value;
                RaisePropertyChanged("SelectedListItem");
            }
        }

        #endregion

        #region Property - Customer

        private string _customer;

        public string Customer
        {
            get { return _customer; }
            set
            {
                if (_customer != value)
                {
                    _customer = value;
                    RaisePropertyChanged("Customer");
                }
            }
        }

        #endregion

        #region Property - Project

        private string _project;

        public string Project
        {
            get { return _project; }
            set 
            { 
                _project = value;
                RaisePropertyChanged("Project");
            
            }
        }

        #endregion

        #region Property - Address
        private string _address;

        public string Address
        {
            get { return _address; }
            set 
            { 
                _address = value;
                RaisePropertyChanged("Address");
            }
        }
        #endregion

        #region Property - Report Title
        private string _reportTitle;

        public string ReportTitle
        {
            get { return _reportTitle; }
            set 
            { 
                _reportTitle = value;
                RaisePropertyChanged("ReportTitle");
            }
        }

        public string FileDirectoryPath
        {
            get { return Properties.Settings.Default.testReportItemList; }
            set { Properties.Settings.Default.testReportItemList = value; }
        }

        #endregion

        #region Property - PopUpVisible

        private bool popUpVisible;

        public bool PopUpVisible
        {
            get { return popUpVisible; }
            set
            {
                popUpVisible = value;
                RaisePropertyChanged("PopUpVisible");
            }
        }

        #endregion

        #region Constructor - ReportGeneratorViewModel
        public ViewModelReportGenerator()
        {
            _chosenTests = new ObservableCollection<ReportComponentBody>();
            AddTestsCommand = new RelayCommand(() => ExecuteAddTestsToListCommand(), () => CanExecuteAddTestsCommand());
            RemoveTestCommand = new RelayCommand(() => ExecuteRemoveTestsCommand(), () => CanExecuteRemoveTestsCommand());
            CreateReportCommand = new RelayCommand(() => ExecuteCreateReportCommand(), () => CanExecuteCreateReportCommand());
            ShowPopUpCommand = new RelayCommand(() => ExecuteShowPopUpCommand(), () => CanExecuteShowPopUpCommand());
            UpdateFileDirectoryPathCommand = new RelayCommand(() => ExecuteUpdateFileDirectoryPath(), () => CanExecuteUpdateFileDirectoryPath());

            repo.Load(FileDirectoryPath);

       
        }
        #endregion

        #region RelayCommand : AddTestsCommand -  Execute and CanExecute
        private void ExecuteAddTestsToListCommand()
        {
            if (!_chosenTests.Contains(SelectedTest))
            { this._chosenTests.Add(SelectedTest); }
        }

        private bool CanExecuteAddTestsCommand()
        {
            if(SelectedTest != null)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region RelayCommand : RemoveTestsCommand -  Execute and CanExecute
        private void ExecuteRemoveTestsCommand()
        {
            if (_chosenTests.Contains(SelectedListItem))
            { this._chosenTests.Remove(SelectedListItem); }
        }

        private bool CanExecuteRemoveTestsCommand()
        {
            if (SelectedTest != null)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region RelayCommand : CreateReportCommand -  Execute and CanExecute
        private void ExecuteCreateReportCommand()
        {

            if (repo.Status != ReportItemReaderState.Unknown && ChosenTests.Count != 0)
            {
                TestReport testReport = new TestReport(); ; 
                testReport.LoadReportItems(ChosenTests);

                testReport.ReplaceWord("<<Customer>>", Customer);
                testReport.ReplaceWord("<<Address>>", Address.Replace("\n", ""));
                testReport.ReplaceWord("<<Project>>", Project);
                testReport.ReplaceWord("<<Title>>", ReportTitle);
                //testReport.AppendTests();
                //testReport.AppendReferences();
            }
        }

        private bool CanExecuteCreateReportCommand()
        {
            return !string.IsNullOrWhiteSpace(Customer) 
                && !string.IsNullOrWhiteSpace(ReportTitle)
                && !string.IsNullOrWhiteSpace(Project) 
                && !string.IsNullOrWhiteSpace(Address)
                && TestList.Count != 0 
                && repo.Status != ReportItemReaderState.Unknown;
        }
        #endregion

        #region RelayCommand : ShowPopUpCoomand - Execute and CanExecute
        private void ExecuteShowPopUpCommand()
        {
            PopUpVisible = !PopUpVisible;
        }

        private bool CanExecuteShowPopUpCommand()
        {
            return true;
        }
        #endregion

        private void ExecuteUpdateFileDirectoryPath()
        {
            if(FileDirectoryPath != null)
            {
                Properties.Settings.Default.Save();
            }  
        }

        private bool CanExecuteUpdateFileDirectoryPath()
        {
            return true;
        }

    }
}
