using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Code.Scripts.SignalR.Packets;
using Code.Scripts.SignalR.Packets.Rooms;
using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SignalRConnector : MonoBehaviour
{
    private HubConnection _hubConnection;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public async void InitializeConnection()
    {
        this._hubConnection = new HubConnectionBuilder().WithUrl(Constants.SIGNALR_HUB_URL).Build();

        this.ConfigureHubEvents();

        try
        {
            await this._hubConnection.StartAsync();
            Debug.Log("SignalR connection established.");
            LobbyManager.Instance.SetConnectionStatus(true);
        }
        catch (Exception ex)
        {
            Debug.LogError($"SignalR connection failed: {ex.Message}");
        }

        this.JoinRoom();
    }

    private void ConfigureHubEvents()
    {
        /*
        this._hubConnection.On<RoomsPacket>(
            "UpdateRoomsRequest",
            Rooms =>
            {
                Debug.Log("Received room update request.");
                foreach (var room in Rooms.listRooms)
                {
                    if (room == null)
                    {
                        Debug.Log("Null");
                        continue;
                    }
                    Debug.Log($"Room: {room.Name}, Players: {room.Players.Count}/{room.Capacity}");
                }

                if (UIManagerPFormulaire.Instance != null)
                {
                    MainThreadDispatcher.Enqueue(() =>
                    {
                        UIManagerPFormulaire.Instance.setActiveShowRooms();
                        LobbyManager.Instance.playerCount = Rooms.listRooms[0].Players.Count;
                        DisplayRooms.Instance.updateLabel();
                    });
                }
                else
                {
                    Debug.LogError("UIManagerPFormulaire instance is null.");
                }
            }
        );*/

        this._hubConnection.On<PacketSignal>(
            "ReceiveMessage",
            Message =>
            {
                Debug.Log(Message.message);
            }
        );

        this._hubConnection.On<StartingGamePacket>(
            "StartGame",
            (players) =>
            {
                Debug.LogWarning("La partie peut commencer");
                //SceneManager.UnloadSceneAsync("SelectionTypeJeu");
                //SceneManager.LoadScene("PlateauInit");


                // if (players.playersList != null)
                // {
                //     LobbyManager.Instance.players.Clear();  // Clear existing players
                //     foreach (var player in players.playersList)
                //     {
                //         Debug.LogWarning(player);

                //         LobbyManager.Instance.players.Add(player);
                //         if (player == _hubConnection.ConnectionId)
                //         {
                //             LobbyManager.Instance.mainPlayer = player;
                //             Debug.Log("Current userID: " + player);
                //         }
                //     }
                // }

                MainThreadDispatcher.Enqueue(() =>
                {
                    Debug.LogWarning($"Le jeu a commence");
                    if (players.playersList != null && players.playersList.Count > 0)
                    {
                        int otherPlayerIndex = 0;
                        for (int i = 0; i < players.playersList.Count; i++)
                        {
                            // string player = players.playersList[i];
                            // store the player value as a string in lower case trim
                            string player = players.playersList[i].Trim().ToLower();
                            // Debug.LogWarning($"Player {i + 1}: {player}");

                            Debug.Log(
                                "Main player: "
                                    + _hubConnection.ConnectionId
                                    + " Player iteration: "
                                    + player
                            );
                            if (player == _hubConnection.ConnectionId.Trim().ToLower())
                            {
                                LobbyManager.Instance.mainPlayer = player;
                                Debug.Log("MainPlayer: " + player);
                            }
                            else
                            {
                                switch (otherPlayerIndex)
                                {
                                    case 0:
                                        LobbyManager.Instance.player2 = player;
                                        // Debug.Log($"Player 2 set to: {player}");
                                        break;
                                    case 1:
                                        LobbyManager.Instance.player3 = player;
                                        // Debug.Log($"Player 3 set to: {player}");
                                        break;
                                    case 2:
                                        LobbyManager.Instance.player4 = player;
                                        // Debug.Log($"Player 4 set to: {player}");
                                        break;
                                }
                                otherPlayerIndex++;
                            }
                        }
                    }
                    else
                    {
                        Debug.LogError("No players found in playersList.");
                    }
                    SceneManager.LoadSceneAsync("PlateauInit");
                });
            }
        );

        this._hubConnection.On<TuilePiocheePacket>(
            "ReceiveTuilePiochee",
            (tuileInfos) =>
            {
                if (tuileInfos.position == 0)
                {
                    MainThreadDispatcher.Enqueue(() =>
                    {
                        Destroy(
                            Chevalet
                                .PileDroitePlaceHolder.transform.GetChild(
                                    Chevalet.PileDroitePlaceHolder.transform.childCount - 1
                                )
                                .gameObject
                        );
                    });
                }
                else if (tuileInfos.position == 1)
                {
                    MainThreadDispatcher.Enqueue(() =>
                    {
                        if (tuileInfos.numero != null)
                        {
                            var tuileData = new TuileData(
                                tuileInfos.Couleur,
                                int.Parse(tuileInfos.numero),
                                tuileInfos.Couleur != null
                                    && (
                                        tuileInfos.Couleur.Equals("M", StringComparison.Ordinal)
                                        || tuileInfos.Couleur.Equals("X", StringComparison.Ordinal)
                                    )
                            );
                            Debug.LogWarning(Chevalet.FromTuileToSpriteName(tuileData));
                            var spriteRen =
                                Chevalet.PileHautDroitePlaceHolder.GetComponent<SpriteRenderer>();
                            spriteRen.sprite = Chevalet.spritesDic[
                                Chevalet.FromTuileToSpriteName(tuileData)
                            ];
                        }
                        else
                        {
                            var spriteRen =
                                Chevalet.PileHautDroitePlaceHolder.GetComponent<SpriteRenderer>();
                            spriteRen.sprite = null;
                        }
                    });
                }
                else
                {
                    MainThreadDispatcher.Enqueue(() =>
                    {
                        if (tuileInfos.numero != null)
                        {
                            var tuileData = new TuileData(
                                tuileInfos.Couleur,
                                int.Parse(tuileInfos.numero),
                                tuileInfos.Couleur != null
                                    && (
                                        tuileInfos.Couleur.Equals("M", StringComparison.Ordinal)
                                        || tuileInfos.Couleur.Equals("X", StringComparison.Ordinal)
                                    )
                            );
                            Debug.LogWarning(Chevalet.FromTuileToSpriteName(tuileData));
                            var spriteRen =
                                Chevalet.PileHautGauchePlaceHolder.GetComponent<SpriteRenderer>();
                            spriteRen.sprite = Chevalet.spritesDic[
                                Chevalet.FromTuileToSpriteName(tuileData)
                            ];
                        }
                        else
                        {
                            var spriteRen =
                                Chevalet.PileHautGauchePlaceHolder.GetComponent<SpriteRenderer>();
                            spriteRen.sprite = null;
                        }
                    });
                }
            }
        );

        this._hubConnection.On<string>(
            "TurnSignal",
            (PlayerName) =>
            {
                Chevalet.Instance.IsJete = false;
                Chevalet.Instance.TuileJete = null;
                Debug.Log($"C'est le tour de {PlayerName}");
                MainThreadDispatcher.Enqueue(() =>
                {
                    // LobbyManager.Instance.SetMyTurn(false);
                    // LobbyManager.Instance.SetCurrentPlayerTurn(PlayerName);
                    // Timer.Instance.LaunchTimer();
                    // Debug.Log("Signal received");
                    PlateauSignals.Instance.SetPlayerSignal(PlayerName);
                    Timer.Instance.LaunchTimer();
                });
            }
        );

        this._hubConnection.On(
            "YourTurnSignal",
            () =>
            {
                Debug.Log($"C'est votre tour");
                MainThreadDispatcher.Enqueue(() =>
                {
                    // LobbyManager.Instance.SetMyTurn(true);
                    // Debug.Log("Signal received");
                    PlateauSignals.Instance.SetMainPlayerTurnSignal();
                    // Debug.Log("restarting timer");
                    Timer.Instance.LaunchTimer();
                });
            }
        );

        this._hubConnection.On<TuilePacket>(
            "TuileJeteeAuto",
            (Tuile) =>
            {
                Chevalet.Instance.IsTireeHasard = true;
                Chevalet.Instance.TuileTireeHasard = Tuile;
                Debug.Log($"La tuile jetee automatiquement a pour coord {Tuile.X} {Tuile.Y}");
                MainThreadDispatcher.Enqueue(() =>
                {
                    Chevalet.Instance.MoveFromChevaletToDefausse(Tuile);
                });
            }
        );

        this._hubConnection.On<PiocheInfosPacket>(
            "ReceivePiocheUpdate",
            (Pioche) =>
            {
                MainThreadDispatcher.Enqueue(() =>
                {
                    if (Pioche.PiocheTete != null)
                    {
                        var piocheCentale = Chevalet.PilePiochePlaceHolder.GetComponent<Tuile>();

                        if (piocheCentale.transform.childCount > 0)
                        {
                            for (int i = 0; i < piocheCentale.transform.childCount; i++)
                            {
                                Destroy(
                                    Chevalet.PilePiochePlaceHolder.transform.GetChild(i).gameObject
                                );
                            }
                        }

                        var piocheTuileData = new TuileData(
                            Chevalet.FromStringToCouleurTuile(Pioche.PiocheTete.Couleur),
                            int.Parse(Pioche.PiocheTete.numero),
                            Pioche.PiocheTete.Couleur != null
                                && (
                                    Pioche.PiocheTete.Couleur.Equals("M", StringComparison.Ordinal)
                                    || Pioche.PiocheTete.Couleur.Equals(
                                        "X",
                                        StringComparison.Ordinal
                                    )
                                )
                        );

                        piocheCentale.SetCouleur(piocheTuileData.couleur);
                        piocheCentale.SetValeur(piocheTuileData.num);

                        var childObject = new GameObject("SpriteChild");
                        childObject.transform.SetParent(Chevalet.PilePiochePlaceHolder.transform);
                        childObject.AddComponent<Tuile>();
                        childObject.GetComponent<Tuile>().SetCouleur(piocheTuileData.couleur);
                        childObject.GetComponent<Tuile>().SetValeur(piocheTuileData.num);
                        var spriteRen = childObject.AddComponent<SpriteRenderer>();
                        var mat = new Material(Shader.Find("Sprites/Default"))
                        {
                            color = new Color(
                                0.9529411764705882f,
                                0.9411764705882353f,
                                0.8156862745098039f
                            )
                        };
                        spriteRen.material = mat;
                        spriteRen.sprite = Chevalet.spritesDic["Pioche"];
                        spriteRen.sortingOrder = 3;
                        var transform1 = spriteRen.transform;
                        transform1.localPosition = new Vector3(0, 0, 0);
                        transform1.localScale = new Vector3(1, 1, 1);
                        var boxCollider2D = childObject.AddComponent<BoxCollider2D>();
                        boxCollider2D.size = new Vector2((float)0.875, (float)1.25);

                        //Chevalet
                        //    .PilePiochePlaceHolder.transform.GetChild(0)
                        //   .GetComponent<Tuile>()
                        //   .SetCouleur(piocheTuileData.couleur);
                        //Chevalet
                        //   .PilePiochePlaceHolder.transform.GetChild(0)
                        //    .GetComponent<Tuile>()
                        //    .SetValeur(piocheTuileData.num);

                        Chevalet.Instance._pilePioche.Push(piocheCentale);
                        Chevalet
                            .PilePiochePlaceHolder.transform.GetChild(0)
                            .gameObject.GetComponent<Tuile>()
                            .SetIsDeplacable(true);
                        Chevalet
                            .PilePiochePlaceHolder.transform.GetChild(0)
                            .gameObject.GetComponent<Tuile>()
                            .SetIsInPioche(true);
                    }
                });
            }
        );

        this._hubConnection.On<PacketSignal>(
            "JoinRoomFail",
            (signal) =>
            {
                Debug.LogError(signal.message);
            }
        );

        this._hubConnection.On<TuileStringPacket>(
            "ReceiveTuileCentre",
            (tuile) =>
            {
                MainThreadDispatcher.Enqueue(() =>
                {
                    Chevalet.Instance.SetTuileCentre(tuile);
                });
            }
        );

        this._hubConnection.On<TuileJeteePacket>(
            "ReceiveTuileJete",
            (tuile) =>
            {
                if (tuile.position == 0)
                {
                    MainThreadDispatcher.Enqueue(() =>
                    {
                        var tuileData = new TuileData(
                            tuile.Couleur,
                            int.Parse(tuile.numero),
                            tuile.Couleur.Equals("M", StringComparison.Ordinal)
                                || tuile.Couleur.Equals("X", StringComparison.Ordinal)
                        );
                        var childObject = new GameObject("SpriteChild");
                        childObject.transform.SetParent(Chevalet.PileGauchePlaceHolder.transform);
                        var spriteRen = childObject.AddComponent<SpriteRenderer>();
                        var mat = new Material(Shader.Find("Sprites/Default"))
                        {
                            color = new Color(
                                0.9529411764705882f,
                                0.9411764705882353f,
                                0.8156862745098039f
                            )
                        };
                        spriteRen.material = mat;
                        spriteRen.sprite = Chevalet.spritesDic[
                            Chevalet.FromTuileToSpriteName(tuileData)
                        ];
                        spriteRen.sortingOrder =
                            3 + Chevalet.PileGauchePlaceHolder.transform.childCount;
                        var transform1 = spriteRen.transform;
                        transform1.localPosition = new Vector3(0, 0, 0);
                        transform1.localScale = new Vector3(1, 1, 1);
                        childObject.AddComponent<Tuile>();
                        var boxCollider2D = childObject.AddComponent<BoxCollider2D>();
                        boxCollider2D.size = new Vector2((float)0.875, (float)1.25);

                        Chevalet
                            .PileGauchePlaceHolder.GetComponent<Tuile>()
                            .SetCouleur(tuileData.couleur);
                        Chevalet
                            .PileGauchePlaceHolder.GetComponent<Tuile>()
                            .SetValeur(tuileData.num);

                        Chevalet.Instance._pileGauche.Push(
                            Chevalet.PileGauchePlaceHolder.GetComponent<Tuile>()
                        );
                    });
                }
                else if (tuile.position == 1)
                {
                    MainThreadDispatcher.Enqueue(() =>
                    {
                        var tuileData = new TuileData(
                            tuile.Couleur,
                            int.Parse(tuile.numero),
                            tuile.Couleur.Equals("M", StringComparison.Ordinal)
                                || tuile.Couleur.Equals("X", StringComparison.Ordinal)
                        );
                        var spriteRen =
                            Chevalet.PileHautDroitePlaceHolder.GetComponent<SpriteRenderer>();
                        spriteRen.sprite = Chevalet.spritesDic[
                            Chevalet.FromTuileToSpriteName(tuileData)
                        ];
                    });
                }
                else
                {
                    MainThreadDispatcher.Enqueue(() =>
                    {
                        var tuileData = new TuileData(
                            tuile.Couleur,
                            int.Parse(tuile.numero),
                            tuile.Couleur.Equals("M", StringComparison.Ordinal)
                                || tuile.Couleur.Equals("X", StringComparison.Ordinal)
                        );
                        var spriteRen =
                            Chevalet.PileHautGauchePlaceHolder.GetComponent<SpriteRenderer>();
                        spriteRen.sprite = Chevalet.spritesDic[
                            Chevalet.FromTuileToSpriteName(tuileData)
                        ];
                    });
                }
            }
        );

        this._hubConnection.On<TuilePacket>(
            "FirstJeterActionRequest",
            () =>
            {
                var chevaletInstance = Chevalet.Instance;
                Debug.Log("Ok il faut jeter une tuile");
                var tuile = new TuilePacket
                {
                    X = "0",
                    Y = "0",
                    gagner = false
                };

                while (chevaletInstance.IsJete == false) { }
                Debug.Log("La tuile vient d'etre jetee");

                if (chevaletInstance.TuileJete != null)
                {
                    return chevaletInstance.TuileJete;
                }

                // add code here signal
                MainThreadDispatcher.Enqueue(() =>
                {
                    LobbyManager.Instance.SetMyTurn(false);
                });
                return tuile;
            }
        );

        this._hubConnection.On<TuilePacket>(
            "JeterRequest",
            () =>
            {
                var chevaletInstance = Chevalet.Instance;
                Debug.Log("Ok il faut jeter une tuile");
                var tuile = new TuilePacket
                {
                    X = "0",
                    Y = "0",
                    gagner = false
                };

                while (chevaletInstance.IsJete == false) { }
                Debug.Log("La tuile vient d'etre jetee");

                if (chevaletInstance.TuileJete != null)
                {
                    Debug.Log(
                        $"Ok on envoie un truc non null {chevaletInstance.TuileJete.Y} {chevaletInstance.TuileJete.X}, {chevaletInstance.TuileJete.gagner}"
                    );
                    return chevaletInstance.TuileJete;
                }

                // add code here signal
                // MainThreadDispatcher.Enqueue(() =>
                // {
                //     LobbyManager.Instance.SetMyTurn(false);
                //     PlateauSignals.Instance.mainPlayerTurnSignal.gameObject.SetActive(false);
                //     Timer.Instance.ResetTimer();
                // });
                return tuile;
            }
        );

        this._hubConnection.On<DefaussePacket>(
            "ReceiveListeDefausse",
            (Defausse) =>
            {
                MainThreadDispatcher.Enqueue(() =>
                {
                    var tuilesList = new List<TuileData>();

                    if (Defausse.Defausse != null)
                    {
                        foreach (var tuileData in Defausse.Defausse)
                        {
                            if (
                                !tuileData.Equals(
                                    "couleur=;num=;defausse=;dansPioche=;Nom=;",
                                    StringComparison.Ordinal
                                )
                            )
                            {
                                var keyValuePairs = tuileData.Split(';');
                                string couleur = null;
                                var num = 0;
                                string nom = null;

                                foreach (var pair in keyValuePairs)
                                {
                                    if (string.IsNullOrWhiteSpace(pair))
                                    {
                                        continue;
                                    }

                                    var parts = pair.Split('=');

                                    if (parts.Length != 2)
                                    {
                                        continue;
                                    }

                                    var key = parts[0].Trim();
                                    var value = parts[1].Trim();
                                    switch (key)
                                    {
                                        case "couleur":
                                            couleur = value;
                                            break;
                                        case "num":
                                            int.TryParse(value, out num);
                                            break;
                                        case "defausse":
                                            bool.TryParse(value, out _);
                                            break;
                                        case "dansPioche":
                                            bool.TryParse(value, out _);
                                            break;
                                        case "Nom":
                                            nom = value;
                                            break;
                                        default:
                                            // Handle unknown keys (if needed)
                                            break;
                                    }
                                }

                                CouleurTuile coul;
                                switch (couleur)
                                {
                                    case "J":
                                        coul = CouleurTuile.J;
                                        break;
                                    case "N":
                                        coul = CouleurTuile.N;
                                        break;
                                    case "R":
                                        coul = CouleurTuile.R;
                                        break;
                                    case "B":
                                        coul = CouleurTuile.B;
                                        break;
                                    case "M":
                                        coul = CouleurTuile.M;
                                        break;
                                    default:
                                        throw new Exception();
                                }
                                tuilesList.Add(
                                    new TuileData(
                                        coul,
                                        num,
                                        nom != null && nom.Equals("Jo", StringComparison.Ordinal)
                                    )
                                );
                            }
                            else
                            {
                                tuilesList.Add(null);
                            }
                        }

                        // Fonctionaliteé qui affiche les defausses, a implementer plus tard....
                    }
                });
            }
        );

        this._hubConnection.On(
            "TuilesDistribueesSignal",
            () =>
            {
                Debug.Log($"Les tuiles ont ete distribuees");
            }
        );

        this._hubConnection.On<PiochePacket>(
            "PiochePacketRequest",
            () =>
            {
                var chevaletInstance = Chevalet.Instance;
                Debug.Log("Ok il faut piocher une tuile");
                var tuile = new PiochePacket { Centre = true, Defausse = false };

                while (chevaletInstance.IsPiochee == false) { }
                Debug.Log("La tuile vient d'etre piochee");

                if (chevaletInstance.TuilePiochee != null)
                {
                    chevaletInstance.IsPiochee = false;
                    return chevaletInstance.TuilePiochee;
                }
                // add code here signal
                MainThreadDispatcher.Enqueue(() =>
                {
                    LobbyManager.Instance.SetMyTurn(false);
                });
                return tuile;
            }
        );

        this._hubConnection.On<ChevaletPacket>(
            "ReceiveChevalet",
            (chevalet) =>
            {
                var chevaletInstance = Chevalet.Instance;
                MainThreadDispatcher.Enqueue(() =>
                {
                    while (chevaletInstance == null)
                    {
                        chevaletInstance = Chevalet.Instance;
                    }

                    var tuilesData = new TuileData[2, 14];
                    var i = 0;
                    foreach (var tuileStr in chevalet.PremiereRangee)
                    {
                        if (
                            !tuileStr.Equals(
                                "couleur=;num=;defausse=;dansPioche=;Nom=;",
                                StringComparison.Ordinal
                            )
                        )
                        {
                            var keyValuePairs = tuileStr.Split(';');
                            string couleur = null;
                            var num = 0;
                            string nom = null;

                            foreach (var pair in keyValuePairs)
                            {
                                if (string.IsNullOrWhiteSpace(pair))
                                {
                                    continue;
                                }

                                var parts = pair.Split('=');

                                if (parts.Length != 2)
                                {
                                    continue;
                                }

                                var key = parts[0].Trim();
                                var value = parts[1].Trim();
                                switch (key)
                                {
                                    case "couleur":
                                        couleur = value;
                                        break;
                                    case "num":
                                        int.TryParse(value, out num);
                                        break;
                                    case "defausse":
                                        bool.TryParse(value, out _);
                                        break;
                                    case "dansPioche":
                                        bool.TryParse(value, out _);
                                        break;
                                    case "Nom":
                                        nom = value;
                                        break;
                                    default:
                                        // Handle unknown keys (if needed)
                                        break;
                                }
                            }

                            CouleurTuile coul;
                            switch (couleur)
                            {
                                case "J":
                                    coul = CouleurTuile.J;
                                    break;
                                case "N":
                                    coul = CouleurTuile.N;
                                    break;
                                case "R":
                                    coul = CouleurTuile.R;
                                    break;
                                case "B":
                                    coul = CouleurTuile.B;
                                    break;
                                case "M":
                                    coul = CouleurTuile.M;
                                    break;
                                default:
                                    throw new Exception();
                            }

                            tuilesData[0, i] = new TuileData(
                                coul,
                                num,
                                nom != null && nom.Equals("Jo", StringComparison.Ordinal)
                            );
                            i++;
                        }
                    }

                    i = 0;
                    foreach (var tuileStr in chevalet.SecondeRangee)
                    {
                        if (
                            !tuileStr.Equals(
                                "couleur=;num=;defausse=;dansPioche=;Nom=;",
                                StringComparison.Ordinal
                            )
                        )
                        {
                            var keyValuePairs = tuileStr.Split(';');

                            string couleur = null;
                            var num = 0;
                            string nom = null;

                            foreach (var pair in keyValuePairs)
                            {
                                if (string.IsNullOrWhiteSpace(pair))
                                {
                                    continue;
                                }

                                var parts = pair.Split('=');

                                if (parts.Length != 2)
                                {
                                    continue;
                                }

                                var key = parts[0].Trim();
                                var value = parts[1].Trim();
                                switch (key)
                                {
                                    case "couleur":
                                        couleur = value;
                                        break;
                                    case "num":
                                        int.TryParse(value, out num);
                                        break;
                                    case "defausse":
                                        bool.TryParse(value, out _);
                                        break;
                                    case "dansPioche":
                                        bool.TryParse(value, out _);
                                        break;
                                    case "Nom":
                                        nom = value;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            CouleurTuile coul;
                            switch (couleur)
                            {
                                case "J":
                                    coul = CouleurTuile.J;
                                    break;
                                case "N":
                                    coul = CouleurTuile.N;
                                    break;
                                case "R":
                                    coul = CouleurTuile.R;
                                    break;
                                case "B":
                                    coul = CouleurTuile.B;
                                    break;
                                case "M":
                                    coul = CouleurTuile.M;
                                    break;
                                default:
                                    throw new Exception();
                            }

                            tuilesData[1, i] = new TuileData(
                                coul,
                                num,
                                nom != null && nom.Equals("Jo", StringComparison.Ordinal)
                            );
                            i++;
                        }
                    }

                    if (Chevalet.neverReceivedChevalet)
                    {
                        chevaletInstance.SetTuiles(tuilesData);
                        chevaletInstance.TuilesPack = tuilesData;
                        chevaletInstance.Print2DMatrix();
                        chevaletInstance.Init();
                        Chevalet.neverReceivedChevalet = false;
                    }
                    else
                    {
                        chevaletInstance.TuilesPack = tuilesData;
                    }
                });
            }
        );
    }

    //public static void UpdateRoomDisplay(RoomsPacket roomsPacket)
    //{
    //    //if (PlayerInLobbyProgressScreen.Instance != null)
    //    //{
    //    //    PlayerInLobbyProgressScreen.Instance.SetRooms(roomsPacket);
    //    //}
    //    //else
    //    //{
    //    //    Debug.LogError("PlayerInLobbyProgressScreen instance is not found.");
    //    //}

    //    if (DisplayRooms.Instance != null)
    //    {
    //        DisplayRooms.Instance.changeLabel(roomsPacket);
    //    }
    //    else
    //    {
    //        Debug.LogError("DisplayRooms instance is not found.");
    //    }

    //}




    public async Task JoinRoom() // takes parameter roomName in future
    {
        if (
            this._hubConnection != null
            && this._hubConnection.State == HubConnectionState.Connected
        )
        {
            try
            {
                await this._hubConnection.SendAsync("LobbyConnection");
                MainThreadDispatcher.Enqueue(() =>
                {
                    UIManagerPFormulaire.Instance.showRooms.SetActive(false);
                    UIManagerPFormulaire.Instance.lobbyPlayerWaiting.SetActive(true);
                });
                Debug.Log($"Request to join room 1 sent.");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to join room 1: {ex.Message}");
            }
        }
        else
        {
            Debug.LogError("Cannot join room. Hub connection is not established.");
        }
    }

    public async void SendBoardState(TuileData[,] BoardState)
    {
        // Ensure the connection is open before attempting to send a message
        if (
            this._hubConnection != null
            && this._hubConnection.State == HubConnectionState.Connected
        )
        {
            try
            {
                await this._hubConnection.InvokeAsync("SendBoardState", BoardState);
                Debug.Log("Board state sent successfully.");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to send board state: {ex.Message}");
            }
        }
        else
        {
            Debug.LogError("Hub connection is not established.");
        }
    }

    private async void OnDestroy()
    {
        if (
            this._hubConnection != null
            && this._hubConnection.State == HubConnectionState.Connected
        )
        {
            await this._hubConnection.StopAsync();
            await this._hubConnection.DisposeAsync();
        }
    }

    // private IEnumerator ActivateShowRooms()
    // {
    //     yield return new WaitForEndOfFrame();

    //     if (UIManagerPFormulaire.Instance == null)
    //     {
    //         Debug.LogError("UIManagerPFormulaire instance is null.");
    //         yield break;
    //     }
    //     Debug.Log("setActiveShowRooms");
    //     UIManagerPFormulaire.Instance.setActiveShowRooms();
    // }
}
