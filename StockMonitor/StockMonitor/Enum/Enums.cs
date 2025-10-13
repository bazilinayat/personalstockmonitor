using System.ComponentModel;
using System.Reflection;

namespace StockMonitor.Enums
{
    /// <summary>
    /// Class to take care of enum description
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Method to get the enum description for the given Enum value
        /// </summary>
        /// <param name="value">Value of enum</param>
        /// <returns>Returns description of the enum value</returns>
        public static string GetDescription(this Enum value)
        {
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
            if (fieldInfo == null) return value.ToString();

            var attribute = (DescriptionAttribute)fieldInfo.GetCustomAttribute(typeof(DescriptionAttribute));
            return attribute?.Description ?? value.ToString();
        }

        /// <summary>
        /// To get the list of all the descriptions from an enum
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <returns>Returns a list of all the descriptions</returns>
        //public static List<ValueForPicker> GetAllDescriptions<T>() where T : Enum
        //{
        //    var list = new List<ValueForPicker>();

        //    var type = typeof(T);
        //    return Enum.GetValues(type)
        //               .Cast<T>()
        //               .Select(value =>
        //               {
        //                   var fieldInfo = type.GetField(value.ToString());
        //                   var attribute = fieldInfo?.GetCustomAttribute<DescriptionAttribute>();

        //                   return new ValueForPicker
        //                   {
        //                       Key = Convert.ToInt32(value),
        //                       Value = attribute?.Description ?? value.ToString()
        //                   };
        //               })
        //               .ToList();
        //}
    }

    /// <summary>
    /// Positions that the user will be taking
    /// Either in buy or sell
    /// </summary>
    public enum Positions
    {
        /// <summary>
        /// For the postion to buy
        /// </summary>
        [Description("Buy")]
        Buy = 0,
        /// <summary>
        /// For the position to sell
        /// </summary>
        [Description("Sell")]
        Sell = 1
    }

    /// <summary>
    /// Predictions made my the user
    /// </summary>
    public enum Predictions
    {
        /// <summary>
        /// For the chart to go up
        /// </summary>
        [Description("Up")]
        Up = 0,
        /// <summary>
        /// For the chart to go down
        /// </summary>
        [Description("Down")]
        Down = 1
    }

    /// <summary>
    /// To know what tha result is of the prediction remark made
    /// </summary>
    public enum Results
    {
        /// <summary>
        /// For the result to be same as predicted
        /// </summary>
        [Description("For")]
        For = 0,
        /// <summary>
        /// For the result to be wrong
        /// </summary>
        [Description("Against")]
        Against = 1
    }

    /// <summary>
    /// To know the chart types
    /// </summary>
    public enum ChartTypes
    {
        /// <summary>
        /// To know if the chart is on daily time frame
        /// </summary>
        [Description("Daily")]
        Daily,
        /// <summary>
        /// To know if the chart is on weekly time frame
        /// </summary>
        [Description("Weekly")]
        Weekly,
        /// <summary>
        /// To know if the chart is on monthly time frame
        /// </summary>
        [Description("Monthly")]
        Monthly
    }
}
