namespace DarkMadness2.Client

open DarkMadness2.Core.EventSource
open DarkMadness2.Core.EventSource.EventSourceUtils
open DarkMadness2.Core.EventProcessing
open DarkMadness2.NetworkCommunication.Serializer

module EventSources =

    let private consoleEventSource = SyncEventSourceWrapper (fun () -> System.Console.ReadKey true) 

    let private eventSource (communicator : IEventSource<_>) = 
        combine (event communicator) (event consoleEventSource) (
            function
            | Choice1Of2 messageFromServer -> ServerEvent (messageFromServer |> deserialize)
            | Choice2Of2 keyInfo -> ClientEvent keyInfo.Key
        )

    let startEventLoop (eventHandler : Event -> bool) (communicator : IEventSource<_>) =
        eventSource communicator |> eventLoop eventHandler
