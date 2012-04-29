namespace DarkMadness2.Client

open DarkMadness2.NetworkCommunication

/// General event payload in a client.
type Event = 

    /// Client-only event, like key press.
    | ClientEvent of System.ConsoleKey

    /// Server event received from network.
    | ServerEvent of Message
