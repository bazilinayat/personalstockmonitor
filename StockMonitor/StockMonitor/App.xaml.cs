using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using StockMonitor.Core;
using StockMonitor.ViewModels;
using StockMonitor.Views;
using System.IO;
using System.Windows;

namespace StockMonitor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        /// <summary>
        /// The host to start the application
        /// </summary>
        private IHost _host;

        /// <summary>
        /// The startup method to add dependencies and initializations
        /// </summary>
        /// <param name="e"></param>
        protected override async void OnStartup(StartupEventArgs e)
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

                _host = Host.CreateDefaultBuilder()
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

                        // Views
                        services.AddSingleton<MainWindow>();
                        services.AddSingleton<LoadDataWindow>();
                        services.AddSingleton<RemarkWindow>();
                        services.AddSingleton<SaveResultWindow>();
                        services.AddSingleton<ReportWindow>();

                        // ViewModels
                        services.AddSingleton<MainViewModel>();
                        services.AddSingleton<LoadDataViewModel>();
                        services.AddSingleton<RemarkViewModel>();
                        services.AddSingleton<SaveResultViewModel>();
                        services.AddSingleton<ReportViewModel>();
                    })
                    .Build();

                // Initialize database (async)
                var dbService = _host.Services.GetRequiredService<DatabaseService>();
                await dbService.InitializeDatabaseAsync();
                Log.Information("Database initialized successfully.");

                var mainWindow = _host.Services.GetRequiredService<MainWindow>();
                mainWindow.Show();
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

        /// <summary>
        /// To stop the host on exiting the application
        /// </summary>
        /// <param name="e"></param>
        protected override void OnExit(ExitEventArgs e)
        {
            _host?.Dispose();
            base.OnExit(e);
        }
    }

}
