using CircuitSim.BaseObjects;

namespace CircuitSim.IO
{
    /// <summary>
    /// Interaction logic for Power.xaml
    /// </summary>
    public partial class Power : CircuitObject
    {
        /// <summary>
        /// Creates a new power object
        /// </summary>
        public Power()
        {
            InitializeComponent();
            
            //Sets the output of the power to true
            PowerOutput.State = true;
        }
    }
}
