using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PageConnexion : MonoBehaviour
{
    public Image connexionImage;
    [SerializeField]
    private InputField Username;
    [SerializeField]
    private InputField Login;
    [SerializeField]
    private InputField Password;
    [SerializeField]
    private Button createButton;
    [SerializeField]
    private Button backButton;

    // Start is called before the first frame update
    void Start()
    {
        Password.contentType = InputField.ContentType.Password;
        // Ajoute un écouteur au bouton "Créer"
        createButton.onClick.AddListener(OnCreateClicked);

        // Ajoute un écouteur au bouton "Retour"
        backButton.onClick.AddListener(onBackBtnClicked);
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
    private void onBackBtnClicked()
    {
        connexionImage.gameObject.SetActive(false);
    }
}
