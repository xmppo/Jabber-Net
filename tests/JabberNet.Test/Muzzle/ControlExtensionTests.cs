using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using JabberNet.Muzzle;
using NUnit.Framework;

namespace JabberNet.Test.Muzzle
{
    [TestFixture]
    [Platform(Exclude = "Linux", Reason = "Travis have no support for X-Server")]
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
        public void InvocationOfFuncShouldBePerformedInTheSameThread()
        {
            ExecuteInStaThread(() =>
            {
                var threadId = Thread.CurrentThread.ManagedThreadId;
                var form = new Form();
                var handle = form.Handle;

                var result = form.InvokeFunc(() => Thread.CurrentThread.ManagedThreadId);
                Assert.AreEqual(threadId, result);
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

                RunApplicationWithParallelTask(
                    form,
                    () =>
                    {
                        Assert.IsTrue(form.InvokeRequired);
                        form.InvokeAction(() =>
                        {
                            Assert.AreEqual(threadId, Thread.CurrentThread.ManagedThreadId);
                        });
                    });
            });
        }

        [Test]
        public void BeginInvokeShouldReturnFinishedTask()
        {
            Task result = null;
            ExecuteInStaThread(() =>
            {
                var form = new Form();
                var handle = form.Handle;

                RunApplicationWithParallelTask(
                    form,
                    () =>
                    {
                        Assert.IsTrue(form.InvokeRequired);
                        result = form.BeginInvokeAction(Application.Exit);
                    });
            });

            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsCompleted);
        }

        [Test]
        public void BindHandlesShouldCreateThem()
        {
            ExecuteInStaThread(() =>
            {
                var f = new Form();
                var c = new TextBox();
                f.Controls.Add(c);

                Assert.IsFalse(f.IsHandleCreated);
                Assert.IsFalse(c.IsHandleCreated);

                f.BindHandlesToCurrentThread();

                Assert.IsTrue(f.IsHandleCreated);
                Assert.IsTrue(c.IsHandleCreated);
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

        private static void ExecuteInTask(Action action, Action<Exception> catchClause, Action finallyClause)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    action();
                }
                catch (Exception exception)
                {
                    catchClause(exception);
                }
                finally
                {
                    finallyClause();
                }
            });
        }

        private static void RunApplicationWithParallelTask(Form mainForm, Action task)
        {
            Exception exception = null;
            ExecuteInTask(task, ex => exception = ex, Application.Exit);

            Application.Run(mainForm);
            if (exception != null)
            {
                throw exception;
            }
        }
    }
}
