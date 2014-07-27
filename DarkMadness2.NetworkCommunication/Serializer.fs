/// Serializes and deserializes data transfer objects.
module DarkMadness2.NetworkCommunication.Serializer

/// Serialize data transfer object into string.
let serialize msg =
    match msg with
    | ConnectionRequest (x, y) -> "ConnectionRequest " + string x + " " + string y
    | ConnectionResponse (id, x, y) -> "ConnectionResponse " + string id + " " + string x + " " + string y
    | CharacterMoveRequest (x, y) -> "CharacterMoveRequest " + string x + " " + string y
    | CharacterPositionUpdate (id, x, y) -> "CharacterPositionUpdate " + string id + " " + string x + " " + string y 

/// Deserialize data transfer object from string.
let deserialize (msg : string) =
    let messageParts = msg.Split [|' '|] |> List.ofArray
    match messageParts with
    | ["ConnectionRequest"; x; y] -> ConnectionRequest (int x, int y)
    | ["ConnectionResponse"; id; x; y] -> ConnectionResponse (int id, int x, int y)
    | ["CharacterMoveRequest"; x; y] -> CharacterMoveRequest (int x, int y)
    | ["CharacterPositionUpdate"; id; x; y] -> CharacterPositionUpdate (int id, int x, int y)
    | _ -> failwith "Malformed message"
