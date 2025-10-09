using Microsoft.Extensions.Logging;
using StockMonitor.Core;
using StockMonitor.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace StockMonitor.ViewModels
{
    /// <summary>
    /// View model for the load data window
    /// </summary>
    public class LoadDataViewModel : ViewModelBase
    {
        /// <summary>
        /// The logger for this view model
        /// </summary>
        private readonly ILogger<LoadDataViewModel> _logger;

        /// <summary>
        /// The database service to be used for operations
        /// </summary>
        private readonly DatabaseService _db;

        /// <summary>
        /// radio button optitons on the screen for uploading the data
        /// </summary>
        public ObservableCollection<OptionItem> Options { get; } = new();

        /// <summary>
        /// The selected option
        /// </summary>
        private OptionItem? _selectedOption;
        /// <summary>
        /// the selection option for public use
        /// </summary>
        public OptionItem? SelectedOption
        {
            get => _selectedOption;
            set => SetProperty(ref _selectedOption, value);
        }
        /// <summary>
        /// The text of company names which needs to be read
        /// </summary>
        private string _companyText = string.Empty;
        /// <summary>
        /// The list of company names
        /// </summary>
        public string CompanyText
        {
            get => _companyText;
            set => SetProperty(ref _companyText, value);
        }

        /// <summary>
        /// The command for uplaod button
        /// </summary>
        public ICommand UploadCommand { get; }
        /// <summary>
        /// The command for selecting option
        /// </summary>
        public ICommand SelectOptionCommand { get; }

        /// <summary>
        /// The constructor for the view model
        /// </summary>
        /// <param name="logger">DIed logger</param>
        public LoadDataViewModel(ILogger<LoadDataViewModel> logger, DatabaseService db)
        {
            _db = db;
            _logger = logger;

            UploadCommand = new RelayCommand(UploadData);
            SelectOptionCommand = new RelayCommand<OptionItem>(opt => SelectedOption = opt);
        }

        /// <summary>
        /// To initialize the data from db and other sources
        /// </summary>
        /// <returns></returns>
        public async ValueTask InitializeData()
        {

            var types = await _db.TypesOperation.GetAllTypes();

            Options.Clear();
            foreach (var type in types)
            {
                Options.Add(new OptionItem { Id = type.Id, Name = type.Name });
            }
        }

        /// <summary>
        /// to upload the database based on the type and text we have
        /// split the company names by new line
        /// upload for the selected type
        /// </summary>
        private void UploadData()
        {
            // Split text by new line
            var selected = SelectedOption?.Name ?? "No Option Selected";
            var lines = (CompanyText ?? string.Empty)
                        .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            System.Diagnostics.Debug.WriteLine($"Selected Option: {selected}");
            foreach (var l in lines)
                System.Diagnostics.Debug.WriteLine($"To insert: {l}");

            Console.WriteLine("\nCompanies to Upload:");
            foreach (var company in lines)
                Console.WriteLine($"  {company}");

            MessageBox.Show($"Uploaded {lines.Length} companies.", "Upload Complete");
        }
    }
}
