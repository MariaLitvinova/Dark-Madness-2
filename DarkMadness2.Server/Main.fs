module DarkMadness2.Server.Main

open DarkMadness2.Server.GameMap
open Microsoft.FSharp.Collections
open DarkMadness2.NetworkCommunication
open DarkMadness2.NetworkCommunication.Serializer

/// Server that listens for connections on port 8181
let server = Server 8181

/// List of connected clients. This is not a list of connected TCP clients from server, because one client can disconnect
/// and then reconnect, and still needs to be counted by the game as one client.
let mutable connectedClients : (obj * int) list = []

/// Map with all current players.
let gameMap = GameMap ()

/// Used to assign unique ids to clients.
let mutable nextClientId = 0

/// Handler for ConnectionRequest message.
let processConnectionRequest client (x, y) =
    connectedClients <- (client, nextClientId) :: connectedClients
    let otherPlayerCoordinates = 
        match gameMap.ListOfConnectedPlayers |> List.isEmpty with
        | true -> (-1, -1)
        | false -> gameMap.ListOfPlayersWithTheirCoordinates |> List.map (fun player -> player |> snd) |> List.head
    ConnectionResponse (nextClientId, otherPlayerCoordinates |> fst, otherPlayerCoordinates |> snd) |> serialize |> server.SendTo client
    gameMap.AddNewPlayer nextClientId x y
    nextClientId <- nextClientId + 1

/// Handler for CharacterMoveRequest message.
let processCharacterMoveRequest client (x, y) =
    let clientId = connectedClients |> List.find ((=) client << fst) |> snd  // It just finds client id in a list
    CharacterPositionUpdate (clientId, x, y) |> serialize |> server.SendToAllClients 

/// Client message dispatcher.
let processMessage client msg = 
    match msg with
    | ConnectionRequest (x, y) -> processConnectionRequest client (x, y)
    | CharacterMoveRequest (x, y) -> processCharacterMoveRequest client (x, y)
    | _ -> failwith "Protocol violation"

/// Handler for client disconnection event.
let clientDisconnected client =
    connectedClients <- connectedClients |> List.filter ((<>) client << fst)

server.NewMessage.Add (fun (client, msg) -> deserialize msg |> processMessage client)
server.ClientDisconnected.Add clientDisconnected

server.Start ()
