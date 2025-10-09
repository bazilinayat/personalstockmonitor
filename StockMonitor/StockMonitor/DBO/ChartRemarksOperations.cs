using SQLite;
using StockMonitor.Data;

namespace StockMonitor.DBO
{
    /// <summary>
    /// To handle all the DB operations for the ChartRemarks table
    /// </summary>
    public class ChartRemarksOperations
    {
        /// <summary>
        /// Readonly connection object to our SQLite db
        /// </summary>
        private readonly SQLiteAsyncConnection _connection;

        /// <summary>
        /// Contructor to assign the connection
        /// </summary>
        /// <param name="connection">Connection object to be used henceforth</param>
        public ChartRemarksOperations(SQLiteAsyncConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Get all the ChartRemarks
        /// </summary>
        /// <returns>A list of ChartRemarks</returns>
        public async Task<List<ChartRemarks>> GetAllTypes() =>
            await _connection.Table<ChartRemarks>().ToListAsync();

        /// <summary>
        /// Get all the ChartRemarks as per the type id given
        /// </summary>
        /// <param name="companyId">typeId to be searched</param>
        /// <returns>list of ChartRemarks</returns>
        public async Task<List<ChartRemarks>> GetChartRemarksBasedOnCompanyId(Guid companyId) =>
            (await _connection.Table<ChartRemarks>().Where(o => o.CDId == companyId).ToListAsync());

        /// <summary>
        /// Get the ChartRemarks object as per the id given
        /// </summary>
        /// <param name="Id">Id to be searched</param>
        /// <returns>ChartRemarks object</returns>
        public async Task<ChartRemarks> GetChartRemarksBasedOnId(Guid Id) =>
            (await _connection.Table<ChartRemarks>().FirstOrDefaultAsync(o => o.CDId == Id));

        /// <summary>
        /// Method to save the ChartRemarks in DB
        /// </summary>
        /// <param name="details">ChartRemarks list to be saved</param>
        /// <returns>Error message string, null on success</returns>
        public async Task<string?> SaveChartRemarksAsync(List<ChartRemarks> details)
        {

            if (await _connection.InsertAllAsync(details) > 0)
                return null;

            return "Error in saving multiple chart remarks";
        }

        /// <summary>
        /// Method to save the ChartRemarks in DB
        /// </summary>
        /// <param name="details">ChartRemark to be saved</param>
        /// <returns>Error message string, null on success</returns>
        public async Task<string?> SaveChartRemarkAsync(ChartRemarks details)
        {

            if (await _connection.InsertAsync(details) > 0)
                return null;

            return "Error in saving chart remark";
        }
    }
}
