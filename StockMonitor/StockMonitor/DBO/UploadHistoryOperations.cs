using SQLite;
using StockMonitor.Data;

namespace StockMonitor.DBO
{
    /// <summary>
    /// To handle all the DB operations for the UploadHistory table
    /// </summary>
    public class UploadHistoryOperations
    {
        /// <summary>
        /// Readonly connection object to our SQLite db
        /// </summary>
        private readonly SQLiteAsyncConnection _connection;

        /// <summary>
        /// Contructor to assign the connection
        /// </summary>
        /// <param name="connection">Connection object to be used henceforth</param>
        public UploadHistoryOperations(SQLiteAsyncConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Get all the UploadHistory
        /// </summary>
        /// <returns>A list of UploadHistory</returns>
        public async Task<List<UploadHistory>> GetAllTypes() =>
            await _connection.Table<UploadHistory>().ToListAsync();

        /// <summary>
        /// Method to save the UploadHistory in DB
        /// </summary>
        /// <param name="uploadData">UploadHistory object to be saved</param>
        /// <returns>Error message string, null on success</returns>
        public async Task<string?> SaveUploadDataAsync(UploadHistory uploadData)
        {

            if (await _connection.InsertAsync(uploadData) > 0)
                return null;

            return "Error in saving upload data";
            
        }
    }
}
