using CircuitSim.BaseObjects;
using System.Windows.Media;

namespace CircuitSim.IO
{
    /// <summary>
    /// Interaction logic for Junction.xaml
    /// </summary>
    public partial class Junction : CircuitObject
    {
        /// <summary>
        /// Creates a new Junction
        /// </summary>
        public Junction()
        {
            InitializeComponent();

            //Subscribe to the output change
            Input.StateChanged += Input_StateChanged;
        }

        /// <summary>
        /// Called when the input changes
        /// </summary>
        private void Input_StateChanged()
        {
            //Sets the line of the junction to the current output
            if (Input.State)
            {
                JunctionLine.Stroke = new SolidColorBrush(Colors.Red);
            }
            else
            {
                JunctionLine.Stroke = new SolidColorBrush(Colors.Black);
            }

            //Mirrors the input
            Output.State = Input.State;
        }
    }
}
