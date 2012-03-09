open System.Net.Sockets

let server = TcpListener (System.Net.IPAddress [|127uy; 0uy; 0uy; 1uy|], 8181)
let mutable connectedClients : TcpClient list = []

let notifyAllClients (message : string) =
    connectedClients |> List.iteri (fun i client -> 
            let writer = new System.IO.StreamWriter (client.GetStream ())
            printfn "Sending %s to client %d" message i
            writer.WriteLine message
            writer.Flush ()
        )

let clientCommunication (client : TcpClient) =
    let reader = new System.IO.StreamReader (client.GetStream ())
    let rec processClientMessages () =
        try
            let message = reader.ReadLine ()
            printfn "%d : %s" (List.findIndex ((=) client) connectedClients) message
            notifyAllClients message
        with
            _ -> printfn "Client %d disconnected" (List.findIndex ((=) client) connectedClients)
                 connectedClients <- List.filter ((<>) client) connectedClients
        if List.exists ((=) client) connectedClients then
            processClientMessages ()
    processClientMessages ()

let launchClientCommunicationProcess client =
    connectedClients <- client :: connectedClients
    let communication () = clientCommunication client
    let thread = System.Threading.Thread (System.Threading.ThreadStart communication)
    thread.Start ()

let rec listen () =
    if server.Pending () then
        let client = server.AcceptTcpClient ()
        printfn "Client connected"
        launchClientCommunicationProcess client
    listen ()

server.Start ()
printfn "Server started"
listen ()