using System;
using System.Collections.Generic;
using LogiqueJeu.Joueur;
using UnityEngine;

public class JoueurManager : MonoBehaviour
{
    private readonly List<Joueur> Joueurs = new(3);
    private SelfJoueur SoiMeme;
    public readonly List<JoueurSO> JoueursSOs = new(4);

    private void Awake()
    {
        this.SoiMeme = new();

        // code pour tester
        this.SoiMeme.NomUtilisateur = "Testeur1";
        this.SoiMeme.TokenConnexion =
            "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJnaXZlbl9uYW1lIjoiVGVzdGV1cjEiLCJuYmYiOjE3MTE4NDc3MDksImV4cCI6MTcxMjQ1MjUwOSwiaWF0IjoxNzExODQ3NzA5LCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjUyNDYiLCJhdWQiOiJodHRwOi8vbG9jYWxob3N0OjUyNDYifQ.uf4hmpxz6MwjWD7txbxuZtf64gEMg_kxuQRYFNmWEGnw5pDLJShABigmZrFhUdODs11nBQNvQMVzV3v8VFRyfQ";
        this.SoiMeme.SaveXML();
        // fin code pour tester

        this.SoiMeme.LoadSelf(this);

        var SoiMemeSO = ScriptableObject.CreateInstance<JoueurSO>();
        SoiMemeSO.Joueur = this.SoiMeme;
        this.JoueursSOs.Add(SoiMemeSO);
    }

    private void Update() { }

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
        if (this.Joueurs.Count == 3 || this.JoueursSOs.Count == 4)
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
            var JSO = this.JoueursSOs.FindAll(JoueurSO => JoueurSO.Joueur.InGame.ID == ID);
            if (J.Count > 1 || JSO.Count > 1)
            {
                throw new DataMisalignedException(
                    "There are multiple players with the same ID, internal error, this should've never happenned"
                );
            }
            foreach (var JF in J)
            {
                this.Joueurs.Remove(JF);
            }
            foreach (var JF in JSO)
            {
                this.JoueursSOs.Remove(JF);
                ScriptableObject.Destroy(JF);
            }
        }
        this.Joueurs.Add(Joueur);
        var JoueurSO = ScriptableObject.CreateInstance<JoueurSO>();
        JoueurSO.Joueur = Joueur;
        this.JoueursSOs.Add(JoueurSO);
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
        var JSO = this.JoueursSOs.Find(JoueurSO =>
            JoueurSO.Joueur.InGame.ID == ID && JoueurSO.Joueur is not SelfJoueur
        );
        if (JSO != null)
        {
            this.JoueursSOs.Remove(JSO);
            ScriptableObject.Destroy(JSO);
        }
        return this.Joueurs.RemoveAll(Joueur => Joueur.InGame.ID == ID && Joueur is not SelfJoueur);
    }

    public int RemoveJoueur(JoueurSO JoueurSO)
    {
        return this.RemoveJoueur(JoueurSO.Joueur.InGame.ID);
    }

    public int RemoveJoueur(Joueur Joueur)
    {
        return this.RemoveJoueur(Joueur.InGame.ID);
    }

    public void UpdateJoueurs()
    {
        foreach (var Joueur in this.Joueurs)
        {
            Joueur.LoadSelf(this);
        }
    }

    public void SaveSelfJoueur()
    {
        this.SoiMeme.SaveXML();
    }
}
