using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManagerPAcceuil : MonoBehaviour
{
    [SerializeField]
    private Button playBtn;

    [SerializeField]
    private Button paramBtn;

    [SerializeField]
    private Button connexionBtn;

    [SerializeField]
    private TextMeshProUGUI connexionBtnTxt;

    [SerializeField]
    private TextMeshProUGUI playBtnTxt;

    [SerializeField]
    private int sceneId;

    [SerializeField]
    private ParametreScreen parametres;

    [SerializeField]
    private LogInScreen login;

    [SerializeField]
    public GameObject PanelAvatar;

    private bool connected = false;

    [SerializeField]
    public JoueurManager manager;

    [SerializeField]
    public GameObject rankingNotConnected;

    // Start is called before the first frame update
    void Start()
    {
        playBtn.onClick.AddListener(onPlayBtnClicked);
        paramBtn.onClick.AddListener(onSettingsClicked);
        connexionBtn.onClick.AddListener(onLoginClicked);
        manager.SelfJoueurChangeEvent.AddListener(updateAvatar);
    }

    // Update is called once per frame
    void Update()
    {
        if (UIManager.singleton.language)
        {
            playBtnTxt.text = "Play";
            if (!connected)
            {
                connexionBtn.gameObject.SetActive(true);
                connexionBtnTxt.text = "LogIn";
            }
        }
        else
        {
            playBtnTxt.text = "Jouer";
            if (!connected)
            {
                connexionBtn.gameObject.SetActive(true);
                connexionBtnTxt.text = "Connexion";
            }
        }
    }

    void onPlayBtnClicked()
    {
        SceneManager.LoadScene(sceneId);
        Debug.Log("Scene changed to " + sceneId);
    }

    void onSettingsClicked()
    {
        if (parametres.parametresImage != null)
        {
            // Toggle the active state of the image
            parametres.parametresImage.gameObject.SetActive(
                !parametres.parametresImage.gameObject.activeSelf
            );
        }
        else
        {
            Debug.LogWarning("Parametres Image reference is not set!");
        }
        rankingNotConnected.SetActive(false);
    }

    void onLoginClicked()
    {
        if (login.Panel != null)
        {
            // Toggle the active state of the image
            login.Panel.SetActive(!login.Panel.activeSelf);
        }
        else
        {
            Debug.LogWarning("Login Image reference is not set!");
        }
        rankingNotConnected.SetActive(false);
    }

    public void setConnected(bool isConnected)
    {
        this.connected = isConnected;
    }

    public void updateAvatar()
    {
        PanelAvatar.SetActive(true);
        PanelAvatar.GetComponentInChildren<TextMeshProUGUI>().text = manager
            .GetSelfJoueur()
            .NomUtilisateur;
        connexionBtn.gameObject.SetActive(false);
        rankingNotConnected.SetActive(false);
    }
}
