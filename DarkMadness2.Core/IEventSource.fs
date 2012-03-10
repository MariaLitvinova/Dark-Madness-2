namespace DarkMadness2.Core

type IEventSource =
    abstract HasNext : unit -> bool
    abstract Next : unit -> obj
        