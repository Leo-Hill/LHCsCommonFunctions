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

        public Class_RelayCommand(Action<object> qAExecute, Predicate<object> qPCanExecute)
        {
            if (qAExecute == null)
                throw new ArgumentNullException("execute");

            _execute = qAExecute;
            _canExecute = qPCanExecute;
        }

        public Class_RelayCommand(Action<object> qAExecute) : this(qAExecute, null)
        {

        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object qObject)
        {
            return _canExecute == null ? true : _canExecute(qObject);
        }

        public void Execute(object qObject)
        {
            _execute(qObject);
        }
    }
}
