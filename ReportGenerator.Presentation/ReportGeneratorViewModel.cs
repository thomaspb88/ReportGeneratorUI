using Report.Components;
using ReportItemReader.Factory;
using ReportItemReader.Interface;
using System.Collections.ObjectModel;
using ReportDocument;
using GalaSoft.MvvmLight.CommandWpf;
using ReportItemReader.XML;
using System.Collections.Generic;
using System.ComponentModel;
using System;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;

namespace ReportGenerator.Presentation
{
    public class ReportGeneratorViewModel : BaseViewModel, INotifyDataErrorInfo

    {
        //IReportItemReader repo = ReportItemReaderFactory.GetRepository();
        protected IReportItemReader DataReader;

        #region Property - Test List for Combobox

        private ObservableCollection<ReportComponentBody> testList;

        public ObservableCollection<ReportComponentBody> TestList
        {
            get 
            {
                if (DataReader.Status == ReportItemReaderState.Loaded)
                {
                    return new ObservableCollection<ReportComponentBody>(DataReader.GetAllReportItems());
                }
                else
                {
                    return new ObservableCollection<ReportComponentBody>()
                    {
                       new ReportComponentBody(){ Title = "Error - Something went wrong" }
                    };
                }

            }

            set
            {
                testList = value;
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
                    ValidateCustomerField();
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
                ValidateProjectnumberField();
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
                ValidateAddressField();
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

        public ReportGeneratorViewModel(IReportItemReader dataReader)
        {
            DataReader = dataReader;
            DataReader.Load(FileDirectoryPath);

            _chosenTests = new ObservableCollection<ReportComponentBody>();
            AddTestsCommand = new RelayCommand(() => ExecuteAddTestsToListCommand(), () => CanExecuteAddTestsCommand());
            RemoveTestCommand = new RelayCommand(() => ExecuteRemoveTestsCommand(), () => CanExecuteRemoveTestsCommand());
            CreateReportCommand = new RelayCommand(() => ExecuteCreateReportCommand(), () => CanExecuteCreateReportCommand());
            ShowPopUpCommand = new RelayCommand(() => ExecuteShowPopUpCommand(), () => CanExecuteShowPopUpCommand());
            UpdateFileDirectoryPathCommand = new RelayCommand(() => ExecuteUpdateFileDirectoryPath(), () => CanExecuteUpdateFileDirectoryPath());
        }
        #endregion

        public RelayCommand UpdateFileDirectoryPathCommand { get; private set; }
        public RelayCommand ShowPopUpCommand { get; private set; }
        public RelayCommand AddTestsCommand { get; private set; }
        public RelayCommand RemoveTestCommand { get; private set; }
        public RelayCommand CreateReportCommand { get; private set; }



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
            if (DataReader.Status != ReportItemReaderState.Unknown && ChosenTests.Count != 0)
            {
                TestReport testReport = new TestReport(); ;
                testReport.OpenDocument(Properties.Settings.Default.DataSourceFilePath)
;               testReport.LoadReportItems(ChosenTests);
                testReport.WriteReport();
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
            var FormHasErrors = _errorsByPropertyName.Count > 0;

            return !FormHasErrors
                && TestList.Count != 0 
                && DataReader.Status != ReportItemReaderState.Unknown;
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

        private void ValidateCustomerField()
        {
            ClearErrors(nameof(Customer));
            if (string.IsNullOrWhiteSpace(Customer))
                AddError(nameof(Customer), "Customer cannot be empty");

        }

        private void ValidateAddressField()
        {
            ClearErrors(nameof(Address));
            if (string.IsNullOrWhiteSpace(Address))
                AddError(nameof(Address), "Address cannot be empty");

        }

        private void ValidateProjectnumberField()
        {
            Match match = Regex.Match(Project, @"P\d{6}\-\d{4}");

            ClearErrors(nameof(Project));
            if (!match.Success)
                AddError(nameof(Project), "Incorrect format for project number. Example P123456-1000");

        }

        private readonly Dictionary<string, List<string>> _errorsByPropertyName = new Dictionary<string, List<string>>();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public bool HasErrors => _errorsByPropertyName.Any();

        private void AddError(string propertyName, string error)
        {
            if (!_errorsByPropertyName.ContainsKey(propertyName))
                _errorsByPropertyName[propertyName] = new List<string>();

            if (!_errorsByPropertyName[propertyName].Contains(error))
            {
                _errorsByPropertyName[propertyName].Add(error);
                OnErrorsChanged(propertyName);
            }
        }

        private void ClearErrors(string propertyName)
        {
            if (_errorsByPropertyName.ContainsKey(propertyName))
            {
                _errorsByPropertyName.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }
        }

        public IEnumerable GetErrors(string propertyName)
        {
            return _errorsByPropertyName.ContainsKey(propertyName) ?
                _errorsByPropertyName[propertyName] : null;
        }

        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }
}
