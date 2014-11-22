using CircuitSim.BaseObjects;
using System.Collections;

namespace CircuitSim.IO
{
    /// <summary>
    /// Interaction logic for TextLED.xaml
    /// </summary>
    public partial class TextLED : CircuitObject
    {
        /// <summary>
        /// Creates a new TextLED
        /// </summary>
        public TextLED()
        {
            InitializeComponent();

            //Subscribes to the 8-bit input
            Input1.StateChanged += StateChanged;
            Input2.StateChanged += StateChanged;
            Input3.StateChanged += StateChanged;
            Input4.StateChanged += StateChanged;
            Input5.StateChanged += StateChanged;
            Input6.StateChanged += StateChanged;
            Input7.StateChanged += StateChanged;
            Input8.StateChanged += StateChanged;
        }

        /// <summary>
        /// Called when any bit is changed
        /// </summary>
        private void StateChanged()
        {
            //Put the bits in a boolean array
            bool[] boolArray = new bool[] {Input8.State,
                                           Input7.State,
                                           Input6.State,
                                           Input5.State,
                                           Input4.State,
                                           Input3.State,
                                           Input2.State,
                                           Input1.State};

            //Convert the boolean array to a bit array
            BitArray array = new BitArray(boolArray);

            //Set the binary output
            BinaryOutput.Content = "0b";
            for (int i = boolArray.Length - 1; i >= 0; i--)
            {
                bool bit = boolArray[i];
                BinaryOutput.Content += bit ? "1" : "0";
            }

            //Get the integer value of the binary
            var result = new int[1];
            array.CopyTo(result, 0);
            DecimalOutput.Content = result[0];

            //Get the hexadecimal value of the integer
            HexOutput.Content = string.Format("0x{0}", result[0].ToString("X2"));
        }
    }
}
