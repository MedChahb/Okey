using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using LogiqueJeu.Joueur;
using UnityEngine;
using UnityEngine.Events;

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
        this.SoiMeme.JoueurChangeEvent += (_, _) => this.SelfJoueurChangeEvent?.Invoke();
        this.SoiMeme.LoadSelf(this);
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
                this.Joueurs.Remove(JF);
            }
        }
        this.Joueurs.Add(Joueur);
    }

    public void AssignPositionGenericJoueur(int ID, Position Pos)
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
    }

    public int RemoveJoueur(int ID)
    {
        return this.Joueurs.RemoveAll(Joueur => Joueur.InGame.ID == ID && Joueur is not SelfJoueur);
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

    // For now doesn't return a clone, but the actual object
    // Should maybe change that later to abide by encapsulation
    // and protect inner data structures
    public SelfJoueur GetSelfJoueur()
    {
        return (SelfJoueur)this.SoiMeme.Clone();
    }

    // For now doesn't return a clone, but the actual object
    // Should maybe change that later to abide by encapsulation
    // and protect inner data structures
    public ReadOnlyCollection<Joueur> GetOtherJoueurs()
    {
        return this.Joueurs.AsReadOnly();
        // return this.Joueurs.ConvertAll(Joueur => Joueur.Clone());
    }

    // For now doesn't return a clone, but the actual object
    // Should maybe change that later to abide by encapsulation
    // and protect inner data structures
    public List<Joueur> GetAllJoueurs()
    {
        var Result = new List<Joueur>(this.Joueurs);
        Result.Insert(0, this.SoiMeme);
        return Result;
        // return this.Joueurs.ConvertAll(Joueur => Joueur.Clone()).Insert(0, this.SoiMeme.Clone());
    }

    public void ConnexionSelfJoueur(
        string NomUtilisateur,
        string MotDePasse,
        Action<int> CallbackResult = null
    )
    {
        this.SoiMeme.ConnexionCompte(this, NomUtilisateur, MotDePasse, CallbackResult);
    }

    public void CreationCompteSelfJoueur(
        string NomUtilisateur,
        string MotDePasse,
        Action<int> CallbackResult = null
    )
    {
        this.SoiMeme.CreationCompte(this, NomUtilisateur, MotDePasse, CallbackResult);
    }

    public void DeconnexionSelfJoueur()
    {
        this.SoiMeme.DeconnexionCompte();
    }

    public void SetIconeSelfJoueur(int Icone)
    {
        throw new NotImplementedException();
    }
}
