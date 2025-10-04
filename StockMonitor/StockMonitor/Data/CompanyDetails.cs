using SQLite;
using SQLiteNetExtensions.Attributes;

namespace StockMonitor.Data
{
    /// <summary>
    /// Class to represent the CompanyDetails in the DB
    /// </summary>
    public class CompanyDetails
    {
        /// <summary>
        /// Primary key to identify the record
        /// </summary>
        [PrimaryKey]
        public Guid CDId { get; set; } = Guid.NewGuid();
        
        /// <summary>
        /// The symbol of the company, this is used to identify the chart
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// The Name of the company
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Type Id that this record corresponds to
        /// Used as a foreign key to Types
        /// </summary>
        [ForeignKey(typeof(Types))]
        public Guid TypeId { get; set; }

        /// <summary>
        /// Types object corresponding to the foreign key
        /// </summary>
        [ManyToOne]
        public Types Type { get; set; }
    }
}
