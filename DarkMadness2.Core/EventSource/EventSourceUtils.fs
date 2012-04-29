namespace DarkMadness2.Core.EventSource

/// Useful functions to work with events.
module EventSourceUtils =

    /// Combines two events into third event using given combine function.
    /// That function shall take one of the event payloads (wrapped in Choice) and produce payload for resulting event.
    /// Allows to combine arbitrary events and do some transformations along the way.
    let combine (source1 : IEvent<'a>) (source2 : IEvent<'b>) (combineFunction : Choice<'a, 'b> -> 'c) =
        let result = Event<'c> ()
        source1.Add (Choice1Of2 >> combineFunction >> result.Trigger)
        source2.Add (Choice2Of2 >> combineFunction >> result.Trigger)
        result.Publish
