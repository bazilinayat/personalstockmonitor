using Microsoft.Extensions.Logging;
using StockMonitor.Models;
using StockMonitor.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StockMonitor.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// The view model which is bound to this window
        /// </summary>
        private readonly MainViewModel _viewModel;
        
        /// <summary>
        /// The logger for this window
        /// </summary>
        private readonly ILogger<MainWindow> _logger;

        /// <summary>
        /// Constructor for launching and bounding the view model
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="logger"></param>
        public MainWindow(MainViewModel viewModel, ILogger<MainWindow> logger)
        {
            InitializeComponent();

            _viewModel = viewModel;
            _logger = logger;

            DataContext = _viewModel;

            _logger.LogInformation("MainWindow initialized");

            Initialize();
        }

        /// <summary>
        /// To initialize the data on the window
        /// </summary>
        /// <returns></returns>
        private async void Initialize()
        {
            await _viewModel.InitializeData();
        }

        /// <summary>
        /// The double click event for the items in daily list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is ListView listView && listView.SelectedItem is OptionItem selectedCompany)
            {
                if (DataContext is MainViewModel vm)
                {
                    vm.MarkDailyResult(selectedCompany);
                }
            }
        }

        /// <summary>
        /// The double click event for the items in weekly list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListView_MouseDoubleClick_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is ListView listView && listView.SelectedItem is OptionItem selectedCompany)
            {
                if (DataContext is MainViewModel vm)
                {
                    vm.MarkWeeklyResult(selectedCompany);
                }
            }
        }

        /// <summary>
        /// The double click event for the items in monthly list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListView_MouseDoubleClick_2(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is ListView listView && listView.SelectedItem is OptionItem selectedCompany)
            {
                if (DataContext is MainViewModel vm)
                {
                    vm.MarkMonthlyResult(selectedCompany);
                }
            }
        }

        private void ListBox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBox listBox && listBox.SelectedItem is OptionItem selected)
            {
                if (DataContext is MainViewModel vm)
                {
                    vm.SelectedCompany = selected;
                    vm.CompanySearchText = selected.Name;
                    vm.IsSuggestionVisible = false;
                }
            }
        }
    }
}
