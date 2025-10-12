using SQLite;
using SQLiteNetExtensions.Attributes;
using StockMonitor.Enums;

namespace StockMonitor.Data
{
    /// <summary>
    /// Class to represent the MonthlyRemarks in the DB
    /// </summary>
    public class MonthlyRemarks
    {
        /// <summary>
        /// Primary key to identify the record
        /// </summary>
        [PrimaryKey]
        public Guid MRId { get; set; } = Guid.NewGuid();

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

        /// <summary>
        /// Postion taken on the remark
        /// </summary>
        public Positions Position { get; set; }

        /// <summary>
        /// Prediciton thought of on the remark
        /// </summary>
        public Predictions Prediction { get; set; }

        /// <summary>
        /// Date on which the chart needs to be checked after prediction
        /// </summary>
        public DateTime CheckDate { get; set; }

        /// <summary>
        /// To hold the notes of the remark
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// To know if the remark is checked or not
        /// </summary>
        public bool IsChecked { get; set; }

    }
}
