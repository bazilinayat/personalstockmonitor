using SQLite;
using StockMonitor.Data;

namespace StockMonitor.DBO
{
    /// <summary>
    /// To handle all the db operations for the Types table
    /// </summary>
    public class TypesOperations
    {
        /// <summary>
        /// Readonly connection object to our SQLite db
        /// </summary>
        private readonly SQLiteAsyncConnection _connection;

        /// <summary>
        /// Contructor to assign the connection
        /// </summary>
        /// <param name="connection">Connection object to be used henceforth</param>
        public TypesOperations(SQLiteAsyncConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Get all the types
        /// </summary>
        /// <returns>A list of Types</returns>
        public async Task<List<Types>> GetAllTypes() =>
            await _connection.Table<Types>().ToListAsync();

        /// <summary>
        /// Get the Types object as per the id given
        /// </summary>
        /// <param name="Id">Id to be searched</param>
        /// <returns>Types object</returns>
        public async Task<Types> GetTypesObjectBasedOnId(Guid Id) =>
            (await _connection.Table<Types>().FirstOrDefaultAsync(o => o.Id == Id));

        /// <summary>
        /// Method to save the types in DB
        /// </summary>
        /// <param name="types">Types list to be saved</param>
        /// <returns>Error message string, null on success</returns>
        public async Task<string?> SaveTypeAsync(List<Types> types)
        {

            if (await _connection.InsertAllAsync(types) > 0)
                return null;

            return "Error in saving type data";
            
        }
    }
}
