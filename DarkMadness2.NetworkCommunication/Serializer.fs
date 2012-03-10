namespace DarkMadness2.NetworkCommunication

type Serializer =
    static member Serialize msg =
        match msg with
        | ConnectionRequest -> "ConnectionRequest"
        | ConnectionResponse id -> "ConnectionResponse " + string id
        | CharacterMoveRequest (x, y) -> "CharacterMoveRequest " + string x + " " + string y
        | CharacterPositionUpdate (id, x, y) -> "CharacterPositionUpdate " + string id + string x + string y 

    static member Deserialize (msg : string) =
        let messageParts = msg.Split [|' '|] |> List.ofArray
        match messageParts with
        | ["ConnectionRequest"] -> ConnectionRequest
        | ["ConnectionResponse"; id] -> ConnectionResponse (int id)
        | ["CharacterMoveRequest"; x; y] -> CharacterMoveRequest (int x, int y)
        | ["CharacterPositionUpdate"; id; x; y] -> CharacterPositionUpdate (int id, int x, int y)
        | _ -> failwith "Malformed message"
