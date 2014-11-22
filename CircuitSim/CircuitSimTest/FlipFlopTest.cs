using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CircuitSim.FlipFlop;
using CircuitSim.BaseObjects;

namespace CircuitSimTest
{
    [TestClass]
    public class FlipFlopTest
    {
        [TestMethod]
        public void TestD()
        {
            D DFlipFlop = new D();
            Input DInputData = (Input)DFlipFlop.FindName("InputData");
            Input DInputClock = (Input)DFlipFlop.FindName("InputClock");
            Output DOutput = (Output)DFlipFlop.FindName("Output");

            Output testOutput = new Output();
            testOutput.State = false;
            Output testClock = new Output();
            testClock.State = false;

            DInputData.LinkInputs(testOutput);
            DInputData._state_StateChange();

            DInputClock.LinkInputs(testClock);
            DInputClock._state_StateChange();

            Assert.IsTrue(DOutput.State == false);

            //Set data to true
            testOutput.State = true;
            DInputData._state_StateChange();

            Assert.IsTrue(DOutput.State == false);

            //Pulse clock
            testClock.State = true;
            DInputClock._state_StateChange();

            Assert.IsTrue(DOutput.State == true);

            //Set data to false
            testOutput.State = false;
            DInputData._state_StateChange();

            Assert.IsTrue(DOutput.State == true);

            //Pulse clock
            testClock.State = false;
            DInputClock._state_StateChange();

            Assert.IsTrue(DOutput.State == true);

            testClock.State = true;
            DInputClock._state_StateChange();

            Assert.IsTrue(DOutput.State == false);
        }

        [TestMethod]
        public void TestJK()
        {
            JK JKFlipFlop = new JK();
            Input JKInputJ = (Input)JKFlipFlop.FindName("InputJ");
            Input JKInputClock = (Input)JKFlipFlop.FindName("InputClock");
            Input JKInputK = (Input)JKFlipFlop.FindName("InputK");
            Output JKOutput = (Output)JKFlipFlop.FindName("Output");

            Output testJ = new Output();
            testJ.State = false;
            Output testClock = new Output();
            testClock.State = false;
            Output testK = new Output();
            testK.State = false;

            JKInputJ.LinkInputs(testJ);
            JKInputJ._state_StateChange();

            JKInputClock.LinkInputs(testClock);
            JKInputClock._state_StateChange();

            JKInputK.LinkInputs(testK);
            JKInputK._state_StateChange();

            Assert.IsTrue(JKOutput.State == false);

            //Test Toggle
            testJ.State = true;
            JKInputJ._state_StateChange();
            testK.State = true;
            JKInputK._state_StateChange();
            testClock.State = true;
            JKInputClock._state_StateChange();

            Assert.IsTrue(JKOutput.State == true);

            //Test K = 1 J = 0
            testClock.State = false;
            JKInputClock._state_StateChange();
            testJ.State = false;
            JKInputJ._state_StateChange();
            testClock.State = true;
            JKInputClock._state_StateChange();

            Assert.IsTrue(JKOutput.State == false);

            //Test J = 1 K = 0
            testClock.State = false;
            JKInputClock._state_StateChange();
            testJ.State = true;
            JKInputJ._state_StateChange();
            testK.State = false;
            JKInputK._state_StateChange();
            testClock.State = true;
            JKInputClock._state_StateChange();

            Assert.IsTrue(JKOutput.State == true);

            //Test no change (j&k = 0)
            testClock.State = false;
            JKInputClock._state_StateChange();
            testJ.State = false;
            JKInputJ._state_StateChange();
            testK.State = false;
            JKInputK._state_StateChange();
            testClock.State = true;
            JKInputClock._state_StateChange();

            Assert.IsTrue(JKOutput.State == true);
        }
    }
}
