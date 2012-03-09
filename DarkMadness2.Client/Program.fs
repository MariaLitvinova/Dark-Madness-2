open DarkMadness2.Console

let mutable charPosition = (10, 10)

let (+) (x1, y1) (x2, y2) = (x1 + x2, y1 + y2)

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

maximizeWindow ()
hideCursor ()
putChar '@' charPosition

Seq.initInfinite (fun _ -> System.Console.ReadKey true) 
    |> Seq.takeWhile (fun key -> key.Key <> System.ConsoleKey.Escape)
    |> Seq.iter processKeyPress
