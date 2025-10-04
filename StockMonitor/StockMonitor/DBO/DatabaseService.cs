using SQLite;
using StockMonitor.Data;
using StockMonitor.Service;

namespace StockMonitor.DBO
{
    /// <summary>
    /// Class to connect and talk to our database
    /// </summary>
    public class DatabaseService : IAsyncDisposable
    {
        /// <summary>
        /// Readonly connection object to our SQLite db
        /// </summary>
        private readonly SQLiteAsyncConnection _connection;

        /// <summary>
        /// Variable for getting initial data
        /// </summary>
        private readonly SeedData _seedData;

        /// <summary>
        /// To handle calls to TypesOperation in the db
        /// </summary>
        public TypesOperations TypesOperation;

        /// <summary>
        /// To handle calls to UploadHistoryOperations in the db
        /// </summary>
        public UploadHistoryOperations UploadHistoryOperation;

        /// <summary>
        /// To handle calls to CompanyDetailsOperations in the db
        /// </summary>
        public CompanyDetailsOperations CompanyDetailsOperation;

        /// <summary>
        /// To handle calls to ChartRemarksOperations in the db
        /// </summary>
        public ChartRemarksOperations ChartRemarksOperation;

        /// <summary>
        /// To handle calls to DailyRemarksOperations in the db
        /// </summary>
        public DailyRemarksOperations DailyRemarksOperation;

        /// <summary>
        /// To handle calls to WeeklyRemarksOperations in the db
        /// </summary>
        public WeeklyRemarksOperations WeeklyRemarksOperation;

        /// <summary>
        /// To handle calls to MonthlyRemarksOperations in the db
        /// </summary>
        public MonthlyRemarksOperations MonthlyRemarksOperation;

        /// <summary>
        /// To handle calls to DailyResultsOperations in the db
        /// </summary>
        public DailyResultsOperations DailyResultsOperation;
        
        /// <summary>
        /// To handle calls to WeeklyResultsOperations in the db
        /// </summary>
        public WeeklyResultsOperations WeeklyResultsOperation;

        /// <summary>
        /// To handle calls to MonthlyResultsOperations in the db
        /// </summary>
        public MonthlyResultsOperations MonthlyResultsOperation;

        /// <summary>
        /// A list of different entity types we have
        /// </summary>
        private readonly List<Type> _entityTypes;

        /// <summary>
        /// The migration manager used for altering or creating new tables.
        /// </summary>
        private MigrationManager _migrationManager;

        /// <summary>
        /// Class constructor, to generate database and connection
        /// </summary>
        public DatabaseService()
        {
            _seedData = new SeedData();

            if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data"))) 
            {
                Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data"));
            }

            var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data",  "RestPOS.db3");
            _connection = new SQLiteAsyncConnection(dbPath, 
                SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.SharedCache);

            TypesOperation = new TypesOperations(_connection);
            UploadHistoryOperation = new UploadHistoryOperations(_connection);
            CompanyDetailsOperation = new CompanyDetailsOperations(_connection);
            ChartRemarksOperation = new ChartRemarksOperations(_connection);
            DailyRemarksOperation = new DailyRemarksOperations(_connection);
            WeeklyRemarksOperation = new WeeklyRemarksOperations(_connection);
            MonthlyRemarksOperation = new MonthlyRemarksOperations(_connection);
            DailyResultsOperation = new DailyResultsOperations(_connection);
            WeeklyResultsOperation = new WeeklyResultsOperations(_connection);
            MonthlyResultsOperation = new MonthlyResultsOperations(_connection);

            // Register all your entity types here
            _entityTypes = new List<Type>
            {
                typeof(Types),
                typeof(UploadHistory),
                typeof(CompanyDetails),
                typeof(ChartRemarks),
                typeof(DailyRemarks),
                typeof(WeeklyRemarks),
                typeof(MonthlyRemarks),
                typeof(DailyResults),
                typeof(WeeklyResults),
                typeof(MonthlyResults)
            };

            // Create the migration manager
            _migrationManager = new MigrationManager(dbPath, _entityTypes);
        }

        /// <summary>
        /// Method to initialize database - create necessary tables.
        /// </summary>
        /// <returns>Returns a Task object</returns>
        public async Task InitializeDatabaseAsync()
        {
            // Create tables for your entities if they don't exist
            foreach (var entityType in _entityTypes)
            {
                await _connection.CreateTableAsync(entityType, CreateFlags.None);
            }

            // Run automated migrations
            await _migrationManager.MigrateAsync();

            await SeedDataAsync();
        }

        /// <summary>
        /// Method to load our seed data, taken from SeedData class
        /// </summary>
        /// <returns>Returns a Task object</returns>
        private async Task SeedDataAsync()
        {
            // Checking and return if data is already seeded
            var firstType = await _connection.Table<Types>().FirstOrDefaultAsync();
            if (firstType == null)
            {
                var typesInSettings = _seedData.GetTypesFromSettings();

                await TypesOperation.SaveTypeAsync(typesInSettings);
            }
        }

        /// <summary>
        /// Implementation of IAsyncDisposable interface
        /// </summary>
        /// <returns>Returns a Task object</returns>
        public async ValueTask DisposeAsync()
        {
            if (_connection != null)
                await _connection.CloseAsync();
        }
    }
}
