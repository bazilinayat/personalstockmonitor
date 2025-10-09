using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
        /// The type options, will come from database
        /// </summary>
        public ObservableCollection<string> TypeOptions { get; } = new()
        {
            "Type A", "Type B", "Type C"
        };

        /// <summary>
        /// The type which is selected for checking
        /// </summary>
        private string _selectedType;
        /// <summary>
        /// The active selected type
        /// </summary>
        public string SelectedType
        {
            get => _selectedType;
            set => SetProperty(ref _selectedType, value);
        }

        /// <summary>
        /// The query or company name for search
        /// </summary>
        private string _searchQuery;
        /// <summary>
        /// The actual query typed
        /// </summary>
        public string SearchQuery
        {
            get => _searchQuery;
            set => SetProperty(ref _searchQuery, value);
        }

        /// <summary>
        /// The list of companies to check today in daily frame
        /// </summary>
        public ObservableCollection<string> DailyCompanies { get; } = new();

        /// <summary>
        /// The list of companies to check today in weekly frame
        /// </summary>
        public ObservableCollection<string> WeeklyCompanies { get; } = new();

        /// <summary>
        /// The list of companies to check today in monthly frame
        /// </summary>
        public ObservableCollection<string> MonthlyCompanies { get; } = new();

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
        /// Constructor to bind and initialize
        /// </summary>
        /// <param name="serviceProvider">DIed ServiceProvider</param>
        /// <param name="logger">DIed logger</param>
        public MainViewModel(IServiceProvider serviceProvider, ILogger<MainViewModel> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;

            _logger.LogInformation("MainViewModel initialized at {Time}", DateTime.Now);

            SearchCommand = new RelayCommand(OnSearch);
            RandomSearchCommand = new RelayCommand(OnRandomSearch);
            LoadDataCommand = new RelayCommand(OnLoadData);
            DailyReportCommand = new RelayCommand(() => LoadReport("Daily"));
            WeeklyReportCommand = new RelayCommand(() => LoadReport("Weekly"));
            MonthlyReportCommand = new RelayCommand(() => LoadReport("Monthly"));
        }

        /// <summary>
        /// To see if the searched query exists
        /// show options and open the company if existing for putting remarks
        /// based on the type selected
        /// </summary>
        private void OnSearch()
        {
            // Example logic
            DailyCompanies.Clear();
            DailyCompanies.Add($"Searched: {SearchQuery} in {SelectedType}");
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
            WeeklyCompanies.Add("Random Corp Ltd");
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
            DailyCompanies.Add($"{type} Report Loaded");
        }
    }
}
