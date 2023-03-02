using System;
using System.Windows.Input;

// public delegate void TappedEventHandler(object sender, TappedRoutedEventArgs e);

namespace UwwpSample.ViewModels
{
    public class GenericDelegateCommand<T> : ICommand
    {
        private readonly Predicate<object> _canExecute;
        private Action<T> _handler { get; set; }
        public event EventHandler CanExecuteChanged;

        public GenericDelegateCommand(Action<T> handler, Predicate<object> canExecute)
        {
            this._handler = handler;
            _canExecute = canExecute;
        }

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, EventArgs.Empty);
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _handler((T)parameter);
        }
    }
}
