using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CircuitSim.Gates;
using System.Windows;
using CircuitSim.BaseObjects;
using System.Windows.Media;

namespace CircuitSimTest
{
    [TestClass]
    public class GatesTest
    {
        [TestMethod]
        public void TestNot()
        {
            Not not = new Not();
            Input notInput = (Input)not.FindName("Input");
            Output notOutput = (Output)not.FindName("Output");

            Output testOutput = new Output();
            testOutput.State = false;

            notInput.LinkInputs(testOutput);
            notInput._state_StateChange();

            Assert.IsTrue(notOutput.State == true);
            Assert.IsTrue(notInput.State == false);

            testOutput.State = true;
            notInput._state_StateChange();

            Assert.IsTrue(notOutput.State == false);
            Assert.IsTrue(notInput.State == true);
        }

        [TestMethod]
        public void TestAnd()
        {
            And and = new And();
            Input InputOne = (Input)and.FindName("InputOne");
            Input InputTwo = (Input)and.FindName("InputTwo");
            Output andOutput = (Output)and.FindName("Output");

            Output testOutputOne = new Output();
            testOutputOne.State = false;
            Output testOutputTwo = new Output();
            testOutputTwo.State = false;

            InputOne.LinkInputs(testOutputOne);
            InputTwo.LinkInputs(testOutputTwo);

            InputOne._state_StateChange();
            InputTwo._state_StateChange();

            //Input 0 0
            Assert.IsTrue(InputOne.State == false);
            Assert.IsTrue(InputTwo.State == false);
            Assert.IsTrue(andOutput.State == false);

            testOutputOne.State = true;
            InputOne._state_StateChange();

            //Input 1 0
            Assert.IsTrue(InputOne.State == true);
            Assert.IsTrue(InputTwo.State == false);
            Assert.IsTrue(andOutput.State == false);

            testOutputTwo.State = true;
            InputTwo._state_StateChange();

            //Input 1 1
            Assert.IsTrue(InputOne.State == true);
            Assert.IsTrue(InputTwo.State == true);
            Assert.IsTrue(andOutput.State == true);

            testOutputOne.State = false;
            InputOne._state_StateChange();

            //Input 0 1
            Assert.IsTrue(InputOne.State == false);
            Assert.IsTrue(InputTwo.State == true);
            Assert.IsTrue(andOutput.State == false);
        }

        [TestMethod]
        public void TestOr()
        {
            Or or = new Or();
            Input InputOne = (Input)or.FindName("InputOne");
            Input InputTwo = (Input)or.FindName("InputTwo");
            Output orOutput = (Output)or.FindName("Output");

            Output testOutputOne = new Output();
            testOutputOne.State = false;
            Output testOutputTwo = new Output();
            testOutputTwo.State = false;

            InputOne.LinkInputs(testOutputOne);
            InputTwo.LinkInputs(testOutputTwo);

            InputOne._state_StateChange();
            InputTwo._state_StateChange();

            //Input 0 0
            Assert.IsTrue(InputOne.State == false);
            Assert.IsTrue(InputTwo.State == false);
            Assert.IsTrue(orOutput.State == false);

            testOutputOne.State = true;
            InputOne._state_StateChange();

            //Input 1 0
            Assert.IsTrue(InputOne.State == true);
            Assert.IsTrue(InputTwo.State == false);
            Assert.IsTrue(orOutput.State == true);

            testOutputTwo.State = true;
            InputTwo._state_StateChange();

            //Input 1 1
            Assert.IsTrue(InputOne.State == true);
            Assert.IsTrue(InputTwo.State == true);
            Assert.IsTrue(orOutput.State == true);

            testOutputOne.State = false;
            InputOne._state_StateChange();

            //Input 0 1
            Assert.IsTrue(InputOne.State == false);
            Assert.IsTrue(InputTwo.State == true);
            Assert.IsTrue(orOutput.State == true);
        }
    }
}
