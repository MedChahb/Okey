namespace OkeyServer.Hubs;

using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.SignalR;
using Misc;
using Okey;
using Okey.Game;
using Okey.Joueurs;
using Packets;
using Security;

/// <summary>
/// Hub de communication entre les clients et le serveur
/// </summary>

public sealed class OkeyHub : Hub
{
    private static ConcurrentDictionary<string, string> _clientServeur =
        new ConcurrentDictionary<string, string>();
    private readonly IRoomManager _roomManager;
    private readonly IHubContext<OkeyHub> _hubContext;
    private static readonly char[] Separator = new char[] { ';' };
    private ConcurrentDictionary<string, bool> _isPlayerTurn;

    public OkeyHub(IHubContext<OkeyHub> hubContext, IRoomManager roomManager)
    {
        this._roomManager = roomManager;
        this._hubContext = hubContext;
        this._isPlayerTurn = new ConcurrentDictionary<string, bool>();
    }

    /// <summary>
    /// On ajoute automatiquement le client au groupe Hub,
    /// ce groupe contient tous les clients qui ne sont pas dans une partie
    /// </summary>
    public override async Task OnConnectedAsync()
    {
        await this._hubContext.Groups.AddToGroupAsync(this.Context.ConnectionId, "Hub");
        await this.SendRoomListUpdate();
        await base.OnConnectedAsync();
    }

    /// <summary>
    /// On supprime automatiquement le client au groupe Hub.
    /// </summary>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var roomId = this._roomManager.GetRoomIdByConnectionId(this.Context.ConnectionId);
        this._roomManager.PlayerDisconnected(this.Context.ConnectionId);
        await this.SendRoomListUpdate();
        if (!string.IsNullOrEmpty(roomId))
        {
            await this
                ._hubContext.Clients.Group(roomId)
                .SendAsync(
                    "ReceiveMessage",
                    new PacketSignal
                    {
                        _message = $"Player {this.Context.ConnectionId} has left the lobby."
                    }
                );
        }

        await base.OnDisconnectedAsync(exception);
    }

    /// <summary>
    /// Quand le joueur fourni un JWT Token lie la session de connxion au nom d'utilisateur
    /// </summary>
    /// <param name="token">JWT token</param>
    public Task ConnectAccount(string token)
    {
        // Premièrement on vérifie la validité du token
        if (JWTCheck.CheckToken(token))
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
            if (securityToken != null)
            {
                // on recupère le champ "given_name" correspondant au username
                var claim = securityToken.Claims.FirstOrDefault(c => c.Type == "given_name");

                if (claim != null)
                {
                    // on récupère le string
                    var claimValue = claim.Value;
                    try
                    {
                        //on ajoute ou on update le lien entre le connexion_id et l'username
                        _clientServeur.AddOrUpdate(
                            this.Context.ConnectionId,
                            cle => claimValue,
                            (cle, ancienneValeur) => claimValue.ToString()
                        );
                        //Console.WriteLine("user : "+ claimValue + " associé à l'id : " + this.Context.ConnectionId );
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }
                else
                {
                    // a remplacer par une exception
                    Console.WriteLine("No given_name in the token");
                }
            }
            else
            {
                // a remplacer par une exception
                Console.WriteLine("Invalid JWT token format.");
            }
        }
        return Task.CompletedTask;
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
            await this
                .Clients.Group(lobbyName)
                .SendAsync("ReceiveMessage", new PacketSignal { _message = message });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    /// <summary>
    /// Effectue la connection au lobby correspondant au lobbyName si ce lobby est joignable
    /// </summary>
    /// <param name="lobbyName"> chaine de caractère correspondant au lobby souhaitant être rejoint </param>
    public async Task LobbyConnection(string lobbyName)
    {
        var success = this._roomManager.TryJoinRoom(lobbyName, this.Context.ConnectionId);
        if (success)
        {
            await this._hubContext.Groups.RemoveFromGroupAsync(this.Context.ConnectionId, "Hub");

            await this._hubContext.Groups.AddToGroupAsync(this.Context.ConnectionId, lobbyName);
            await this.SendRoomListUpdate();

            await this
                ._hubContext.Clients.Group(lobbyName)
                .SendAsync(
                    "ReceiveMessage",
                    new PacketSignal
                    {
                        _message = $"Player {this.Context.ConnectionId} joined {lobbyName}"
                    }
                );

            if (this._roomManager.IsRoomFull(lobbyName))
            {
                this.OnGameStarted(lobbyName);
            }
        }
        else
        {
            await this
                ._hubContext.Clients.Client(this.Context.ConnectionId)
                .SendAsync(
                    "ReceiveMessage",
                    new PacketSignal { _message = "Unable to join the room. It may be full." }
                );
        }
    }

    /// <summary>
    /// deconnecte le client du lobby en question
    /// </summary>
    /// <param name="lobbyName"> nom du lobby à quitter </param>
    public async Task LobbyDisconnection(string lobbyName)
    {
        this._roomManager.LeaveRoom(lobbyName, this.Context.ConnectionId);
        await this
            ._hubContext.Clients.Group(lobbyName)
            .SendAsync(
                "ReceiveMessage",
                new PacketSignal
                {
                    _message = $"Player {this.Context.ConnectionId} left the lobby."
                }
            );
        await this.SendRoomListUpdate();
    }

    public Coord ReadCoords(string str)
    {
        Console.WriteLine(str);
        var parts = str.Split(Separator, StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length != 2)
        {
            throw new ArgumentException(
                "La chaîne de coordonnées doit contenir exactement deux parties."
            );
        }
        return new Coord(int.Parse(parts[0]), int.Parse(parts[1]));
    }

    /// <summary>
    /// Permet de lancer la partie
    /// </summary>
    /// <param name="roomName">Nom de room</param>
    private async void OnGameStarted(string roomName) => await this.StartGameForRoom(roomName);

    /// <summary>
    /// Envoie un message a tout les utilisateurs d'un groupe
    /// </summary>
    /// <param name="roomName">nom de room</param>
    /// <param name="message">message a envoyer</param>
    private async Task BroadCastInRoom(string roomName, PacketSignal message) =>
        await this._hubContext.Clients.Group(roomName).SendAsync("ReceiveMessage", message);

    /// <summary>
    /// Envoie un message a tout un utilisateur d'un groupe
    /// </summary>
    /// <param name="userId">nom de l'utilisateur</param>
    /// <param name="message">message a envoyer</param>
    private async Task SendMpToPlayer(string userId, string message) =>
        await this
            ._hubContext.Clients.Client(userId)
            .SendAsync("ReceiveMessage", new PacketSignal { _message = message });

    /// <summary>
    /// Requete de coordoonnees
    /// </summary>
    /// <param name="connectionId">Id de l'utilisateur</param>
    /// <returns>Contrat</returns>
    private async Task<string> CoordsRequest(string connectionId) =>
        await this
            ._hubContext.Clients.Client(connectionId)
            .InvokeAsync<string>("ReceiveRequest", cancellationToken: CancellationToken.None);

    /// <summary>
    /// Requete de pioche
    /// </summary>
    /// <param name="connectionId"></param>
    /// <returns>Contrat</returns>
    public async Task<string> PiocheRequest(string connectionId) =>
        await this
            ._hubContext.Clients.Client(connectionId)
            .InvokeAsync<string>("PiocheRequest", cancellationToken: CancellationToken.None);

    /// <summary>
    /// Permet de definir l'etat du tour d'un joueur
    /// </summary>
    /// <param name="playerName">Nom du joueur</param>
    /// <param name="isTurn">Booleen: vrai -> c'est son tour ; false => ce n'est pas son tour</param>
    private void SetPlayerTurn(string playerName, bool isTurn) =>
        this._isPlayerTurn[playerName] = isTurn;

    /// <summary>
    /// Fonction se declanchant quand il y a assez de monde, lancement du jeu
    /// </summary>
    /// <param name="roomName">nom de la room qui accueille le jeu</param>
    public async Task StartGameForRoom(string roomName)
    {
        await this.BroadCastInRoom(
            roomName,
            new PacketSignal { _message = "La partie va commencer" }
        );

        var playerIds = this._roomManager.GetRooms()[roomName].GetPlayerIds();
        Joueur[] joueurs =
        {
            new Humain(1, playerIds[0], 800),
            new Humain(2, playerIds[1], 100),
            new Humain(3, playerIds[2], 2400),
            new Humain(4, playerIds[3], 2400)
        };

        var jeu = new Jeu(1, joueurs);
        jeu.DistibuerTuile();
        await this.BroadCastInRoom(
            roomName,
            new PacketSignal { _message = "Les tuiles ont été distribuees" }
        );

        foreach (var t in joueurs)
        {
            this.SetPlayerTurn(t.getName(), false);
        }

        var joueurStarter = jeu.getJoueurActuel();
        this.SetPlayerTurn(joueurStarter.getName(), true);
        await this.SendMpToPlayer(joueurStarter.getName(), jeu.StringChevaletActuel());
        var coords = await this.CoordsRequest(joueurStarter.getName());
        joueurStarter.JeterTuile(this.ReadCoords(coords), jeu);
        this.SetPlayerTurn(joueurStarter.getName(), false);
        this.SetPlayerTurn(jeu.getJoueurActuel().getName(), true);

        while (!jeu.isTermine())
        {
            var currentPlayer = jeu.getJoueurActuel();
            if (this._isPlayerTurn[currentPlayer.getName()])
            {
                Console.WriteLine($"C'est le tour de {currentPlayer.getName()}");
                await this.SendMpToPlayer(currentPlayer.getName(), "C'est votre tour");
                await this.SendMpToPlayer(currentPlayer.getName(), jeu.StringChevaletActuel());

                var pioche = await this.PiocheRequest(currentPlayer.getName());
                if (pioche == "Move")
                {
                    await this.MoveInLoop(currentPlayer, jeu);
                    continue;
                }
                currentPlayer.PiocherTuile(pioche, jeu);

                await this.SendMpToPlayer(currentPlayer.getName(), jeu.StringChevaletActuel());

                var coordinates = await this.CoordsRequest(currentPlayer.getName());
                currentPlayer.JeterTuile(this.ReadCoords(coordinates), jeu);

                this.SetPlayerTurn(currentPlayer.getName(), false);
                this.SetPlayerTurn(jeu.getJoueurActuel().getName(), true);
            }
        }
    }

    /// <summary>
    /// Permet de bouger une tuile sur son chevalet
    /// </summary>
    /// <param name="pl"></param>
    /// <param name="j"></param>
    public async Task MoveInLoop(Joueur pl, Jeu j)
    {
        Console.Write("Donner les coords de la tuile à deplacer (y x): ");
        var from = await this.CoordsRequest(pl.getName());
        Console.Write("Donner les coords d'où la mettre (y x): ");
        var to = await this.CoordsRequest(pl.getName());
        pl.MoveTuileChevalet(this.ReadCoords(from), this.ReadCoords(to), j);
    }

    /// <summary>
    /// Permet d'envoyer le nouvel etat des rooms au clients dans le Hub.
    /// </summary>
    private async Task SendRoomListUpdate()
    {
        var roomsInfo = this._roomManager.GetRoomsInfo();
        await this
            ._hubContext.Clients.Group("Hub")
            .SendAsync("ReceiveMessage", new PacketSignal { _message = roomsInfo });
    }

    /* test pruposes only
    public async Task getAllAssociations()
    {
        foreach (var elmt in _clientServeur)
        {
            Console.WriteLine($"Key: {elmt.Key}, Value: {elmt.Value}");
        }
    }
    */
}
