using SQLite;

namespace StockMonitor.Data
{
    /// <summary>
    /// Class to represent the UploadHistory table in DB
    /// </summary>
    public class UploadHistory
    {
        /// <summary>
        /// Primary key to identify the record
        /// </summary>
        [PrimaryKey]
        public Guid Id { get; set; }
        
        /// <summary>
        /// The date on which upload was done
        /// </summary>
        public DateTime UploadDate { get; set; }
    }
}
