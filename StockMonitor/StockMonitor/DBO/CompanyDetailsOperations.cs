using SQLite;
using StockMonitor.Data;

namespace StockMonitor.DBO
{
    /// <summary>
    /// To handle all the DB operations for the CompanyDetails table
    /// </summary>
    public class CompanyDetailsOperations
    {
        /// <summary>
        /// Readonly connection object to our SQLite db
        /// </summary>
        private readonly SQLiteAsyncConnection _connection;

        /// <summary>
        /// Contructor to assign the connection
        /// </summary>
        /// <param name="connection">Connection object to be used henceforth</param>
        public CompanyDetailsOperations(SQLiteAsyncConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Get all the CompanyDetails
        /// </summary>
        /// <returns>A list of CompanyDetails</returns>
        public async Task<List<CompanyDetails>> GetAllTypes() =>
            await _connection.Table<CompanyDetails>().ToListAsync();

        /// <summary>
        /// Get all the CompanyDetails as per the type id given
        /// </summary>
        /// <param name="typeId">typeId to be searched</param>
        /// <returns>list of CompanyDetails</returns>
        public async Task<List<CompanyDetails>> GetCompanyDetailsBasedOnTypeId(Guid typeId) =>
            (await _connection.Table<CompanyDetails>().Where(o => o.TypeId == typeId).ToListAsync());

        /// <summary>
        /// Get the CompanyDetails object as per the id given
        /// </summary>
        /// <param name="Id">Id to be searched</param>
        /// <returns>CompanyDetails object</returns>
        public async Task<CompanyDetails> GetCompanyDetailsBasedOnId(Guid Id) =>
            (await _connection.Table<CompanyDetails>().FirstOrDefaultAsync(o => o.CDId == Id));

        /// <summary>
        /// To Get company detail by name thorugh searching in database
        /// </summary>
        /// <param name="searchText">The company symbol to search</param>
        /// <returns>Returns a list of company details</returns>
        public async Task<List<CompanyDetails>> GetCompanyDetailBySearch(string searchText) =>
            await _connection.Table<CompanyDetails>().Where(o => o.Symbol.Contains(searchText) || o.Name.ToLower().Contains(searchText.ToLower())).ToListAsync();

        /// <summary>
        /// Method to save the CompanyDetails in DB
        /// </summary>
        /// <param name="details">CompanyDetails list to be saved</param>
        /// <returns>Error message string, null on success</returns>
        public async Task<string?> SaveCompanyDetailsAsync(List<CompanyDetails> details)
        {

            if (await _connection.InsertAllAsync(details) > 0)
                return null;

            return "Error in saving company details";
            
        }

        /// <summary>
        /// Delete all CompanyDetails from the database
        /// </summary>
        /// <param name="typeId">The type id for which we want to delete the details</param>
        /// <returns>Returns the number of rows deleted</returns>
        public async Task<int> DeleteAllCompanyDetailsItemAsync(Guid typeId)
        {
            var details = await GetCompanyDetailsBasedOnTypeId(typeId);
            foreach(var detail in details)
                await _connection.DeleteAsync(detail);

            return 0;
        }
    }
}
