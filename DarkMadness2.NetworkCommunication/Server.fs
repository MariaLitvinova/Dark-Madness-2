namespace DarkMadness2.NetworkCommunication

open System.Net.Sockets

type Server (port) =
    let server = TcpListener (System.Net.IPAddress [|127uy; 0uy; 0uy; 1uy|], port)
    let mutable connectedClients : TcpClient list = []

    let newMessage = Event<TcpClient * string> ()
    let clientConnected = Event<TcpClient> ()
    let clientDisconnected = Event<TcpClient> ()

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

    let launchClientCommunicationProcess client =
        connectedClients <- client :: connectedClients
        printfn "Client %A connected" <| client.GetHashCode ()
        clientConnected.Trigger client
        let communication () = clientCommunication client
        let thread = System.Threading.Thread (System.Threading.ThreadStart communication)
        thread.Start ()

    let rec listen () =
        if server.Pending () then
            let client = server.AcceptTcpClient ()
            launchClientCommunicationProcess client
        System.Threading.Thread.Sleep 100
        listen ()

    member this.NewMessage = newMessage.Publish

    member this.ClientConnected = clientConnected.Publish

    member this.ClientDisconnected = clientDisconnected.Publish

    member this.SendTo (client : obj) (message : string) =
        printfn "Sending to %A message %s" (client.GetHashCode ()) message
        let client = client :?> TcpClient
        let writer = new System.IO.StreamWriter (client.GetStream ())
        writer.WriteLine message
        writer.Flush ()        

    member this.SendToAllClients (message : string) =
        printfn "Sending to all clients: %s" message
        connectedClients |> List.iter (fun client -> this.SendTo client message)

    member this.Start () =
        server.Start ()
        printfn "Server started"
        listen ()
