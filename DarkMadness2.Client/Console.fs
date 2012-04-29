/// Functions for working with console.
module DarkMadness2.Client.Console

open System

// Window-related functions.

/// Sets window size (columns*rows).
let setWindowSize w h = Console.SetWindowSize (w, h)

/// Maximizes console window.
let maximizeWindow () = 
    setWindowSize Console.LargestWindowWidth Console.LargestWindowHeight
    Console.SetBufferSize (Console.LargestWindowWidth, Console.LargestWindowHeight)

/// Current maximum for X coordinate within window.
let maxX () = Console.BufferWidth - 1

/// Current maximum for Y coordinate within window.
let maxY () = Console.BufferHeight - 1

// Cursor-related functions.

/// Set cursor to a given position (column, row).
let setCursorPosition x y = Console.SetCursorPosition (x, y)

/// Shows cursor.
let showCursor () = Console.CursorVisible <- true

/// Hides cursor.
let hideCursor () = Console.CursorVisible <- false

// Drawing functions

/// Puts char with given color and background to given coordinates.
let putCharWithColor ch foreground background (x, y) = 
    setCursorPosition x y
    Console.ForegroundColor <- foreground
    Console.BackgroundColor <- background
    Console.Write (ch : char)

/// Puts char with current color and background to given coordinates.
let putChar ch (x, y) =
    setCursorPosition x y
    Console.Write (ch : char)
