using System;
using System.Windows.Input;

namespace LHCommonFunctions.Source
{
    /***********************************************************************************************
    * 
    * This class provides functions for command usage in viewmodels
    * 
    **********************************************************************************************/
    public class Class_RelayCommand : ICommand
    {
        readonly Action<object> _execute;
        readonly Predicate<object> _canExecute;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="execute">The function which should be executed if the command was triggered</param>
        /// <param name="canExecute">Predicate determining if the command can be executed</param>
        /// <exception cref="ArgumentNullException"></exception>
        public Class_RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("Execution function can not be null");
            }
            _execute = execute;
            _canExecute = canExecute;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="execute">The function which should be executed if the command was triggered</param>
        public Class_RelayCommand(Action<object> execute) : this(execute, null)
        {

        }

        /// <summary>
        /// See interface for documentation
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// See interface for documentation
        /// </summary>
        public bool CanExecute(object param)
        {
            return _canExecute == null ? true : _canExecute(param);
        }

        /// <summary>
        /// See interface for documentation
        /// </summary>
        public void Execute(object param)
        {
            _execute(param);
        }
    }
}
