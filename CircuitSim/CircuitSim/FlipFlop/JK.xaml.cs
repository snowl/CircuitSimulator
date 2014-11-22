using CircuitSim.BaseObjects;
using System.Windows.Media;

namespace CircuitSim.FlipFlop
{
    /// <summary>
    /// Interaction logic for JK.xaml
    /// </summary>
    public partial class JK : CircuitObject
    {
        /// <summary>
        /// Checks if a clock has occured or not
        /// </summary>
        private bool _lastClock;

        /// <summary>
        /// Creates a new JK FlipFlop
        /// </summary>
        public JK()
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
                if (InputJ.State && InputK.State)
                {
                    //Toggle state when both J & K are high
                    Output.State = !Output.State;
                    OutputInverted.State = !OutputInverted.State;
                }
                else if (InputJ.State)
                {
                    //Toggle true when J is high
                    Output.State = true;
                    OutputInverted.State = false;
                }
                else if (InputK.State)
                {
                    //Toggle false when K is high
                    Output.State = false;
                    OutputInverted.State = true;
                }

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
