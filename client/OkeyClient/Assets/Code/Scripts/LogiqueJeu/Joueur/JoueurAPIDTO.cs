namespace LogiqueJeu.Joueur
{
    using System;

    [Serializable]
    public class JoueurAPIDTO
    {
        public string username;
        public int elo;

        public override string ToString()
        {
            return $"NomUtilisateur: {this.username}, Elo: {this.elo}";
        }
    }
}
