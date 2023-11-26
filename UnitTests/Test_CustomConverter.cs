using LHCommonFunctions.Source;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Classes
{
    [TestClass]
    public class Test_CustomConverter
    {

        [TestMethod]
        public void IVCBooleanNotToVisibilityConvertsCorrectly()
        {
            IVCBooleanNotToVisibility converter = new IVCBooleanNotToVisibility();
            Visibility visibility = (Visibility)converter.Convert(true, null, null, System.Globalization.CultureInfo.InvariantCulture);
            Assert.AreEqual(Visibility.Collapsed, visibility);

            visibility = (Visibility)converter.Convert(false, null, null, System.Globalization.CultureInfo.InvariantCulture);
            Assert.AreEqual(Visibility.Visible, visibility);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void IVCBooleanNotToVisibilityConvertsBackCorrectly()
        {
            IVCBooleanNotToVisibility converter = new IVCBooleanNotToVisibility();
            converter.ConvertBack(Visibility.Visible, null, null, System.Globalization.CultureInfo.InvariantCulture);
        }


        [TestMethod]
        public void IVCBooleanToVisibilityHiddenConvertsCorrectly()
        {
            IVCBooleanToVisibilityHidden converter = new IVCBooleanToVisibilityHidden();
            Visibility visibility = (Visibility)converter.Convert(true, null, null, System.Globalization.CultureInfo.InvariantCulture);
            Assert.AreEqual(Visibility.Visible, visibility);

            visibility = (Visibility)converter.Convert(false, null, null, System.Globalization.CultureInfo.InvariantCulture);
            Assert.AreEqual(Visibility.Hidden, visibility);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void IVCBooleanToVisibilityHiddenConvertsBackCorrectly()
        {
            IVCBooleanToVisibilityHidden converter = new IVCBooleanToVisibilityHidden();
            converter.ConvertBack(Visibility.Visible, null, null, System.Globalization.CultureInfo.InvariantCulture);
        }

        [TestMethod]
        public void IVCDateTimeToStringConvertsCorrectly()
        {
            IVCDateTimeToString converter = new IVCDateTimeToString();
            DateTime now = DateTime.Now;
            String date = (String)converter.Convert(now, null, null, System.Globalization.CultureInfo.InvariantCulture);
            Assert.AreEqual(now.ToString("dd.MM.yyyy"), date);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void IVCDateTimeToStringConvertsBackCorrectly()
        {
            IVCDateTimeToString converter = new IVCDateTimeToString();
            converter.ConvertBack("01.01.2020", null, null, System.Globalization.CultureInfo.InvariantCulture);
        }


        [TestMethod]
        public void IVCFilterStringToVisibilityConvertsCorrectly()
        {
            IVCFilterStringToVisibility converter = new IVCFilterStringToVisibility();
            String filter = "View1";
            String name = "View1";

            Visibility visibility = (Visibility)converter.Convert(filter, null, name, System.Globalization.CultureInfo.InvariantCulture);
            Assert.AreEqual(Visibility.Visible, visibility);

            name = "View2";
            visibility = (Visibility)converter.Convert(filter, null, name, System.Globalization.CultureInfo.InvariantCulture);
            Assert.AreEqual(Visibility.Collapsed, visibility);

            name = "View1 View2";
            visibility = (Visibility)converter.Convert(filter, null, name, System.Globalization.CultureInfo.InvariantCulture);
            Assert.AreEqual(Visibility.Visible, visibility);
        }
        [TestMethod]
        public void IVCFilterStringToVisibilityConvertsCorrectlyIfParametersAreInvalid()
        {
            IVCFilterStringToVisibility converter = new IVCFilterStringToVisibility();

            Visibility visibility = (Visibility)converter.Convert("", null, "", System.Globalization.CultureInfo.InvariantCulture);
            Assert.AreEqual(Visibility.Visible, visibility);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void IVCFilterStringToVisibilityConvertsBackCorrectly()
        {
            IVCFilterStringToVisibility converter = new IVCFilterStringToVisibility();
            converter.ConvertBack(" ", null, " ", System.Globalization.CultureInfo.InvariantCulture);
        }

        [TestMethod]
        public void IVCInverseBooleanConvertsCorrectly()
        {
            IVCInverseBoolean converter = new IVCInverseBoolean();

            bool result = (bool)converter.Convert(true, null, null, System.Globalization.CultureInfo.InvariantCulture);
            Assert.IsFalse(result);

            result = (bool)converter.Convert(false, null, null, System.Globalization.CultureInfo.InvariantCulture);
            Assert.IsTrue(result);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void IVCInverseBooleanConvertsBackCorrectly()
        {
            IVCInverseBoolean converter = new IVCInverseBoolean();
            converter.ConvertBack(true, null, null, System.Globalization.CultureInfo.InvariantCulture);
        }

        [TestMethod]
        public void IVCRoutedCommandToInputGestureTextConvertsCorrectly()
        {
            RoutedUICommand command = new RoutedUICommand
                 (
                     "TestText",
                     "TestName",
                     typeof(Test_CustomConverter),
                     new InputGestureCollection()
                     {
                       new KeyGesture(Key.T,ModifierKeys.Control)
                     }
                 );

            IVCRoutedCommandToInputGestureText converter = new IVCRoutedCommandToInputGestureText();
            String text = (String)converter.Convert(command, null, null, System.Globalization.CultureInfo.InvariantCulture);
            Assert.AreEqual("(Ctrl+T)", text);
        }

        [TestMethod]
        public void IVCRoutedCommandToInputGestureTextConvertsCorrectlyIfNoGestureIsSet()
        {
            RoutedUICommand command = new RoutedUICommand
                 (
                     "TestText",
                     "TestName",
                     typeof(Test_CustomConverter)
                 );

            IVCRoutedCommandToInputGestureText converter = new IVCRoutedCommandToInputGestureText();
            object result = converter.Convert(command, null, null, System.Globalization.CultureInfo.InvariantCulture);
            Assert.AreEqual(Binding.DoNothing, result);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void IVCRoutedCommandToInputGestureTextConvertsBackCorrectly()
        {
            IVCRoutedCommandToInputGestureText converter = new IVCRoutedCommandToInputGestureText();
            converter.ConvertBack(null, null, null, System.Globalization.CultureInfo.InvariantCulture);
        }


        [TestMethod]
        public void MVCBooleanAndToVisibilityConvertsCorrectly()
        {
            MVCBooleanAndToVisibility converter = new MVCBooleanAndToVisibility();

            object[] input1 = { true, true, false };
            Visibility visibility = (Visibility)converter.Convert(input1, null, null, System.Globalization.CultureInfo.InvariantCulture);
            Assert.AreEqual(Visibility.Collapsed, visibility);

            object[] input2 = { true, true, true };
            visibility = (Visibility)converter.Convert(input2, null, null, System.Globalization.CultureInfo.InvariantCulture);
            Assert.AreEqual(Visibility.Visible, visibility);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void MVCBooleanAndToVisibilityConvertsBackCorrectly()
        {
            MVCBooleanAndToVisibility converter = new MVCBooleanAndToVisibility();
            converter.ConvertBack(null, null, null, System.Globalization.CultureInfo.InvariantCulture);
        }
        [TestMethod]
        public void MVCBooleanOrToVisibilityConvertsCorrectly()
        {
            MVCBooleanOrToVisibility converter = new MVCBooleanOrToVisibility();

            object[] input1 = { false, false, false };
            Visibility visibility = (Visibility)converter.Convert(input1, null, null, System.Globalization.CultureInfo.InvariantCulture);
            Assert.AreEqual(Visibility.Collapsed, visibility);

            object[] input2 = { true, false, true };
            visibility = (Visibility)converter.Convert(input2, null, null, System.Globalization.CultureInfo.InvariantCulture);
            Assert.AreEqual(Visibility.Visible, visibility);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void MVCBooleanOrToVisibilityConvertsBackCorrectly()
        {
            MVCBooleanOrToVisibility converter = new MVCBooleanOrToVisibility();
            converter.ConvertBack(null, null, null, System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
