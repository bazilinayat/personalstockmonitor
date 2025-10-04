using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using StockMonitor.DBO;
using StockMonitor.Models;
using StockMonitor.Services;    
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace StockMonitor.ViewModels
{
    /// <summary>
    /// View Model for LoadData page
    /// </summary>
    public partial class LoadDataViewModel : ObservableObject
    {
        /// <summary>
        /// DIed variable for DatabaseService
        /// </summary>
        private readonly DatabaseService _databaseService;

        /// <summary>
        /// DIed LogService
        /// </summary>
        private readonly LogService _logger;

        /// <summary>
        /// To indicate that the ViewModel data is loading
        /// </summary>
        [ObservableProperty]
        private bool _isLoading;

        public ObservableCollection<ValueForPickerSelectionForFile> PickerOptions { get; set; } = new();

        /// <summary>
        /// Constructor for the view model
        /// </summary>
        /// <param name="databaseService">DIed Database Service</param>
        public LoadDataViewModel(LogService logger, DatabaseService databaseService)
        {
            _logger = logger;
            _databaseService = databaseService;
        }

        public int Key { get; set; }
        public string Value { get; set; }

        [ObservableProperty]
        private bool _isSelected;

        [ObservableProperty]
        private string? _selectedFileName;

        /// <summary>
        /// To Initialize the Page components and update the data
        /// </summary>
        /// <returns></returns>
        public async ValueTask InitializeAsync()
        {
            try
            {
                // Load items from DB or repository
                var entities = new List<ValueForPicker>
                {
                    new() { Key = 1, Value = "Company Master" },
                    new() { Key = 2, Value = "Sales Data" },
                    new() { Key = 3, Value = "Expenses Report" }
                };

                PickerOptions = new ObservableCollection<ValueForPickerSelectionForFile>(entities.Select(ValueForPickerSelectionForFile.FromEntity));
            }
            catch (Exception ex)
            {
                _logger.LogError("SettingsVM-InitializeAsync Error", ex);
                await Shell.Current.DisplayAlert("Fault", "Error in Loading Settings Screen", "OK"); 
            }
        }
    }
}
