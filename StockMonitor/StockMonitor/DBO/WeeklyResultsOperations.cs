using SQLite;
using StockMonitor.Data;

namespace StockMonitor.DBO
{
    /// <summary>
    /// To handle all the DB operations for the WeeklyResults table
    /// </summary>
    public class WeeklyResultsOperations
    {
        /// <summary>
        /// Readonly connection object to our SQLite db
        /// </summary>
        private readonly SQLiteAsyncConnection _connection;

        /// <summary>
        /// Contructor to assign the connection
        /// </summary>
        /// <param name="connection">Connection object to be used henceforth</param>
        public WeeklyResultsOperations(SQLiteAsyncConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Get all the WeeklyResults
        /// </summary>
        /// <returns>A list of WeeklyResults</returns>
        public async Task<List<WeeklyResults>> GetAllTypes() =>
            await _connection.Table<WeeklyResults>().ToListAsync();

        /// <summary>
        /// Get all the WeeklyResults as per the type id given
        /// </summary>
        /// <param name="companyId">typeId to be searched</param>
        /// <returns>list of WeeklyResults</returns>
        public async Task<List<WeeklyResults>> GetWeeklyResultsBasedOnCompanyId(Guid companyId)
        {
            List<ChartRemarks> listOfCharts = await _connection.Table<ChartRemarks>().Where(o => o.CDId == companyId).ToListAsync();
            if (listOfCharts.Count == 0)
                return null;

            List<Guid> chartGuidsToSearch = listOfCharts.Select(o => o.CRId).ToList();

            List<WeeklyRemarks> dailyRemarks = await _connection.Table<WeeklyRemarks>().Where(o => chartGuidsToSearch.Contains(o.CRId)).ToListAsync();
            if (dailyRemarks.Count == 0)
                return null;

            List<Guid> remarkGuidsToSearch = dailyRemarks.Select(o => o.WRId).ToList();
            return await _connection.Table<WeeklyResults>().Where(o => remarkGuidsToSearch.Contains(o.WRId)).ToListAsync();
        }

        /// <summary>
        /// Get all the WeeklyResults as per the dates given
        /// </summary>
        /// <param name="from">from date</param>
        /// <param name="to">to date</param>
        /// <returns>list of WeeklyResults</returns>
        public async Task<List<WeeklyResults>> GetWeeklyResultsBasedOnDates(DateTime from, DateTime to)
        {
            var fromDate = new DateTime(from.Year, from.Month, from.Day, 0, 0, 0);
            var toDate = new DateTime(to.Year, to.Month, to.Day, 0, 0, 0);

            return await _connection.Table<WeeklyResults>().Where(o => o.ResultDate >= fromDate && o.ResultDate <= toDate).ToListAsync();
        }

        /// <summary>
        /// Get all the WeeklyResults as per the dates given
        /// </summary>
        /// <param name="from">from date</param>
        /// <param name="to">to date</param>
        /// <returns>list of WeeklyResults</returns>
        public async Task<List<WeeklyResults>> GetWeeklyResultsBasedOnRemarkDates(DateTime from, DateTime to)
        {
            var fromDate = new DateTime(from.Year, from.Month, from.Day, 0, 0, 0);
            var toDate = new DateTime(to.Year, to.Month, to.Day, 0, 0, 0);

            List<WeeklyRemarks> remarks = await _connection.Table<WeeklyRemarks>().Where(o => o.RemarkDate >= fromDate && o.RemarkDate <= toDate).ToListAsync();
            if (remarks.Count == 0)
                return null;

            List<Guid> remarkGuidsToSearch = remarks.Select(o => o.WRId).ToList();
            return await _connection.Table<WeeklyResults>().Where(o => remarkGuidsToSearch.Contains(o.WRId)).ToListAsync();
        }

        /// <summary>
        /// Get the WeeklyResults object as per the id given
        /// </summary>
        /// <param name="Id">Id to be searched</param>
        /// <returns>WeeklyResults object</returns>
        public async Task<WeeklyResults> GetWeeklyResultsBasedOnId(Guid Id) =>
            (await _connection.Table<WeeklyResults>().FirstOrDefaultAsync(o => o.WResultId == Id));

        /// <summary>
        /// Method to save the WeeklyResults in DB
        /// </summary>
        /// <param name="details">DailyRemark to be saved</param>
        /// <returns>Error message string, null on success</returns>
        public async Task<string?> SaveDailyRemarkAsync(WeeklyResults details)
        {

            if (await _connection.InsertAsync(details) > 0)
                return null;

            return "Error in saving daily result";
        }
    }
}
