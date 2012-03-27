namespace DarkMadness2.Client

open DarkMadness2.NetworkCommunication

type Event = 
| ClientEvent of System.ConsoleKey
| ServerEvent of Message
