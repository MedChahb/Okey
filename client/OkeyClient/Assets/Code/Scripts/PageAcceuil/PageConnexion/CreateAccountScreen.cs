using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreatAccountScreen : MonoBehaviour
{
    public GameObject Panel;

    //public GameObject PanelAvatar;
    //public GameObject creationPanel;

    [SerializeField]
    private TMP_InputField Username;

    [SerializeField]
    private TMP_InputField Password;

    [SerializeField]
    private TMP_InputField PasswordValidation;

    [SerializeField]
    private TMP_InputField DayInput;

    [SerializeField]
    private TMP_InputField MonthInput;

    [SerializeField]
    private TMP_InputField YearInput;

    [SerializeField]
    private Button createButton;

    [SerializeField]
    private Button connectionButton;

    [SerializeField]
    private Button backButton;

    private JoueurManager manager;

    [SerializeField]
    private UIManagerPAcceuil accueil;

    [SerializeField]
    private GameObject logInScreen;

    [SerializeField]
    private TextMeshProUGUI erreurTxt;

    // Start is called before the first frame update
    void Start()
    {
        this.manager = JoueurManager.Instance;

        Password.contentType = TMP_InputField.ContentType.Password;

        PasswordValidation.contentType = TMP_InputField.ContentType.Password;

        createButton.onClick.AddListener(OnCreateClicked);

        connectionButton.onClick.AddListener(onBackBtnClicked);

        // Ajoute un écouteur au bouton "Retour"
        backButton.onClick.AddListener(onBackBtnClicked);

        Username.onValueChanged.AddListener(OnInputChanged);

        Password.onValueChanged.AddListener(OnInputChanged);

        PasswordValidation.onValueChanged.AddListener(OnInputChanged);

        erreurTxt.gameObject.SetActive(false);
    }

    void OnCreateClicked()
    {
        string username = Username.text.Trim();
        string password = Password.text.Trim();
        string passwordValidation = PasswordValidation.text.Trim();

        if (
            !string.IsNullOrEmpty(username)
            && !string.IsNullOrEmpty(password)
            && !string.IsNullOrEmpty(passwordValidation)
        )
        {
            manager.StartCreationCompteSelfJoueur(username, password, UpdateWithConnection);
        }
        else
        {
            erreurTxt.text = "Veuillez remplir tous les champs !";
            erreurTxt.gameObject.SetActive(true);
        }
    }

    // Méthode pour charger la scène précédente
    private void onBackBtnClicked()
    {
        Panel.SetActive(false);
        logInScreen.SetActive(true);
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
            erreurTxt.text = "La création de votre compte a échouée";
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
