using StockMonitor.ViewModels;

namespace StockMonitor.Pages;

public partial class MainPage : ContentPage
{
    int count = 0;

    /// <summary>
    /// DIed property to handle the MainPageViewModel
    /// </summary>
    private readonly MainPageViewModel _mainPageViewModel;

    public MainPage(MainPageViewModel mainPageViewModel)
    {
        InitializeComponent();

        _mainPageViewModel = mainPageViewModel;
        BindingContext = _mainPageViewModel;
    }

    private void OnCounterClicked(object sender, EventArgs e)
    {
        count++;

        if (count == 1)
            CounterBtn.Text = $"Clicked {count} time";
        else
            CounterBtn.Text = $"Clicked {count} times";

        SemanticScreenReader.Announce(CounterBtn.Text);
    }
}
