using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LHCommonFunctions.Source
{
    /***********************************************************************************************
    * 
    * This class provides a functions for command usage in viewmodels
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

    /***********************************************************************************************
    * 
    * This class provides a base class for viem model classes.
    * 
    **********************************************************************************************/
    public abstract class Class_ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(String qSPropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(qSPropertyName));
        }
    }


   /***********************************************************************************************
   * 
   * This class provides a function to validate bound input data
   * 
   **********************************************************************************************/
    public static class CLass_Validator
    {
        public static bool IsValid(DependencyObject obj)
        {
            // The dependency object is valid if it has no errors and all of its children (that are dependency objects) are error-free.
            return !Validation.GetHasError(obj) &&LogicalTreeHelper.GetChildren(obj).OfType<DependencyObject>().All(IsValid);
        }
    }
}
