using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class DisplayRooms : MonoBehaviour
{
    public static DisplayRooms Instance { get; private set; }

    [SerializeField]
    private TextMeshProUGUI lobbyCountLabel;

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

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame

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
