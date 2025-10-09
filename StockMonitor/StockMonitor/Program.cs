using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using StockMonitor.Core;
using StockMonitor.ViewModels;
using StockMonitor.Views;
using System.IO;

namespace StockMonitor
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            // Setup configuration first
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .CreateLogger();

            try
            {
                Log.Information("Application starting...");

                string logFilePath = Path.Combine(AppContext.BaseDirectory, "logs");

                var host = Host.CreateDefaultBuilder(args)
                    .UseSerilog((context, services, loggerConfig) =>
                    {
                        // This overload will read from the Host configuration and DI services
                        loggerConfig.ReadFrom.Configuration(context.Configuration)
                                    .ReadFrom.Services(services)
                                    .Enrich.FromLogContext();
                    })
                    .ConfigureAppConfiguration(config => config.AddConfiguration(configuration))
                    .ConfigureServices((context, services) =>
                    {
                        services.AddSingleton(new LoggingService(logFilePath));

                        // Core Services (Singletons)
                        services.AddSingleton<DatabaseService>();
                        services.AddSingleton<ConfigService>();

                        // ViewModels
                        services.AddSingleton<MainViewModel>();

                        // Views
                        services.AddSingleton<MainWindow>();
                    })
                    .Build();

                var app = new App();
                var mainWindow = host.Services.GetRequiredService<MainWindow>();
                app.Run(mainWindow);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application terminated unexpectedly!");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
