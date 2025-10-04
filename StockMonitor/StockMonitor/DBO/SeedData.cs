using StockMonitor.Data;

namespace StockMonitor.DBO
{
    /// <summary>
    /// Class for initial data for application
    /// Menu data should come from menu.json file
    /// </summary>
    public class SeedData
    {

        /// <summary>
        /// Read Types from json file for initial data
        /// </summary>
        /// <returns>Returns a List of Types</returns>
        public List<Types> GetTypesFromSettings()
        {
            try
            {
                var types = new List<Types>()
                {
                    new Types{ Name = "NIFTY 50"},
                    new Types{ Name = "NIFTY 100"},
                    new Types{ Name = "NIFTY 200"},
                    new Types{ Name = "NIFTY 500"},
                    new Types{ Name = "FUTURES"},

                };

                return types;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
