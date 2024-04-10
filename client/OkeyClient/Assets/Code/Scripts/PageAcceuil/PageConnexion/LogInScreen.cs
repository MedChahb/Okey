using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    [SerializeField]
    private JoueurManager manager;

    [SerializeField]
    private UIManagerPAcceuil accueil;

    [SerializeField]
    private TextMeshProUGUI erreurTxt;

    // Start is called before the first frame update
    void Start()
    {
        Password.contentType = TMP_InputField.ContentType.Password;
        // Ajoute un écouteur au bouton "Créer"
        connexionButton.onClick.AddListener(OnConnexionClicked);

        createButton.onClick.AddListener(OnCreateClicked);

        // Ajoute un écouteur au bouton "Retour"
        backButton.onClick.AddListener(onBackBtnClicked);
        erreurTxt.gameObject.SetActive(false);
    }

    // Méthode appelée lors du clic sur le bouton "Créer"
    void OnConnexionClicked()
    {
        string username = Username.text.Trim();
        string password = Password.text.Trim();

        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
        {
            //manager.ConnexionSelfJoueur(username, password);
            //mettre le Panel de connexion en off
            Panel.SetActive(false);

            //activer le Panel de User avec avatar
            //PanelAvatar.SetActive(true);
            accueil.setConnected(true);
        }
        else
        {
            erreurTxt.text = "Veuillez remplir tous les champs !";
            erreurTxt.gameObject.SetActive(true);
        }
    }

    void OnCreateClicked()
    {
        string username = Username.text.Trim();
        string password = Password.text.Trim();

        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
        {
            //manager.CreationCompteSelfJoueur(username, password);
            //mettre le Panel de connexion en off
            Panel.SetActive(false);
            accueil.setConnected(true);
            //Debug.LogWarning("Compte créé !");
            //Debug.LogWarning(username);
            //Debug.LogWarning(password);
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
    }

    void Update()
    {
        if (GameManager.singleton.language) { }
        else { }
    }
}
