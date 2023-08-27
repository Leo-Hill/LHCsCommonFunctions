using System;
using System.Windows.Input;

namespace LHCommonFunctions.Classes {
    /// <summary>
    ///  This class provides functions for command usage in view-models.
    /// </summary>
    public class RelayCommand : ICommand {
        readonly Action<object> _action;
        readonly Predicate<object> _predicate;
        
        /// <summary>
        /// Use this constructor in case the command should be executed without a predicate to be evaluated.
        /// </summary>
        /// <param name="action">The action to be performed when the command is executed</param>
        public RelayCommand(Action<object> action) : this(action, null) {
        }

        /// <summary>
        /// Use this constructor in case predicate should be evaluated when the command is executed.
        /// In case the predicate is not true, the action will not be performed.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="predicate"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public RelayCommand(Action<object> action, Predicate<object> predicate) {
            if (action == null) {
                throw new ArgumentNullException("execute");
            }
            _action = action;
            _predicate = predicate;
        }

        /// <summary>
        /// With this property a control can subscribe to the event CanExecuteChanged.
        /// When the CommandManager.RequerySuggested event will be triggered, we want to re-evaluate if the command can be executed.
        /// </summary>
        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// This function will be called in order to evaluate if all criteria for the predicate are met.
        /// </summary>
        /// <param name="commandParameter">Optional parameter which can be passed to the predicate</param>
        /// <returns>True if predicate mets all requirements, false if not</returns>
        public bool CanExecute(object commandParameter) {
            return _predicate == null ? true : _predicate(commandParameter);
        }

        /// <summary>
        /// This function will be called in order to execute the action of this command.
        /// </summary>
        /// <param name="commandParameter"></param>
        public void Execute(object commandParameter) {
            _action(commandParameter);
        }
    }
}
