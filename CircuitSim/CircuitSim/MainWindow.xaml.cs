using CircuitSim.BaseObjects;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace CircuitSim
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //The boolean that signifys when an output is being linked
        private bool _linkingStarted = false;
        //The temporary line that shows when linking an output
        private LineGeometry _tempLink;
        //The output that is being linked to
        private Output _tempOutput;
        //A list of all power objects that exist on the canvas
        private List<PowerObject> _powerList;
        //The timer that runs the simulation
        private Timer _internalTick;

        /// <summary>
        /// Called on first start
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            //Create the Power Object list
            _powerList = new List<PowerObject>();

            //Set up the internal timer at 100 ms
            _internalTick = new Timer { Interval = 100 };
            _internalTick.Elapsed += TickOutputs;
            _internalTick.Enabled = true;
            _internalTick.Start();
        }

        /// <summary>
        /// Called when the window is loaded
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Create the events on load
            CircuitCanvas.MouseDown += CircuitCanvas_MouseDown;
            CircuitCanvas.MouseMove += CircuitCanvas_MouseMove;
            CircuitCanvas.MouseUp += CircuitCanvas_MouseUp;
            CircuitCanvas.Drop += CircuitCanvas_Drop;
        }
        
        /// <summary>
        /// Called when the canvas is clicked on
        /// </summary>
        private void CircuitCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //Get the position of the mouse relative to the circuit canvas
            Point MousePosition = e.GetPosition(CircuitCanvas);

            //Do a hit test under the mouse position
            HitTestResult result = VisualTreeHelper.HitTest(CircuitCanvas, MousePosition);

            //Make sure that there is something under the mouse
            if (result == null || result.VisualHit == null)
                return;

            //If the mouse has hit a border
            if (result.VisualHit is Border)
            {
                //Get the parent class of the border
                Border border = (Border)result.VisualHit;
                var IO = border.Parent;
                
                //If the parent class is an Output
                if (IO is Output)
                {
                    //Cast to output
                    Output IOOutput = (Output)IO;

                    //Get the center of the output relative to the canvas
                    Point position = IOOutput.TransformToAncestor(CircuitCanvas).Transform(new Point(IOOutput.ActualWidth / 2, IOOutput.ActualHeight / 2));

                    //Creates a new line
                    _linkingStarted = true;
                    _tempLink = new LineGeometry(position, position);

                    //Assign it to the list of connections to be displayed
                    Connections.Children.Add(_tempLink);

                    //Assign the temporary output to the current output
                    _tempOutput = (Output)IO;

                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// Called when the mouse moves
        /// </summary>
        private void CircuitCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            //If there is a linking in progress
            if (_linkingStarted)
            {
                //Move the link endpoint to the current location of the mouse
                _tempLink.EndPoint = e.GetPosition(CircuitCanvas);
                e.Handled = true;
            }
        }

        /// <summary>
        /// Called when the mouse button is let go
        /// </summary>
        private void CircuitCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //If there is a linking in progress
            if (_linkingStarted)
            {
                //Temporary value to show 
                bool linked = false;

                //Get the type of the element that the mouse went up on
                var BaseType = e.Source.GetType().BaseType;

                if (BaseType == typeof(CircuitObject))
                {
                    //Convert to a circuit object
                    CircuitObject obj = (CircuitObject)e.Source;

                    //Get the position of the mouse relative to the circuit object
                    Point MousePosition = e.GetPosition(obj);

                    //Get the element underneath the mouse
                    HitTestResult result = VisualTreeHelper.HitTest(obj, MousePosition);

                    //Return if there is no element under the cursor
                    if (result == null || result.VisualHit == null)
                    {
                        //Remove the temporary line
                        Connections.Children.Remove(_tempLink);
                        _tempLink = null;
                        _linkingStarted = false;
                        return;
                    }

                    //If the underlying element is a border element
                    if (result.VisualHit is Border)
                    {
                        Border border = (Border)result.VisualHit;
                        var IO = border.Parent;

                        //Check if the border element is a input element in disguise
                        if (IO is Input)
                        {
                            //Convert to a input element
                            Input IOInput = (Input)IO;

                            //Get the center of the input relative to the canvas
                            Point inputPoint = IOInput.TransformToAncestor(CircuitCanvas).Transform(new Point(IOInput.ActualWidth / 2, IOInput.ActualHeight / 2));

                            //Ends the line in the centre of the input
                            _tempLink.EndPoint = inputPoint;
                            
                            //Links the output to the input
                            IOInput.LinkInputs(_tempOutput);

                            //Adds to the global list
                            _powerList.Add(_tempOutput);
                            _powerList.Add(IOInput);

                            //Attaches the line to the object
                            obj.AttachInputLine(_tempLink);

                            //Some evil casting (the outputs' parent of the parent is the circuit object that contains the output). Attaches the output side to the object
                            ((CircuitObject)((Grid)_tempOutput.Parent).Parent).AttachOutputLine(_tempLink);

                            //Set linked to true
                            linked = true;
                        }
                    }
                }

                //If it isn't linked remove the temporary link
                if (!linked)
                {
                    Connections.Children.Remove(_tempLink);
                    _tempLink = null;
                }

                //Stop handling linking
                _linkingStarted = false;
                e.Handled = true;
            }
        }

        /// <summary>
        /// Called when an element is clicked on in the selector
        /// </summary>
        private void ObjectSelector_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            //Don't do anything if no element clicked
            if (ObjectSelector.SelectedItem == null)
                return;

            //Copy the element to the drag & drop clipboard
            DragDrop.DoDragDrop(ObjectSelector, ObjectSelector.SelectedItem, DragDropEffects.Copy | DragDropEffects.Move);
        }

        /// <summary>
        /// Called when a element is dropped onto the canvas
        /// </summary>
        private void CircuitCanvas_Drop(object sender, DragEventArgs e)
        {
            //Get the type of element that is dropped onto the canvas
            String[] allFormats = e.Data.GetFormats();
            //Make sure there is a format there
            if (allFormats.Length == 0)
                return;

            string ItemType = allFormats[0];

            //Create a new type of the format
            CircuitObject instance = (CircuitObject)Assembly.GetExecutingAssembly().CreateInstance(ItemType);

            //If the format doesn't exist do nothing
            if (instance == null)
                return;

            //Add the element to the canvas
            CircuitCanvas.Children.Add(instance);

            //Get the point of the mouse relative to the canvas
            Point p = e.GetPosition(CircuitCanvas);

            //Take 15 from the mouse position to center the element on the mouse
            Canvas.SetLeft(instance, p.X - 15);
            Canvas.SetTop(instance, p.Y - 15);
        }

        /// <summary>
        /// Called when the step button is clicked
        /// </summary>
        private void StepButton_Click(object sender, RoutedEventArgs e)
        {
            //Tick the simulation once
            TickOutputs(null, null);
        }

        /// <summary>
        /// Called when the pause button is clicked
        /// </summary>
        private void ToggleTimerButton_Click(object sender, RoutedEventArgs e)
        {
            //If the simulation timer is running
            if (_internalTick.Enabled)
            {
                //Turns the simulation ticker off
                _internalTick.Enabled = false;
                ToggleTimerButton.Content = "Un-Pause";
            }
            else
            {
                //Turns the simulation ticker on
                _internalTick.Enabled = true;
                ToggleTimerButton.Content = "Pause";
            }
        }

        /// <summary>
        /// Called when the timer slider's value is changed
        /// </summary>
        private void TimerSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //Sets the label to the current value 
            TimerTickLabel.Content = string.Format("{0}ms", Math.Floor(TimerSlider.Value));
            
            //If the timer is not null (is null at startup)
            if (_internalTick != null)
            {
                //Sets the timer interval to the slider
                _internalTick.Interval = TimerSlider.Value;
            }
        }

        /// <summary>
        /// Ticks the simulation once
        /// </summary>
        private void TickOutputs(object sender, ElapsedEventArgs e)
        {
            //Runs the simulation on the UI thread
            Dispatcher.BeginInvoke(new Action(() =>
            {
                //A list of IO that has already been ticked
                List<PowerObject> tickedObjects = new List<PowerObject>();

                //Go through each output
                foreach (PowerObject o in _powerList)
                {
                    //If the output hasn't been ticked (CAN BE in the list more in the once!)
                    if (!tickedObjects.Contains(o))
                    {
                        if (o is Output)
                        {
                            //Calls the events linked to the output
                            ((Output)o).CallChange();
                        }
                        else if (o is Input)
                        {
                            //Changes the state of the input
                            ((Input)o)._state_StateChange();
                        }
                        //Adds the output to the ticked object list
                        tickedObjects.Add(o);
                    }
                }
            }));
        }
    }
}
