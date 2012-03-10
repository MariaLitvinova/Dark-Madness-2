open Microsoft.FSharp.Collections
open DarkMadness2.NetworkCommunication
open DarkMadness2.NetworkCommunication.Serializer

let server = Server 8181

let mutable connectedClients : (obj * int) list = []
let mutable nextClientId = 0

let processConnectionRequest client =
    connectedClients <- (client, nextClientId) :: connectedClients
    ConnectionResponse nextClientId |> serialize |> server.SendTo client
    nextClientId <- nextClientId + 1

let processCharacterMoveRequest client (x, y) =
    let clientId = connectedClients |> List.find ((=) client << fst) |> snd  // It just finds client id in a list
    CharacterPositionUpdate (clientId, x, y) |> serialize |> server.SendToAllClients 

let processMessage client msg = 
    match msg with
    | ConnectionRequest -> processConnectionRequest client
    | CharacterMoveRequest (x, y) -> processCharacterMoveRequest client (x, y)
    | _ -> failwith "Protocol violation"

let clientDisconnected client =
    connectedClients <- connectedClients |> List.filter ((<>) client << fst)

server.NewMessage.Add (fun (client, msg) -> deserialize msg |> processMessage client)
server.ClientDisconnected.Add clientDisconnected

server.Start ()
