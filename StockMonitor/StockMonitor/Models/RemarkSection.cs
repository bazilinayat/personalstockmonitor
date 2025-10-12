using StockMonitor.ViewModels;
using System.ComponentModel;
using System.Windows.Markup.Localizer;

namespace StockMonitor.Models
{
    public class RemarkSection : ViewModelBase
    {
        private bool _isEnabled;
        private bool _isBuy;
        private bool _isSell;
        private bool _isUp;
        private bool _isDown;
        private string _notes = string.Empty;

        public string Title { get; }
        public string CheckDateLabel { get; }

        public DateTime CheckDate { get; }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        public bool IsBuy
        {
            get => _isBuy;
            set
            {
                SetProperty(ref _isBuy, value);
                if (value) _isSell = false; // manually uncheck Sell
                Notify(nameof(IsSell));
            }
        }

        public bool IsSell
        {
            get => _isSell;
            set
            {
                SetProperty(ref _isSell, value);
                if (value) _isBuy = false; // manually uncheck Buy
                Notify(nameof(IsBuy));
            }
        }

        public bool IsUp
        {
            get => _isUp;
            set
            {
                SetProperty(ref _isUp, value);
                if (value) _isDown = false; // manually uncheck Down
                Notify(nameof(IsDown));
            }
        }

        public bool IsDown
        {
            get => _isDown;
            set
            {
                SetProperty(ref _isDown, value);
                if (value) _isUp = false; // manually uncheck Up
                Notify(nameof(IsUp));
            }
        }

        public string Notes
        {
            get => _notes;
            set => SetProperty(ref _notes, value);
        }

        public string PositionGroupName { get; }
        public string PredictionGroupName { get; }

        public RemarkSection(string title)
        {
            
            Title = title;

            DateTime today = DateTime.Today;
            CheckDate = today;
            switch (title)
            {
                case "Daily":
                    CheckDate = today.AddDays(1);
                    break;
                case "Weekly":
                    int daysUntilEndOfWeek = ((int)DayOfWeek.Sunday - (int)today.DayOfWeek + 7) % 7;
                    CheckDate = today.AddDays(daysUntilEndOfWeek);
                    break;
                case "Monthly":
                    CheckDate = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
                    break;
            }

            CheckDateLabel = CheckDate.ToString("yyyy-MMM-dd");

            PositionGroupName = $"{Title}_Position";
            PredictionGroupName = $"{Title}_Prediction";
        }
    }

}
