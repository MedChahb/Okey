using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreateAccountScreen : MonoBehaviour
{
    public GameObject Panel;

    [SerializeField]
    private TMP_InputField Username;

    [SerializeField]
    private TMP_InputField Password;

    [SerializeField]
    private Button backButton;

    [SerializeField]
    private Button createAccount;

    [SerializeField]
    private JoueurManager manager;

    // Start is called before the first frame update
    void Start()
    {
        Password.contentType = TMP_InputField.ContentType.Password;

        // Ajoute un écouteur au bouton "Retour"
        backButton.onClick.AddListener(OnBackBtnClicked);

        createAccount.onClick.AddListener(OnCreateAccountClicked);
    }

    void OnCreateAccountClicked()
    {
        string username = Username.text.Trim();
        string password = Password.text.Trim();

        if (
            !string.IsNullOrEmpty(username)
            && !string.IsNullOrEmpty(password)
        )
        {
            manager.CreationCompteSelfJoueur(username, password);
            //mettre le Panel de connexion en off
            Panel.SetActive(false);
            Debug.LogWarning("Compte créé !");
            Debug.LogWarning(username);
            Debug.LogWarning(password);
        }
        else
        {
            Debug.LogWarning("Veuillez remplir tous les champs !");
        }
    }

    // Méthode pour charger la scène précédente
    private void OnBackBtnClicked()
    {
        Panel.gameObject.SetActive(false);
    }
}
