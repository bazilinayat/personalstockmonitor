namespace StockMonitor.Models
{
    /// <summary>
    /// The class to represent the key-value pair for radio, check and drop downs
    /// </summary>
    public class OptionItem
    {
        /// <summary>
        /// The id of the pair
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// The text name of the pair
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}
