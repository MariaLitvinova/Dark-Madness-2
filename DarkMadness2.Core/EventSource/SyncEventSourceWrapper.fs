namespace DarkMadness2.Core.EventSource

type SyncEventSourceWrapper<'a> (eventProducer : unit -> 'a) =
    let event = Event<_> ()
    let rec eventWaitingLoop () = 
        let eventPayload = eventProducer ()
        event.Trigger eventPayload
        eventWaitingLoop ()
    let thread = System.Threading.Thread (System.Threading.ThreadStart eventWaitingLoop)
    do thread.IsBackground <- true
    do thread.Start ()

    interface IEventSource<'a> with

        member this.Event = event.Publish

