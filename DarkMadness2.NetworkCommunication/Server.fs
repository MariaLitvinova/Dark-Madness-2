namespace DarkMadness2.NetworkCommunication

open System.Net.Sockets

/// Network server that allows multiple clients to connect, handles disconnection, allows to
/// send strings to all clients or to a particular client.
type Server (port) =
    
    /// Underlying TCP server.
    let server = TcpListener (System.Net.IPAddress [|127uy; 0uy; 0uy; 1uy|], port)

    /// A list of connected clients.
    let mutable connectedClients : TcpClient list = []

    /// Event that is triggered when new message from a client appears.
    let newMessage = Event<TcpClient * string> ()

    /// Event that is triggered when new client is connected.
    let clientConnected = Event<TcpClient> ()

    /// Event that is triggered when client is disconnected.
    let clientDisconnected = Event<TcpClient> ()

    /// Loop that is responsible for listening messages from one client.
    let clientCommunication (client : TcpClient) =
        let reader = new System.IO.StreamReader (client.GetStream ())
        let rec processClientMessages () =
            try
                let message = reader.ReadLine ()
                printfn "%A : %s" (client.GetHashCode ()) message
                newMessage.Trigger (client, message)
            with
                _ -> printfn "Client %A disconnected" <| client.GetHashCode ()
                     connectedClients <- List.filter ((<>) client) connectedClients
                     clientDisconnected.Trigger client
            if List.exists ((=) client) connectedClients then
                processClientMessages ()
        processClientMessages ()

    /// Registers new client and launches listener for it in a separate thread.
    let launchClientCommunicationThread client =
        connectedClients <- client :: connectedClients
        printfn "Client %A connected" <| client.GetHashCode ()
        clientConnected.Trigger client
        let communication () = clientCommunication client
        let thread = System.Threading.Thread (System.Threading.ThreadStart communication)
        thread.Start ()

    /// Loop that listens for incoming connections and launches communication threads for them.
    let rec listen () =
        if server.Pending () then
            let client = server.AcceptTcpClient ()
            launchClientCommunicationThread client
        System.Threading.Thread.Sleep 100
        listen ()

    /// Event that is triggered when new message from a client appears.
    member this.NewMessage = newMessage.Publish

    /// Event that is triggered when new client is connected.
    member this.ClientConnected = clientConnected.Publish

    /// Event that is triggered when client is disconnected.
    member this.ClientDisconnected = clientDisconnected.Publish

    /// Send a string to a given client.
    member this.SendTo (client : obj) (message : string) =
        printfn "Sending to %A message %s" (client.GetHashCode ()) message
        let client = client :?> TcpClient
        let writer = new System.IO.StreamWriter (client.GetStream ())
        writer.WriteLine message
        writer.Flush ()        

    /// Send a string to all clients.
    member this.SendToAllClients (message : string) =
        printfn "Sending to all clients: %s" message
        connectedClients |> List.iter (fun client -> this.SendTo client message)

    /// Start listening for incoming connections.
    member this.Start () =
        server.Start ()
        printfn "Server started"
        listen ()
