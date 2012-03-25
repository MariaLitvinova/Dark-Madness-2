namespace DarkMadness2.Core.EventSource

module EventSourceUtils =

    let receiveFromEvent (source : IEvent<'a>) =
        let buffer = ref None
        let handler = new Handler<'a> (fun _ x -> buffer := Some x)
        source.AddHandler handler
        while (!buffer).IsNone do
            ()
        source.RemoveHandler handler
        (!buffer).Value

    let receive (source : IEventSource<'a>) = receiveFromEvent source.Event

    let combine (source1 : IEvent<'a>) (source2 : IEvent<'b>) (combineFunction : Choice<'a, 'b> -> 'c) =
        let result = Event<'c> ()
        source1.Add (fun x -> x |> Choice1Of2 |> combineFunction |> result.Trigger)
        source2.Add (fun x -> x |> Choice2Of2 |> combineFunction |> result.Trigger)
        result.Publish

    let event (source : IEventSource<'a>) = source.Event

    let listenUntil (f : 'a -> bool) (event : IEvent<'a>) =
        let stop = ref false
        let handler = new Handler<'a> (fun _ x -> if f x then stop := true)
        event.AddHandler handler
        while not !stop do
            ()
        event.RemoveHandler handler