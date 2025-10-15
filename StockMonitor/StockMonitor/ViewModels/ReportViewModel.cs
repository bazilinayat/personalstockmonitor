using Microsoft.Extensions.Logging;
using StockMonitor.Core;
using StockMonitor.Enums;
using StockMonitor.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace StockMonitor.ViewModels
{
    /// <summary>
    /// Viewmodel for Report window
    /// </summary>
    public class ReportViewModel : ViewModelBase
    {
        /// <summary>
        /// The logger for this view model
        /// </summary>
        private readonly ILogger<ReportViewModel> _logger;

        /// <summary>
        /// The database service to be used for operations
        /// </summary>
        private readonly DatabaseService _db;

        /// <summary>
        /// the selected chart type from drop down
        /// </summary>
        private ChartTypes _selectedChartType;
        /// <summary>
        /// The public property for the chart types
        /// </summary>
        public ChartTypes SelectedChartType
        {
            get => _selectedChartType;
            set => SetProperty(ref _selectedChartType, value);
        }

        /// <summary>
        /// The date from which we check
        /// </summary>
        private DateTime _fromDate = DateTime.Today.AddDays(-30);
        /// <summary>
        /// the public property for the screen
        /// </summary>
        public DateTime FromDate
        {
            get => _fromDate;
            set => SetProperty(ref _fromDate, value);
        }

        /// <summary>
        /// The date till which we have to check the data
        /// </summary>
        private DateTime _toDate = DateTime.Today;
        /// <summary>
        /// The public property for todate
        /// </summary>
        public DateTime ToDate
        {
            get => _toDate;
            set => SetProperty(ref _toDate, value);
        }

        /// <summary>
        /// The items to be displayed on a table
        /// </summary>
        public ObservableCollection<ReportItem> ReportItems { get; } = new();

        /// <summary>
        /// the total number of charts checked between the date range
        /// </summary>
        private int _totalChecked;
        /// <summary>
        /// total checked for screen
        /// </summary>
        public int TotalChecked
        {
            get => _totalChecked;
            set => SetProperty(ref _totalChecked, value);
        }

        /// <summary>
        /// The total number of predictions that were correct between the date range
        /// for the selected chart type
        /// </summary>
        private int _totalFor;
        /// <summary>
        /// total for for screen
        /// </summary>
        public int TotalFor
        {
            get => _totalFor;
            set => SetProperty(ref _totalFor, value);
        }

        /// <summary>
        /// The total number of predictions that were not correct between the date range
        /// for the selected chart type
        /// </summary>
        private int _totalAgainst;
        /// <summary>
        /// total against for screen
        /// </summary>
        public int TotalAgainst
        {
            get => _totalAgainst;
            set => SetProperty(ref _totalAgainst, value);
        }

        /// <summary>
        /// the success percentage based on for and against
        /// for selected chart type and date range
        /// </summary>
        private double _successPercentage;
        /// <summary>
        /// success percentage for screen
        /// </summary>
        public double SuccessPercentage
        {
            get => _successPercentage;
            set => SetProperty(ref _successPercentage, value);
        }

        /// <summary>
        /// The chart options that will be shown in the drop down menu
        /// </summary>
        public ObservableCollection<ChartTypes> ChartOptions { get; } =
            new ObservableCollection<ChartTypes>((ChartTypes[])Enum.GetValues(typeof(ChartTypes)));


        /// <summary>
        /// Command to load information
        /// </summary>
        public ICommand LoadCommand { get; }

        /// <summary>
        /// Constructor for the view model to assigne important dependencies
        /// </summary>
        /// <param name="db">DIed DatabaseService</param>
        /// <param name="logger">DIed Logger</param>
        public ReportViewModel(DatabaseService db, ILogger<ReportViewModel> logger)
        {
            _db = db;
            _logger = logger;

            LoadCommand = new AsyncRelayCommand(LoadReportAsync);

            LoadReportAsync();
        }

        /// <summary>
        /// A function to initialize the whole window
        /// </summary>
        /// <returns>Task</returns>
        public async Task InitializeData()
        {

        }

        /// <summary>
        /// To load the report on the screen
        /// Fetchs relevant data from db and assigns
        /// </summary>
        /// <returns></returns>
        private async Task LoadReportAsync()
        {
            var companies = await _db.CompanyDetailsOperation.GetAllCompanyDetails();
            ReportItems.Clear();
            switch (SelectedChartType)
            {
                case ChartTypes.Daily:
                    var dailyRemarks = await _db.DailyRemarksOperation.GetDailyRemarksBasedOnDates(FromDate, ToDate);
                    var dailyResults = await _db.DailyResultsOperation.GetDailyResultsBasedOnRemarkDates(FromDate, ToDate);

                    if (dailyRemarks.Count == 0 || dailyResults == null)
                    {
                        MessageBox.Show("No Information for this Combination", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    foreach (var remark in dailyRemarks)
                    {
                        var item = new ReportItem
                        {
                            Date = remark.RemarkDate,
                            Company = companies.Where(o => o.CDId == remark.CDId).Select(o => o.Name).FirstOrDefault(),
                            Prediction = remark.Prediction == Predictions.Up ? "Up" : "Down",
                            Result = dailyResults.Where(o => o.DRId == remark.DRId).Select(o => o.Result).FirstOrDefault() == Results.For ? "For" : "Against"
                        };
                        ReportItems.Add(item);
                    }

                    break;
                case ChartTypes.Weekly:
                    var weeklyRemarks = await _db.WeeklyRemarksOperation.GetWeeklyRemarksBasedOnDates(FromDate, ToDate);
                    var weeklyResults = await _db.WeeklyResultsOperation.GetWeeklyResultsBasedOnRemarkDates(FromDate, ToDate);

                    if (weeklyRemarks.Count == 0 || weeklyResults == null)
                    {
                        MessageBox.Show("No Information for this Combination", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    foreach (var remark in weeklyRemarks)
                    {
                        var item = new ReportItem
                        {
                            Date = remark.RemarkDate,
                            Company = companies.Where(o => o.CDId == remark.CDId).Select(o => o.Name).FirstOrDefault(),
                            Prediction = remark.Prediction == Predictions.Up ? "Up" : "Down",
                            Result = weeklyResults.Where(o => o.WRId == remark.WRId).Select(o => o.Result).FirstOrDefault() == Results.For ? "For" : "Against"
                        };
                        ReportItems.Add(item);
                    }

                    break;
                case ChartTypes.Monthly:
                    var monthlyRemarks = await _db.MonthlyRemarksOperation.GetMonthlyRemarksBasedOnDates(FromDate, ToDate);
                    var monthlyResults = await _db.MonthlyResultsOperation.GetMonthlyResultsBasedOnRemarkDates(FromDate, ToDate);

                    if (monthlyRemarks.Count == 0 || monthlyResults == null)
                    {
                        MessageBox.Show("No Information for this Combination", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    foreach (var remark in monthlyRemarks)
                    {
                        var item = new ReportItem
                        {
                            Date = remark.RemarkDate,
                            Company = companies.Where(o => o.CDId == remark.CDId).Select(o => o.Name).FirstOrDefault(),
                            Prediction = remark.Prediction == Predictions.Up ? "Up" : "Down",
                            Result = monthlyResults.Where(o => o.MRId == remark.MRId).Select(o => o.Result).FirstOrDefault() == Results.For ? "For" : "Against"
                        };
                        ReportItems.Add(item);
                    }

                    break;
            }
            ComputeSummary();
        }

        /// <summary>
        /// To show the summary of the data that is taken from db
        /// </summary>
        private void ComputeSummary()
        {
            TotalChecked = ReportItems.Count;
            TotalFor = ReportItems.Count(r => r.Result == "For");
            TotalAgainst = ReportItems.Count(r => r.Result == "Against");
            SuccessPercentage = TotalChecked > 0
                ? (double)TotalFor / TotalChecked * 100
                : 0;

            BuildChartData();
        }

        public ObservableCollection<ChartPoint> ChartData { get; } = new();

        private void BuildChartData()
        {
            ChartData.Clear();
            // Example: group by date
            var grouped = ReportItems.GroupBy(r => r.Date.Date)
                .Select(g => new ChartPoint
                {
                    Date = g.Key,
                    Success = g.Count(x => x.Result == "For"),
                    Failure = g.Count(x => x.Result == "Against")
                })
                .OrderBy(c => c.Date);

            foreach (var item in grouped)
                ChartData.Add(item);
        }
    }

    public class ChartPoint
    {
        public DateTime Date { get; set; }
        public int Success { get; set; }
        public int Failure { get; set; }
    }
}
