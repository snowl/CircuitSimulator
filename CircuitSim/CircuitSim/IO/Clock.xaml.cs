using CircuitSim.BaseObjects;
using System.Windows.Media;

namespace CircuitSim.IO
{
    /// <summary>
    /// Interaction logic for Clock.xaml
    /// </summary>
    public partial class Clock : CircuitObject
    {
        /// <summary>
        /// Creates a new clock that toggles every tick.
        /// </summary>
        public Clock()
        {
            InitializeComponent();
            
            //Sets the state to false
            PowerOutput.State = false;
            //Subscribe to the output change
            PowerOutput.StateChange += PowerOutput_StateChange;
        }

        /// <summary>
        /// Called when the output changes
        /// </summary>
        private void PowerOutput_StateChange()
        {
            //Change the output to be the opposite of the current output
            PowerOutput.State = !PowerOutput.State;

            //Set the output colors on the flipflop
            if (PowerOutput.State)
            {
                ClockRect.Fill = new SolidColorBrush(Colors.Red);
            }
            else
            {
                ClockRect.Fill = new SolidColorBrush(Colors.Black);
            }
        }
    }
}
