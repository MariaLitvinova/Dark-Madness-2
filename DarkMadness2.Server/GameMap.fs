module DarkMadness2.Server.GameMap

/// This class has information about all players and their position on map.
type GameMap () =

    /// Current list of players.
    /// Each element contains of player number, player x coordinate and player y coordinate.
    let mutable listOfPlayers : (int * (int * int)) list = []

    /// Appends new player to the list of players.
    member this.AddNewPlayer newPlayerNumber newPlayerX newPlayerY =
        listOfPlayers <- (newPlayerNumber, (newPlayerX, newPlayerY)) :: listOfPlayers

    /// Updates coordinates of given player.
    /// :(
    member this.UpdateCoordinatesOfGivenPlayer playerNumber newX newY =
        listOfPlayers <- listOfPlayers |> List.filter ((<>) playerNumber << fst)
        listOfPlayers <- (playerNumber, (newX, newY)) :: listOfPlayers

    /// Returns list of players.
    member this.ListOfConnectedPlayers = 
        listOfPlayers |> List.map (fst)

    /// Returns list of players with their coordinates.
    member this.ListOfPlayersWithTheirCoordinates = 
        listOfPlayers

   /// Returns player coordinates by his number.
    member this.GetCoordinatesByPlayerNumber playerNumber = 
        listOfPlayers |> List.find ((=) playerNumber << fst) |> snd

    /// Removes player from list of connected players.
    member this.RemovePlayerFromList playerNumber = 
        listOfPlayers <- listOfPlayers |> List.filter ((<>) playerNumber << fst)

