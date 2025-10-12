using Microsoft.Extensions.Logging;
using StockMonitor.Core;
using StockMonitor.Data;
using StockMonitor.Enums;
using StockMonitor.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace StockMonitor.ViewModels
{
    /// <summary>
    /// View model for RemarkWindow
    /// </summary>
    public class RemarkViewModel : ViewModelBase
    {
        /// <summary>
        /// The logger for this view model
        /// </summary>
        private readonly ILogger<RemarkViewModel> _logger;

        /// <summary>
        /// The database service to be used for operations
        /// </summary>
        private readonly DatabaseService _db;

        /// <summary>
        /// The current date to be displayed
        /// </summary>
        public DateTime CurrentDate { get; } = DateTime.Now;

        #region Daily
        /// <summary>
        /// To see if the check box for daily remark is checked
        /// </summary>
        public bool IsDailyChecked { get => _isDailyChecked; set => SetProperty(ref _isDailyChecked, value); }
        private bool _isDailyChecked;
        /// <summary>
        /// To see if the position is buy on daily
        /// </summary>
        public bool IsDailyBuy { get => _isDailyBuy; set => SetProperty(ref _isDailyBuy, value); }
        private bool _isDailyBuy;
        /// <summary>
        /// To see if the position is sell on daily
        /// </summary>
        public bool IsDailySell { get => _isDailySell; set => SetProperty(ref _isDailySell, value); }
        private bool _isDailySell;
        /// <summary>
        /// To see if the prediction is up on daily
        /// </summary>
        public bool IsDailyUp { get => _isDailyUp; set => SetProperty(ref _isDailyUp, value); }
        private bool _isDailyUp;
        /// <summary>
        /// To see if the prediction is down on daily
        /// </summary>
        public bool IsDailyDown { get => _isDailyDown; set => SetProperty(ref _isDailyDown, value); }
        private bool _isDailyDown;
        /// <summary>
        /// To store the notes taken on daily
        /// </summary>
        public string DailyNotes { get => _dailyNotes; set => SetProperty(ref _dailyNotes, value); }
        private string _dailyNotes = string.Empty;
        /// <summary>
        /// The date on which we will check this daily result
        /// </summary>
        public string DailyCheckDateText => $"{DateTime.Now.AddDays(1):dd-MMM-yyyy}";

        #endregion

        #region Weekly
        /// <summary>
        /// To see if the check box for weekly remark is checked
        /// </summary>
        public bool IsWeeklyChecked { get => _isWeeklyChecked; set => SetProperty(ref _isWeeklyChecked, value); }
        private bool _isWeeklyChecked;
        /// <summary>
        /// To see if the position is buy on weekly
        /// </summary>
        public bool IsWeeklyBuy { get => _isWeeklyBuy; set => SetProperty(ref _isWeeklyBuy, value); }
        private bool _isWeeklyBuy;
        /// <summary>
        /// To see if the position is sell on weekly
        /// </summary>
        public bool IsWeeklySell { get => _isWeeklySell; set => SetProperty(ref _isWeeklySell, value); }
        private bool _isWeeklySell;
        /// <summary>
        /// To see if the prediction is up on weekly
        /// </summary>
        public bool IsWeeklyUp { get => _isWeeklyUp; set => SetProperty(ref _isWeeklyUp, value); }
        private bool _isWeeklyUp;
        /// <summary>
        /// To see if the prediction is down on weekly
        /// </summary>
        public bool IsWeeklyDown { get => _isWeeklyDown; set => SetProperty(ref _isWeeklyDown, value); }
        private bool _isWeeklyDown;
        /// <summary>
        /// To store the notes taken on weekly
        /// </summary>
        public string WeeklyNotes { get => _weeklyNotes; set => SetProperty(ref _weeklyNotes, value); }
        private string _weeklyNotes = string.Empty;
        /// <summary>
        /// The date on which we will check this weekly result
        /// </summary>
        public string WeeklyCheckDateText => $"{GetWeekEndDate():dd-MMM-yyyy}";

        #endregion

        #region Monthly
        /// <summary>
        /// To see if the check box for monthly remark is checked
        /// </summary>
        public bool IsMonthlyChecked { get => _isMonthlyChecked; set => SetProperty(ref _isMonthlyChecked, value); }
        private bool _isMonthlyChecked;
        /// <summary>
        /// To see if the position is buy on monthly
        /// </summary>
        public bool IsMonthlyBuy { get => _isMonthlyBuy; set => SetProperty(ref _isMonthlyBuy, value); }
        private bool _isMonthlyBuy;
        /// <summary>
        /// To see if the position is sell on monthly
        /// </summary>
        public bool IsMonthlySell { get => _isMonthlySell; set => SetProperty(ref _isMonthlySell, value); }
        private bool _isMonthlySell;
        /// <summary>
        /// To see if the prediction is up on monthly
        /// </summary>
        public bool IsMonthlyUp { get => _isMonthlyUp; set => SetProperty(ref _isMonthlyUp, value); }
        private bool _isMonthlyUp;
        /// <summary>
        /// To see if the prediction is down on monthly
        /// </summary>
        public bool IsMonthlyDown { get => _isMonthlyDown; set => SetProperty(ref _isMonthlyDown, value); }
        private bool _isMonthlyDown;
        /// <summary>
        /// To store the notes taken on monthly
        /// </summary>
        public string MonthlyNotes { get => _monthlyNotes; set => SetProperty(ref _monthlyNotes, value); }
        private string _monthlyNotes = string.Empty;
        /// <summary>
        /// The date on which we will check this monthly result
        /// </summary>
        public string MonthlyCheckDateText => $"{GetMonthEndDate():dd-MMM-yyyy}";

        #endregion

        private OptionItem _selectedOption;
        private OptionItem _selectedCompany;

        /// <summary>
        /// The selected type name to be displayed
        /// </summary>
        public OptionItem SelectedOption
        {
            get => _selectedOption;
            set => SetProperty(ref _selectedOption, value);
        }
        /// <summary>
        /// The company name to be displayed
        /// </summary>
        public OptionItem SelectedCompany
        {
            get => _selectedCompany;
            set => SetProperty(ref _selectedCompany, value);
        }

        /// <summary>
        /// Sections to be displayed on the UI
        /// </summary>
        public ObservableCollection<RemarkSection> RemarkSections { get; } = new();

        /// <summary>
        /// Command to save information
        /// </summary>
        public ICommand SaveCommand { get; }

        /// <summary>
        /// Command to close window
        /// </summary>
        public ICommand CancelCommand { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="window"></param>
        /// <param name="option"></param>
        /// <param name="company"></param>
        public RemarkViewModel(DatabaseService db, ILogger<RemarkViewModel> logger)
        {
            _db = db;
            _logger = logger;

            RemarkSections.Add(new RemarkSection("Daily"));
            RemarkSections.Add(new RemarkSection("Weekly"));
            RemarkSections.Add(new RemarkSection("Monthly"));

            SaveCommand = new AsyncRelayCommand(SaveAsync);
        }


        /// <summary>
        /// A function to initialize the whole window
        /// </summary>
        /// <returns>Task</returns>
        public async Task InitializeData()
        {
        }

        /// <summary>
        /// A function to save the remarks that we have made
        /// </summary>
        /// <returns>Task</returns>
        private async Task SaveAsync()
        {
            // Example: You can insert remarks into DB

            try
            {
                foreach (var section in RemarkSections)
                {
                    if (section.IsEnabled)
                        if (!IsEverythingGiven(section))
                        {
                            MessageBox.Show("Give the mandatory fields", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                }

                bool saveSuccess = false;
                foreach (var section in RemarkSections)
                {
                    switch (section.Title)
                    {
                        case "Daily":
                            if (section.IsEnabled)
                            {
                                DailyRemarks dailyRemarks = new DailyRemarks
                                {
                                    CDId = SelectedCompany.Id,
                                    RemarkDate = DateTime.Now,
                                    Position = section.IsBuy ? Positions.Buy : Positions.Sell,
                                    Prediction = section.IsUp ? Predictions.Up : Predictions.Down,
                                    Remarks = section.Notes,
                                    CheckDate = section.CheckDate,
                                    IsChecked = false
                                };

                                if (await _db.DailyRemarksOperation.SaveDailyRemarkAsync(dailyRemarks) == null)
                                    saveSuccess = true;
                            }
                            break;
                        case "Weekly":
                            if (section.IsEnabled)
                            {
                                WeeklyRemarks weeklyRemarks = new WeeklyRemarks
                                {
                                    CDId = SelectedCompany.Id,
                                    RemarkDate = DateTime.Now,
                                    Position = section.IsBuy ? Positions.Buy : Positions.Sell,
                                    Prediction = section.IsUp ? Predictions.Up : Predictions.Down,
                                    Remarks = section.Notes,
                                    CheckDate = section.CheckDate,
                                    IsChecked = false
                                };

                                if (await _db.WeeklyRemarksOperation.SaveWeeklyRemarkAsync(weeklyRemarks) == null)
                                    saveSuccess = true;
                            }
                            break;
                        case "Monthly":
                            if (section.IsEnabled)
                            {
                                MonthlyRemarks monthlyRemarks = new MonthlyRemarks
                                {
                                    CDId = SelectedCompany.Id,
                                    RemarkDate = DateTime.Now,
                                    Position = section.IsBuy ? Positions.Buy : Positions.Sell,
                                    Prediction = section.IsUp ? Predictions.Up : Predictions.Down,
                                    Remarks = section.Notes,
                                    CheckDate = section.CheckDate,
                                    IsChecked = false
                                };

                                if (await _db.MonthlyRemarksOperation.SaveMonthlyRemarkAsync(monthlyRemarks) == null)
                                    saveSuccess = true;
                            }
                            break;
                    }
                }

                if (saveSuccess)
                    MessageBox.Show("Remarks saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Problem in saving remarks!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                _logger.LogError("RemarkViewModel - SaveAsync", ex);
                MessageBox.Show("Exception while saving remarks!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }                
        }

        /// <summary>
        /// To check if all the mandatory fields are given or not
        /// </summary>
        /// <param name="section">The section to check</param>
        /// <returns>false if something is not given, otherwise true</returns>
        private bool IsEverythingGiven(RemarkSection section)
        {
            if (!section.IsBuy && !section.IsSell) return false;

            if (!section.IsUp && !section.IsDown) return false;

            if (string.IsNullOrWhiteSpace(section.Notes)) return false;

            return true;
        }

        /// <summary>
        /// To get the date for week end
        /// </summary>
        /// <returns>DateTime</returns>
        private static DateTime GetWeekEndDate()
        {
            var today = DateTime.Now;
            int daysUntilSunday = ((int)DayOfWeek.Sunday - (int)today.DayOfWeek + 7) % 7;
            return today.AddDays(daysUntilSunday);
        }

        /// <summary>
        /// To get the date of month end
        /// </summary>
        /// <returns>DateTime</returns>
        private static DateTime GetMonthEndDate()
        {
            var today = DateTime.Now;
            return new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
        }
    }
}
