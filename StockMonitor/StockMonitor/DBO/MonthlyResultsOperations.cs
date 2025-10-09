using SQLite;
using StockMonitor.Data;

namespace StockMonitor.DBO
{
    /// <summary>
    /// To handle all the DB operations for the MonthlyResults table
    /// </summary>
    public class MonthlyResultsOperations
    {
        /// <summary>
        /// Readonly connection object to our SQLite db
        /// </summary>
        private readonly SQLiteAsyncConnection _connection;

        /// <summary>
        /// Contructor to assign the connection
        /// </summary>
        /// <param name="connection">Connection object to be used henceforth</param>
        public MonthlyResultsOperations(SQLiteAsyncConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Get all the MonthlyResults
        /// </summary>
        /// <returns>A list of MonthlyResults</returns>
        public async Task<List<MonthlyResults>> GetAllTypes() =>
            await _connection.Table<MonthlyResults>().ToListAsync();

        /// <summary>
        /// Get all the MonthlyResults as per the type id given
        /// </summary>
        /// <param name="companyId">typeId to be searched</param>
        /// <returns>list of MonthlyResults</returns>
        public async Task<List<MonthlyResults>> GetMonthlyResultsBasedOnCompanyId(Guid companyId)
        {
            List<ChartRemarks> listOfCharts = await _connection.Table<ChartRemarks>().Where(o => o.CDId == companyId).ToListAsync();
            if (listOfCharts.Count == 0)
                return null;

            List<Guid> chartGuidsToSearch = listOfCharts.Select(o => o.CRId).ToList();

            List<MonthlyRemarks> monthlyRemarks = await _connection.Table<MonthlyRemarks>().Where(o => chartGuidsToSearch.Contains(o.CRId)).ToListAsync();
            if (monthlyRemarks.Count == 0)
                return null;

            List<Guid> remarkGuidsToSearch = monthlyRemarks.Select(o => o.MRId).ToList();
            return await _connection.Table<MonthlyResults>().Where(o => remarkGuidsToSearch.Contains(o.MRId)).ToListAsync();
        }

        /// <summary>
        /// Get all the MonthlyResults as per the dates given
        /// </summary>
        /// <param name="from">from date</param>
        /// <param name="to">to date</param>
        /// <returns>list of MonthlyResults</returns>
        public async Task<List<MonthlyResults>> GetMonthlyResultsBasedOnDates(DateTime from, DateTime to)
        {
            var fromDate = new DateTime(from.Year, from.Month, from.Day, 0, 0, 0);
            var toDate = new DateTime(to.Year, to.Month, to.Day, 0, 0, 0);

            return await _connection.Table<MonthlyResults>().Where(o => o.ResultDate >= fromDate && o.ResultDate <= toDate).ToListAsync();
        }

        /// <summary>
        /// Get all the MonthlyResults as per the dates given
        /// </summary>
        /// <param name="from">from date</param>
        /// <param name="to">to date</param>
        /// <returns>list of MonthlyResults</returns>
        public async Task<List<MonthlyResults>> GetMonthlyResultsBasedOnRemarkDates(DateTime from, DateTime to)
        {
            var fromDate = new DateTime(from.Year, from.Month, from.Day, 0, 0, 0);
            var toDate = new DateTime(to.Year, to.Month, to.Day, 0, 0, 0);

            List<MonthlyRemarks> remarks = await _connection.Table<MonthlyRemarks>().Where(o => o.RemarkDate >= fromDate && o.RemarkDate <= toDate).ToListAsync();
            if (remarks.Count == 0)
                return null;

            List<Guid> remarkGuidsToSearch = remarks.Select(o => o.MRId).ToList();
            return await _connection.Table<MonthlyResults>().Where(o => remarkGuidsToSearch.Contains(o.MRId)).ToListAsync();
        }

        /// <summary>
        /// Get the MonthlyResults object as per the id given
        /// </summary>
        /// <param name="Id">Id to be searched</param>
        /// <returns>MonthlyResults object</returns>
        public async Task<MonthlyResults> GetMonthlyResultsBasedOnId(Guid Id) =>
            (await _connection.Table<MonthlyResults>().FirstOrDefaultAsync(o => o.MResultId == Id));

        /// <summary>
        /// Method to save the MonthlyResults in DB
        /// </summary>
        /// <param name="details">DailyRemark to be saved</param>
        /// <returns>Error message string, null on success</returns>
        public async Task<string?> SaveDailyRemarkAsync(MonthlyResults details)
        {

            if (await _connection.InsertAsync(details) > 0)
                return null;

            return "Error in saving daily result";
        }
    }
}
