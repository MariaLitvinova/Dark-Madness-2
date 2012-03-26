namespace DarkMadness2.Core

type private EventDispatcher<'a> (locker : System.Threading.AutoResetEvent) =
    let eventBuffer = System.Collections.Generic.Queue<_> ()

    member this.Listener (event : 'a) = 
        eventBuffer.Enqueue event
        locker.Set () |> ignore

    member this.GetEvent () = eventBuffer.Dequeue ()


module EventProcessing =
    
    open DarkMadness2.Core.EventSource

    let eventLoop (handleEvent : 'a -> bool) (event : IEvent<'a>) =
        let locker = new System.Threading.AutoResetEvent false
        let dispatcher = EventDispatcher locker
        event.Add  <| dispatcher.Listener
        let rec doLoop () =
            locker.WaitOne () |> ignore
            let event = dispatcher.GetEvent ()
            if handleEvent event then
                doLoop ()
        doLoop () 

    let receiveFromEvent (source : IEvent<'a>) =
        let buffer = ref None
        let handler = new Handler<'a> (fun _ x -> buffer := Some x)
        let handler event =
            buffer := Some event
            false
        eventLoop handler source
        (!buffer).Value

    let receive (source : IEventSource<'a>) = receiveFromEvent source.Event