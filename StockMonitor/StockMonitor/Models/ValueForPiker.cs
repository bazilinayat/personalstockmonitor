using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace StockMonitor.Models
{
    /// <summary>
    /// This class will be used to make lists of things to populate a picker control
    /// Later we can use the key value here for identifying records
    /// </summary>
    public class ValueForPicker
    {
        /// <summary>
        /// Key of the value, to identify the record
        /// </summary>
        public int Key {  get; set; }

        /// <summary>
        /// Text to be displayed on picker
        /// </summary>
        public string Value { get; set; }
    }

    /// <summary>
    /// This class will be used to make lists of things to populate a picker control
    /// Later we can use the key value here for identifying records
    /// </summary>
    public partial class ValueForPickerSelection : ObservableObject
    {
        /// <summary>
        /// Key of the value, to identify the record
        /// </summary>
        public int Key { get; set; }

        /// <summary>
        /// Text to be displayed on picker
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Observable Property to see if MenuCategory is selected
        /// </summary>
        [ObservableProperty]
        private bool _isSelected;

        public static ValueForPickerSelection FromEntity(ValueForPicker entity) =>
            new()
            {
                Key = entity.Key,
                Value = entity.Value,
                IsSelected = false
            };
    }

    public partial class ValueForPickerSelectionForFile : ObservableObject
    {
        public int Key { get; set; }
        public string Value { get; set; }

        [ObservableProperty]
        private bool _isSelected;

        [ObservableProperty]
        private string? _selectedFileName;

        [RelayCommand]
        private async Task PickFileAsync()
        {
            try
            {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = $"Select file for {Value}",
                    FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.WinUI, new[] { ".csv", ".xls", ".xlsx" } },
                    { DevicePlatform.Android, new[] { "text/csv", "application/vnd.ms-excel", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" } },
                    { DevicePlatform.iOS, new[] { "public.comma-separated-values-text", "org.openxmlformats.spreadsheetml.sheet" } }
                })
                });

                if (result != null)
                {
                    SelectedFileName = result.FileName;
                }
            }
            catch
            {
                // user cancelled, ignore
            }
        }

        public static ValueForPickerSelectionForFile FromEntity(ValueForPicker entity) =>
            new()
            {
                Key = entity.Key,
                Value = entity.Value,
                IsSelected = false
            };
    }
}
