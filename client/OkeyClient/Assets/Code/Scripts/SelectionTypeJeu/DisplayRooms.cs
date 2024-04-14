using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DisplayRooms : MonoBehaviour
{
    public static DisplayRooms Instance { get; private set; }

    [SerializeField]
    private TextMeshProUGUI lobbyCountLabel;

    public GameObject playerNumberCounter;

    [SerializeField]
    private Button room1;

    private int numberOfUsersInFirstRoom;
    RoomsPacket rooms;

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
        room1.onClick.AddListener(() => onJoinRoomPressed());
    }

    public void onJoinRoomPressed()
    {
        if (LobbyManager.Instance == null)
        {
            Debug.LogError("LobbyManager instance is null.");
            return;
        }

        LobbyManager.Instance.JoinRoom();
        playerNumberCounter.SetActive(true);
    }

    void UpdateLobbyCountDisplay()
    {
        if (rooms != null && rooms.listRooms.Count > 0)
        {
            RoomDto firstRoom = rooms.listRooms[0];
            if (firstRoom != null && firstRoom.Players != null)
            {
                numberOfUsersInFirstRoom = firstRoom.Players.Count;
                Debug.Log("number of users in lobby ----> " + numberOfUsersInFirstRoom);
                lobbyCountLabel.text = numberOfUsersInFirstRoom.ToString() + "/4";
            }
            else
            {
                lobbyCountLabel.text = "0/4"; // Default or error text if no players
            }
        }
        else
        {
            Debug.Log("Room data unavailable");
        }
    }

    public void SetRooms(RoomsPacket newRooms)
    {
        Debug.Log("SetRooms called.");
        this.rooms = newRooms;
        this.UpdateLobbyCountDisplay();
    }

    void Update()
    {
        //UpdateLobbyCountDisplay();
    }
}
