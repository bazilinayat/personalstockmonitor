using StockMonitor.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StockMonitor.Views.Controls
{
    /// <summary>
    /// Interaction logic for SearchableComboBox.xaml
    /// </summary>
    public partial class SearchableComboBox : UserControl
    {
        public SearchableComboBox()
        {
            InitializeComponent();
        }

        // 🔹 The full list to filter from
        public ObservableCollection<OptionItem> ItemsSource
        {
            get => (ObservableCollection<OptionItem>)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(ObservableCollection<OptionItem>), typeof(SearchableComboBox), new PropertyMetadata(null));

        // 🔹 The selected item
        public OptionItem SelectedItem
        {
            get => (OptionItem)GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(nameof(SelectedItem), typeof(OptionItem), typeof(SearchableComboBox), new PropertyMetadata(null));

        // 🔹 Search text binding
        public string SearchText
        {
            get => (string)GetValue(SearchTextProperty);
            set => SetValue(SearchTextProperty, value);
        }
        public static readonly DependencyProperty SearchTextProperty =
            DependencyProperty.Register(nameof(SearchText), typeof(string), typeof(SearchableComboBox),
                new PropertyMetadata(string.Empty, OnSearchTextChanged));

        private static void OnSearchTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (SearchableComboBox)d;
            control.FilterItems();
        }

        // 🔹 Filtered items collection
        public ObservableCollection<OptionItem> FilteredItems { get; } = new();

        public bool IsPopupOpen
        {
            get => (bool)GetValue(IsPopupOpenProperty);
            set => SetValue(IsPopupOpenProperty, value);
        }
        public static readonly DependencyProperty IsPopupOpenProperty =
            DependencyProperty.Register(nameof(IsPopupOpen), typeof(bool), typeof(SearchableComboBox), new PropertyMetadata(false));

        private void FilterItems()
        {
            if (ItemsSource == null)
                return;

            var search = SearchText?.Trim() ?? "";

            FilteredItems.Clear();

            if (search.Length < 3)
            {
                IsPopupOpen = false;
                return;
            }

            var matches = ItemsSource
                .Where(i => i.Name.Contains(search, System.StringComparison.OrdinalIgnoreCase))
                .Take(5)
                .ToList();

            foreach (var m in matches)
                FilteredItems.Add(m);

            IsPopupOpen = FilteredItems.Any();
        }

        private void ListBox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBox list && list.SelectedItem is OptionItem selected)
            {
                SelectedItem = selected;
                SearchText = selected.Name;
                IsPopupOpen = false;
            }
        }
    }
}
