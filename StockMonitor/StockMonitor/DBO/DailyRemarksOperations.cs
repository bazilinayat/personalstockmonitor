using SQLite;
using StockMonitor.Data;

namespace StockMonitor.DBO
{
    /// <summary>
    /// To handle all the DB operations for the DailyRemarks table
    /// </summary>
    public class DailyRemarksOperations
    {
        /// <summary>
        /// Readonly connection object to our SQLite db
        /// </summary>
        private readonly SQLiteAsyncConnection _connection;

        /// <summary>
        /// Contructor to assign the connection
        /// </summary>
        /// <param name="connection">Connection object to be used henceforth</param>
        public DailyRemarksOperations(SQLiteAsyncConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Get all the DailyRemarks
        /// </summary>
        /// <returns>A list of DailyRemarks</returns>
        public async Task<List<DailyRemarks>> GetAllTypes() =>
            await _connection.Table<DailyRemarks>().ToListAsync();

        /// <summary>
        /// Get all the DailyRemarks as per the type id given
        /// </summary>
        /// <param name="companyId">typeId to be searched</param>
        /// <returns>list of DailyRemarks</returns>
        public async Task<List<DailyRemarks>> GetDailyRemarksBasedOnCompanyId(Guid companyId)
        {
            List<ChartRemarks> listOfCharts = await _connection.Table<ChartRemarks>().Where(o => o.CDId == companyId).ToListAsync();
            if (listOfCharts.Count == 0)
                return null;

            List<Guid> guidsToSearch = listOfCharts.Select(o => o.CRId).ToList();
            return await _connection.Table<DailyRemarks>().Where(o => guidsToSearch.Contains(o.CRId)).ToListAsync();
        }

        /// <summary>
        /// Get all the DailyRemarks as per the dates given
        /// </summary>
        /// <param name="from">from date</param>
        /// <param name="to">to date</param>
        /// <returns>list of DailyRemarks</returns>
        public async Task<List<DailyRemarks>> GetDailyRemarksBasedOnDates(DateTime from, DateTime to)
        {
            var fromDate = new DateTime(from.Year, from.Month, from.Day, 0, 0, 0);
            var toDate = new DateTime(to.Year, to.Month, to.Day, 0, 0, 0);

            return await _connection.Table<DailyRemarks>().Where(o => o.RemarkDate >= fromDate && o.RemarkDate <= toDate).ToListAsync();
        }

        /// <summary>
        /// Get the DailyRemarks object as per the id given
        /// </summary>
        /// <param name="Id">Id to be searched</param>
        /// <returns>DailyRemarks object</returns>
        public async Task<DailyRemarks> GetDailyRemarksBasedOnId(Guid Id) =>
            (await _connection.Table<DailyRemarks>().FirstOrDefaultAsync(o => o.DRId == Id));

        /// <summary>
        /// Method to save the DailyRemarks in DB
        /// </summary>
        /// <param name="details">DailyRemark to be saved</param>
        /// <returns>Error message string, null on success</returns>
        public async Task<string?> SaveDailyRemarkAsync(DailyRemarks details)
        {

            if (await _connection.InsertAsync(details) > 0)
                return null;

            return "Error in saving daily remark";
        }
    }
}
