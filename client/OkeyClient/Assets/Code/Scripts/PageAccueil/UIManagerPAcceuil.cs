using System.Collections;
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
    private Button tutorialBtn;

    [SerializeField]
    private Button paramBtn;

    [SerializeField]
    private Button connexionBtn;

    [SerializeField]
    private TextMeshProUGUI connexionBtnTxt;

    [SerializeField]
    private TextMeshProUGUI playBtnTxt;

    public TextMeshProUGUI parametreLabel;
    public TextMeshProUGUI classementLabel;
    public TextMeshProUGUI classementTitle;
    public TextMeshProUGUI classementTitleAfterLogin;
    public TextMeshProUGUI classementTitlePage;

    public TextMeshProUGUI nomUtilisateurLoginHint;
    public TextMeshProUGUI motDePasseLoginHint;

    public TextMeshProUGUI niveau;
    public TextMeshProUGUI statistiques;
    public TextMeshProUGUI tempsTotaldeJeu;
    public TextMeshProUGUI partiePerdues;
    public TextMeshProUGUI partieGagnees;
    public TextMeshProUGUI classement;

    public TextMeshProUGUI deconnexionBtnLabel;
    public TextMeshProUGUI changerAvatarLabel;

    public TextMeshProUGUI createAccountTitle;

    public TextMeshProUGUI nom;
    public TextMeshProUGUI nomHintText;

    public TextMeshProUGUI prenom;
    public TextMeshProUGUI prenomHintText;

    public TextMeshProUGUI password;
    public TextMeshProUGUI passwordHintText;

    public TextMeshProUGUI confirmPassword;
    public TextMeshProUGUI confirmPasswordHintText;

    public TextMeshProUGUI usernameHintText;

    public TextMeshProUGUI dateNaissance;
    public TextMeshProUGUI JJ;
    public TextMeshProUGUI MM;
    public TextMeshProUGUI AAAA;

    public TextMeshProUGUI creeUnCompte;
    public TextMeshProUGUI seConnecter;

    [SerializeField]
    private int sceneId;

    [SerializeField]
    private ParametreScreen parametres;

    [SerializeField]
    private LogInScreen login;

    [SerializeField]
    public GameObject PanelAvatar;

    [SerializeField]
    public GameObject PanelConnected;

    public JoueurManager manager;

    public GameObject rankingNotConnected;

    private float updateInterval = 1.0f;

    public AudioSource audioSourceAmbience;
    public AudioSource audioSourceSoundEffects;

    void Start()
    {
        audioSourceAmbience.volume = UIManager.singleton.backgroundMusic;
        audioSourceSoundEffects.volume = UIManager.singleton.soundEffects;
        manager.SelfJoueurChangeEvent.AddListener(updateConnexion);
        manager.ConnexionChangeEvent.AddListener(updateConnexion);
        playBtn.onClick.AddListener(onPlayBtnClicked);
        paramBtn.onClick.AddListener(onSettingsClicked);
        connexionBtn.onClick.AddListener(onLoginClicked);
        tutorialBtn.onClick.AddListener(onTutoClicked);
        updateConnexion();
        StartCoroutine(UpdateConnexionRoutine());
    }

    private IEnumerator UpdateConnexionRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(updateInterval);
            updateConnexion();
        }
    }

    void Update()
    {
        if (UIManager.singleton.language)
        {
            connexionBtnTxt.text = "Log In";
            playBtnTxt.text = "Play";
            parametreLabel.text = "Settings";
            classementLabel.text = "Log in to your account to view";
            classementTitle.text = "Player Rankings";
            classementTitleAfterLogin.text = "Player Rankings";
            classementTitleAfterLogin.text = "Player Rankings";
            nom.text = "Last Name";
            nomHintText.text = "Enter your last name";
            prenom.text = "First Name";
            prenomHintText.text = "Enter your first name";
            password.text = "Password";
            passwordHintText.text = "Enter your password";
            confirmPassword.text = "Confirm Password";
            confirmPasswordHintText.text = "Confirm your password";
            usernameHintText.text = "Enter your username";
            dateNaissance.text = "Date of Birth";
            JJ.text = "DD";
            MM.text = "MM";
            AAAA.text = "YYYY";
            creeUnCompte.text = "Create Account";
            seConnecter.text = "Log In";
            createAccountTitle.text = "Create an account";
            classementTitlePage.text = "Player Rankings";
            niveau.text = "Level";
            statistiques.text = "Statistics";
            tempsTotaldeJeu.text = "Total play time";
            partiePerdues.text = "Lost games";
            partieGagnees.text = "Won games";
            classement.text = "Ranking";
            deconnexionBtnLabel.text = "Log Out";
            changerAvatarLabel.text = "Change Avatar";
            nomUtilisateurLoginHint.text = "Enter your username";
            motDePasseLoginHint.text = "Enter your password";
        }
        else
        {
            connexionBtnTxt.text = "Connexion";
            playBtnTxt.text = "Jouer";
            parametreLabel.text = "Paramètres";
            classementLabel.text = "Connectez vous à votre compte pour visualiser";
            classementTitle.text = "Classement Des Joueurs";
            classementTitleAfterLogin.text = "Classement Des Joueurs";
            classementTitleAfterLogin.text = "Classement Des Joueurs";
            nom.text = "Nom";
            nomHintText.text = "Entrez votre nom";
            prenom.text = "Prénom";
            prenomHintText.text = "Entrez votre prénom";
            password.text = "Mot de passe";
            passwordHintText.text = "Entrez votre mot de passe";
            confirmPassword.text = "Confirmer le mot de passe";
            confirmPasswordHintText.text = "Confirmez votre mot de passe";
            usernameHintText.text = "Entrez votre nom d'utilisateur";
            dateNaissance.text = "Date de naissance";
            JJ.text = "JJ";
            MM.text = "MM";
            AAAA.text = "AAAA";
            creeUnCompte.text = "Créer Compte";
            seConnecter.text = "Se connecter";
            createAccountTitle.text = "Créer un compte";
            classementTitlePage.text = "Classement Des Joueurs";
            niveau.text = "Niveau";
            statistiques.text = "Statistiques";
            tempsTotaldeJeu.text = "Temps total de jeu";
            partiePerdues.text = "Parties perdues";
            partieGagnees.text = "Parties gagnées";
            classement.text = "Classement";
            deconnexionBtnLabel.text = "Déconnexion";
            changerAvatarLabel.text = "Changer d'avatar";
            nomUtilisateurLoginHint.text = "Entrez votre nom d'utilisateur";
            motDePasseLoginHint.text = "Entrez votre mot de passe";
        }
    }

    void onPlayBtnClicked()
    {
        SceneManager.LoadScene(sceneId);
        Debug.Log("Scene changed to " + sceneId);
    }

    void onTutoClicked()
    {
        Application.OpenURL(
            "https://mai-projet-integrateur.u-strasbg.fr/vmProjetIntegrateurgrp0-1/#tutoriel"
        );
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
    }

    private void updateAvatar()
    {
        PanelConnected.SetActive(true);
        PanelAvatar.SetActive(true);
        PanelAvatar.GetComponentInChildren<TextMeshProUGUI>().text = manager
            .GetSelfJoueur()
            .NomUtilisateur;
        Sprite newSprite = Resources.Load<Sprite>("Avatar/avatarn4");
        switch ((int)manager.GetSelfJoueur().IconeProfil)
        {
            case (int)IconeProfil.Icone1:
                newSprite = Resources.Load<Sprite>("Avatar/avatarn1");
                break;
            case (int)IconeProfil.Icone2:
                newSprite = Resources.Load<Sprite>("Avatar/avatarn2");
                break;
            case (int)IconeProfil.Icone3:
                newSprite = Resources.Load<Sprite>("Avatar/avatarn3");
                break;
        }
        if (newSprite != null)
        {
            // Modification du sprite de l'Image
            PanelAvatar.GetComponentInChildren<Image>().sprite = newSprite;
        }
        else
        {
            Debug.LogWarning("Sprite introuvable ou Image non définie !");
        }
    }

    private void updateConnexion()
    {
        if (!this.manager.IsConnected)
        {
            connexionBtn.gameObject.SetActive(true);
            rankingNotConnected.SetActive(true);
            PanelAvatar.SetActive(false);
            PanelConnected.SetActive(false);
            connexionBtnTxt.text = UIManager.singleton.language ? "LogIn" : "Connexion";
        }
        else
        {
            connexionBtn.gameObject.SetActive(false);
            rankingNotConnected.SetActive(false);
            updateAvatar();
        }
    }
}
