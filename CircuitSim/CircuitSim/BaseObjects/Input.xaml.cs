using System;
using System.Windows.Controls;

namespace CircuitSim.BaseObjects
{
    /// <summary>
    /// Interaction logic for Input.xaml
    /// </summary>
    public partial class Input : UserControl, PowerObject
    {
        /// <summary>
        /// Called when the state of the input is changed
        /// </summary>
        public event StateChangedHandler StateChanged;
        public delegate void StateChangedHandler();

        /// <summary>
        /// The output state that is linked to the input. Only one output to an input.
        /// </summary>
        private Output _state;
        /// <summary>
        /// The delayed state of the input. Doesn't get updated internally until one tick has passed.
        /// </summary>
        private bool _delayedState;

        /// <summary>
        /// The current state of the output linked to the input
        /// </summary>
        public bool State
        {
            get
            {
                //Returns the state of the connected output, otherwise returns false (since no output connected)
                return _delayedState;
            }
            set
            {
                throw new Exception("Cannot set the state directly");
            }
        }

        /// <summary>
        /// Creates a new input.
        /// </summary>
        public Input() : this(false)
        {
        }

        /// <summary>
        /// Creates a new input.
        /// </summary>
        /// <param name="state">The state the input should be in</param>
        public Input(bool state)
        {
            _state = null;
            _delayedState = state;
            InitializeComponent();
        }

        /// <summary>
        /// Links an input to an output
        /// </summary>
        /// <param name="output">The output to link to</param>
        public void LinkInputs(Output output)
        {
            //Makes sure that it only listens to one output event at a time
            if (_state != null)
                _state.StateChange -= _state_StateChange;

            //Sets the state to the output
            _state = output;
        }

        /// <summary>
        /// Called when the state of the input is changed
        /// </summary>
        public void _state_StateChange()
        {
            //Set the delayed state AFTER a tick
            _delayedState = _state.State;

            //Make sure there is a subscriber to the event
            if (StateChanged != null)
            {
                //Passes the value of the output to the circuit
                StateChanged();
            }
        }
    }
}
