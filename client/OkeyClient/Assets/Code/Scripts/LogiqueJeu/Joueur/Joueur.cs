namespace LogiqueJeu.Joueur
{
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class Joueur : ScriptableObject
    {
        [field: SerializeField]
        public string NomUtilisateur { get; set; }

        [field: SerializeField]
        public int Elo { get; set; }

        [field: SerializeField]
        public int IconeProfil { get; set; }

#pragma warning disable IDE0052, IDE0044
        // A placeholder for the time being until it gets potentially implemented
        private List<Achievement> Achievements;
#pragma warning restore IDE0052, IDE0044
        [field: SerializeField]
        public int Score { get; set; }

        [field: SerializeField]
        public int Niveau { get; set; }

        public override string ToString()
        {
            return $"NomUtilisateur: {this.NomUtilisateur}, Elo: {this.Elo}, IconeProfil: {this.IconeProfil}, Score: {this.Score}, Niveau: {this.Niveau}";
        }
    }
}
