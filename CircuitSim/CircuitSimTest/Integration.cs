using System;
using CircuitSim;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CircuitSim.BaseObjects;

namespace CircuitSimTest
{
    [TestClass]
    public class Integration
    {
        [TestMethod]
        public void TestLinkingInputs()
        {
            Input input = new Input();
            Output output = new Output();

            input.LinkInputs(output);

            Assert.IsTrue(input.State == false);
            Assert.IsTrue(output.State == false);

            output.State = true;
            input._state_StateChange();

            Assert.IsTrue(input.State == true);
            Assert.IsTrue(output.State == true);
        }

        [TestMethod]
        public void TestInputEvent()
        {
            Input input = new Input();
            Output output = new Output();
            input.StateChanged += delegate ()
            {
                Assert.IsTrue(input.State == output.State);
            };

            input.LinkInputs(output);

            Assert.IsTrue(input.State == false);
            Assert.IsTrue(output.State == false);

            output.State = true;
            input._state_StateChange();

            Assert.IsTrue(input.State == true);
            Assert.IsTrue(output.State == true);
        }

        [TestMethod]
        public void TestOutputEvent()
        {
            Output output = new Output();
            output.StateChange += delegate ()
            {
                Assert.IsTrue(output.State == true);
            };

            output.State = true;
        }
    }
}
