using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StockMonitor.Core;
using StockMonitor.Data;
using StockMonitor.Enums;
using StockMonitor.Models;
using StockMonitor.Views;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace StockMonitor.ViewModels
{
    /// <summary>
    /// the view model of the main window
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// The service provider to take dependent services
        /// </summary>
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// The logger for this view model
        /// </summary>
        private readonly ILogger<MainViewModel> _logger;

        /// <summary>
        /// The database service to be used for operations
        /// </summary>
        private readonly DatabaseService _db;

        /// <summary>
        /// The type options, will come from database
        /// </summary>
        public ObservableCollection<OptionItem> TypeOptions { get; } = new();
        /// <summary>
        /// The company options, will come from database
        /// </summary>
        public ObservableCollection<OptionItem> CompanyOptions { get; } = new();

        /// <summary>
        /// The selected option
        /// </summary>
        private OptionItem? _selectedType;
        /// <summary>
        /// the selection option for public use
        /// </summary>
        public OptionItem? SelectedOption
        {
            get => _selectedType;
            set
            {
                SetProperty(ref _selectedType, value);
                _ = LoadCompaniesAsync();
            }
        }

        /// <summary>
        /// The query or company name for search
        /// </summary>
        private OptionItem _selectedCompany;
        /// <summary>
        /// The actual query typed
        /// </summary>
        public OptionItem SelectedCompanyKey
        {
            get => _selectedCompany;
            set => SetProperty(ref _selectedCompany, value);
        }

        /// <summary>
        /// The list of companies to check today in daily frame
        /// </summary>
        public ObservableCollection<OptionItem> DailyCompanies { get; } = new();

        /// <summary>
        /// The list of companies to check today in weekly frame
        /// </summary>
        public ObservableCollection<OptionItem> WeeklyCompanies { get; } = new();

        /// <summary>
        /// The list of companies to check today in monthly frame
        /// </summary>
        public ObservableCollection<OptionItem> MonthlyCompanies { get; } = new();

        /// <summary>
        /// The current date to display and use
        /// </summary>
        public DateTime CurrentDate => DateTime.Now;

        /// <summary>
        /// The command to search the companies
        /// </summary>
        public ICommand SearchCommand { get; }
        /// <summary>
        /// The command to select a random company based on the type selected
        /// </summary>
        public ICommand RandomSearchCommand { get; }
        /// <summary>
        /// The command to open load popup for adding company data
        /// </summary>
        public ICommand LoadDataCommand { get; }
        /// <summary>
        /// The command to open daily timeframe report
        /// </summary>
        public ICommand DailyReportCommand { get; }
        /// <summary>
        /// The command to open weekly timeframe report
        /// </summary>
        public ICommand WeeklyReportCommand { get; }
        /// <summary>
        /// the command to open monthly timeframe report
        /// </summary>
        public ICommand MonthlyReportCommand { get; }
        /// <summary>
        /// The command to refresh the info on the page
        /// </summary>
        public ICommand RefreshCommand { get; }

        /// <summary>
        /// Constructor to bind and initialize
        /// </summary>
        /// <param name="serviceProvider">DIed ServiceProvider</param>
        /// <param name="logger">DIed logger</param>
        public MainViewModel(IServiceProvider serviceProvider, ILogger<MainViewModel> logger, DatabaseService db)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _db = db;

            _logger.LogInformation("MainViewModel initialized at {Time}", DateTime.Now);

            SearchCommand = new RelayCommand(OnSearch);
            RandomSearchCommand = new RelayCommand(OnRandomSearch);
            LoadDataCommand = new RelayCommand(OnLoadData);
            DailyReportCommand = new RelayCommand(() => LoadReport("Daily"));
            WeeklyReportCommand = new RelayCommand(() => LoadReport("Weekly"));
            MonthlyReportCommand = new RelayCommand(() => LoadReport("Monthly"));
            RefreshCommand = new AsyncRelayCommand(RefreshAsync);
        }

        /// <summary>
        /// To initialize the data from db and other sources
        /// </summary>
        /// <returns></returns>
        public async ValueTask InitializeData()
        {
            TypeOptions.Clear();

            var types = await _db.TypesOperation.GetAllTypes();
            foreach (var type in types)
            {
                TypeOptions.Add(new OptionItem { Id = type.Id, Name = type.Name });
            }
            if (TypeOptions.Any())
                SelectedOption = TypeOptions.First();

            await RefreshAsync();
        }

        /// <summary>
        /// To load the list of companies to check for remarks today
        /// </summary>
        /// <returns>Task</returns>
        private async Task RefreshAsync()
        {
            _logger.LogInformation("Refreshing charts at {Time}", DateTime.Now);

            DailyCompanies.Clear();
            WeeklyCompanies.Clear();
            MonthlyCompanies.Clear();

            var companies = await _db.CompanyDetailsOperation.GetAllCompanyDetails();

            var dailyToCheck = await _db.DailyRemarksOperation.GetDailyRemarksToCheckToday();
            foreach (var daily in dailyToCheck)
            {
                DailyCompanies.Add(new OptionItem
                {
                    Id = daily.DRId,
                    Name = companies.FirstOrDefault(o => o.CDId == daily.CDId)?.Name ?? ""
                });
            }

            var weeklyToCheck = await _db.WeeklyRemarksOperation.GetWeeklyRemarksToCheckToday();
            foreach (var weekly in weeklyToCheck)
            {
                WeeklyCompanies.Add(new OptionItem
                {
                    Id = weekly.WRId,
                    Name = companies.FirstOrDefault(o => o.CDId == weekly.CDId)?.Name ?? ""
                });
            }

            var monthlyToCheck = await _db.MonthlyRemarksOperation.GetMonthlyRemarksToCheckToday();
            foreach (var monthly in monthlyToCheck)
            {
                MonthlyCompanies.Add(new OptionItem
                {
                    Id = monthly.MRId,
                    Name = companies.FirstOrDefault(o => o.CDId == monthly.CDId)?.Name ?? ""
                });
            }
        }

        /// <summary>
        /// To load the list of companies based on the type selection
        /// </summary>
        /// <returns>Task</returns>
        private async Task LoadCompaniesAsync()
        {
            CompanyOptions.Clear();

            if (SelectedOption == null)
                return;

            var companies = await _db.CompanyDetailsOperation.GetCompanyDetailsBasedOnTypeId(SelectedOption.Id);
            foreach (var c in companies)
                CompanyOptions.Add(new OptionItem { Id = c.CDId, Name = c.Symbol });

            if (CompanyOptions.Count > 0)
                SelectedCompanyKey = CompanyOptions.First();
        }

        /// <summary>
        /// To see if the searched query exists
        /// show options and open the company if existing for putting remarks
        /// based on the type selected
        /// </summary>
        private void OnSearch()
        {

            if (SelectedOption == null) return;
            if (SelectedCompanyKey == null) return;

            var viewModel = _serviceProvider.GetRequiredService<RemarkViewModel>();
            viewModel.SelectedOption = SelectedOption;
            viewModel.SelectedCompany = SelectedCompanyKey;
            var window = new RemarkWindow(viewModel);

            window.ShowDialog();
        }

        /// <summary>
        /// To select a random company from the list of comapnies
        /// based on the selected type
        /// and load our window for putting remarks
        /// </summary>
        private void OnRandomSearch()
        {
            // Example logic
            WeeklyCompanies.Clear();
        }

        /// <summary>
        /// To open the popup for loading new data
        /// </summary>
        private void OnLoadData()
        {
            var viewModel = _serviceProvider.GetRequiredService<LoadDataViewModel>();
            var loadDataWindow = new LoadDataWindow(viewModel);

            loadDataWindow.Owner = Application.Current.MainWindow;

            loadDataWindow.ShowDialog(); // always creates a new popup
        }

        /// <summary>
        /// To open the report, based on the type selected
        /// </summary>
        /// <param name="type"></param>
        private void LoadReport(string type)
        {
            // Example logic
        }

        /// <summary>
        /// To open the mark result popup for daily chart
        /// </summary>
        /// <param name="company">The company remark that was selected</param>
        public void MarkDailyResult(OptionItem company)
        {
            if (company == null) return;

            _logger.LogInformation("Opening detail for {Company}", company.Name);

            var viewModel = _serviceProvider.GetRequiredService<SaveResultViewModel>();
            viewModel.SelectedCompanyRemark = company;
            viewModel.ChartType = ChartTypes.Daily;

            var window = new SaveResultWindow(viewModel)
            {
                Owner = Application.Current.MainWindow
            };
            window.ShowDialog();
        }

        /// <summary>
        /// To open the mark result popup for weekly chart
        /// </summary>
        /// <param name="company">The company remark that was selected</param>
        public void MarkWeeklyResult(OptionItem company)
        {
            if (company == null) return;

            _logger.LogInformation("Opening detail for {Company}", company.Name);

            var viewModel = _serviceProvider.GetRequiredService<SaveResultViewModel>();
            viewModel.SelectedCompanyRemark = company;
            viewModel.ChartType = ChartTypes.Weekly;

            var window = new SaveResultWindow(viewModel)
            {
                Owner = Application.Current.MainWindow
            };
            window.ShowDialog();
        }

        /// <summary>
        /// To open the mark result popup for monthly chart
        /// </summary>
        /// <param name="company">The company remark that was selected</param>
        public void MarkMonthlyResult(OptionItem company)
        {
            if (company == null) return;

            _logger.LogInformation("Opening detail for {Company}", company.Name);

            var viewModel = _serviceProvider.GetRequiredService<SaveResultViewModel>();
            viewModel.SelectedCompanyRemark = company;
            viewModel.ChartType = ChartTypes.Monthly;

            var window = new SaveResultWindow(viewModel)
            {
                Owner = Application.Current.MainWindow
            };
            window.ShowDialog();
        }
    }
}
