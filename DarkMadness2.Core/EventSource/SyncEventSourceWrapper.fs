namespace DarkMadness2.Core.EventSource

/// Wraps blocking function call into event source where event is triggered every time function returns, 
/// then that function gets called again in a loop.
type SyncEventSourceWrapper<'a> (eventProducer : unit -> 'a) =

    /// Event that is triggered when function returns.
    let event = Event<_> ()

    /// Function that calls blocking function in a loop and triggers event.
    let rec eventWaitingLoop () = 
        let eventPayload = eventProducer ()
        event.Trigger eventPayload
        eventWaitingLoop ()

    /// Separate thread for waiting for blocking function, to avoid blocking of caller.
    let thread = System.Threading.Thread (System.Threading.ThreadStart eventWaitingLoop)
    // Thread shall be background to be terminated when application exits.
    do thread.IsBackground <- true
    do thread.Start ()

    interface IEventSource<'a> with

        /// Event that is triggered when wrapped function returns.
        member this.Event = event.Publish

