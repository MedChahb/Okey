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
}
