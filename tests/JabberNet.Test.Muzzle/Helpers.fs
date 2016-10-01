module JabberNet.Test.Muzzle.Helpers

open System
open System.Threading
open System.Threading.Tasks
open System.Windows.Forms

let internal executeInStaThread (action : unit -> unit) : unit =
    let mutable error = None
    let thread = Thread (fun () ->
                             try action ()
                             with e -> error <- Some e)
    thread.SetApartmentState ApartmentState.STA
    thread.Start ()
    thread.Join ()

    match error with
    | Some ex -> raise ex
    | None -> ()

let private executeInTask (action : unit -> unit)
                          (catchClause : Exception -> unit)
                          (finallyClause : unit -> unit) : unit =
    Task.Factory.StartNew (fun () ->
                               try
                                   try action ()
                                   with e -> catchClause e
                               finally finallyClause ()) |> ignore

let internal runApplicationWithParallelTask (mainForm : Form) (task : unit -> unit) : unit =
    let mutable error = None
    executeInTask task (fun ex -> error <- Some ex) Application.Exit

    Application.Run mainForm
    match error with
    | Some ex -> raise ex
    | None -> ()

