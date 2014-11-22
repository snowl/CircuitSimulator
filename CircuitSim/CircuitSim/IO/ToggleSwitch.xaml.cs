using CircuitSim.BaseObjects;
using System.Windows.Input;
using System.Windows.Media;

namespace CircuitSim.IO
{
    /// <summary>
    /// Interaction logic for ToggleSwitch.xaml
    /// </summary>
    public partial class ToggleSwitch : CircuitObject
    {
        //The internal state of the toggle switch
        private bool _toggleState;

        /// <summary>
        /// Creates a new ToggleSwitch
        /// </summary>
        public ToggleSwitch()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Called when the toggle switch is clicked
        /// </summary>
        private void ToggleRect_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //Toggles the internal state of the switch
            _toggleState = !_toggleState;

            //Sets the color of the LED
            if (_toggleState)
            {
                ToggleRect.Fill = new SolidColorBrush(Colors.Red);
            }
            else
            {
                ToggleRect.Fill = new SolidColorBrush(Colors.Black);
            }

            //Changes the output of the ToggleSwitch
            Output.State = _toggleState;
        }
    }
}
