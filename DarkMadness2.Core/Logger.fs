namespace DarkMadness2.Core

module Logger =
    let private fileName = "log.txt"

    let clear () =
        System.IO.File.Delete fileName
    
    let log str =
        System.Threading.Monitor.Enter fileName
        let str = "Thread " + string System.Threading.Thread.CurrentThread.ManagedThreadId + ": " + str + "\n"
        System.IO.File.AppendAllText (fileName, str)
        System.Threading.Monitor.Exit fileName


