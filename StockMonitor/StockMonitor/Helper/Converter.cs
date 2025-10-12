using System.Globalization;
using System.Windows.Data;

namespace StockMonitor.Helper
{   
    // IsLessThanConverter //
    public class IsLessThanConverter : IValueConverter
    {
        public static readonly IValueConverter Instance = new IsLessThanConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double doubleValue = System.Convert.ToDouble(value);
            double compareToValue = System.Convert.ToDouble(parameter);

            return doubleValue < compareToValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    // IsGreaterThanConverter //
    public class IsGreaterThanConverter : IValueConverter
    {
        public static readonly IValueConverter Instance = new IsGreaterThanConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double doubleValue = System.Convert.ToDouble(value);
            double compareToValue = System.Convert.ToDouble(parameter);

            return doubleValue > compareToValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class OptionToBooleanConverter : IValueConverter
    {
        // value = SelectedOption, parameter = current OptionItem
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Equals(value, parameter);
        }

        // when user checks a RadioButton, this returns the OptionItem to be set as SelectedOption
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? parameter : Binding.DoNothing;
        }
    }

    public class OptionToBooleanMultiConverter : IMultiValueConverter
    {
        // values[0] = SelectedOption, values[1] = current OptionItem
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2) return false;
            var selected = values[0];
            var current = values[1];
            return Equals(selected, current);
        }

        // When a radio button is checked, return that option as the new SelectedOption
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            var isChecked = (bool)value;
            if (!isChecked)
                return new object[] { Binding.DoNothing, Binding.DoNothing };

            // Return new SelectedOption = current OptionItem
            return new object[] { Binding.DoNothing, Binding.DoNothing };
        }
    }

    public class CountToBoolConverter : IValueConverter
    {
        // Converts a collection's Count (or any numeric value) to a bool
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return false;

            if (value is int count)
                return count > 0;

            // If it's a collection
            if (value is System.Collections.ICollection collection)
                return collection.Count > 0;

            return false;
        }

        // Not needed for one-way binding
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
