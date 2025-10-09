using Microsoft.Extensions.Configuration;
using SQLite;
using SQLitePCL;
using StockMonitor.Data;
using StockMonitor.DBO;
using System.IO;

namespace StockMonitor.Core
{
    /// <summary>
    /// Service to work with all the database operations and database migrations too
    /// </summary>
    public class DatabaseService
    {
        /// <summary>
        /// The path to the database file
        /// </summary>
        private readonly string _dbPath;

        /// <summary>
        /// Readonly connection object to our SQLite db
        /// </summary>
        private readonly SQLiteAsyncConnection _connection;

        /// <summary>
        /// For getting the detials from configuration
        /// </summary>
        private readonly IConfiguration _configuration;

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
        /// Constructor
        /// </summary>
        /// <param name="config">To get the database file and other details from the config</param>
        public DatabaseService(IConfiguration config)
        {
            _configuration = config;
            _dbPath = Path.Combine(Directory.GetCurrentDirectory(), config["Database:File"]);

            if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data")))
            {
                Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data"));
            }

            // SQLitePCL initialization
            Batteries_V2.Init();

            // Initialize database
            _connection = new SQLiteAsyncConnection(_dbPath,
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
            _migrationManager = new MigrationManager(_dbPath, _entityTypes);
        }

        /// <summary>
        /// To return the connection if at all needed by anyone
        /// </summary>
        /// <returns>SQLiteAsyncConnection object</returns>
        public SQLiteAsyncConnection GetConnection() => _connection == null ? new SQLiteAsyncConnection(_dbPath,
                SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.SharedCache) : _connection;

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
                var typesInSettings = GetTypesFromSettings();

                await TypesOperation.SaveTypeAsync(typesInSettings);
            }
        }

        /// <summary>
        /// Reads 'Types' from appsettings.json
        /// </summary>
        public List<Types> GetTypesFromSettings()
        {
            try
            {
                var typesSection = _configuration.GetSection("Types").Get<string[]>();

                var types = new List<Types>();

                if (typesSection != null)
                {
                    foreach (var typeName in typesSection)
                        types.Add(new Types { Name = typeName });
                }

                return types;
            }
            catch (Exception ex)
            {
                // handle or log as needed
                Console.WriteLine($"Error reading Types from settings: {ex.Message}");
                return new List<Types>();
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
