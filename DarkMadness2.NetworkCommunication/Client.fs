namespace DarkMadness2.NetworkCommunication

type Client (host, port) =
    let client = new System.Net.Sockets.TcpClient (host, port)
    let stream = client.GetStream ()
    let writer = new System.IO.StreamWriter (stream)
    do writer.AutoFlush <- true
    let reader = new System.IO.StreamReader (stream)
    let rec waitForConnection () =
        if not client.Connected then 
            waitForConnection ()
        else
            System.Threading.Thread.Sleep 100
    do waitForConnection ()
    let eventSource = 
        DarkMadness2.Core.EventSource.SyncEventSourceWrapper (fun () -> reader.ReadLine ()) 

    member this.Send (str : string) = writer.WriteLine str

    member this.ServerEvent = eventSource.Event