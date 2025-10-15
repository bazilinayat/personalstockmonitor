using StockMonitor.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace StockMonitor.Views
{
    /// <summary>
    /// Interaction logic for ReportWindow.xaml
    /// </summary>
    public partial class ReportWindow : Window
    {
        /// <summary>
        /// The view model which is bound to this window
        /// </summary>
        private readonly ReportViewModel _viewModel;

        public ReportWindow(ReportViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;

            DataContext = _viewModel;

            Initialize();

            viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(viewModel.ChartData))
                    DrawChart(viewModel);
            };

            Loaded += (s, e) => DrawChart(viewModel);
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

        private void DrawChart(ReportViewModel vm)
        {
            var canvas = FindName("chartCanvas") as Canvas;
            if (canvas == null) return;

            canvas.Children.Clear();

            var data = vm.ChartData.ToList();
            if (data.Count == 0) return;

            double width = canvas.ActualWidth > 0 ? canvas.ActualWidth : 400;
            double height = canvas.ActualHeight > 0 ? canvas.ActualHeight : 200;

            double stepX = width / (data.Count - 1);
            double maxY = data.Max(d => Math.Max(d.Success, d.Failure));

            PointCollection successPoints = new();
            PointCollection failurePoints = new();

            for (int i = 0; i < data.Count; i++)
            {
                double x = i * stepX;
                double ySuccess = height - (data[i].Success / maxY * height);
                double yFailure = height - (data[i].Failure / maxY * height);

                successPoints.Add(new Point(x, ySuccess));
                failurePoints.Add(new Point(x, yFailure));
            }

            Polyline successLine = new()
            {
                Stroke = Brushes.Green,
                StrokeThickness = 2,
                Points = successPoints
            };
            Polyline failureLine = new()
            {
                Stroke = Brushes.Red,
                StrokeThickness = 2,
                Points = failurePoints
            };

            canvas.Children.Add(successLine);
            canvas.Children.Add(failureLine);
        }
    }
}
