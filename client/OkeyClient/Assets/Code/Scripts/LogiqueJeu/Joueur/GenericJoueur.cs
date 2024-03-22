using System.Collections.Generic;

public sealed class GenericJoueur : Joueur
{
    public GenericJoueur(
        string NomUtilisateur,
        int Elo,
        int IconeProfil,
        List<Achievement> Achievements,
        int Score,
        int Niveau
    )
        : base(NomUtilisateur, Elo, IconeProfil, Achievements, Score, Niveau) { }
}
