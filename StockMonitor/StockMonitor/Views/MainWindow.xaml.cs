using Microsoft.Extensions.Logging;
using StockMonitor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
        }
    }
}
