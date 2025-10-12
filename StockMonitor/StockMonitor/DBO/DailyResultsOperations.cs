using SQLite;
using StockMonitor.Data;

namespace StockMonitor.DBO
{
    /// <summary>
    /// To handle all the DB operations for the DailyResults table
    /// </summary>
    public class DailyResultsOperations
    {
        /// <summary>
        /// Readonly connection object to our SQLite db
        /// </summary>
        private readonly SQLiteAsyncConnection _connection;

        /// <summary>
        /// Contructor to assign the connection
        /// </summary>
        /// <param name="connection">Connection object to be used henceforth</param>
        public DailyResultsOperations(SQLiteAsyncConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Get all the DailyResults
        /// </summary>
        /// <returns>A list of DailyResults</returns>
        public async Task<List<DailyResults>> GetAllTypes() =>
            await _connection.Table<DailyResults>().ToListAsync();

        /// <summary>
        /// Get all the DailyResults as per the type id given
        /// </summary>
        /// <param name="companyId">typeId to be searched</param>
        /// <returns>list of DailyResults</returns>
        public async Task<List<DailyResults>> GetDailyResultsBasedOnCompanyId(Guid companyId)
        {
            List<DailyRemarks> dailyRemarks = await _connection.Table<DailyRemarks>().Where(o => o.CDId == companyId).ToListAsync();
            if (dailyRemarks.Count == 0)
                return null;

            List<Guid> remarkGuidsToSearch = dailyRemarks.Select(o => o.DRId).ToList();
            return await _connection.Table<DailyResults>().Where(o => remarkGuidsToSearch.Contains(o.DRId)).ToListAsync();
        }

        /// <summary>
        /// Get all the DailyResults as per the dates given
        /// </summary>
        /// <param name="from">from date</param>
        /// <param name="to">to date</param>
        /// <returns>list of DailyResults</returns>
        public async Task<List<DailyResults>> GetDailyResultsBasedOnDates(DateTime from, DateTime to)
        {
            var fromDate = new DateTime(from.Year, from.Month, from.Day, 0, 0, 0);
            var toDate = new DateTime(to.Year, to.Month, to.Day, 0, 0, 0);

            return await _connection.Table<DailyResults>().Where(o => o.ResultDate >= fromDate && o.ResultDate <= toDate).ToListAsync();
        }

        /// <summary>
        /// Get all the DailyResults as per the dates given
        /// </summary>
        /// <param name="from">from date</param>
        /// <param name="to">to date</param>
        /// <returns>list of DailyResults</returns>
        public async Task<List<DailyResults>> GetDailyResultsBasedOnRemarkDates(DateTime from, DateTime to)
        {
            var fromDate = new DateTime(from.Year, from.Month, from.Day, 0, 0, 0);
            var toDate = new DateTime(to.Year, to.Month, to.Day, 0, 0, 0);

            List<DailyRemarks> remarks = await _connection.Table<DailyRemarks>().Where(o => o.RemarkDate >= fromDate && o.RemarkDate <= toDate).ToListAsync();
            if (remarks.Count == 0)
                return null;

            List<Guid> remarkGuidsToSearch = remarks.Select(o => o.DRId).ToList();
            return await _connection.Table<DailyResults>().Where(o => remarkGuidsToSearch.Contains(o.DRId)).ToListAsync();
        }

        /// <summary>
        /// Get the DailyResults object as per the id given
        /// </summary>
        /// <param name="Id">Id to be searched</param>
        /// <returns>DailyResults object</returns>
        public async Task<DailyResults> GetDailyResultsBasedOnId(Guid Id) =>
            (await _connection.Table<DailyResults>().FirstOrDefaultAsync(o => o.DRId == Id));

        /// <summary>
        /// Method to save the DailyResults in DB
        /// </summary>
        /// <param name="details">DailyRemark to be saved</param>
        /// <returns>Error message string, null on success</returns>
        public async Task<string?> SaveDailyRemarkAsync(DailyResults details)
        {

            if (await _connection.InsertAsync(details) > 0)
                return null;

            return "Error in saving daily result";
        }
    }
}
