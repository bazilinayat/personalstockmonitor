using SQLite;
using StockMonitor.Data;

namespace StockMonitor.DBO
{
    /// <summary>
    /// To handle all the DB operations for the WeeklyRemarks table
    /// </summary>
    public class WeeklyRemarksOperations
    {
        /// <summary>
        /// Readonly connection object to our SQLite db
        /// </summary>
        private readonly SQLiteAsyncConnection _connection;

        /// <summary>
        /// Contructor to assign the connection
        /// </summary>
        /// <param name="connection">Connection object to be used henceforth</param>
        public WeeklyRemarksOperations(SQLiteAsyncConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Get all the WeeklyRemarks
        /// </summary>
        /// <returns>A list of WeeklyRemarks</returns>
        public async Task<List<WeeklyRemarks>> GetAllTypes() =>
            await _connection.Table<WeeklyRemarks>().ToListAsync();

        /// <summary>
        /// Get all the WeeklyRemarks as per the type id given
        /// </summary>
        /// <param name="companyId">typeId to be searched</param>
        /// <returns>list of WeeklyRemarks</returns>
        public async Task<List<WeeklyRemarks>> GetWeeklyRemarksBasedOnCompanyId(Guid companyId)
        {
            return await _connection.Table<WeeklyRemarks>().Where(o => o.CDId == companyId).ToListAsync();
        }

        /// <summary>
        /// Get all the WeeklyRemarks as per the dates given
        /// </summary>
        /// <param name="from">from date</param>
        /// <param name="to">to date</param>
        /// <returns>list of WeeklyRemarks</returns>
        public async Task<List<WeeklyRemarks>> GetWeeklyRemarksBasedOnDates(DateTime from, DateTime to)
        {
            var fromDate = new DateTime(from.Year, from.Month, from.Day, 0, 0, 0);
            var toDate = new DateTime(to.Year, to.Month, to.Day, 0, 0, 0);

            return await _connection.Table<WeeklyRemarks>().Where(o => o.RemarkDate >= fromDate && o.RemarkDate <= toDate).ToListAsync();
        }

        /// <summary>
        /// Get all the WeeklyRemarks as that are to be checked today
        /// </summary>
        /// <returns>list of WeeklyRemarks</returns>
        public async Task<List<WeeklyRemarks>> GetWeeklyRemarksToCheckToday()
        {
            var fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            var tomorrow = DateTime.Now.AddDays(1);
            var toDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 0, 0, 0);

            return await _connection.Table<WeeklyRemarks>().Where(o => o.CheckDate >= fromDate && o.CheckDate < toDate && !o.IsChecked).ToListAsync();
        }

        /// <summary>
        /// Get the WeeklyRemarks object as per the id given
        /// </summary>
        /// <param name="Id">Id to be searched</param>
        /// <returns>WeeklyRemarks object</returns>
        public async Task<WeeklyRemarks> GetWeeklyRemarksBasedOnId(Guid Id) =>
            (await _connection.Table<WeeklyRemarks>().FirstOrDefaultAsync(o => o.WRId == Id));

        /// <summary>
        /// Method to save the WeeklyRemarks in DB
        /// </summary>
        /// <param name="details">WeeklyRemark to be saved</param>
        /// <returns>Error message string, null on success</returns>
        public async Task<string?> SaveWeeklyRemarkAsync(WeeklyRemarks details)
        {

            if (await _connection.InsertAsync(details) > 0)
                return null;

            return "Error in saving weekly remark";
        }

        /// <summary>
        /// Method to update the WeeklyRemark in DB
        /// </summary>
        /// <param name="details">WeeklyRemark to be saved</param>
        /// <returns>Error message string, null on success</returns>
        public async Task<string?> UpdateWeeklyRemarkAsync(WeeklyRemarks details)
        {

            if (await _connection.UpdateAsync(details) > 0)
                return null;

            return "Error in updating weekly remark";
        }
    }
}
