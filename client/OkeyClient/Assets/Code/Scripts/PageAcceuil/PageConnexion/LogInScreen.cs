using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogInScreen : MonoBehaviour
{
    public GameObject Panel;

    //public GameObject PanelAvatar;
    //public GameObject creationPanel;

    [SerializeField]
    private TMP_InputField Username;

    [SerializeField]
    private TMP_InputField Password;

    [SerializeField]
    private Button connexionButton;

    [SerializeField]
    private Button createButton;

    [SerializeField]
    private Button backButton;

    private JoueurManager manager;

    [SerializeField]
    private UIManagerPAcceuil accueil;

    [SerializeField]
    private TextMeshProUGUI erreurTxt;

    [SerializeField]
    private GameObject createAccountView;

    // Start is called before the first frame update
    void Start()
    {
        this.manager = JoueurManager.Instance;

        Password.contentType = TMP_InputField.ContentType.Password;
        // Ajoute un écouteur au bouton "Créer"
        connexionButton.onClick.AddListener(OnConnexionClicked);

        createButton.onClick.AddListener(OnCreateClicked);

        // Ajoute un écouteur au bouton "Retour"
        backButton.onClick.AddListener(onBackBtnClicked);

        Username.onValueChanged.AddListener(OnInputChanged);

        Password.onValueChanged.AddListener(OnInputChanged);

        erreurTxt.gameObject.SetActive(false);
    }

    // Méthode appelée lors du clic sur le bouton "Créer"
    void OnConnexionClicked()
    {
        string username = Username.text.Trim();
        string password = Password.text.Trim();

        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
        {
            manager.StartConnexionSelfJoueur(username, password, UpdateWithConnection);
        }
        else
        {
            erreurTxt.text = "Veuillez remplir tous les champs !";
            erreurTxt.gameObject.SetActive(true);
        }
    }

    void OnCreateClicked()
    {
        Panel.SetActive(false);
        createAccountView.SetActive(true);
    }

    // Méthode pour charger la scène précédente
    void onBackBtnClicked()
    {
        Panel.SetActive(false);
    }

    void Update()
    {
        if (UIManager.singleton.language) { }
        else { }
    }

    public void UpdateWithConnection(int code)
    {
        if (code == 200)
        {
            Panel.SetActive(false);
            accueil.setConnected(true);
        }
        else
        {
            erreurTxt.text = "Identifiant ou mot de passe invalide";
            erreurTxt.gameObject.SetActive(true);
        }
    }

    private void OnInputChanged(string value)
    {
        if (value != null)
        {
            erreurTxt.gameObject.SetActive(false);
        }
    }
}
