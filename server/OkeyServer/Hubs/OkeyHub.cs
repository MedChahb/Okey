namespace OkeyServer.Hubs;

using System.Collections.Concurrent;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.SignalR;
using Misc;
using Okey;
using Okey.Game;
using Okey.Joueurs;
using Okey.Tuiles;
using Packets;
using Packets.Dtos;
using Security;

/// <summary>
/// Hub de communication entre les clients et le serveur
/// </summary>

public sealed class OkeyHub : Hub
{
    private static ConcurrentDictionary<string, string> _idToUsername =
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
                        message = $"Player {this.Context.ConnectionId} has left the lobby."
                    }
                );
        }
        _idToUsername.TryRemove(this.Context.ConnectionId, out _);
        await base.OnDisconnectedAsync(exception);
    }

    /// <summary>
    /// Quand le joueur fourni un JWT Token lie la session de connxion au nom d'utilisateur
    /// Une transmission AccountLinkResult est envoyé au client indiquant comment c'est passé la tentative d'association
    /// </summary>
    /// <param name="token">JWT token</param>
    public async Task ConnectAccount(string token)
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
                        // on ajoute ou on update le lien entre le connexion_id et l'username
                        _idToUsername[this.Context.ConnectionId] = claimValue.ToString();
                        //Console.WriteLine("user : "+ claimValue + " associé à l'id : " + this.Context.ConnectionId );
                        // 0 = bon déroulement
                        await this.Clients.Caller.SendAsync("AccountLinkResult", 0);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        // 4 = exception lors de l'ajout ou de l'envoi du bon déroulement
                        await this.Clients.Caller.SendAsync("AccountLinkResult", 4);
                        throw;
                    }
                }
                else
                {
                    // 3 = l'attribut given_name n'est pas présent dans le token
                    await this.Clients.Caller.SendAsync("AccountLinkResult", 3);
                    // Console.WriteLine("No given_name in the token");
                }
            }
            else
            {
                // 2 = le token a été mal lu ou un token au format invalide à été transmis
                await this.Clients.Caller.SendAsync("AccountLinkResult", 2);
                // Console.WriteLine("Invalid JWT token format.");
            }
        }
        else
        {
            // 1 = la vérification du token à échouée
            await this.Clients.Caller.SendAsync("AccountLinkResult", 1);
        }
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
                .SendAsync("ReceiveMessage", new PacketSignal { message = message });
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
                        message = $"Player {this.Context.ConnectionId} joined {lobbyName}"
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
                    new PacketSignal { message = "Unable to join the room. It may be full." }
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
                new PacketSignal { message = $"Player {this.Context.ConnectionId} left the lobby." }
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
        return new Coord(
            int.Parse(parts[0], CultureInfo.InvariantCulture),
            int.Parse(parts[1], CultureInfo.InvariantCulture)
        );
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
            .SendAsync("ReceiveMessage", new PacketSignal { message = message });

    /// <summary>
    /// Requete de coordoonnees
    /// </summary>
    /// <param name="connectionId">Id de l'utilisateur</param>
    /// <returns>Contrat</returns>
    private async Task<string> CoordsRequest(string connectionId) =>
        await this
            ._hubContext.Clients.Client(connectionId)
            .InvokeAsync<string>("CoordsActionRequest", cancellationToken: CancellationToken.None);

    private async Task SendRoomsRequest()
    {
        var listToSend = new List<RoomDto>();

        foreach (var room in this._roomManager.GetRooms().Values)
        {
            var r = new RoomDto
            {
                Name = room.Name,
                Capacity = room.Capacity,
                Players = room.Players
            };
            listToSend.Add(r);
        }
        await this
            ._hubContext.Clients.Group("Hub")
            .SendAsync("UpdateRoomsRequest", new RoomsPacket { ListRooms = listToSend });
    }

    private async Task<string> CoordsGainRequest(string connectionId)
    {
        var tuile = await this
            ._hubContext.Clients.Client(connectionId)
            .InvokeAsync<TuilePacket>(
                "CoordsGainRequest",
                cancellationToken: CancellationToken.None
            );
        if (tuile.gagner == true)
        {
            return "gagner";
        }
        return tuile.Y + ";" + tuile.X;
    }

    /* Pour debug
    private async Task<string> FirstCoordsRequest(string connectionId) =>
        await this
            ._hubContext.Clients.Client(connectionId)
            .InvokeAsync<string>(
                "FirstCoordsActionRequest",
                cancellationToken: CancellationToken.None
            );

    /// <summary>
    /// Requete de pioche
    /// </summary>
    /// <param name="connectionId"></param>
    /// <returns>Contrat</returns>
    public async Task<string> PiocheRequest(string connectionId) =>
        await this
            ._hubContext.Clients.Client(connectionId)
            .InvokeAsync<string>("PiocheRequest", cancellationToken: CancellationToken.None);
    */
    public async Task<string> PiochePacketRequest(string connectionId)
    {
        var pioche = await this
            ._hubContext.Clients.Client(connectionId)
            .InvokeAsync<PiochePacket>(
                "PiochePacketRequest",
                cancellationToken: CancellationToken.None
            );
        if (pioche.Centre == true && pioche.Defausse == false)
        {
            return "Centre";
        }
        else if (pioche.Centre == false && pioche.Defausse == true)
        {
            return "Defausse";
        }
        else
        {
            throw new ArgumentException("Le packet n'est pas conforme");
        }
    }

    /// <summary>
    /// Requete de jeter
    /// </summary>
    /// <param name="connectionId"></param>
    /// <returns>coordonnées</returns>
    private async Task<string> JeterRequest(string connectionId)
    {
        var TuileObtenue = await this
            ._hubContext.Clients.Client(connectionId)
            .InvokeAsync<TuilePacket>("JeterRequest", cancellationToken: CancellationToken.None);
        if (TuileObtenue.gagner == true)
        {
            return "gagner";
        }

        return TuileObtenue.Y + ";" + TuileObtenue.X;
    }

    /// <summary>
    /// Requete de jet pour le premier joueur, ne pas remplacer !!!! (thread conditions)
    /// </summary>
    /// <param name="connectionId"></param>
    /// <returns>coordonnes sous la forme y;x</returns>
    private async Task<string> FirstJeterRequest(string connectionId)
    {
        var TuileObtenueSend = await this
            ._hubContext.Clients.Client(connectionId)
            .InvokeAsync<TuilePacket>(
                "FirstJeterActionRequest",
                cancellationToken: CancellationToken.None
            );
        return TuileObtenueSend.Y + ";" + TuileObtenueSend.X;
    }

    private async Task TourSignalRequest(string roomName, string? connectionId)
    {
        foreach (var player in this._roomManager.GetRooms()[roomName].GetPlayerIds())
        {
            if (player.Equals(connectionId, StringComparison.Ordinal))
            {
                await this._hubContext.Clients.Client(player).SendAsync("YourTurnSignal");
            }
            else
            {
                await this
                    ._hubContext.Clients.Client(player)
                    .SendAsync(
                        "TurnSignal",
                        connectionId,
                        cancellationToken: CancellationToken.None
                    );
            }
        }
    }

    private async Task SendChevalets(List<string> connectionIds, List<Joueur> joueurs)
    {
        foreach (var connectionId in connectionIds)
        {
            await this.SendChevalet(
                connectionId,
                joueurs.Find(x => x.getName().Equals(connectionId, StringComparison.Ordinal))!
            );
        }
    }

    private async Task SendChevalet(string connectionId, Joueur joueur)
    {
        var chevaletInit = joueur.GetChevalet();
        var premiereRangee = new List<string>();
        var secondeRangee = new List<string>();

        for (int i = 0; i < 2; i++)
        {
            if (i == 0)
            {
                for (int j = 0; j < 14; j++)
                {
                    var val = chevaletInit[i][j];
                    if (val == null)
                    {
                        premiereRangee.Add($"couleur=;num=;defausse=;dansPioche=;Nom=;");
                    }
                    else
                    {
                        premiereRangee.Add(
                            $"couleur={this.FromEnumToString(chevaletInit[i][j]!.GetCouleur())};num={chevaletInit[i][j]!.GetNum()};defausse={chevaletInit[i][j]!.isDefausse()};dansPioche={chevaletInit[i][j]!.GetPioche()};Nom={chevaletInit[i][j]!.GetName()};"
                        );
                    }
                }
            }
            else
            {
                for (int j = 0; j < 14; j++)
                {
                    var val = chevaletInit[i][j];
                    if (val == null)
                    {
                        secondeRangee.Add($"couleur=;num=;defausse=;dansPioche=;Nom=;");
                    }
                    else
                    {
                        secondeRangee.Add(
                            $"couleur={this.FromEnumToString(chevaletInit[i][j]!.GetCouleur())};num={chevaletInit[i][j]!.GetNum()};defausse={chevaletInit[i][j]!.isDefausse()};dansPioche={chevaletInit[i][j]!.GetPioche()};Nom={chevaletInit[i][j]!.GetName()};"
                        );
                    }
                }
            }
        }
        await this
            ._hubContext.Clients.Client(connectionId)
            .SendAsync(
                "ReceiveChevalet",
                new ChevaletPacket
                {
                    PremiereRangee = premiereRangee,
                    SecondeRangee = secondeRangee
                }
            );
    }

    public string FromEnumToString(CouleurTuile col)
    {
        switch (col)
        {
            case CouleurTuile.J:
                return "J";
            case CouleurTuile.N:
                return "N";
            case CouleurTuile.R:
                return "R";
            case CouleurTuile.B:
                return "B";
            case CouleurTuile.M:
                return "M";
            default:
                return "";
        }
    }

    /// <summary>
    /// Permet de definir l'etat du tour d'un joueur
    /// </summary>
    /// <param name="playerName">Nom du joueur</param>
    /// <param name="isTurn">Booleen: vrai -> c'est son tour ; false => ce n'est pas son tour</param>
    private void SetPlayerTurn(string playerName, bool isTurn) =>
        this._isPlayerTurn[playerName] = isTurn;

    private async Task TuilesDistribueesSignal(string roomName) =>
        await this.Clients.Group(roomName).SendAsync("TuilesDistribueesSignal");

    private async Task StartGameSignal(string roomName) =>
        await this.Clients.Group(roomName).SendAsync("StartGame");

    private async Task PlayerWon(string roomName, string winner) =>
        await this.Clients.Group(roomName).SendAsync("PlayerWon", winner); //TODO: Faire la distincion entre le gagnant et les autres

    /// <summary>
    /// Fonction se declanchant quand il y a assez de monde, lancement du jeu
    /// </summary>
    /// <param name="roomName">nom de la room qui accueille le jeu</param>
    public async Task StartGameForRoom(string roomName)
    {
        await this.StartGameSignal(roomName);

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

        await this.TuilesDistribueesSignal(roomName);

        foreach (var t in joueurs)
        {
            this.SetPlayerTurn(t.getName(), false);
        }

        var joueurStarter = jeu.getJoueurActuel();
        var playerName = string.Empty;
        {
            this.SetPlayerTurn(joueurStarter?.getName() ?? playerName, true);
            await this.TourSignalRequest(roomName, joueurStarter?.getName());
            if (joueurStarter != null)
            {
                //await this.SendChevalet(joueurStarter.getName(), joueurStarter);
                await this.SendChevalets(playerIds, joueurs.ToList());
                var coords = await this.FirstJeterRequest(joueurStarter.getName());
                if (coords.Equals("Move", StringComparison.Ordinal))
                {
                    // Faire le mouvement de tuiles
                    // MoveInLoop(joueurStarter, j);
                    // continue;
                }

                joueurStarter.JeterTuile(this.ReadCoords(coords), jeu);
                await this.SendChevalet(joueurStarter.getName(), joueurStarter);
                this.SetPlayerTurn(joueurStarter.getName(), false);
            }

            this.SetPlayerTurn(
                jeu.getJoueurActuel()?.getName() ?? throw new InvalidOperationException(),
                true
            );

            while (!jeu.isTermine())
            {
                var currentPlayer = jeu.getJoueurActuel();
                await this.TourSignalRequest(roomName, currentPlayer?.getName());
                if (this._isPlayerTurn[currentPlayer?.getName() ?? playerName])
                {
                    if (currentPlayer != null)
                    {
                        await this.SendChevalet(currentPlayer.getName(), currentPlayer);

                        var pioche = await this.PiochePacketRequest(currentPlayer.getName());
                        if (pioche.Equals("Move", StringComparison.Ordinal))
                        {
                            //await this.MoveInLoop(currentPlayer, jeu);
                            continue;
                        }

                        if (
                            string.Equals(pioche, "Centre", StringComparison.OrdinalIgnoreCase)
                            || string.Equals(pioche, "Defausse", StringComparison.OrdinalIgnoreCase)
                        )
                        {
                            currentPlayer.PiocherTuile(pioche, jeu);
                        }
                        else
                        {
                            continue;
                        }

                        await this.SendChevalet(currentPlayer.getName(), currentPlayer);

                        var coordinates = await this.JeterRequest(currentPlayer.getName());

                        if (coordinates.Equals("gagner", StringComparison.Ordinal))
                        {
                            var coordsGain = await this.CoordsGainRequest(currentPlayer.getName());
                            if (
                                currentPlayer?.JeteTuilePourTerminer(
                                    this.ReadCoords(coordsGain),
                                    jeu
                                ) == true
                            )
                            {
                                // Le joueur gagne
                                await this.PlayerWon(roomName, currentPlayer.getName());
                            }

                            if (currentPlayer != null)
                            {
                                await this.SendMpToPlayer(
                                    currentPlayer.getName(),
                                    "Vous n'avez pas de serie dans votre chevalet !"
                                );
                            }

                            continue;
                        }

                        currentPlayer?.JeterTuile(this.ReadCoords(coordinates), jeu);
                        if (currentPlayer != null)
                        {
                            await this.SendChevalet(currentPlayer.getName(), currentPlayer);

                            this.SetPlayerTurn(currentPlayer?.getName() ?? playerName, false);
                        }
                    }

                    this.SetPlayerTurn(jeu.getJoueurActuel()?.getName() ?? playerName, true);
                }
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
    private async Task SendRoomListUpdate() => await this.SendRoomsRequest();
    /*
        var roomsInfo = this._roomManager.GetRoomsInfo();
        await this
            ._hubContext.Clients.Group("Hub")
            .SendAsync("ReceiveMessage", new PacketSignal { message = roomsInfo });*/
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
