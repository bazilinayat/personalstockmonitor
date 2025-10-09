using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace StockMonitor.ViewModels
{
    /// <summary>
    /// The view model for all the view models to use for property changes
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// The handle for property change
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// If a property is set, then to assign the value to the selected property
        /// </summary>
        /// <typeparam name="T">generic</typeparam>
        /// <param name="storage">existing value</param>
        /// <param name="value">value to store</param>
        /// <param name="propertyName">property name for change</param>
        protected void SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(storage, value))
            {
                storage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
