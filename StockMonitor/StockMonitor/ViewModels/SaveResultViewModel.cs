using Microsoft.Extensions.Logging;
using StockMonitor.Core;
using StockMonitor.Data;
using StockMonitor.Enums;
using StockMonitor.Models;
using System.Windows;
using System.Windows.Input;

namespace StockMonitor.ViewModels
{
    /// <summary>
    /// View model for SaveResult window
    /// </summary>
    public class SaveResultViewModel : ViewModelBase
    {
        /// <summary>
        /// The logger for this view model
        /// </summary>
        private readonly ILogger<SaveResultViewModel> _logger;

        /// <summary>
        /// The database service to be used for operations
        /// </summary>
        private readonly DatabaseService _db;

        /// <summary>
        /// The current date to be displayed
        /// </summary>
        public DateTime CurrentDate { get; } = DateTime.Now;

        /// <summary>
        /// Selected company from list view
        /// </summary>
        private OptionItem _selectedCompanyRemark;
        /// <summary>
        /// The company name to be displayed
        /// </summary>
        public OptionItem SelectedCompanyRemark
        {
            get => _selectedCompanyRemark;
            set => SetProperty(ref _selectedCompanyRemark, value);
        }

        /// <summary>
        /// To know the time frame on the screen
        /// </summary>
        private string _heading;
        /// <summary>
        /// public property for the time frame
        /// </summary>
        public string Heading
        {
            get => _heading;
            set => SetProperty(ref _heading, value);
        }

        /// <summary>
        /// The date at which this remark was made
        /// </summary>
        private DateTime _remarkDate;
        /// <summary>
        /// The public reference to remark date
        /// </summary>
        public DateTime RemarkDate
        {
            get => _remarkDate;
            set => SetProperty(ref _remarkDate, value);
        }
        
        /// <summary>
        /// The position that was taken for the remakr
        /// </summary>
        private string _position;
        /// <summary>
        /// The public reference to position
        /// </summary>
        public string Position
        {
            get => _position;
            set => SetProperty(ref _position, value);
        }

        /// <summary>
        /// the prediction that was assumed for the remark
        /// </summary>
        private string _prediction;
        /// <summary>
        /// The public reference to the prediction
        /// </summary>
        public string Prediction
        {
            get => _prediction;
            set => SetProperty(ref _prediction, value);
        }

        /// <summary>
        /// The comments that were made on the remark
        /// </summary>
        private string _previousComments;
        /// <summary>
        /// the public reference to the comments
        /// </summary>
        public string PreviousComments
        {
            get => _previousComments;
            set => SetProperty(ref _previousComments, value);
        }

        /// <summary>
        /// To know if the remark is on a daily chart
        /// </summary>
        public ChartTypes ChartType { get; set; }

        /// <summary>
        /// To know if the result is for 
        /// </summary>
        private bool _isFor;
        /// <summary>
        /// public reference to for
        /// </summary>
        public bool IsFor
        {
            get => _isFor;
            set
            {
                SetProperty(ref _isFor, value);
                if (value) _isAgainst = false;
                Notify(nameof(IsAgainst));
            }
        }

        /// <summary>
        /// To know if the result is against
        /// </summary>
        private bool _isAgainst;
        /// <summary>
        /// public reference to against
        /// </summary>
        public bool IsAgainst
        {
            get => _isAgainst;
            set
            {
                SetProperty(ref _isAgainst, value);
                if (value) _isFor = false;
                Notify(nameof(IsFor));
            }
        }

        /// <summary>
        /// The comment that we are making on the result
        /// </summary>
        private string _newComment;
        /// <summary>
        /// The public reference to the comments
        /// </summary>
        public string NewComment
        {
            get => _newComment;
            set => SetProperty(ref _newComment, value);
        }

        /// <summary>
        /// Command to save information
        /// </summary>
        public ICommand SaveCommand { get; }

        /// <summary>
        /// Constructor for the view model to assigne important dependencies
        /// </summary>
        /// <param name="db">DIed DatabaseService</param>
        /// <param name="logger">DIed Logger</param>
        public SaveResultViewModel(DatabaseService db, ILogger<SaveResultViewModel> logger)
        {
            _db = db;
            _logger = logger;

            SaveCommand = new AsyncRelayCommand(SaveAsync);
        }

        /// <summary>
        /// A function to initialize the whole window
        /// </summary>
        /// <returns>Task</returns>
        public async Task InitializeData()
        {

            Heading = $"{EnumExtensions.GetDescription(ChartType)} Check for {SelectedCompanyRemark.Name}";

            IsFor = IsAgainst = false;
            NewComment = "";

            switch (ChartType)
            {
                case ChartTypes.Daily:
                    var dailyRemark = await _db.DailyRemarksOperation.GetDailyRemarksBasedOnId(SelectedCompanyRemark.Id);

                    RemarkDate = dailyRemark.RemarkDate;
                    Prediction = dailyRemark.Prediction == Predictions.Up ? "Up" : "Down";
                    Position = dailyRemark.Position == Positions.Buy ? "Buy" : "Seel";
                    PreviousComments = dailyRemark.Remarks;

                    break;
                case ChartTypes.Weekly:
                    var weeklyRemark = await _db.WeeklyRemarksOperation.GetWeeklyRemarksBasedOnId(SelectedCompanyRemark.Id);
                    RemarkDate = weeklyRemark.RemarkDate;
                    Prediction = weeklyRemark.Prediction == Predictions.Up ? "Up" : "Down";
                    Position = weeklyRemark.Position == Positions.Buy ? "Buy" : "Seel";
                    PreviousComments = weeklyRemark.Remarks;
                    break;
                case ChartTypes.Monthly:
                    var monthlyRemark = await _db.MonthlyRemarksOperation.GetMonthlyRemarksBasedOnId(SelectedCompanyRemark.Id);
                    RemarkDate = monthlyRemark.RemarkDate;
                    Prediction = monthlyRemark.Prediction == Predictions.Up ? "Up" : "Down";
                    Position = monthlyRemark.Position == Positions.Buy ? "Buy" : "Seel";
                    PreviousComments = monthlyRemark.Remarks;
                    break;
            }
        }

        /// <summary>
        /// A function to save the results that we have checked
        /// </summary>
        /// <returns>Task</returns>
        private async Task SaveAsync()
        {
            try
            {
                if ((!IsFor && !IsAgainst) || string.IsNullOrWhiteSpace(NewComment))
                {
                    MessageBox.Show("All fields are manadatory", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                switch (ChartType)
                {
                    case ChartTypes.Daily:
                        await _db.DailyResultsOperation.SaveDailyRemarkAsync(new DailyResults
                        {
                            DRId = SelectedCompanyRemark.Id,
                            ResultDate = CurrentDate,
                            Result = IsFor ? Results.For : Results.Against,
                            ResultRemark = NewComment,
                        });

                        var dailyRemark = await _db.DailyRemarksOperation.GetDailyRemarksBasedOnId(SelectedCompanyRemark.Id);
                        dailyRemark.IsChecked = true;

                        await _db.DailyRemarksOperation.UpdateDailyRemarkAsync(dailyRemark);

                        break;
                    case ChartTypes.Weekly:
                        await _db.WeeklyResultsOperation.SaveWeeklyResultAsync(new WeeklyResults
                        {
                            WRId = SelectedCompanyRemark.Id,
                            ResultDate = CurrentDate,
                            Result = IsFor ? Results.For : Results.Against,
                            ResultRemarks = NewComment,
                        });

                        var weeklyRemark = await _db.WeeklyRemarksOperation.GetWeeklyRemarksBasedOnId(SelectedCompanyRemark.Id);
                        weeklyRemark.IsChecked = true;

                        await _db.WeeklyRemarksOperation.UpdateWeeklyRemarkAsync(weeklyRemark);

                        break;
                    case ChartTypes.Monthly:
                        await _db.MonthlyResultsOperation.SaveMonthlyResultAsync(new MonthlyResults
                        {
                            MRId = SelectedCompanyRemark.Id,
                            ResultDate = CurrentDate,
                            Result = IsFor ? Results.For : Results.Against,
                            ResultRemarks = NewComment,
                        });

                        var monthlyRemark = await _db.MonthlyRemarksOperation.GetMonthlyRemarksBasedOnId(SelectedCompanyRemark.Id);
                        monthlyRemark.IsChecked = true;

                        await _db.MonthlyRemarksOperation.UpdateMonthlyRemarkAsync(monthlyRemark);

                        break;
                }

                MessageBox.Show("Result saved successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                _logger.LogError("SaveResultViewModel - SaveAsync", ex);
                MessageBox.Show("Error in saving result", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
