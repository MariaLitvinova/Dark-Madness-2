namespace DarkMadness2.NetworkCommunication

/// Network client. Can connect to some endpoint, send and receive string data.
type Client (host, port) =
    
    /// TCP client that is used for communication.
    let client = new System.Net.Sockets.TcpClient (host, port)

    /// Network data stream.
    let stream = client.GetStream ()

    /// Writer for network stream.
    let writer = new System.IO.StreamWriter (stream)
    do writer.AutoFlush <- true

    /// Reader for network stream.
    let reader = new System.IO.StreamReader (stream)
    
    /// Loop that waits for connection to a server to be opened.
    let rec waitForConnection () =
        if not client.Connected then 
            waitForConnection ()
        else
            System.Threading.Thread.Sleep 100
    do waitForConnection ()

    /// Source of an event when new data appears from server.
    let eventSource = 
        DarkMadness2.Core.EventSource.SyncEventSourceWrapper (fun () -> reader.ReadLine ()) 

    /// Send given string to a server.
    member this.Send (str : string) = writer.WriteLine str

    /// Event that is fired when new data is received from server.
    member this.ServerEvent = eventSource.Event
