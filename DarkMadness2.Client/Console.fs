module DarkMadness2.Client.Console

open System

// Window-related functions
let setWindowSize w h = Console.SetWindowSize (w, h)
let maximizeWindow () = 
    setWindowSize Console.LargestWindowWidth Console.LargestWindowHeight
    Console.SetBufferSize (Console.LargestWindowWidth, Console.LargestWindowHeight)
let maxX () = Console.BufferWidth - 1
let maxY () = Console.BufferHeight - 1

// Cursor-related functions
let setCursorPosition x y = Console.SetCursorPosition (x, y)
let showCursor () = Console.CursorVisible <- true
let hideCursor () = Console.CursorVisible <- false

// Drawing functions
let putCharWithColor ch foreground background (x, y) = 
    setCursorPosition x y
    Console.ForegroundColor <- foreground
    Console.BackgroundColor <- background
    Console.Write (ch : char)

let putChar ch (x, y) =
    setCursorPosition x y
    Console.Write (ch : char)


