using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DisplayRooms : MonoBehaviour
{
    public static DisplayRooms Instance { get; private set; }

    [SerializeField]
    private Button room1;

    [SerializeField]
    private Button CreateAndJoin;

    public TextMeshProUGUI label;
    public List<string> messageLogs;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        room1.onClick.AddListener(() => this.onJoinRoomPressed());
        this.CreateAndJoin.onClick.AddListener(() => this.onCreateAndJoinPrivateRoom());
    }

    public void onJoinRoomPressed()
    {
        if (LobbyManager.Instance == null)
        {
            Debug.LogError("LobbyManager instance is null.");
            return;
        }
        LobbyManager.Instance.JoinRoom();
    }

    public void onCreateAndJoinPrivateRoom()
    {
        if (LobbyManager.Instance == null)
        {
            Debug.LogError("LobbyManager instance is null.");
            return;
        }
        LobbyManager.Instance.JoinPrivateRoom();
    }

    public void updateLabel()
    {
        label.text = LobbyManager.Instance.playerCount.ToString() + "/4";
        Debug.Log("Player Count: " + LobbyManager.Instance.playerCount);
    }

    void Update()
    {
        //UpdateLobbyCountDisplay();
    }
}
