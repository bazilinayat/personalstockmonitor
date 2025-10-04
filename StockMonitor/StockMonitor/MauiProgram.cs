using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using StockMonitor.Controls;
using StockMonitor.DBO;
using StockMonitor.Services;
using StockMonitor.ViewModels;

namespace StockMonitor
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .UseMauiCommunityToolkit();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            string logFilePath = Path.Combine(AppContext.BaseDirectory, "logs");

            builder.Services.AddSingleton(new LogService(logFilePath));

            builder.Services.AddSingleton<DatabaseService>()
                .AddSingleton<LoadDataPopup>()

                .AddSingleton<StockMonitor.Pages.MainPage>()

                .AddSingleton<LoadDataViewModel>()
                .AddSingleton<MainPageViewModel>();

            // force initialize windows app runtime components
            if (OperatingSystem.IsWindows())
            {
                builder.ConfigureLifecycleEvents(events =>
                {
                    events.AddWindows(windows => windows
                        .OnWindowCreated(window =>
                        {
                            window.ExtendsContentIntoTitleBar = false;
                        })
                    );
                });
            }

            return builder.Build();
        }
    }
}
