using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using JabberNet.Muzzle;
using NUnit.Framework;

namespace JabberNet.Test.Muzzle
{
    [TestFixture]
    public class ControlExtensionTests
    {
        [Test]
        public void InvocationShouldBePerformedInTheSameThread()
        {
            ExecuteInStaThread(() =>
            {
                var threadId = Thread.CurrentThread.ManagedThreadId;
                var form = new Form();
                var handle = form.Handle;

                form.InvokeAction(() =>
                {
                    Assert.AreEqual(threadId, Thread.CurrentThread.ManagedThreadId);
                });
            });
        }

        [Test]
        public void InvocationShouldBePerformedInAnotherThreadInCaseOfMultiThreading()
        {
            ExecuteInStaThread(() =>
            {
                var threadId = Thread.CurrentThread.ManagedThreadId;
                var form = new Form();
                var handle = form.Handle;

                Exception exception = null;
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        Assert.IsTrue(form.InvokeRequired);
                        form.InvokeAction(() =>
                        {
                            Assert.AreEqual(threadId, Thread.CurrentThread.ManagedThreadId);
                        });
                    }
                    catch (Exception ex)
                    {
                        exception = ex;
                    }
                    finally
                    {
                        Application.Exit();
                    }
                });

                Application.Run(form);
                if (exception != null)
                {
                    throw exception;
                }
            });
        }

        private static void ExecuteInStaThread(Action action)
        {
            Exception exception = null;
            var thread = new Thread(state =>
            {
                try
                {
                    action();
                }
                catch (Exception e)
                {
                    exception = e;
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();

            if (exception != null)
            {
                throw exception;
            }
        }
    }
}
