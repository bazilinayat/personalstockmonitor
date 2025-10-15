using SQLite;
using StockMonitor.Data;

namespace StockMonitor.DBO
{
    /// <summary>
    /// To handle all the DB operations for the MonthlyRemarks table
    /// </summary>
    public class MonthlyRemarksOperations
    {
        /// <summary>
        /// Readonly connection object to our SQLite db
        /// </summary>
        private readonly SQLiteAsyncConnection _connection;

        /// <summary>
        /// Contructor to assign the connection
        /// </summary>
        /// <param name="connection">Connection object to be used henceforth</param>
        public MonthlyRemarksOperations(SQLiteAsyncConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Get all the MonthlyRemarks
        /// </summary>
        /// <returns>A list of MonthlyRemarks</returns>
        public async Task<List<MonthlyRemarks>> GetAllTypes() =>
            await _connection.Table<MonthlyRemarks>().ToListAsync();

        /// <summary>
        /// Get all the MonthlyRemarks as per the type id given
        /// </summary>
        /// <param name="companyId">typeId to be searched</param>
        /// <returns>list of MonthlyRemarks</returns>
        public async Task<List<MonthlyRemarks>> GetMonthlyRemarksBasedOnCompanyId(Guid companyId)
        {
            return await _connection.Table<MonthlyRemarks>().Where(o => o.CDId == companyId).ToListAsync();    
        }

        /// <summary>
        /// Get all the MonthlyRemarks as per the dates given
        /// </summary>
        /// <param name="from">from date</param>
        /// <param name="to">to date</param>
        /// <returns>list of MonthlyRemarks</returns>
        public async Task<List<MonthlyRemarks>> GetMonthlyRemarksBasedOnDates(DateTime from, DateTime to)
        {
            var fromDate = new DateTime(from.Year, from.Month, from.Day, 0, 0, 0);
            var toDate = new DateTime(to.Year, to.Month, to.Day, 0, 0, 0);

            return await _connection.Table<MonthlyRemarks>().Where(o => o.RemarkDate >= fromDate && o.RemarkDate <= toDate).ToListAsync();
        }

        /// <summary>
        /// Get all the MonthlyRemarks as that are to be checked today
        /// </summary>
        /// <returns>list of MonthlyRemarks</returns>
        public async Task<List<MonthlyRemarks>> GetMonthlyRemarksToCheckToday()
        {
            var fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            var tomorrow = DateTime.Now.AddDays(1);
            var toDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 0, 0, 0);

            return await _connection.Table<MonthlyRemarks>().Where(o => o.CheckDate >= fromDate && o.CheckDate < toDate && !o.IsChecked).ToListAsync();
        }

        /// <summary>
        /// Get the MonthlyRemarks object as per the id given
        /// </summary>
        /// <param name="Id">Id to be searched</param>
        /// <returns>MonthlyRemarks object</returns>
        public async Task<MonthlyRemarks> GetMonthlyRemarksBasedOnId(Guid Id) =>
            (await _connection.Table<MonthlyRemarks>().FirstOrDefaultAsync(o => o.MRId == Id));

        /// <summary>
        /// Method to save the MonthlyRemarks in DB
        /// </summary>
        /// <param name="details">WeeklyRemark to be saved</param>
        /// <returns>Error message string, null on success</returns>
        public async Task<string?> SaveMonthlyRemarkAsync(MonthlyRemarks details)
        {

            if (await _connection.InsertAsync(details) > 0)
                return null;

            return "Error in saving monthly remark";
        }

        /// <summary>
        /// Method to update the MonthlyRemark in DB
        /// </summary>
        /// <param name="details">MonthlyRemark to be saved</param>
        /// <returns>Error message string, null on success</returns>
        public async Task<string?> UpdateMonthlyRemarkAsync(MonthlyRemarks details)
        {

            if (await _connection.UpdateAsync(details) > 0)
                return null;

            return "Error in updating monthly remark";
        }
    }
}
