open DarkMadness2.NetworkCommunication

let server = Server 8181

server.NewMessage.Add (fun (_, message) -> server.SendToAllClients message)
server.Start ()
