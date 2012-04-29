namespace DarkMadness2.Core.EventSource

/// Represents something that can produce events.
type IEventSource<'a> =

    /// Event that can be triggered by this object.
    abstract Event : IEvent<'a>

