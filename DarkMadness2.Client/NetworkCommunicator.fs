﻿namespace DarkMadness2.NetworkCommunication

type NetworkCommunicator () =
    let client = new System.Net.Sockets.TcpClient ("127.0.0.1", 8181)
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

    member this.HasIncomingMessages () = stream.DataAvailable

    member this.Read () = reader.ReadLine ()