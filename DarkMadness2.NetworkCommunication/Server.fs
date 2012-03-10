namespace DarkMadness2.NetworkCommunication

open System.Net.Sockets

type Server(port) =
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
                printfn "%d : %s" (List.findIndex ((=) client) connectedClients) message
                newMessage.Trigger (client, message)
            with
                _ -> printfn "Client %d disconnected" (List.findIndex ((=) client) connectedClients)
                     connectedClients <- List.filter ((<>) client) connectedClients
                     clientDisconnected.Trigger client
            if List.exists ((=) client) connectedClients then
                processClientMessages ()
        processClientMessages ()

    let launchClientCommunicationProcess client =
        connectedClients <- client :: connectedClients
        printfn "Client connected"
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

    member this.SendToAllClients (message : string) =
        connectedClients |> List.iteri (fun i client -> 
                let writer = new System.IO.StreamWriter (client.GetStream ())
                printfn "Sending %s to client %d" message i
                writer.WriteLine message
                writer.Flush ()
            )

    member this.Start () =
        server.Start ()
        printfn "Server started"
        listen ()
