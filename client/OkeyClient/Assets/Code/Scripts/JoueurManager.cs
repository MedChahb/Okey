using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json;
using LogiqueJeu.Joueur;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class JoueurManager : MonoBehaviour
{
    public static JoueurManager Instance { get; private set; }

    [SerializeField]
    private bool CleanStart = false; // Manipulez dans l'éditeur Unity pour invalider les données de cache SelfJoueur
    private readonly List<Joueur> Joueurs = new(3);
    private SelfJoueur SoiMeme;

    [HideInInspector]
    public UnityEvent SelfJoueurChangeEvent = new();

    [HideInInspector]
    public UnityEvent OtherJoueurChangeEvent = new();

    [HideInInspector]
    public UnityEvent AnyJoueurChangeEvent = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(this.gameObject);

        this.SoiMeme = new();
        if (this.CleanStart)
        {
            this.SoiMeme.DeleteXML();
        }
        this.SoiMeme.JoueurChangeEvent += this.OnSelfJoueurChange;
        this.SoiMeme.LoadSelf(this);
    }

    private void OnSelfJoueurChange(object O = null, EventArgs E = null)
    {
        this.SelfJoueurChangeEvent?.Invoke();
        this.AnyJoueurChangeEvent?.Invoke();
    }

    private void OnOtherJoueurChange(object O = null, EventArgs E = null)
    {
        this.OtherJoueurChangeEvent?.Invoke();
        this.AnyJoueurChangeEvent?.Invoke();
    }

    public void AddGenericJoueur(
        int ID,
        string NomUtilisateur = null,
        Position? Pos = null,
        bool Replace = false
    )
    {
        if (Pos == Position.SoiMeme)
        {
            throw new ArgumentException("Cannot add a player to the same position as oneself");
        }
        if (this.Joueurs.Count == 3)
        {
            throw new ArgumentException(
                "Cannot add any more players, maximum number of players reached"
            );
        }
        if (!Replace && this.Joueurs.Exists(Joueur => Joueur.InGame.ID == ID))
        {
            throw new ArgumentException("Cannot add a player with the same ID as another player");
        }
        var Joueur = new GenericJoueur();
        Joueur.InGame.ID = ID;
        Joueur.InGame.Pos = Pos;
        if (NomUtilisateur != null)
        {
            Joueur.NomUtilisateur = NomUtilisateur;
            Joueur.LoadSelf(this);
        }

        if (Replace)
        {
            var J = this.Joueurs.FindAll(Joueur => Joueur.InGame.ID == ID);
            if (J.Count > 1)
            {
                throw new DataMisalignedException(
                    "There are multiple players with the same ID, internal error, this should've never happenned"
                );
            }
            foreach (var JF in J)
            {
                JF.JoueurChangeEvent -= this.OnOtherJoueurChange;
                this.Joueurs.Remove(JF);
            }
        }
        this.Joueurs.Add(Joueur);
        Joueur.JoueurChangeEvent += this.OnOtherJoueurChange;
        this.OnOtherJoueurChange();
    }

    public void AssignPositionJoueur(int ID, Position Pos)
    {
        if (Pos == Position.SoiMeme)
        {
            throw new ArgumentException("Cannot add a player to the same position as oneself");
        }
        var Joueur =
            this.Joueurs.Find(Joueur => Joueur.InGame.ID == ID)
            ?? throw new ArgumentException("Player not found");
        if (Joueur is SelfJoueur)
        {
            throw new ArgumentException("Cannot change the position of oneself");
        }
        Joueur.InGame.Pos = Pos;
        this.OnOtherJoueurChange();
    }

    public int RemoveJoueur(int ID)
    {
        var J = this.Joueurs.FindAll(Joueur => Joueur.InGame.ID == ID && Joueur is not SelfJoueur);
        if (J.Count > 1)
        {
            throw new DataMisalignedException(
                "There are multiple players with the same ID, internal error, this should've never happenned"
            );
        }
        foreach (var JF in J)
        {
            JF.JoueurChangeEvent -= this.OnOtherJoueurChange;
            this.Joueurs.Remove(JF);
        }
        return J.Count;
    }

    public int RemoveJoueur(Joueur Joueur)
    {
        return this.RemoveJoueur(Joueur.InGame.ID);
    }

    public void UpdateJoueurs()
    {
        this.SoiMeme.LoadSelf(this);
        foreach (var Joueur in this.Joueurs)
        {
            Joueur.LoadSelf(this);
        }
    }

    public void SaveSelfJoueur()
    {
        this.SoiMeme.SaveXML();
    }

    public SelfJoueur GetSelfJoueur()
    {
        return (SelfJoueur)this.SoiMeme.Clone();
    }

    public List<Joueur> GetOtherJoueurs()
    {
        return this.Joueurs.ConvertAll(Joueur => (Joueur)Joueur.Clone());
    }

    public List<Joueur> GetAllJoueurs()
    {
        var res = this.Joueurs.ConvertAll(Joueur => (Joueur)Joueur.Clone());
        res.Insert(0, (Joueur)this.SoiMeme.Clone());
        return res;
    }

    public Joueur GetLeftJoueur()
    {
        return (Joueur)this.Joueurs.Find(Joueur => Joueur.InGame.Pos == Position.Gauche).Clone();
    }

    public Joueur GetRightJoueur()
    {
        return (Joueur)this.Joueurs.Find(Joueur => Joueur.InGame.Pos == Position.Droite).Clone();
    }

    public Joueur GetTopJoueur()
    {
        return (Joueur)this.Joueurs.Find(Joueur => Joueur.InGame.Pos == Position.Haut).Clone();
    }

    public void StartConnexionSelfJoueur(
        string NomUtilisateur,
        string MotDePasse,
        Action<int> CallbackResult = null
    )
    {
        this.SoiMeme.StartConnexionCompte(this, NomUtilisateur, MotDePasse, CallbackResult);
    }

    public void StartCreationCompteSelfJoueur(
        string NomUtilisateur,
        string MotDePasse,
        int IconeProfil,
        Action<int> CallbackResult = null
    )
    {
        this.SoiMeme.StartCreationCompte(
            this,
            NomUtilisateur,
            MotDePasse,
            IconeProfil,
            CallbackResult
        );
    }

    public void DeconnexionSelfJoueur()
    {
        this.SoiMeme.DeconnexionCompte();
    }

    // Une requête est à faire ici pour mettre à jour l'icone dans l'API
    public void SetIconeSelfJoueur(int Icone)
    {
        throw new NotImplementedException();
    }

    // This method fetches the order of the players in the leaderboard
    // but it does it in two stages. It first fetches the leaderboard
    // and secondly for each entry in the leaderboard, it fetches more
    // information on that specific account from a seperate endpoint.
    // Due to the nature of this two stage behaviour which is dictated by
    // the backend API implementation (there is nothing we can do about it
    // from the client side to make it better), there is a possiblity for a
    // weird pseudo error case where the leaderboard or the player details
    // of some players in the previously fetched leaderboard might have changed
    // in the meantime, making the leaderboard inconsistent.
    //
    // A good way to handle this would be to merge the two endpoints on the backend
    // or implement a database lock mechanism to prevent changes while this method executes.
    // This would then result in a consistent result no matter what with zero race conditions.
    private IEnumerator FetchClassementsBG(
        string NomUtilisateur = null,
        int Limit = 5,
        Action<List<Joueur>> CallbackResult = null
    )
    {
        List<Joueur> Result = null;

        var RequestURL = Constants.API_URL_DEV + "/classement/";
        if (NomUtilisateur != null)
        {
            RequestURL += NomUtilisateur;
        }
        else if (Limit > 0)
        {
            RequestURL += Limit;
        }
        else if (Limit < 0)
        {
            throw new ArgumentException("Limit must be a positive integer or zero");
        }

        var www = UnityWebRequest.Get(RequestURL);
        www.certificateHandler = new BypassCertificate();
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Result = new();
            var unmarshal = JsonSerializer.Deserialize<List<JoueurAPIClassementDTO>>(
                www.downloadHandler.text
            );

            foreach (var JoueurDTO in unmarshal)
            {
                Joueur J;
                if (JoueurDTO.username == this.SoiMeme.NomUtilisateur)
                {
                    this.SoiMeme.UpdateDetails(this);
                    J = (SelfJoueur)this.SoiMeme.Clone();
                }
                else
                {
                    J = new GenericJoueur { NomUtilisateur = JoueurDTO.username };
                    J.LoadSelf(this);
                }
                J.Classement = JoueurDTO.classement;
                Result.Add(J);
            }
        }
        else
        {
            Debug.Log(www.error);
        }

        CallbackResult?.Invoke(Result);
    }

    public void StartFetchClassements(
        string NomUtilisateur = null,
        int Limit = 5,
        Action<List<Joueur>> CallbackResult = null
    )
    {
        this.StartCoroutine(this.FetchClassementsBG(NomUtilisateur, Limit, CallbackResult));
    }
}
