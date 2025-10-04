using SQLite;
using SQLiteNetExtensions.Attributes;
using StockMonitor.Enums;

namespace StockMonitor.Data
{
    /// <summary>
    /// Class to represent the MonthlyResults in the DB
    /// </summary>
    public class MonthlyResults
    {
        /// <summary>
        /// Primary key to identify the record
        /// </summary>
        [PrimaryKey]
        public Guid MResultId { get; set; } = Guid.NewGuid();

        /// <summary>
        /// This is the MonthlyRemarks Id this remark corresponds to
        /// Serves as a foreign key to MonthlyRemarks
        /// </summary>
        [ForeignKey(typeof(MonthlyRemarks))]
        public Guid MRId { get; set; }

        /// <summary>
        /// Object corresponding to the foreign key
        /// </summary>
        [ManyToOne]
        public MonthlyRemarks MonthlyRemark { get; set; }

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
        public string RemarkRemark { get; set; }
    }
}
