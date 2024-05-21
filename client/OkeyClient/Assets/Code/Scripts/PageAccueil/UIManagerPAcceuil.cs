using LogiqueJeu.Joueur;
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

    public TextMeshProUGUI parametreLabel;
    public TextMeshProUGUI classementLabel;
    public TextMeshProUGUI classementTitle;

    [SerializeField]
    private int sceneId;

    [SerializeField]
    private ParametreScreen parametres;

    [SerializeField]
    private LogInScreen login;

    [SerializeField]
    public GameObject PanelAvatar;

    [SerializeField]
    public GameObject PanelConnected;

    public JoueurManager manager;

    public GameObject rankingNotConnected;

    // Start is called before the first frame update
    void Start()
    {
        manager.SelfJoueurChangeEvent.AddListener(updateAvatar);
        manager.ConnexionChangeEvent.AddListener(updateConnexion);
        playBtn.onClick.AddListener(onPlayBtnClicked);
        paramBtn.onClick.AddListener(onSettingsClicked);
        connexionBtn.onClick.AddListener(onLoginClicked);
    }

    // Update is called once per frame
    void Update()
    {
        if (UIManager.singleton.language)
        {
            connexionBtnTxt.text = "Log In";
            playBtnTxt.text = "Play";
            parametreLabel.text = "Connect";
            classementLabel.text = "Log in to your account to view";
            classementTitle.text = "Player Rankings";
        }
        else
        {
            connexionBtnTxt.text = "Connexion";
            playBtnTxt.text = "Jouer";
            parametreLabel.text = "Paramètres";
            classementLabel.text = "Connectez vous à votre compte pour visualiser";
            classementTitle.text = "Classement Des Joueurs";
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
    }

    private void updateAvatar()
    {
        PanelConnected.SetActive(true);
        PanelAvatar.SetActive(true);
        PanelAvatar.GetComponentInChildren<TextMeshProUGUI>().text = manager
            .GetSelfJoueur()
            .NomUtilisateur;
        Sprite newSprite = Resources.Load<Sprite>("Avatar/avatarn4");
        switch ((int)manager.GetSelfJoueur().IconeProfil)
        {
            case (int)IconeProfil.Icone1:
                newSprite = Resources.Load<Sprite>("Avatar/avatarn1");
                break;
            case (int)IconeProfil.Icone2:
                newSprite = Resources.Load<Sprite>("Avatar/avatarn2");
                break;
            case (int)IconeProfil.Icone3:
                newSprite = Resources.Load<Sprite>("Avatar/avatarn3");
                break;
        }
        if (newSprite != null)
        {
            // Modification du sprite de l'Image
            PanelAvatar.GetComponentInChildren<Image>().sprite = newSprite;
        }
        else
        {
            Debug.LogWarning("Sprite introuvable ou Image non définie !");
        }
    }

    private void updateConnexion()
    {
        if (!this.manager.IsConnected)
        {
            connexionBtn.gameObject.SetActive(true);
            rankingNotConnected.SetActive(true);
            PanelAvatar.SetActive(false);
            PanelConnected.SetActive(false);
            connexionBtnTxt.text = UIManager.singleton.language ? "LogIn" : "Connexion";
        }
        else
        {
            connexionBtn.gameObject.SetActive(false);
            rankingNotConnected.SetActive(false);
            updateAvatar();
        }
    }
}
