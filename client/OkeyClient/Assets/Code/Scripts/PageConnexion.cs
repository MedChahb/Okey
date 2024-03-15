using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PageConnexion : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField Username;
    [SerializeField]
    private TMP_InputField Login;
    [SerializeField]
    private TMP_InputField Password;
    [SerializeField]
    private Button createButton;
    [SerializeField]
    private Button backButton;

    // Start is called before the first frame update
    void Start()
    {
        // Ajoute un écouteur au bouton "Créer"
        createButton.onClick.AddListener(OnCreateClicked);

        // Ajoute un écouteur au bouton "Retour"
        backButton.onClick.AddListener(LoadPreviousScene);
    }

    // Méthode appelée lors du clic sur le bouton "Créer"
    void OnCreateClicked()
    {
        string username = Username.text.Trim();
        string login = Login.text.Trim();
        // Mdp à hasher avant envoi
        string password = Password.text.Trim();

        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(login) && !string.IsNullOrEmpty(password))
        {
            // Envoi du formulaire (à implémenter)
            Debug.Log("Formulaire envoyé !");
        }
        else
        {
            Debug.LogWarning("Veuillez remplir tous les champs !");
        }
    }

    // Méthode pour charger la scène précédente
    public void LoadPreviousScene()
    {
        // Charge la scène précédente
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
