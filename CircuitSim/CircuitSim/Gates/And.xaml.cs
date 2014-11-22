using CircuitSim.BaseObjects;
using System.Windows;

namespace CircuitSim.Gates
{
    /// <summary>
    /// Interaction logic for And.xaml
    /// </summary>
    public partial class And : CircuitObject
    {
        /// <summary>
        /// Creates a new AND gate
        /// </summary>
        public And()
        {
            InitializeComponent();

            //Subscribe to the input events
            InputOne.StateChanged += StateChange;
            InputTwo.StateChanged += StateChange;
        }

        /// <summary>
        /// Called when the state of an input changes
        /// </summary>
        private void StateChange()
        {
            //Set the output to high if both inputs are true.
            if (InputOne.State == true && InputTwo.State == true)
                Output.State = true;
            else
                Output.State = false;
        }
    }
}
