using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CircuitSim.BaseObjects;
using CircuitSim.IO;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls.Primitives;

namespace CircuitSimTest
{
    [TestClass]
    public class IOTest
    {
        [TestMethod]
        public void TestInputOnTick()
        {
            Output o = new Output();

            Input i = new Input();
            i.LinkInputs(o);
            Assert.IsTrue(i.State == false);
            o.State = true;

            //Hasn't been ticked yet
            Assert.IsTrue(i.State == false);
            i._state_StateChange();

            //Has been ticked
            Assert.IsTrue(i.State == true);
        }

        [TestMethod]
        public void TestInputOnNoTick()
        {
            Output o = new Output();

            Input i = new Input();
            i.LinkInputs(o);
            Assert.IsTrue(i.State == false);
            o.State = true;

            Assert.IsTrue(i.State == false);
        }

        [TestMethod]
        public void TestOutputOnTick()
        {
            Output o = new Output();
            bool testCallback = false;
            o.StateChange += delegate()
            {
                testCallback = true;
            };

            Assert.IsTrue(o.State == false);
            o.State = true;
            o.CallChange();
            Assert.IsTrue(testCallback == true);
        }

        [TestMethod]
        public void TestOutputOnNoTick()
        {
            Output o = new Output();
            bool testCallback = false;
            o.StateChange += delegate()
            {
                testCallback = true;
            };

            Assert.IsTrue(o.State == false);
            o.State = true;
            Assert.IsTrue(testCallback == false);
        }

        [TestMethod]
        public void TestJunction()
        {
            Junction junction = new Junction();
            Input JunctionInput = (Input)junction.FindName("Input");
            Output JunctionOutput = (Output)junction.FindName("Output");

            Output testOutput = new Output();
            testOutput.State = false;

            JunctionInput.LinkInputs(testOutput);
            JunctionInput._state_StateChange();

            Assert.IsTrue(JunctionOutput.State == false);

            testOutput.State = true;
            JunctionInput._state_StateChange();

            Assert.IsTrue(JunctionOutput.State == true);
        }

        [TestMethod]
        public void TestClock()
        {
            Clock clock = new Clock();
            Output ClockOutput = (Output)clock.FindName("PowerOutput");

            Assert.IsTrue(ClockOutput.State == false);

            ClockOutput.CallChange();
            Assert.IsTrue(ClockOutput.State == true);

            ClockOutput.CallChange();
            Assert.IsTrue(ClockOutput.State == false);
        }

        [TestMethod]
        public void TestLED()
        {
            LED led = new LED();
            Input LEDInput = (Input)led.FindName("LeftInput");
            Rectangle LEDRectangle = (Rectangle)led.FindName("LEDRect");

            Output testOutput = new Output();
            testOutput.State = false;

            LEDInput.LinkInputs(testOutput);
            LEDInput._state_StateChange();

            Assert.IsTrue(((SolidColorBrush)LEDRectangle.Fill).Color == Colors.Black);

            testOutput.State = true;
            LEDInput._state_StateChange();

            Assert.IsTrue(((SolidColorBrush)LEDRectangle.Fill).Color == Colors.Red);
        }

        [TestMethod]
        public void TestPower()
        {
            Power power = new Power();
            Output PowerOutput = (Output)power.FindName("PowerOutput");

            Assert.IsTrue(PowerOutput.State == true);
        }

        [TestMethod]
        public void TestToggleSwitch()
        {
            ToggleSwitch toggle = new ToggleSwitch();
            Ellipse ToggleButton = (Ellipse)toggle.FindName("ToggleRect");
            Output ToggleOutput = (Output)toggle.FindName("Output");

            Assert.IsTrue(ToggleOutput.State == false);

            //Simulate click on the toggle button
            ToggleButton.RaiseEvent(new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left) { RoutedEvent = Mouse.MouseDownEvent, Source = this });
            ToggleOutput.CallChange();

            Assert.IsTrue(ToggleOutput.State == true);
        }

        [TestMethod]
        public void TestTextLED()
        {
            TextLED textLED = new TextLED();

            //Linked backwards to have compatibility with CEDAR
            Input InputOne = (Input)textLED.FindName("Input8");
            Input InputTwo = (Input)textLED.FindName("Input7");

            Label DecimalLabel = (Label)textLED.FindName("DecimalOutput");
            Label HexadecimalLabel = (Label)textLED.FindName("HexOutput");
            Label BinaryLabel = (Label)textLED.FindName("BinaryOutput");

            Output outputOne = new Output();
            InputOne.LinkInputs(outputOne);
            Output outputTwo = new Output();
            InputTwo.LinkInputs(outputTwo);

            Assert.IsTrue((string)DecimalLabel.Content == "0");
            Assert.IsTrue((string)HexadecimalLabel.Content == "0x00");
            Assert.IsTrue((string)BinaryLabel.Content == "0b00000000");

            outputOne.State = true;
            InputOne._state_StateChange();

            Console.WriteLine(BinaryLabel.Content);
            Assert.IsTrue((int)DecimalLabel.Content == 1);
            Assert.IsTrue((string)HexadecimalLabel.Content == "0x01");
            Assert.IsTrue((string)BinaryLabel.Content == "0b00000001");

            outputOne.State = false;
            InputOne._state_StateChange();
            outputTwo.State = true;
            InputTwo._state_StateChange();

            Assert.IsTrue((int)DecimalLabel.Content == 2);
            Assert.IsTrue((string)HexadecimalLabel.Content == "0x02");
            Assert.IsTrue((string)BinaryLabel.Content == "0b00000010");

            outputOne.State = true;
            InputOne._state_StateChange();

            Assert.IsTrue((int)DecimalLabel.Content == 3);
            Assert.IsTrue((string)HexadecimalLabel.Content == "0x03");
            Assert.IsTrue((string)BinaryLabel.Content == "0b00000011");
        }
    }
}
