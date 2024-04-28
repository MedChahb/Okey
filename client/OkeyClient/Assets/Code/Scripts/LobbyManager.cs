using System.Collections.Generic; // Add this line
using System.Linq;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager Instance;

    public SignalRConnector signalRConnector;

    //public RoomsPacket rooms;

    public bool connectionStarted = false;
    public bool rommsListFilled = false;
    public int playerCount;

    public bool myTurn = false;

    public string currentPlayerTurn;

    public List<string> players;
    public string player2;
    public string player3;
    public string player4;

    private void Awake() { }

    void Start()
    {
        Debug.Log("[LobbyManager] Awake called.");

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("[LobbyManager] Instance set and marked as DontDestroyOnLoad.");
        }
        else if (Instance != this)
        {
            Debug.LogWarning("[LobbyManager] Duplicate instance detected, destroying this one.");
            Destroy(gameObject);
        }
    }

    // SignalR connection.
    public void InitializeConnection()
    {
        Debug.Log("[LobbyManager] InitializeConnection called.");

        if (signalRConnector == null)
        {
            Debug.LogError("SignalRConnector is not set on LobbyManager");
            connectionStarted = false;
            return;
        }

        Debug.Log("Initializing Connection");
        signalRConnector.InitializeConnection();
    }

    public async void JoinRoom()
    {
        await signalRConnector.JoinRoom(); // Awaiting the async operation
    }

    public void SetConnectionStatus(bool value)
    {
        connectionStarted = value;
    }

    public void setRoomCount(int count)
    {
        playerCount = count;
        DisplayRooms.Instance.updateLabel();
    }

    public bool GetConnectedStatus()
    {
        return connectionStarted;
    }

    private void Update()
    {
        // Gameloop here
        // requests
        // all checks
    }

    public void SetMyTurn(bool value)
    {
        myTurn = value;
        PlateauSignals.Instance.SetMainPlayerTurnSignal();
    }

    public void SetPlayers(List<string> players)
    {
        if (players.Count >= 3)
        {
            this.players = players.ToList();
            player2 = players[0];
            player3 = players[1];
            player4 = players[2];
            Debug.Log(
                "Players set: "
                    + "player 2:  "
                    + player2
                    + ", "
                    + "player 3: "
                    + player3
                    + ", "
                    + "player 4: "
                    + player4
            );
        }
        else
        {
            Debug.LogError(
                "Insufficient players provided. Expected at least 3, received " + players.Count
            );
        }
    }

    public void SetCurrentPlayerTurn(string currentPlayerTurn)
    {
        this.currentPlayerTurn = currentPlayerTurn;
        PlateauSignals.Instance.SetPlayerSignal(currentPlayerTurn);
    }
}
