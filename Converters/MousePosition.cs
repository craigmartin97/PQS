using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Converters
{
    public class MousePosition
    {
        /// <summary>
        /// Gets the mouse position on the screen
        /// </summary>
        /// <returns></returns>
        public Point GetMousePosition(Control control, double left, double top)
        {
            return new Point(Mouse.GetPosition(control).X + left, Mouse.GetPosition(control).Y + top);
        }
    }
}
