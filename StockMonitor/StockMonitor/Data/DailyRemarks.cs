using SQLite;
using SQLiteNetExtensions.Attributes;
using StockMonitor.Enums;

namespace StockMonitor.Data
{
    /// <summary>
    /// Class to represent the DailyRemarks in the DB
    /// </summary>
    public class DailyRemarks
    {
        /// <summary>
        /// Primary key to identify the record
        /// </summary>
        [PrimaryKey]
        public Guid DRId { get; set; } = Guid.NewGuid();

        /// <summary>
        /// This is the ChartRemarks Id this remark corresponds to
        /// Serves as a foreign key to ChartRemarks
        /// </summary>
        [ForeignKey(typeof(ChartRemarks))]
        public Guid CRId { get; set; }

        /// <summary>
        /// Object corresponding to the foreign key
        /// </summary>
        [ManyToOne]
        public ChartRemarks CompanyDetail { get; set; }

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
