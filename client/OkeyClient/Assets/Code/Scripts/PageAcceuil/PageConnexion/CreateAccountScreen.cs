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

    private int currentAvatarId = 0;

    [SerializeField]
    private GameObject avatar0;

    [SerializeField]
    private GameObject avatar1;

    [SerializeField]
    private GameObject avatar2;

    [SerializeField]
    private GameObject avatar3;

    private float scaleFactor = 1.3f;

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
        
        avatar0.transform.localScale *= scaleFactor;
    }

    void Update()
    {
        if (UIManager.singleton.language) { }
        else { }
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.collider.gameObject == avatar0.gameObject)
                {
                    OnAvatarButtonClick(0);
                }
                else if (hit.collider.gameObject == avatar1.gameObject)
                {
                    OnAvatarButtonClick(1);
                }
                else if (hit.collider.gameObject == avatar2.gameObject)
                {
                    OnAvatarButtonClick(2);
                }
                else if (hit.collider.gameObject == avatar3.gameObject)
                {
                    OnAvatarButtonClick(3);
                }
            }
        }
    }

    void OnAvatarButtonClick(int avatarId)
    {
        ResetAvatarView();
        currentAvatarId = avatarId;
        switch(currentAvatarId)
        {
            case 0:
                avatar0.transform.localScale *= scaleFactor;
                break;
            case 1:
                avatar1.transform.localScale *= scaleFactor;
                break;
            case 2:
                avatar2.transform.localScale *= scaleFactor;
                break;
            case 3:
                avatar3.transform.localScale *= scaleFactor;
                break;
        }
    }

    void ResetAvatarView()
    {
        switch(currentAvatarId)
        {
            case 0:
                avatar0.transform.localScale /= scaleFactor;
                break;
            case 1:
                avatar1.transform.localScale /= scaleFactor;
                break;
            case 2:
                avatar2.transform.localScale /= scaleFactor;
                break;
            case 3:
                avatar3.transform.localScale /= scaleFactor;
                break;
        }
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
    void onBackBtnClicked()
    {
        Panel.SetActive(false);
        logInScreen.SetActive(true);
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

    void OnInputChanged(string value)
    {
        if (value != null)
        {
            erreurTxt.gameObject.SetActive(false);
        }
    }
}
