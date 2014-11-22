using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CircuitSim.BaseObjects
{
    public class CircuitObject : UserControl
    {
        public static readonly DependencyProperty CanMoveProperty = DependencyProperty.Register("CanMove", typeof(bool), typeof(CircuitObject), new PropertyMetadata(true));
        /// <summary>
        /// Allows the circuit objects to be able to be frozen.
        /// </summary>
        public bool CanMove
        {
            get { return (bool)GetValue(CanMoveProperty); }
            set { SetValue(CanMoveProperty, value); }
        }

        //The anchor point of the object when being moved
        private Point _anchorPoint;

        //The current location of the point
        private Point _currentPoint;

        //The transformer that will change the position of the object
        private TranslateTransform _transform = new TranslateTransform();

        //Boolean to check if the object is being dragged
        private bool _isInDrag = false;

        //The lines being connected to the input
        private List<LineGeometry> _attachedInputLines;

        //The lines being connected to the output
        private List<LineGeometry> _attachedOutputLines;

        /// <summary>
        /// Creates a new Circuit Object to be manipulated
        /// </summary>
        public CircuitObject()
        {
            //Set the events for the object
            this.MouseLeftButtonDown += DragObject_MouseLeftButtonDown;
            this.MouseMove += DragObject_MouseMove;
            this.MouseLeftButtonUp += DragObject_MouseLeftButtonUp;

            //Initialize the lists
            _attachedInputLines = new List<LineGeometry>();
            _attachedOutputLines = new List<LineGeometry>();
        }

        /// <summary>
        /// Called when the mouse button is held on the object
        /// </summary>
        /// <param name="sender">The element that is calling the event</param>
        /// <param name="e">The event parameters</param>
        private void DragObject_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Don't start the drag if we can't interact with the object
            if (CanMove == false)
                return;

            //Get the element the object is directly over
            var x = Mouse.DirectlyOver;

            //Don't drag when on input/output
            if (x is Border)
                return;

            //Get the element that called it
            var element = sender as FrameworkElement;

            //Set the variables up to the event parameters
            _anchorPoint = e.GetPosition(null);
            _isInDrag = true;

            //Hide the mouse and signal that the event was handled.
            element.CaptureMouse();
            e.Handled = true;
        }

        /// <summary>
        /// Called when the user lets go of the mouse.
        /// </summary>
        /// <param name="sender">The element that is calling the event</param>
        /// <param name="e">The event parameters</param>
        private void DragObject_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //Make sure the object is being dragged
            if (_isInDrag)
            {
                //Stop dragging and uncapture the mouse
                _isInDrag = false;
                var element = sender as FrameworkElement;
                element.ReleaseMouseCapture();
                e.Handled = true;
            }
        }

        /// <summary>
        /// Called when the user drags the mouse.
        /// </summary>
        /// <param name="sender">The element that is calling the event</param>
        /// <param name="e">The event parameters</param>
        private void DragObject_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            //Make sure the object is being dragged
            if (_isInDrag)
            {
                //Get the current position of the element
                var element = sender as FrameworkElement;
                _currentPoint = e.GetPosition(null);

                //Transform the element based off the last position
                _transform.X += _currentPoint.X - _anchorPoint.X;
                _transform.Y += _currentPoint.Y - _anchorPoint.Y;

                //Transform the attached line if its an input (uses EndPoint)
                foreach (LineGeometry attachedLine in _attachedInputLines)
                {
                    attachedLine.EndPoint = MoveLine(attachedLine.EndPoint,
                                                     (_currentPoint.X - _anchorPoint.X),
                                                     (_currentPoint.Y - _anchorPoint.Y));
                }

                //Transform the attached line if its an output (uses StartPoint)
                foreach (LineGeometry attachedLine in _attachedOutputLines)
                {
                    attachedLine.StartPoint = MoveLine(attachedLine.StartPoint,
                                                     (_currentPoint.X - _anchorPoint.X),
                                                     (_currentPoint.Y - _anchorPoint.Y));
                }

                //Transform the elements location
                this.RenderTransform = _transform;
                //Update the anchor point
                _anchorPoint = _currentPoint;
            }
        }

        /// <summary>
        /// Translates a lines position.
        /// </summary>
        /// <param name="PointToMove">The point of the line to move</param>
        /// <param name="AmountToMoveX">The amount to translate by in the X axis</param>
        /// <param name="AmountToMoveY">The amount to translate by in the Y axis</param>
        /// <returns></returns>
        private Point MoveLine(Point PointToMove, double AmountToMoveX, double AmountToMoveY)
        {
            Point transformedPoint = new Point();
            transformedPoint.X = PointToMove.X + AmountToMoveX;
            transformedPoint.Y = PointToMove.Y + AmountToMoveY;
            return transformedPoint;
        }

        /// <summary>
        /// Adds an input line to the list of attached lines
        /// </summary>
        /// <param name="line">The line to add</param>
        public void AttachInputLine(LineGeometry line)
        {
            _attachedInputLines.Add(line);
        }

        /// <summary>
        /// Adds an output line to the list of attached lines
        /// </summary>
        /// <param name="line">The line to add</param>
        public void AttachOutputLine(LineGeometry line)
        {
            _attachedOutputLines.Add(line);
        }
    }
}
