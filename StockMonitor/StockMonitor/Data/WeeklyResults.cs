using SQLite;
using SQLiteNetExtensions.Attributes;
using StockMonitor.Enums;

namespace StockMonitor.Data
{
    /// <summary>
    /// Class to represent the WeeklyResults in the DB
    /// </summary>
    public class WeeklyResults
    {
        /// <summary>
        /// Primary key to identify the record
        /// </summary>
        [PrimaryKey]
        public Guid WResultId { get; set; } = Guid.NewGuid();

        /// <summary>
        /// This is the WeeklyRemarks Id this remark corresponds to
        /// Serves as a foreign key to WeeklyRemarks
        /// </summary>
        [ForeignKey(typeof(WeeklyRemarks))]
        public Guid WRId { get; set; }

        /// <summary>
        /// Object corresponding to the foreign key
        /// </summary>
        [ManyToOne]
        public WeeklyRemarks WeeklyRemark { get; set; }

        /// <summary>
        /// The date on which the result was checked
        /// </summary>
        public DateTime ResultDate { get; set; }

        /// <summary>
        /// Result of the remark made
        /// </summary>
        public Results Result { get; set; }

        /// <summary>
        /// To hold the notes of the result
        /// </summary>
        public string ResultRemarks { get; set; }
    }
}
