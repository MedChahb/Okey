namespace LogiqueJeu.Joueur
{
    public class SelfJoueurAPIConnexionDTO
    {
        public string userName;
        public string password;

        public SelfJoueurAPIConnexionDTO(string userName, string password)
        {
            this.userName = userName;
            this.password = password;
        }
    }
}
