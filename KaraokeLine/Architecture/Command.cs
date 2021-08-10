using System;
using System.Reflection;
using System.Windows.Input;

namespace KaraokeLine.Architecture
{
    public class Command : ICommand
    {
        private Action<object> _execute;
        private Func<object, bool> _canExecute;

        public Command(Action<object> execute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        }

        public Command(Action execute) : this(o => execute())
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));
        }

        public Command(Action<object> execute, Func<object, bool> canExecute) : this(execute)
        {
            _canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
        }

        public Command(Action execute, Func<bool> canExecute) : this(o => execute(), o => canExecute())
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));
            if (canExecute == null)
                throw new ArgumentNullException(nameof(canExecute));
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }

    public sealed class Command<T> : Command
    {
        public Command(Action<T> execute)
            : base(o =>
            {
                if (IsValidParameter(o)) execute((T) o);
            })
        {
            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }
        }

        public Command(Action<T> execute, Func<T, bool> canExecute)
            : base(o =>
            {
                if (IsValidParameter(o)) execute((T) o);
            }, o => IsValidParameter(o) && canExecute((T) o))
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));
            if (canExecute == null)
                throw new ArgumentNullException(nameof(canExecute));
        }

        private static bool IsValidParameter(object o)
        {
            if (o != null)
                return o is T;

            var t = typeof(T);
            if (Nullable.GetUnderlyingType(t) != null)
                return true;
            return !t.GetTypeInfo().IsValueType;
        }
    }
}