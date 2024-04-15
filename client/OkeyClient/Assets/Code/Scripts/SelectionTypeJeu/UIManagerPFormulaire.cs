using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManagerPFormulaire : MonoBehaviour
{
    public static UIManagerPFormulaire Instance { get; private set; }

    [SerializeField]
    private Button backBtn;

    [SerializeField]
    private Button joinLobbyBtn;

    [SerializeField]
    private TextMeshProUGUI partieSimpleTxt;

    [SerializeField]
    private GameObject showRooms;

    [SerializeField]
    private int SceneId;

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
        backBtn.onClick.AddListener(onBackBtnClicked);
        joinLobbyBtn.onClick.AddListener(onJoinLobbyButtonPressed);

        showRooms.SetActive(false);
    }

    void Update()
    {
        if (UIManager.singleton.language) // En
        {
            partieSimpleTxt.text = "Texte en anglais";
        }
        else // Fr
        {
            partieSimpleTxt.text = "Partie Simple";
        }

        if (LobbyManager.Instance.GetConnectedStatus())
        {
            showRooms.SetActive(true);
        }
    }

    void onBackBtnClicked()
    {
        SceneManager.LoadScene(SceneId);
    }

    public void onLoadBtnClicked()
    {
        SceneManager.LoadScene(2); // Charge la scène `PlateauInit`
    }

    public void onJoinLobbyButtonPressed()
    {
        if (LobbyManager.Instance == null)
        {
            Debug.LogError("LobbyManager instance is null.");
            return;
        }
        LobbyManager.Instance.InitializeConnection();
    }

    //public void UpdateLobbyCountDisplay(RoomsPacket rooms)
    //{
    //    buttonLabel.text = LobbyManager.Instance.playerCount.ToString();
    //    //if (rooms != null && rooms.listRooms.Count > 0)
    //    //{
    //    //    RoomDto firstRoom = rooms.listRooms[0];
    //    //    if (firstRoom != null && firstRoom.Players != null)
    //    //    {
    //    //        numberOfUsersInFirstRoom = firstRoom.Players.Count;
    //    //        buttonLabel.text = "Room1: " + numberOfUsersInFirstRoom.ToString() + "/4";
    //    //        //buttonLabel.text = DisplayRooms.Instance.messageLogs.Count.ToString();
    //    //    }
    //    //    else
    //    //    {
    //    //        buttonLabel.text = "No Players"; // Default or error text if no players
    //    //    }
    //    //}
    //    //else
    //    //{
    //    //    Debug.Log("Room data unavailable");
    //    //}
    //    //Debug.Log("Finished updating lobby count");
    //}



    //public void setRooms(RoomsPacket roomsPacket)
    //{
    //    Debug.Log("here");
    //    rooms = roomsPacket;
    //    UpdateLobbyCountDisplay(rooms);
    //}


    //public void changeLabel(RoomsPacket rooms)
    //{
    //    buttonLabel.text = $"Rejoindre Room 1 {rooms.listRooms[0].Players.Count}/{rooms.listRooms[0].Capacity}";

    //}
}
