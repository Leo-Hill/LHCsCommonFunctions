using System;
using System.ComponentModel;

namespace LHCommonFunctions.Source
{
    /***********************************************************************************************
    * 
    * This class provides a base class for viem model classes.
    * 
    **********************************************************************************************/
    public abstract class Class_ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        //This function calls the property changed event of an indexed property
        public void OnPropertyChanged(String qsPropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(qsPropertyName));
        }

        //This function calls the property changed event of an indexed property (e.g. Observablecollection for combobox) and returns the original index.
        public int OnPropertyChangedIndexed(String qsPropertyName, int qiIndex)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(qsPropertyName));
            return qiIndex;
        }
    }
}
