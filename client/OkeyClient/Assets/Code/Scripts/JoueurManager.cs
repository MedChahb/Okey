using System;
using System.Collections;
using LogiqueJeu.Constants;
using LogiqueJeu.Joueur;
using UnityEngine;
using UnityEngine.Networking;

public class JoueurManager : MonoBehaviour
{
    private Joueur playerLeft;

    private void Awake()
    {
        this.playerLeft = ScriptableObject.CreateInstance<GenericJoueur>();

        // Concurrent
        // this.StartCoroutine(this.FetchUserBG("Testeur1", this.UnmarshalAndInit));

        // Sequential
        this.UnmarshalAndInit(this.FetchUserFG("Testeur1"));

        Debug.Log(this.playerLeft);
    }

    private void Update() { }

    private void UnmarshalAndInit(string Json)
    {
        var unmarshal = JsonUtility.FromJson<JoueurAPICompteDTO>(Json);
        this.playerLeft.NomUtilisateur = unmarshal.username;
        this.playerLeft.Elo = unmarshal.elo;
    }

    private IEnumerator FetchUserBG(string Username, Action<string> CallbackJSON = null)
    {
        var Response = "";

        var www = UnityWebRequest.Get(Constants.API_URL_DEV + "/compte/watch/" + Username);
        www.certificateHandler = new BypassCertificate();
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Response = www.downloadHandler.text;
        }

        CallbackJSON?.Invoke(Response);
    }

    private string FetchUserFG(string Username)
    {
        var Response = "";

        var www = UnityWebRequest.Get(Constants.API_URL_DEV + "/compte/watch/" + Username);
        www.certificateHandler = new BypassCertificate();
        www.SendWebRequest();

        while (!www.isDone) { }

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Response = www.downloadHandler.text;
        }

        return Response;
    }
}

public class BypassCertificate : CertificateHandler
{
    // Cela devrait pas être nécessaire vu que la connexion passe par HTTPS
    // avec un certificat de l'Unistra bien reconnu.
    // Il faut mettre l'API en HTTP simple.
    // À mon avis il y a deux HTTPS en jeu en ce moment,
    // 1) le bastion Unistra, 2) l'API ou le reverse proxy Nginx.
    // Il faut enlever le HTTPS de l'API pour que ça marche mieux sans cette duplication.
    protected override bool ValidateCertificate(byte[] certificateData)
    {
        //Simply return true no matter what
        return true;
    }
}
