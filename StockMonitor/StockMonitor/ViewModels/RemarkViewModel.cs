using Microsoft.Extensions.Logging;
using StockMonitor.Core;
using StockMonitor.Data;
using StockMonitor.Enums;
using StockMonitor.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
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
        /// The buy price entered by user
        /// </summary>
        private decimal _buyPrice;
        /// <summary>
        /// The public buy price for screen
        /// </summary>
        public decimal BuyPrice
        {
            get => _buyPrice;
            set => SetProperty(ref _buyPrice, value);
        }

        /// <summary>
        /// The sl value entered by user
        /// </summary>
        private decimal _slValue;
        /// <summary>
        /// The public sl value for screen
        /// </summary>
        public decimal SLValue
        {
            get => _slValue;
            set => SetProperty(ref _slValue, value);
        }

        /// <summary>
        /// The target value entered by user
        /// </summary>
        private decimal _targetValue;
        /// <summary>
        /// The public target value for screen
        /// </summary>
        public decimal TargetValue
        {
            get => _targetValue;
            set => SetProperty(ref _targetValue, value);
        }

        /// <summary>
        /// The calcualted RRR value to be displayed on screen
        /// </summary>
        private decimal _rrrCalculated;
        /// <summary>
        /// The public RRR for binding
        /// </summary>
        public decimal RRRCalculated
        {
            get => _rrrCalculated;
            set => SetProperty(ref _rrrCalculated, value);
        }

        /// <summary>
        /// Command to save information
        /// </summary>
        public ICommand SaveCommand { get; }

        /// <summary>
        /// Command to calculate RRR
        /// </summary>
        public ICommand RRRCalculateCommand { get; }

        /// <summary>
        /// Action to close window
        /// </summary>
        public Action CloseAction { get; set; }

        /// <summary>
        /// Constructor for the view model to assigne important dependencies
        /// </summary>
        /// <param name="db">DIed DatabaseService</param>
        /// <param name="logger">DIed Logger</param>
        public RemarkViewModel(DatabaseService db, ILogger<RemarkViewModel> logger)
        {
            _db = db;
            _logger = logger;

            SaveCommand = new AsyncRelayCommand(SaveAsync);
            RRRCalculateCommand = new RelayCommand(CalculateRRR);
        }


        /// <summary>
        /// A function to initialize the whole window
        /// </summary>
        /// <returns>Task</returns>
        public async Task InitializeData()
        {
            RemarkSections.Clear();
            RemarkSections.Add(new RemarkSection("Daily"));
            RemarkSections.Add(new RemarkSection("Weekly"));
            RemarkSections.Add(new RemarkSection("Monthly"));
        }

        /// <summary>
        /// To calculate the RRR for given values
        /// </summary>
        private void CalculateRRR()
        {
            if (BuyPrice == 0 || SLValue == 0 || TargetValue == 0)
            {
                MessageBox.Show("All the three fields are importat", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            decimal one = BuyPrice - SLValue;
            decimal two = Math.Abs(BuyPrice - TargetValue);

            if (one <= 0)
            {
                MessageBox.Show("SL should be less than CMP / Buying Price", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            decimal ratio = two / one;

            BuyPrice = SLValue = TargetValue = 0;


            RRRCalculated = ratio;
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
                {
                    MessageBox.Show("Remarks saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    CloseAction?.Invoke();
                }
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
