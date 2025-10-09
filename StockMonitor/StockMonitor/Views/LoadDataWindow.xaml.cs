using StockMonitor.ViewModels;
using System.Threading.Tasks;
using System.Windows;

namespace StockMonitor.Views
{
    /// <summary>
    /// Interaction logic for LoadDataWindow.xaml
    /// </summary>
    public partial class LoadDataWindow : Window
    {
        /// <summary>
        /// The view model for this window
        /// </summary>
        private readonly LoadDataViewModel _viewModel;

        /// <summary>
        /// To initialize and bind the view model
        /// </summary>
        /// <param name="viewModel"></param>
        public LoadDataWindow(LoadDataViewModel viewModel)
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
            this.Close(); // this works now properly
        }
    }
}
