using CircuitSim.BaseObjects;
using System.Windows;
using System.Windows.Media;

namespace CircuitSim.IO
{
    /// <summary>
    /// Interaction logic for LED.xaml
    /// </summary>
    public partial class LED : CircuitObject
    {
        /// <summary>
        /// Creates a new LED
        /// </summary>
        public LED()
        {
            InitializeComponent();

            //Subscribe to the input change
            LeftInput.StateChanged += LedStateChanged;
            TopInput.StateChanged += LedStateChanged;
            BottomInput.StateChanged += LedStateChanged;
        }
        
        /// <summary>
        /// Called when an input is changed
        /// </summary>
        private void LedStateChanged()
        {
            //Reset the internal state
            bool StateSet = false;
            RightOutput.State = false;

            //Check if any Input is high. If so, set state to high
            foreach (UIElement e in LEDGrid.Children)
            {
                if (e is Input)
                {
                    Input IO = (Input)e;
                    if (IO.State)
                    {
                        StateSet = true;
                        RightOutput.State = true;
                        break;
                    }
                }
            }

            //Color the LED based on the internal state
            if (StateSet)
            {
                LEDRect.Fill = new SolidColorBrush(Colors.Red); 
            }
            else
            {
                LEDRect.Fill = new SolidColorBrush(Colors.Black); 
            }
        }

    }
}
