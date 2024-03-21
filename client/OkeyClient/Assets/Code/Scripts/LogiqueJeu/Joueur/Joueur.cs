using System.Collections.Generic;
using UnityEngine;

public abstract class Joueur : MonoBehaviour
{
    public string NomUtilisateur { get; set; }
    public int Elo { get; set; }
    public int IconeProfil { get; set; }

#pragma warning disable IDE0052, IDE0044
    // A placeholder for the time being until it gets potentially implemented
    private List<Achievement> Achievements;
#pragma warning restore IDE0052, IDE0044

    public int Score { get; set; }
    public int Niveau { get; set; }

    public Joueur(
        string NomUtilisateur,
        int Elo,
        int IconeProfil,
        List<Achievement> Achievements,
        int Score,
        int Niveau
    )
    {
        this.NomUtilisateur = NomUtilisateur;
        this.Elo = Elo;
        this.IconeProfil = IconeProfil;
        this.Achievements = Achievements;
        this.Score = Score;
        this.Niveau = Niveau;
    }
}
