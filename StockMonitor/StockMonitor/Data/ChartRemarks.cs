using SQLite;
using SQLiteNetExtensions.Attributes;

namespace StockMonitor.Data
{
    /// <summary>
    /// Class to represent the ChartRemarks in the DB
    /// </summary>
    public class ChartRemarks
    {
        /// <summary>
        /// Primary key to identify the record
        /// </summary>
        [PrimaryKey]
        public Guid CRId { get; set; } = Guid.NewGuid();

        /// <summary>
        /// This is the CompanyDetail Id this remark corresponds to
        /// Serves as a foreign key to CompanyDetails
        /// </summary>
        [ForeignKey(typeof(CompanyDetails))]
        public Guid CDId { get; set; }

        /// <summary>
        /// Object corresponding to the foreign key
        /// </summary>
        [ManyToOne]
        public CompanyDetails CompanyDetail { get; set; }

        /// <summary>
        /// The date on which the remark was made
        /// </summary>
        public DateTime RemarkDate { get; set; }
    }
}
