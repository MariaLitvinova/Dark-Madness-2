namespace DarkMadness2.Core

/// Logs debug output to a file.
module Logger =

    /// File name (with path relative to current working dir) to write log to.
    let private fileName = "log.txt"

    /// Delete current log, thread-safe.
    let clear () =
        System.Threading.Monitor.Enter fileName
        System.IO.File.Delete fileName
        System.Threading.Monitor.Exit fileName
    
    /// Write message to a log, thread-safe.
    let log str =
        System.Threading.Monitor.Enter fileName
        let str = "Thread " + string System.Threading.Thread.CurrentThread.ManagedThreadId + ": " + str + "\n"
        System.IO.File.AppendAllText (fileName, str)
        System.Threading.Monitor.Exit fileName

