namespace LogiqueJeu.Joueur
{
    using System;

    [Serializable]
    public class JoueurAPIClassementDTO
    {
        public string username;
        public int classement;
        public int elo;

        public override string ToString()
        {
            return $"NomUtilisateur: {this.username}, Classement: {this.classement}, Elo: {this.elo}";
        }
    }
}
