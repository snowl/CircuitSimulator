using CircuitSim.BaseObjects;

namespace CircuitSim.Gates
{
    /// <summary>
    /// Interaction logic Not.xaml
    /// </summary>
    public partial class Not : CircuitObject
    {
        /// <summary>
        /// Creates a new NOT gate
        /// </summary>
        public Not()
        {
            InitializeComponent();

            //Subscribe to the input events
            Input.StateChanged += StateChange;
        }

        /// <summary>
        /// Called when the state of an input changes
        /// </summary>
        private void StateChange()
        {
            //Inverts the input
            Output.State = !Input.State;
        }
    }
}