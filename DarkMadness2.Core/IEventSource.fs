namespace DarkMadness2.Core

type IEventSource<'a> =
    abstract HasNext : unit -> bool
    abstract Next : unit -> 'a
        