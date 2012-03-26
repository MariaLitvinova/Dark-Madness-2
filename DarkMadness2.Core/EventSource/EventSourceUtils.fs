namespace DarkMadness2.Core.EventSource

module EventSourceUtils =

    let combine (source1 : IEvent<'a>) (source2 : IEvent<'b>) (combineFunction : Choice<'a, 'b> -> 'c) =
        let result = Event<'c> ()
        source1.Add (fun x -> x |> Choice1Of2 |> combineFunction |> result.Trigger)
        source2.Add (fun x -> x |> Choice2Of2 |> combineFunction |> result.Trigger)
        result.Publish

    let event (source : IEventSource<'a>) = source.Event