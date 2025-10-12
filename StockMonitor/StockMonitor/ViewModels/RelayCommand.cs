using System.Windows.Input;

namespace StockMonitor.ViewModels
{
    /// <summary>
    /// Relay command class to be used by all the other view models
    /// </summary>
    public class RelayCommand : ICommand
    {
        /// <summary>
        /// Action of the command
        /// </summary>
        private readonly Action _execute;

        /// <summary>
        /// To know if the command can be execute or not
        /// </summary>
        private readonly Func<bool> _canExecute;

        /// <summary>
        /// Event handler that is assigned to the command
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// To initialize and assign the specific commands
        /// </summary>
        /// <param name="execute">Action to execute</param>
        /// <param name="canExecute">Condition for execution</param>
        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        /// <summary>
        /// To see if the function can be executed
        /// </summary>
        /// <param name="parameter">Parameter to check</param>
        /// <returns>boolean</returns>
        public bool CanExecute(object parameter) => _canExecute?.Invoke() ?? true;

        /// <summary>
        /// To execute the function
        /// </summary>
        /// <param name="parameter">paramter to check</param>
        public void Execute(object parameter) => _execute();

        /// <summary>
        /// To change the state of the command
        /// </summary>
        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T?> _execute;
        private readonly Func<T?, bool>? _canExecute;

        public RelayCommand(Action<T?> execute, Func<T?, bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            if (_canExecute == null) return true;
            return _canExecute(ConvertParameter(parameter));
        }

        public void Execute(object? parameter) => _execute(ConvertParameter(parameter));

        public event EventHandler? CanExecuteChanged;
        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        private static T? ConvertParameter(object? parameter)
        {
            if (parameter == null) return default;
            if (parameter is T t) return t;
            // try change type e.g. when binding passes string etc.
            return (T?)System.Convert.ChangeType(parameter, typeof(T));
        }
    }

    public class AsyncRelayCommand : ICommand
    {
        private readonly Func<Task> _execute;
        private readonly Func<bool>? _canExecute;
        private bool _isExecuting;

        public event EventHandler? CanExecuteChanged;

        public AsyncRelayCommand(Func<Task> execute, Func<bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter) => !_isExecuting && (_canExecute?.Invoke() ?? true);

        public async void Execute(object? parameter)
        {
            if (!CanExecute(parameter)) return;
            _isExecuting = true;
            RaiseCanExecuteChanged();

            try
            {
                await _execute();
            }
            finally
            {
                _isExecuting = false;
                RaiseCanExecuteChanged();
            }
        }

        public void RaiseCanExecuteChanged() =>
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
