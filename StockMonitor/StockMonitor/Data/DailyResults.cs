using SQLite;
using SQLiteNetExtensions.Attributes;
using StockMonitor.Enums;

namespace StockMonitor.Data
{
    /// <summary>
    /// Class to represent the DailyResults in the DB
    /// </summary>
    public class DailyResults
    {
        /// <summary>
        /// Primary key to identify the record
        /// </summary>
        [PrimaryKey]
        public Guid DResultId { get; set; } = Guid.NewGuid();

        /// <summary>
        /// This is the DailyRemarks Id this remark corresponds to
        /// Serves as a foreign key to DailyRemarks
        /// </summary>
        [ForeignKey(typeof(DailyRemarks))]
        public Guid DRId { get; set; }

        /// <summary>
        /// Object corresponding to the foreign key
        /// </summary>
        [ManyToOne]
        public DailyRemarks DailyRemark { get; set; }

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
        public string ResultRemark { get; set; }
    }
}
