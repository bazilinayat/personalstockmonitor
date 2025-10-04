using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using StockMonitor.Controls;
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
    public partial class MainPageViewModel : ObservableObject
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
        /// ServiceProvider for the DIs
        /// </summary>
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// To indicate that the ViewModel data is loading
        /// </summary>
        [ObservableProperty]
        private bool _isLoading;

        /// <summary>
        /// Constructor for the view model
        /// </summary>
        /// <param name="databaseService">DIed Database Service</param>
        public MainPageViewModel(IServiceProvider serviceProvider, LogService logger, DatabaseService databaseService)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _databaseService = databaseService;
        }

        [RelayCommand]
        private async Task CounterButton()
        {
            var loadDataViewModel = _serviceProvider.GetRequiredService<LoadDataViewModel>();
            var setInfo = new LoadDataPopup(loadDataViewModel);
            await Shell.Current.ShowPopupAsync(setInfo);
        }
    }
}
