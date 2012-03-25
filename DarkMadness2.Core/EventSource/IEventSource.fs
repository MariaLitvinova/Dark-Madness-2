namespace DarkMadness2.Core.EventSource

type IEventSource<'a> =
    abstract Event : IEvent<'a>

