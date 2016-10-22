using CircuitSim.BaseObjects;
using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace CircuitSim.IO
{
    /// <summary>
    /// Interaction logic for Keypad.xaml
    /// </summary>
    public partial class Keypad : CircuitObject
    {
        /// <summary>
        /// Creates a new clock that toggles every tick.
        /// </summary>
        public Keypad()
        {
            InitializeComponent();

            //Sets the state to false
            Output1.State = false;
        }

        private void Radio_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            byte val = byte.Parse((String)((RadioButton)e.OriginalSource).Content, System.Globalization.NumberStyles.AllowHexSpecifier);
            SetState(val);
        }

        private void SetState(byte state)
        {
            Output1.State = GetBit(state, 3);
            Output2.State = GetBit(state, 2);
            Output3.State = GetBit(state, 1);
            Output4.State = GetBit(state, 0);
        }

        public static bool GetBit(byte b, int bitNumber)
        {
            return (b & (1 << bitNumber)) != 0;
        }

        private void One_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            SetState(1);
        }
    }
}
