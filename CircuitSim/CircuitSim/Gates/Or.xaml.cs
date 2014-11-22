using CircuitSim.BaseObjects;

namespace CircuitSim.Gates
{
    /// <summary>
    /// Interaction logic Or.xaml
    /// </summary>
    public partial class Or : CircuitObject
    {
        /// <summary>
        /// Creates a new OR gate
        /// </summary>
        public Or()
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
            //If either of the inputs are true, sets the output to high
            if (InputOne.State == true || InputTwo.State == true)
                Output.State = true;
            else
                Output.State = false;
        }
    }
}
