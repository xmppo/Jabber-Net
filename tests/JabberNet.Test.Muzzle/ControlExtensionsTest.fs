namespace JabberNet.Test.Muzzle

open System
open System.Threading
open System.Windows.Forms

open NUnit.Framework

open JabberNet.Muzzle
open JabberNet.Test.Muzzle.Helpers


[<TestFixture>]
type ControlExtensionsTest () =

    [<Test>]
    member __.InvocationShouldBePerformedInTheSameThread () : unit =
        executeInStaThread (fun () ->
                                let threadId = Thread.CurrentThread.ManagedThreadId
                                use form = new Form ()
                                let handle = form.Handle
                                form.InvokeAction (fun () ->
                                                       Assert.AreEqual (threadId,
                                                                        Thread.CurrentThread.ManagedThreadId)))

    [<Test>]
    member __.InvocationOfFuncShouldBePerformedInTheSameThread () : unit =
        executeInStaThread (fun () ->
                                let threadId = Thread.CurrentThread.ManagedThreadId
                                use form = new Form ()
                                let handle = form.Handle

                                let result = form.InvokeFunc (fun () -> Thread.CurrentThread.ManagedThreadId)
                                Assert.AreEqual (threadId, result))

    [<Test>]
    member __.InvocationShouldBePerformedInAnotherThreadInCaseOfMultiThreading () : unit =
        let test () =
            let threadId = Thread.CurrentThread.ManagedThreadId
            use form = new Form ()
            let handle = form.Handle

            runApplicationWithParallelTask form
                <| fun () ->
                        Assert.IsTrue form.InvokeRequired
                        form.InvokeAction (fun () ->
                                               Assert.AreEqual (threadId,
                                                                Thread.CurrentThread.ManagedThreadId))
        executeInStaThread test

    [<Test>]
    member __.BeginInvokeShouldReturnFinishedTask () : unit =
        let mutable result = null
        let test () =
            use form = new Form ()
            let handle = form.Handle

            runApplicationWithParallelTask form (fun () ->
                                                        Assert.IsTrue form.InvokeRequired
                                                        result <- form.BeginInvokeAction (Action Application.Exit))
        executeInStaThread test

        Assert.IsNotNull result
        Assert.IsTrue result.IsCompleted

    [<Test>]
    member __.BindHandlesShouldCreateThem () : unit =
        let test () =
            use f = new Form ()
            use c = new TextBox ()
            f.Controls.Add(c)

            Assert.IsFalse f.IsHandleCreated
            Assert.IsFalse c.IsHandleCreated

            f.BindHandlesToCurrentThread ()

            Assert.IsTrue f.IsHandleCreated
            Assert.IsTrue c.IsHandleCreated

        executeInStaThread test
