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

    [SerializeField]
    private int sceneId;

    [SerializeField]
    private ParametreScreen parametres;

    [SerializeField]
    private LogInScreen login;

    public GameObject PanelAvatar;

    public JoueurManager manager;

    public GameObject rankingNotConnected;

    // Start is called before the first frame update
    void Start()
    {
        playBtn.onClick.AddListener(onPlayBtnClicked);
        paramBtn.onClick.AddListener(onSettingsClicked);
        connexionBtn.onClick.AddListener(onLoginClicked);
        manager.SelfJoueurChangeEvent.AddListener(updateAvatar);
        manager.ConnexionChangeEvent.AddListener(updateConnexion);
    }

    // Update is called once per frame
    void Update()
    {
        if (UIManager.singleton.language)
        {
            playBtnTxt.text = "Play";
        }
        else
        {
            playBtnTxt.text = "Jouer";
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

    private void updateAvatar()
    {
        PanelAvatar.SetActive(true);
        PanelAvatar.GetComponentInChildren<TextMeshProUGUI>().text = manager
            .GetSelfJoueur()
            .NomUtilisateur;
        switch (manager.GetSelfJoueur().IconeProfil)
        {
            case IconeProfil.Icone1:
                //Màj l'image avec l'avatar 1
                break;
            case IconeProfil.Icone2:
                //Màj l'image avec l'avatar 2
                break;
            case IconeProfil.Icone3:
                //Màj l'image avec l'avatar 3
                break;
            case IconeProfil.Icone4:
                //Màj l'image avec l'avatar 4
                break;
        }
        connexionBtn.gameObject.SetActive(false);
        rankingNotConnected.SetActive(false);
    }

    private void updateConnexion()
    {
        if (!this.manager.IsConnected)
        {
            connexionBtn.gameObject.SetActive(true);
            connexionBtnTxt.text = UIManager.singleton.language ? "LogIn" : "Connexion";
        }
    }
}
