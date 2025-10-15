namespace StockMonitor.Models
{
    /// <summary>
    /// to represent the summary for the selected char type and date range in form of a table
    /// </summary>
    public class ReportItem
    {
        /// <summary>
        /// The date for this record
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// The company name for this record
        /// </summary>
        public string Company { get; set; }
        /// <summary>
        /// The prediction that was made for the company
        /// </summary>
        public string Prediction { get; set; }
        /// <summary>
        /// The result that came for this company
        /// </summary>
        public string Result { get; set; }
    }
}
