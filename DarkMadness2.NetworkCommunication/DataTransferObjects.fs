namespace DarkMadness2.NetworkCommunication

/// All possible network message types.
type Message =

    /// Connection request from client, contains initial coordinates of player.
    | ConnectionRequest of int * int

    /// Response to connection request, contains assigned client id
    /// and list of other players coordinates
    /// TODO: rewrite it for list of players, not for two players
    | ConnectionResponse of int * int * int

    /// Message from client that its character has moved, contains new character coords.
    | CharacterMoveRequest of int * int

    /// Message from server about character movement, contains client id of a client whose character has been moved
    /// and new coords of that character.
    | CharacterPositionUpdate of int * int * int
