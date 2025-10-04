using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Input;
using StockMonitor.ViewModels;

namespace StockMonitor.Controls;

/// <summary>
/// Popup to complete the order
/// </summary>
public partial class LoadDataPopup : Popup
{
    /// <summary>
    /// DIed OrderComplete View Model
    /// </summary>
    private readonly LoadDataViewModel _loadDataViewModel;

    /// <summary>
    /// Constructor for the page for completing order
    /// </summary>
    /// <param name="loadDataViewModel">DIed LoadDataViewModel</param>
    public LoadDataPopup(LoadDataViewModel loadDataViewModel)
    {
        InitializeComponent();

        _loadDataViewModel = loadDataViewModel;

        BindingContext = _loadDataViewModel;

        Initialize();
    }

    /// <summary>
    /// To load the initial data and add any other logic
    /// </summary>
    private async void Initialize()
    {
        await _loadDataViewModel.InitializeAsync();
    }

    ///// <summary>
    ///// Event will be called when X label is tapped
    ///// </summary>
    ///// <param name="sender">Sender as Label</param>
    ///// <param name="e">TappedEventArgs</param>
    //private async void CloseLabel_Tapped(object sender, TappedEventArgs e) => await this.CloseAsync();

    //private async void Button_Clicked_2(object sender, EventArgs e)
    //{
    //    await _settingsViewModel.SaveRestaurantInfoCommand.ExecuteAsync(null);
    //    if (_settingsViewModel.InfoInitialized)
    //        await this.CloseAsync();
    //}

    private async void CloseLabel_Tapped(object sender, EventArgs e) => await this.CloseAsync();
}