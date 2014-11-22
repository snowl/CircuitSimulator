using System.Windows.Controls;

namespace CircuitSim.BaseObjects
{
    /// <summary>
    /// Interaction logic for Output.xaml
    /// </summary>
    public partial class Output : UserControl, PowerObject
    {
        /// <summary>
        /// Called when the state of the output is changed
        /// </summary>
        public event StateChangeHandler StateChange;
        public delegate void StateChangeHandler();
        
        // The state of the output
        private bool _state;
        
        /// <summary>
        /// The current state of the Output
        /// </summary>
        public bool State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
            }
        }

        /// <summary>
        /// Creates a new output class
        /// </summary>
        public Output()
        {
            _state = false;
            InitializeComponent();
        }

        /// <summary>
        /// Updates any connected inputs. Called once per tick.
        /// </summary>
        public void CallChange()
        {
            if (StateChange != null)
            {
                StateChange();
            }
        }
    }
}
