using System;
using System.Windows.Input;

namespace Clippy.Wpf.Demo.ViewModels
{
    public class DelegateCommand<TType> : ICommand
    {
        private readonly Predicate<TType> m_canExecute;
        private readonly Action<TType> m_execute;
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="execute"></param>
        public DelegateCommand(Action<TType> execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        public DelegateCommand(Action<TType> execute, Predicate<TType> canExecute)
        {
            m_execute = execute;
            m_canExecute = canExecute;
        }

        /// <summary>
        /// Check to see if the command can be executed
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            if (m_canExecute == null)
            {
                return true;
            }

            if (!(parameter is TType))
            {
                return false;
            }

            return m_canExecute((TType)parameter);
        }

        /// <summary>
        /// Execute the delegate command
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            m_execute((TType)parameter);
        }

        /// <summary>
        /// Raise can execute event
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }
}
