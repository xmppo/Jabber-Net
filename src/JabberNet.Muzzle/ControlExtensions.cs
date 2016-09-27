using System;
using System.Windows.Forms;

namespace JabberNet.Muzzle
{
    /// <summary>
    /// Extension methods for <see cref="Control"/>.
    /// </summary>
    public static class ControlExtensions
    {
        /// <summary>
        /// Invoke the action in the control owning thread if invocation is required.
        /// </summary>
        /// <param name="control">Control on which the action should be invoked.</param>
        /// <param name="action">The invocable action.</param>
        public static void InvokeAction(this Control control, Action action)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(action);
            }
            else
            {
                action();
            }
        }
    }
}
