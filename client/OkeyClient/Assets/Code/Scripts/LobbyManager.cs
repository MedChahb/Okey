using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager Instance;

    // Ref SignalRConnector component
    public SignalRConnector signalRConnector;

    public bool connectionStarted = false;

    private void Awake()
    {
        //Debug.Log("[LobbyManager] Awake called.");

        //if (Instance == null)
        //{
        //    Instance = this;
        //    DontDestroyOnLoad(gameObject);
        //    Debug.Log("[LobbyManager] Instance set and marked as DontDestroyOnLoad.");
        //}
        //else if (Instance != this)
        //{
        //    Debug.LogWarning("[LobbyManager] Duplicate instance detected, destroying this one.");
        //    Destroy(gameObject);
        //}
    }

    // Start is called before the first frame update
    void Start()
    {
        // You might still want to keep Start() for other initializations
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

    public void JoinRoom()
    {
        if (signalRConnector == null)
        {
            Debug.LogError("SignalRConnector is not set on LobbyManager");
            connectionStarted = false;
            return;
        }
        Debug.Log("Joining Room");
        signalRConnector.JoinRoom("ddsss");
    }

    public void SetConnectionStatus(bool value)
    {
        connectionStarted = value;
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
