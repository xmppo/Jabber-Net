using System;
using System.Linq;
using System.Threading.Tasks;
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
            control.InvokeFunc<object>(() =>
            {
                action();
                return null;
            });
        }

        /// <summary>
        /// Asynchronously invokes the action in the control owning thread if invocation is required.
        /// </summary>
        /// <param name="control">Control on which the action should be invoked.</param>
        /// <param name="action">The invocable action.</param>
        public static Task BeginInvokeAction(this Control control, Action action)
        {
            var source = new TaskCompletionSource<object>();
            if (control.InvokeRequired)
            {
                control.BeginInvoke((Action)(() =>
                {
                    try
                    {
                        action();
                        source.SetResult(null);
                    }
                    catch (Exception exception)
                    {
                        source.SetException(exception);
                    }
                }));
            }
            else
            {
                action();
                source.SetResult(null);
            }

            return source.Task;
        }

        /// <summary>
        /// Invoke the function in the control owning thread if invocation is required.
        /// </summary>
        /// <param name="control">Control on which the action should be invoked.</param>
        /// <param name="func">The invocable function.</param>
        public static T InvokeFunc<T>(this Control control, Func<T> func)
        {
            if (control.InvokeRequired)
            {
                var result = default(T);
                control.Invoke((Action)(() => { result = func(); }));
                return result;
            }

            return func();
        }

        /// <summary>
        /// Bind all the handles of the component and its children to the current thread to avoid multithreading
        /// issues.
        /// </summary>
        public static void BindHandlesToCurrentThread(this Control control)
        {
            var handle = control.Handle;
            foreach (var child in control.Controls.OfType<Control>())
            {
                child.BindHandlesToCurrentThread();
            }
        }
    }
}
