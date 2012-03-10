namespace DarkMadness2.NetworkCommunication

// All possible network message types
type Message =

// Connection request from client
| ConnectionRequest

// Response to connection request, contains assigned client id
| ConnectionResponse of int

// Message from client that its character has moved, contains new character coords
| CharacterMoveRequest of int * int

// Message from server about character movement, contains client id of a client whose character has been moved
// and new coords of that character
| CharacterPositionUpdate of int * int * int