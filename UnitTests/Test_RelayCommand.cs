using LHCommonFunctions.Source;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Classes
{
    [TestClass]
    public class Test_RelayCommand
    {

        private bool _executeCalled = false;
        private bool _canExecuteCalled = false;

        private void Execute(Object param)
        {
            _executeCalled = true;
        }
        private bool CanExecute(Object param)
        {
            _canExecuteCalled = true;
            return true;
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorThrowsExceptionWhenExecuteIsNull()
        {
            Class_RelayCommand relayCommand = new Class_RelayCommand(null);
        }

        [TestMethod]
        public void CommandIsExecutedCorrectly()
        {
            _executeCalled = false;
            Class_RelayCommand relayCommand = new Class_RelayCommand(Execute);
            relayCommand.Execute(null);
            Assert.IsTrue(_executeCalled);
        }

        [TestMethod]
        public void CommandCanExecuteIsCalledCorrectly()
        {
            _canExecuteCalled = false;
            Class_RelayCommand relayCommand = new Class_RelayCommand(Execute, CanExecute);
            Assert.IsTrue(relayCommand.CanExecute(null));
            Assert.IsTrue(_canExecuteCalled);
        }

    }
}
