using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using LogiqueJeu.Joueur;
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

    private int currentAvatarId = 1;

    [SerializeField]
    private GameObject avatar0;

    [SerializeField]
    private GameObject avatar1;

    [SerializeField]
    private GameObject avatar2;

    [SerializeField]
    private GameObject avatar3;

    private float scaleFactor = 1.3f;

    public const int MAX_REQUEST_RETRIES = 5; // Superieur ou égale à 1
    public const int REQUEST_RETRY_DELAY = 1000; // En milisecondes
    private readonly CancellationTokenSource Source = new();

    // Start is called before the first frame update
    void Start()
    {
        this.manager = JoueurManager.Instance;

        Password.contentType = TMP_InputField.ContentType.Password;

        PasswordValidation.contentType = TMP_InputField.ContentType.Password;

        Password.text = string.Empty;

        Username.text = string.Empty;

        PasswordValidation.text = string.Empty;

        DayInput.text = string.Empty;

        MonthInput.text = string.Empty;

        YearInput.text = string.Empty;

        createButton.onClick.AddListener(OnCreateClicked);

        connectionButton.onClick.AddListener(onBackBtnClicked);

        // Ajoute un écouteur au bouton "Retour"
        backButton.onClick.AddListener(onBackBtnClicked);

        Username.onValueChanged.AddListener(OnInputChanged);

        Password.onValueChanged.AddListener(OnInputChanged);

        PasswordValidation.onValueChanged.AddListener(OnInputChanged);

        this.manager.ConnexionChangeEvent.AddListener(() => Panel.SetActive(false));

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
                    OnAvatarButtonClick(1);
                }
                else if (hit.collider.gameObject == avatar1.gameObject)
                {
                    OnAvatarButtonClick(2);
                }
                else if (hit.collider.gameObject == avatar2.gameObject)
                {
                    OnAvatarButtonClick(3);
                }
                else if (hit.collider.gameObject == avatar3.gameObject)
                {
                    OnAvatarButtonClick(4);
                }
            }
        }
    }

    void OnAvatarButtonClick(int avatarId)
    {
        ResetAvatarView();
        currentAvatarId = avatarId;
        switch (currentAvatarId)
        {
            case 1:
                avatar0.transform.localScale *= scaleFactor;
                break;
            case 2:
                avatar1.transform.localScale *= scaleFactor;
                break;
            case 3:
                avatar2.transform.localScale *= scaleFactor;
                break;
            case 4:
                avatar3.transform.localScale *= scaleFactor;
                break;
        }
    }

    void ResetAvatarView()
    {
        switch (currentAvatarId)
        {
            case 1:
                avatar0.transform.localScale /= scaleFactor;
                break;
            case 2:
                avatar1.transform.localScale /= scaleFactor;
                break;
            case 3:
                avatar2.transform.localScale /= scaleFactor;
                break;
            case 4:
                avatar3.transform.localScale /= scaleFactor;
                break;
        }
    }

    async void OnCreateClicked()
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
            await this.UpdateWithConnection(
                username,
                password,
                (IconeProfil)(this.currentAvatarId)
            );
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

    public async Task UpdateWithConnection(string username, string password, IconeProfil icone)
    {
        for (var i = 0; i < MAX_REQUEST_RETRIES; i++)
        {
            try
            {
                await this.manager.CreationCompteSelfJoueur(
                    username,
                    password,
                    icone,
                    this.Source.Token
                );
                return;
            }
            catch (HttpRequestException e)
            {
                var Code = e.GetStatusCode();
                if (Code != null)
                {
                    Debug.Log("Request error with response code " + Code);
                    erreurTxt.text = "La création de votre compte a échouée";
                    erreurTxt.gameObject.SetActive(true);
                    return;
                }
                else
                {
                    Debug.Log("Network error");
                    if (i != MAX_REQUEST_RETRIES - 1)
                    {
                        Debug.Log("Retrying...");
                        try
                        {
                            await Task.Delay(REQUEST_RETRY_DELAY, this.Source.Token);
                        }
                        catch (OperationCanceledException)
                        {
                            Debug.Log("Request cancelled.");
                            return;
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Request cancelled.");
                return;
            }
        }
        erreurTxt.text = "Erreur réseau, votre Internet marche bien ?";
        erreurTxt.gameObject.SetActive(true);
    }

    void OnInputChanged(string value)
    {
        if (value != null)
        {
            erreurTxt.gameObject.SetActive(false);
        }
    }
}
