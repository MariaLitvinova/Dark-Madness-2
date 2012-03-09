module Main

open DarkMadness2.Console

let random = System.Random ()
let mutable charPosition = (random.Next 10, random.Next 10)
let mutable otherCharPosition = (0, 0)
let communicator = DarkMadness2.NetworkCommunication.NetworkCommunicator ()

let distance (x1, y1) (x2, y2) = abs (x1 - x2) + abs (y1 - y2)
let (+) (x1, y1) (x2, y2) = (x1 + x2, y1 + y2)
let split (separator : char array) (str : string) = str.Split(separator, System.StringSplitOptions.RemoveEmptyEntries)
let parsePair (str : string) = str.Trim [|'('; ')'|] |> split [|','; ' '|]  |> Array.map int |> fun arr -> (arr.[0], arr.[1])

let processKeyPress (key : System.ConsoleKey) =
    putChar ' ' charPosition
    let delta = match key with
                | System.ConsoleKey.UpArrow -> (0, -1)
                | System.ConsoleKey.DownArrow -> (0, 1)
                | System.ConsoleKey.LeftArrow -> (-1, 0)
                | System.ConsoleKey.RightArrow -> (1, 0)
                | _ -> (0, 0)
    let correctCoords (x, y) =
        x >= 0 && x < maxX () && y >= 0 && y <= maxY ()
    let newCoords = charPosition + delta
    if correctCoords newCoords then
        charPosition <- newCoords
    putChar '@' charPosition
    communicator.Send <| charPosition.ToString ()

let processNewCoords coords = 
    if distance coords charPosition > 1 then 
        putChar ' ' otherCharPosition
        otherCharPosition <- coords
        putChar '@' otherCharPosition

maximizeWindow ()
hideCursor ()
putChar '@' charPosition

communicator.Send <| charPosition.ToString ()

type Event = 
| ClientEvent of System.ConsoleKey
| ServerEvent of int * int

let processEvent event =
    match event with
    | ClientEvent key -> processKeyPress key
    | ServerEvent (x, y) -> processNewCoords (x, y)

let eventSource : Event seq = seq {
    while true do
        if communicator.HasIncomingMessages () then
            yield ServerEvent (parsePair <| communicator.Read ())
        else if System.Console.KeyAvailable then 
            yield ClientEvent (System.Console.ReadKey true).Key
}

eventSource
    |> Seq.takeWhile (fun event -> match event with
                                   | ClientEvent key -> key <> System.ConsoleKey.Escape
                                   | _ -> true
                     )
    |> Seq.iter processEvent
