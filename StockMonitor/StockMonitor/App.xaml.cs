using StockMonitor.DBO;

namespace StockMonitor
{
    public partial class App : Application
    {
        public App(DatabaseService databaseService)
        {
            try
            {
                InitializeComponent();

                // Set AppTheme permanently to light
                Application.Current.UserAppTheme = AppTheme.Light;

                MainPage = new AppShell();

                // Initialize and Seed Database
                Task.Run(async () => await databaseService.InitializeDatabaseAsync()).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Overriding the create window method for changes as we need.
        /// </summary>
        /// <param name="activationState"></param>
        /// <returns></returns>
        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = base.CreateWindow(activationState);

            window.Height = window.MinimumHeight = 760;
            window.Width = window.MinimumWidth = 1200;

            return window;
        }
    }
}
