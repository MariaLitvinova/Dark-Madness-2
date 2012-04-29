namespace DarkMadness2.Client

open DarkMadness2.Core.EventSource
open DarkMadness2.Core.EventSource.EventSourceUtils
open DarkMadness2.Core.EventProcessing
open DarkMadness2.NetworkCommunication.Serializer

/// Event sources of a client and functions to work with them.
module EventSources =

    /// Event source that receives events from console.
    let private consoleEventSource = SyncEventSourceWrapper (fun () -> System.Console.ReadKey true) 

    /// Combined event source that receives events from console and server.
    let private eventSource (serverEvent : IEvent<_>) = 
        combine serverEvent consoleEventSource.Event (
            function
            | Choice1Of2 messageFromServer -> ServerEvent (messageFromServer |> deserialize)
            | Choice2Of2 keyInfo -> ClientEvent keyInfo.Key
        )

    /// Event loop launcher.
    let startEventLoop (eventHandler : Event -> bool) (serverEvent : IEvent<_>) =
        eventSource serverEvent |> eventLoop eventHandler
