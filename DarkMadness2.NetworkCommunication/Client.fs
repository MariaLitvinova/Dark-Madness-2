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

    member this.Send (str : string) = writer.WriteLine str

    interface DarkMadness2.Core.IEventSource with

        member this.HasNext () = stream.DataAvailable

        member this.Next () = reader.ReadLine () :> obj