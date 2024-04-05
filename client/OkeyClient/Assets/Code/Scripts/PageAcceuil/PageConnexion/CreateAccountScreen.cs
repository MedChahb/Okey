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
    private TMP_InputField Login;

    [SerializeField]
    private TMP_InputField Username;

    [SerializeField]
    private TMP_InputField Password;

    [SerializeField]
    private Button backButton;

    [SerializeField]
    private Button createAccount;

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
        string login = Login.text.Trim();
        string username = Username.text.Trim();
        string password = Password.text.Trim();

        if (
            !string.IsNullOrEmpty(login)
            && !string.IsNullOrEmpty(username)
            && !string.IsNullOrEmpty(password)
        )
        {
            // Envoi du formulaire (à implémenter)
            Debug.Log("Formulaire envoyé !");
            //mettre le Panel de connexion en off
            Panel.SetActive(false);
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
