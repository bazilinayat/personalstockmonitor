using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using POSRestaurant.DBO;
using Microsoft.Maui.LifecycleEvents;

namespace StockMonitor
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
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

            builder.Services.AddSingleton<DatabaseService>();

            //// Force initialize Windows App Runtime components
            //if (OperatingSystem.IsWindows())
            //{
            //    builder.ConfigureLifecycleEvents(events =>
            //    {
            //        events.AddWindows(windows => windows
            //            .OnWindowCreated(window =>
            //            {
            //                window.ExtendsContentIntoTitleBar = false;
            //            })
            //        );
            //    });
            //}

            return builder.Build();
        }
    }
}
