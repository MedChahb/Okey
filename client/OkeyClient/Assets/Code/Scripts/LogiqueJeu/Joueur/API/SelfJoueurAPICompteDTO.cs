namespace LogiqueJeu.Joueur
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class SelfJoueurAPICompteDTO
    {
        public string username;
        public int elo;

        public List<bool> achievements;

        public override string ToString()
        {
            return $"NomUtilisateur: {this.username}, Elo: {this.elo}, Achievements: {string.Join(",", this.achievements)}";
        }
    }
}
