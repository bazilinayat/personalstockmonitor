using StockMonitor.ViewModels;
using System.Windows;

namespace StockMonitor.Views
{
    /// <summary>
    /// Interaction logic for RemarkWindow.xaml
    /// </summary>
    public partial class RemarkWindow : Window
    {
        /// <summary>
        /// The view model which is bound to this window
        /// </summary>
        private readonly RemarkViewModel _viewModel;

        public RemarkWindow(RemarkViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;

            DataContext = _viewModel;

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
        /// In case the cancel button is clicked, we close the window
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
