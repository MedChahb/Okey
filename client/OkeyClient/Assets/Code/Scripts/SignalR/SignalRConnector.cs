using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Code.Scripts.SignalR.Packets;
using Code.Scripts.SignalR.Packets.Emojis;
using Code.Scripts.SignalR.Packets.Rooms;
using LogiqueJeu.Joueur;
using Microsoft.AspNetCore.SignalR.Client;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SignalRConnector : MonoBehaviour
{
    private static HubConnection _hubConnection;

    public static SignalRConnector Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public async void InitializeConnection()
    {
        _hubConnection = new HubConnectionBuilder().WithUrl(Constants.SIGNALR_HUB_URL).Build();

        this.ConfigureHubEvents();

        try
        {
            await _hubConnection.StartAsync();
            Debug.Log("SignalR connection established.");
            LobbyManager.Instance.SetConnectionStatus(true);
        }
        catch (Exception ex)
        {
            Debug.LogError($"SignalR connection failed: {ex.Message}");
        }

        await this.JoinRoom();
    }

    public async void HubDisconnect()
    {
        MainThreadDispatcher.Enqueue(() =>
        {
            _hubConnection.StopAsync();
        });
        SceneManager.LoadScene("Acceuil");
    }

    private void ConfigureHubEvents()
    {
        _hubConnection.On<PacketSignal>(
            "ReceiveMessage",
            Message =>
            {
                Debug.Log(Message.message);
            }
        );

        _hubConnection.On<StartingGamePacket>(
            "StartGame",
            (players) =>
            {
                Debug.LogWarning("La partie peut commencer");

                foreach (var player in players.playersUsernames)
                {
                    Debug.LogWarning(player);
                }

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
                            string username = players.playersUsernames[i];
                            // Debug.LogWarning($"Player {i + 1}: {player}");

                            Debug.Log(
                                "Main player: "
                                    + _hubConnection.ConnectionId
                                    + " Player iteration: "
                                    + player
                            );
                            if (player == _hubConnection.ConnectionId.Trim().ToLower())
                            {
                                LobbyManager.mainPlayer = player;
                                LobbyManager.Instance.mainPlayerUsername = username;
                                Debug.Log("MainPlayer: " + player);
                            }
                            else
                            {
                                switch (otherPlayerIndex)
                                {
                                    case 0:
                                        LobbyManager.player2 = player;
                                        LobbyManager.Instance.player2Username = username;
                                        // Debug.Log($"Player 2 set to: {player}");
                                        break;
                                    case 1:
                                        LobbyManager.player3 = player;
                                        LobbyManager.Instance.player3Username = username;
                                        // Debug.Log($"Player 3 set to: {player}");
                                        break;
                                    case 2:
                                        LobbyManager.player4 = player;
                                        LobbyManager.Instance.player4Username = username;
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

        _hubConnection.On<TuilePiocheePacket>(
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

        _hubConnection.On<string>(
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

        _hubConnection.On(
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

        _hubConnection.On<TuilePacket>(
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

        _hubConnection.On<PiocheInfosPacket>(
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

                        Chevalet.Instance._pilePioche.Push(
                            Chevalet
                                .PilePiochePlaceHolder.transform.GetChild(0)
                                .GetComponent<Tuile>()
                        );

                        childObject.GetComponent<Tuile>().SetIsDeplacable(false);
                    }
                });
            }
        );

        _hubConnection.On<PacketSignal>(
            "JoinRoomFail",
            (signal) =>
            {
                Debug.LogError(signal.message);
            }
        );

        _hubConnection.On<TuileStringPacket>(
            "ReceiveTuileCentre",
            (tuile) =>
            {
                MainThreadDispatcher.Enqueue(() =>
                {
                    Chevalet.Instance.SetTuileCentre(tuile);
                    Chevalet
                        .JokerPlaceHolder.transform.GetChild(1)
                        .gameObject.GetComponent<Tuile>()
                        .SetIsDeplacable(false);
                });
            }
        );

        _hubConnection.On<TuileJeteePacket>(
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

                        for (
                            int i = 0;
                            i < Chevalet.PileGauchePlaceHolder.transform.childCount;
                            i++
                        )
                        {
                            Chevalet
                                .PileGauchePlaceHolder.transform.GetChild(i)
                                .GetComponent<Tuile>()
                                .SetIsDeplacable(false);
                        }
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

        _hubConnection.On<TuilePacket>(
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

        _hubConnection.On<TuilePacket>(
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

        _hubConnection.On<EmotePacket>(
            "ReceiveEmote",
            (packet) =>
            {
                Debug.Log($"On a recu l'emote de {packet.PlayerSource}");
                if (packet.PlayerSource != null && packet.EmoteValue != null)
                {
                    var src = packet.PlayerSource.Trim().ToLower();
                    Debug.Log("On rentre bien ici");
                    if (src.Equals(LobbyManager.player2, StringComparison.Ordinal))
                    {
                        Debug.Log("joueur a droite");
                        MainThreadDispatcher.Enqueue(() =>
                        {
                            Plateau2.Instance.DisplayEmote(2, (int)packet.EmoteValue);
                        });
                    }
                    else if (src.Equals(LobbyManager.player3, StringComparison.Ordinal))
                    {
                        Debug.Log("joueur en face");
                        MainThreadDispatcher.Enqueue(() =>
                        {
                            Plateau2.Instance.DisplayEmote(3, (int)packet.EmoteValue);
                        });
                    }
                    else if (src.Equals(LobbyManager.player4, StringComparison.Ordinal))
                    {
                        Debug.Log("joueur a gauche");
                        MainThreadDispatcher.Enqueue(() =>
                        {
                            Plateau2.Instance.DisplayEmote(4, (int)packet.EmoteValue);
                        });
                    }
                    else if (src.Equals(LobbyManager.mainPlayer, StringComparison.Ordinal))
                    {
                        Debug.Log($"{packet.PlayerSource} <-> {LobbyManager.mainPlayer}");
                    }
                    else
                    {
                        Debug.LogError("Impossible de detecter la source");
                        Debug.Log(LobbyManager.player2);
                        Debug.Log(LobbyManager.player3);
                        Debug.Log(LobbyManager.player4);
                        Debug.Log(LobbyManager.mainPlayer);
                    }
                }
                return Task.CompletedTask;
            }
        );

        _hubConnection.On<DefaussePacket>(
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

        _hubConnection.On<string>(
            "UsernameRequest",
            () =>
            {
                var user = JoueurManager.Instance;

                if (user.IsConnected)
                {
                    return user.GetSelfJoueur().NomUtilisateur;
                }
                return "Guest";
            }
        );

        _hubConnection.On(
            "TuilesDistribueesSignal",
            () =>
            {
                Debug.Log($"Les tuiles ont ete distribuees");
            }
        );

        _hubConnection.On<PiochePacket>(
            "PiochePacketRequest",
            () =>
            {
                var chevaletInstance = Chevalet.Instance;
                Debug.Log("Ok il faut piocher une tuile");

                // Set la pioche a piochable

                MainThreadDispatcher.Enqueue(() =>
                {
                    for (var i = 0; i < Chevalet.PilePiochePlaceHolder.transform.childCount; i++)
                    {
                        Chevalet
                            .PilePiochePlaceHolder.transform.GetChild(i)
                            .GetComponent<Tuile>()
                            .SetIsDeplacable(true);
                    }

                    if (Chevalet.PileGauchePlaceHolder.transform.childCount > 0)
                    {
                        Chevalet
                            .PileGauchePlaceHolder.transform.GetChild(0)
                            .GetComponent<Tuile>()
                            .SetIsDeplacable(true);
                        Chevalet
                            .PileGauchePlaceHolder.transform.GetComponent<Tuile>()
                            .SetIsDeplacable(true);
                    }

                    if (Chevalet.Instance._pileGauche.Count > 0)
                    {
                        Chevalet
                            .PilePiochePlaceHolder.transform.GetChild(0)
                            .gameObject.GetComponent<Tuile>()
                            .SetIsDeplacable(true);
                    }
                });

                var tuile = new PiochePacket { Centre = true, Defausse = false };

                while (Chevalet.IsPiochee == false) { }
                Debug.Log("La tuile vient d'etre piochee");
                if (chevaletInstance.TuilePiochee != null)
                {
                    Chevalet.IsPiochee = false;
                    MainThreadDispatcher.Enqueue(() =>
                    {
                        for (
                            var i = 0;
                            i < Chevalet.PilePiochePlaceHolder.transform.childCount;
                            i++
                        )
                        {
                            Chevalet
                                .PilePiochePlaceHolder.transform.GetChild(i)
                                .GetComponent<Tuile>()
                                .SetIsDeplacable(false);
                        }

                        if (Chevalet.PileGauchePlaceHolder.transform.childCount > 0)
                        {
                            Chevalet
                                .PileGauchePlaceHolder.transform.GetChild(0)
                                .GetComponent<Tuile>()
                                .SetIsDeplacable(false);
                            Chevalet
                                .PileGauchePlaceHolder.transform.GetComponent<Tuile>()
                                .SetIsDeplacable(false);
                        }

                        if (Chevalet.Instance._pileGauche.Count > 0)
                        {
                            if (Chevalet.PilePiochePlaceHolder.transform.childCount > 0)
                            {
                                Chevalet
                                    .PilePiochePlaceHolder.transform.GetChild(0)
                                    .gameObject.GetComponent<Tuile>()
                                    .SetIsDeplacable(false);
                            }
                        }
                    });
                    return chevaletInstance.TuilePiochee;
                }

                // add code here signal
                // MainThreadDispatcher.Enqueue(() =>
                // {
                //     LobbyManager.Instance.SetMyTurn(false);
                //     // Plateau
                // });
                return tuile;
            }
        );

        _hubConnection.On<RoomState>(
            "SendRoomState",
            (roomState) =>
            {
                if (roomState.playerDatas != null)
                {
                    this.UpdateWaitingPlayerUI(roomState.playerDatas);
                    foreach (var data in roomState.playerDatas)
                    {
                        var dSplit = data.Split(';');
                        Debug.LogWarning(
                            $"Username: {dSplit[0]}, Photo: {dSplit[1]}, Elo: {dSplit[2]}, Experience: {dSplit[3]}"
                        );
                    }
                }
            }
        );

        _hubConnection.On<ChevaletPacket>(
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

    private void UpdateWaitingPlayerUI(List<string> PlayerData)
    {
        MainThreadDispatcher.Enqueue(() =>
        {
            var vue = UIManagerPFormulaire.Instance.lobbyPlayerWaiting;

            Debug.Log(PlayerData.Count);

            var bg = vue.transform.GetChild(0);
            var header = bg.transform.GetChild(0);
            var txtCounter = header.transform.GetChild(2);
            txtCounter.transform.GetComponent<TextMeshProUGUI>().text = $"{PlayerData.Count}/4";

            var P1 = bg.transform.GetChild(1);
            var P2 = bg.transform.GetChild(2);
            var P3 = bg.transform.GetChild(3);
            var P4 = bg.transform.GetChild(4);

            var tabP = new List<Transform>();
            tabP.Add(P1);
            tabP.Add(P2);
            tabP.Add(P3);
            tabP.Add(P4);
            for (var i = 0; i < PlayerData.Count; i++)
            {
                var dSplit = PlayerData[i].Split(';');
                var username = tabP[i].transform.GetChild(1);
                var avatar = tabP[i].transform.GetChild(0);
                var elo = tabP[i].transform.GetChild(2);
                var experience = tabP[i].transform.GetChild(3);

                username.transform.GetComponent<TextMeshProUGUI>().text = dSplit[0];
                var res = Resources.Load<Sprite>("Avatar/avatarn" + dSplit[1]);
                avatar.transform.GetComponent<SpriteRenderer>().transform.localScale = new Vector3(
                    50f, //0.342f,
                    50f, //0.342f,
                    1f
                );
                avatar.transform.GetComponent<SpriteRenderer>().sortingOrder = 5;
                avatar.transform.GetComponent<SpriteRenderer>().sprite = res;
                if (dSplit[0].Equals("Guest", StringComparison.Ordinal))
                {
                    elo.transform.GetComponent<TextMeshProUGUI>().text = "";
                    experience.transform.GetComponent<TextMeshProUGUI>().text = "";
                }
                else
                {
                    elo.transform.GetComponent<TextMeshProUGUI>().text = "Elo Score: " + dSplit[2];
                    experience.transform.GetComponent<TextMeshProUGUI>().text =
                        "Niveau: " + dSplit[3];
                }
            }
        });
    }

    public async Task JoinRoom() // takes parameter roomName in future
    {
        if (_hubConnection != null && _hubConnection.State == HubConnectionState.Connected)
        {
            try
            {
                await _hubConnection.SendAsync("LobbyConnection");
                MainThreadDispatcher.Enqueue(() =>
                {
                    UIManagerPFormulaire.Instance.showRooms.SetActive(false);
                    UIManagerPFormulaire.Instance.lobbyPlayerWaiting.SetActive(true);

                    var vue = UIManagerPFormulaire.Instance.lobbyPlayerWaiting;
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

    public async void SendEmoji(int indexEmoji)
    {
        var emotePacket = new EmotePacket
        {
            EmoteValue = indexEmoji,
            PlayerSource = _hubConnection.ConnectionId
        };
        await _hubConnection.SendAsync("EnvoyerEmoteAll", emotePacket);
    }

    public async void SendBoardState(TuileData[,] BoardState)
    {
        // Ensure the connection is open before attempting to send a message
        if (_hubConnection != null && _hubConnection.State == HubConnectionState.Connected)
        {
            try
            {
                await _hubConnection.InvokeAsync("SendBoardState", BoardState);
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
        if (_hubConnection != null && _hubConnection.State == HubConnectionState.Connected)
        {
            await _hubConnection.StopAsync();
            await _hubConnection.DisposeAsync();
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
