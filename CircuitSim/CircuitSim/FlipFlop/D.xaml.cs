using CircuitSim.BaseObjects;
using System.Windows.Media;

namespace CircuitSim.FlipFlop
{
    /// <summary>
    /// Interaction logic for D.xaml
    /// </summary>
    public partial class D : CircuitObject
    {
        /// <summary>
        /// Checks if a clock has occured or not
        /// </summary>
        private bool _lastClock;

        /// <summary>
        /// Creates a new D FlipFlop
        /// </summary>
        public D()
        {
            InitializeComponent();
            //Subscribe to input changes
            InputClock.StateChanged += InputClock_StateChanged;

            //Set the output state
            Output.State = false;
            OutputInverted.State = true;

            //The flipflip hasn't been clocked yet
            _lastClock = false;
        }

        /// <summary>
        /// Called when the clock is ticked.
        /// </summary>
        private void InputClock_StateChanged()
        {
            //If it hasn't been clocked AND the clock is high
            if (!_lastClock && InputClock.State == true)
            {
                //The state is set to the input data
                Output.State = InputData.State;
                OutputInverted.State = !InputData.State;

                //Set the output colors on the flipflop
                if (Output.State)
                {
                    QRect.Fill = new SolidColorBrush(Colors.Red);
                    QInvertedRect.Fill = new SolidColorBrush(Colors.Black);
                }
                else
                {
                    QRect.Fill = new SolidColorBrush(Colors.Black);
                    QInvertedRect.Fill = new SolidColorBrush(Colors.Red);
                }

                //Set the last clock to true
                _lastClock = true;
            }
            else if (InputClock.State == false)
            {
                //Set the clock to false
                _lastClock = false;
            }
        }

    }
}
