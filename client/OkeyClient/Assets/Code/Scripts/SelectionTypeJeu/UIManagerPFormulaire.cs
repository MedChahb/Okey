using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManagerPFormulaire : MonoBehaviour
{
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

    // Start is called before the first frame update
    void Start()
    {
        backBtn.onClick.AddListener(onBackBtnClicked);
        joinLobbyBtn.onClick.AddListener(onJoinLobbyButtonPressed);
    }

    // Update is called once per frame
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

    public void onJoinLobbyButtonPressed()
    {
        if (LobbyManager.Instance == null)
        {
            Debug.LogError("LobbyManager instance is null.");
            return;
        }
        LobbyManager.Instance.InitializeConnection();
    }
}
