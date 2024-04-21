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
    private Button backBtnPublic;

    [SerializeField]
    private Button backBtnPrivate;

    [SerializeField]
    private Button joinPrivateLobbyBtn;

    [SerializeField]
    private Button joinPublicLobbyBtn;

    [SerializeField]
    private TextMeshProUGUI partieSimpleTxt;

    [SerializeField]
    private GameObject showRooms;

    [SerializeField]
    private GameObject lobbyPrivateConfig;

    [SerializeField]
    private GameObject lobbyPlayerWaiting;

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
        backBtnPrivate.onClick.AddListener(onBackPrivateLobbyClicked);
        backBtnPublic.onClick.AddListener(onBackPublicLobbyClicked);
        backBtn.onClick.AddListener(onBackBtnClicked);
        //joinLobbyBtn.onClick.AddListener(onJoinLobbyButtonPressed);
        joinPublicLobbyBtn.onClick.AddListener(onPublicLobbyClicked);
        joinPrivateLobbyBtn.onClick.AddListener(onPrivateLobbyClicked);

        showRooms.SetActive(false);
        lobbyPlayerWaiting.SetActive(false);
        lobbyPrivateConfig.SetActive(false);
    }

    void Update()
    {
        /*if (UIManager.singleton.language) // En
        {
            partieSimpleTxt.text = "Texte en anglais";
        }
        else // Fr
        {
            partieSimpleTxt.text = "Lobby Public";
        }

        if (LobbyManager.Instance.GetConnectedStatus())
        {
            showRooms.SetActive(true);
        }*/
    }

    void onBackBtnClicked()
    {
        SceneManager.LoadScene(SceneId);
    }

    public void onPublicLobbyClicked()
    {
        lobbyPlayerWaiting.SetActive(true);
    }

    public void onPrivateLobbyClicked()
    {
        lobbyPrivateConfig.SetActive(true);
    }

    public void onLoadBtnClicked()
    {
        SceneManager.LoadScene(2); // Charge la scène `PlateauInit`
    }

    public void onBackPrivateLobbyClicked()
    {
        lobbyPrivateConfig.SetActive(false);
    }

    public void onBackPublicLobbyClicked()
    {
        lobbyPlayerWaiting.SetActive(false);
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
