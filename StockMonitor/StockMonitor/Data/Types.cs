using SQLite;

namespace StockMonitor.Data
{
    /// <summary>
    /// Class to hold the structure for the Types table in DB
    /// </summary>
    public class Types
    {
        /// <summary>
        /// Primary key for the type to represent it
        /// </summary>
        [PrimaryKey]
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Name of the type
        /// </summary>
        [Unique]
        public string Name { get; set; }
    }
}
