using System;
using System.ComponentModel;

namespace LHCommonFunctions.Classes {

    /// <summary>
    /// This class provides a base class for view model classes.
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// This function calls the property changed event of a property.
        /// All clients interested/bound to this property will be informed about it being changed.
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        public void OnPropertyChanged(String propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //This function calls the property changed event of an indexed property (e.g. Observable collection for combo-box) and returns the original index.

        /// <summary>
        /// This function calls the property changed event of a property.
        /// All clients interested/bound to this property will be informed about it being changed.
        /// If a control bound to a list is getting informed about the list was changed, it will reload the list and might reset the selected list-index.
        /// In case the list index is also bound, it this might lead to a no intended change of the index. 
        /// This function returns you the initial index saving you to create a temporary index every time you update the list property
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="index"></param>
        /// <returns></returns>
        public int OnPropertyChangedIndexed(String propertyName, int index) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return index;
        }
    }
}
