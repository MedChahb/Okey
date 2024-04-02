namespace OkeyServer.Hubs;

using Lobby.Exception;
using Microsoft.AspNetCore.SignalR;
using Misc;

/// <summary>
/// Hub de communication entre les clients et le serveur
/// </summary>
public sealed class OkeyHub : Hub
{
    // necessité d'avoir accès à l'ensemble des rooms ici

    private static List<Room> _roomsAvailable;
    private static List<Room> _roomsBusy;
    private static int _nbJoueursOnline;

    static OkeyHub()
    {
        var r1 = new Room("room1");
        var r2 = new Room("room2");
        var r3 = new Room("room3");
        var r4 = new Room("room4");

        _roomsAvailable = new List<Room>() { r1, r2, r3, r4 };
        _roomsBusy = new List<Room>();
        _nbJoueursOnline = 0;
    }

    /// <summary>
    /// Méthode temporaire permettant de recuperer les rooms avec toutes les informations
    /// </summary>
    /// <returns>Chaine de caractere contenant l'affichage de l'etat actuel des rooms</returns>
    private static string DisplayRoomsAvailable()
    {
        var buffer =
            $"\nIl y a {_nbJoueursOnline} joueurs en ligne.\nVoici les chambres que vous pouvez rejoindre: \n";
        foreach (var r in _roomsAvailable)
        {
            buffer += "----\n";
            buffer += $"Nom: {r.GetRoomName()}\n";
            buffer += $"État de la room: {r.GetNbCurrent()} / {r.GetCapacity()}\n";
        }
        buffer += "---";
        return buffer;
    }

    /// <summary>
    /// On ajoute automatiquement le client au groupe Hub,
    /// ce groupe contient tous les clients qui ne sont pas dans une partie
    /// </summary>
    public override async Task OnConnectedAsync()
    {
        try
        {
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, "Hub");
            _nbJoueursOnline++;
            await this.SendToLobby("Hub", DisplayRoomsAvailable());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var roomId = string.Empty;
        foreach (var r in _roomsAvailable)
        {
            foreach (var id in r.GetUserIds())
            {
                if (this.Context.ConnectionId.Equals(id, StringComparison.Ordinal))
                {
                    roomId = r.GetRoomName();
                }
            }
        }
        _nbJoueursOnline--;
        if (roomId != string.Empty)
        {
            await this.LobbyDisconnection(roomId);
        }
        else
        {
            await this.SendRoomListUpdate();
        }
        await base.OnDisconnectedAsync(exception);
    }

    /// <summary>
    /// Envoi un message à tous lees membres d'un groupe
    /// </summary>
    /// <param name="lobbyName"> groupe qui reçoit le message de test</param>
    /// <param name="message"> message envoyé à tous les membres du groupe</param>
    public async Task SendToLobby(string lobbyName, string message)
    {
        try
        {
            await this.Clients.Group(lobbyName).SendAsync("ReceiveMessage", $"{message}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    /// <summary>
    /// Donne l'objet Room correspondant à son nom.
    /// </summary>
    /// <param name="lobbyName">Nom de la room recherché</param>
    /// <returns>Room ayant pour attribut roomName le lobbyName</returns>
    private static Room? GetRoomById(string lobbyName) =>
        _roomsAvailable.FirstOrDefault(r =>
            r.GetRoomName().Equals(lobbyName, StringComparison.Ordinal)
        );

    /// <summary>
    /// Permet d'envoyer le nouvel etat des rooms au clients dans le Hub.
    /// </summary>
    private async Task SendRoomListUpdate() =>
        await this.Clients.Group("Hub").SendAsync("ReceiveMessage", DisplayRoomsAvailable());

    /// <summary>
    /// Effectue la connection au lobby correspondant au lobbyName si ce lobby est joignable
    /// </summary>
    /// <param name="lobbyName"> chaine de caractère correspondant au lobby souhaitant être rejoint </param>
    public async Task LobbyConnection(string lobbyName)
    {
        var room = GetRoomById(lobbyName);
        if ( /* vérifie que lobbyname correspond à un lobby existant et disponible*/
            room != null
        )
        {
            /* soit on assigne automatiquement le lobby soit le joueur à le choix  enlever lobbyname si auto*/
            try
            {
                await this.Groups.RemoveFromGroupAsync(this.Context.ConnectionId, "Hub");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await this.Clients.Caller.SendAsync(
                    "ReceiveMessage",
                    $"An error Occured or you tried to connect to a lobby from another lobby \nException : {e}"
                );
                throw;
            }

            try
            {
                var res = room.JoueurJoinRoom(this.Context.ConnectionId);
                if (res == null)
                {
                    /* La room est pleine */
                    await this
                        .Clients.Client(this.Context.ConnectionId)
                        .SendAsync(
                            "ReceiveMessage",
                            "Le lobby est plein, veuillez en choisir un autre."
                        );
                    await this.Groups.AddToGroupAsync(this.Context.ConnectionId, "Hub");
                }
                else if (res == false)
                {
                    /* La room est désormais complète il faut la retirer des rooms disponnibles */
                    await this.Groups.AddToGroupAsync(this.Context.ConnectionId, lobbyName);
                    _roomsBusy.Add(room);
                    _roomsAvailable.Remove(room);
                    await this.SendRoomListUpdate();
                    await this
                        .Clients.Group(lobbyName)
                        .SendAsync(
                            "ReceiveMessage",
                            $"Le joueur {this.Context.ConnectionId} a rejoint {lobbyName}"
                        );
                    /* Ici on lance la partie ! */
                    await this.LaunchGame(room);
                }
                else
                {
                    /* Tout se passe bien ici */
                    await this.Groups.AddToGroupAsync(this.Context.ConnectionId, lobbyName);
                    await this.SendRoomListUpdate();
                    await this
                        .Clients.Group(lobbyName)
                        .SendAsync(
                            "ReceiveMessage",
                            $"Le joueur {this.Context.ConnectionId} a rejoint {lobbyName}"
                        );
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(
                    $"{e} \nAn error occured while user was added to the group {lobbyName}"
                );
                throw;
            }
        }
    }

    /// <summary>
    /// deconnecte le client du lobby en question
    /// </summary>
    /// <param name="lobbyName"> nom du lobby à quitter </param>
    public async Task LobbyDisconnection(string lobbyName)
    {
        try
        {
            var room = GetRoomById(lobbyName);
            await this.Groups.RemoveFromGroupAsync(this.Context.ConnectionId, lobbyName);
            var b = room?.JoueurLeaveRoom(this.Context.ConnectionId);
            if (b == true)
            {
                /* Une nouvelle room est disponible */
                if (room != null)
                {
                    _roomsAvailable.Add(room);
                    _roomsBusy.Remove(room);
                }
                else
                {
                    throw new RoomNotFoundException("Erreur systeme.");
                }
            }
            await this
                .Clients.Group(lobbyName)
                .SendAsync(
                    "ReceiveMessage",
                    $"Le joueur {this.Context.ConnectionId} a quitté le lobby!"
                );
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, "Hub");
            await this.SendRoomListUpdate();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task LaunchGame(Room room)
    {
        await this
            .Clients.Group(room.GetRoomName())
            .SendAsync("ReceiveMessage", "La partie va commencer !");
    }
}
