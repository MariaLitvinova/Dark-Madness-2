module Main

open DarkMadness2.Console

let random = System.Random ()
let mutable charPosition = (random.Next 10, random.Next 10)
let mutable otherCharPosition = (0, 0)
let communicator = DarkMadness2.NetworkCommunication.NetworkCommunicator ()

let distance (x1, y1) (x2, y2) = abs (x1 - x2) + abs (y1 - y2)
let (+) (x1, y1) (x2, y2) = (x1 + x2, y1 + y2)
let split (separator : char array) (str : string) = str.Split(separator, System.StringSplitOptions.RemoveEmptyEntries)
let makePair (arr : 'a array) = (arr.[0], arr.[1])
let parsePair (str : string) = str.Trim [|'('; ')'|] |> split [|','; ' '|]  |> Array.map int |> makePair

let processKeyPress (key : System.ConsoleKeyInfo) =
    putChar ' ' charPosition
    let delta = match key.Key with
                | System.ConsoleKey.UpArrow -> (0, -1)
                | System.ConsoleKey.DownArrow -> (0, 1)
                | System.ConsoleKey.LeftArrow -> (-1, 0)
                | System.ConsoleKey.RightArrow -> (1, 0)
                | _ -> (0, 0)
    charPosition <- charPosition + delta
    putChar '@' charPosition
    communicator.Send <| charPosition.ToString ()
    while communicator.HasIncomingMessages () do
        let incomingMessage = communicator.Read ()
        let coords = parsePair incomingMessage
        if distance coords charPosition > 1 then 
            putChar ' ' otherCharPosition
            otherCharPosition <- coords
            putChar '@' otherCharPosition

maximizeWindow ()
hideCursor ()
putChar '@' charPosition

communicator.Send <| charPosition.ToString ()

Seq.initInfinite (fun _ -> System.Console.ReadKey true) 
    |> Seq.takeWhile (fun key -> key.Key <> System.ConsoleKey.Escape)
    |> Seq.iter processKeyPress
