namespace LogiqueJeu.Joueur
{
    public class SelfJoueurAPIConnexionResponseDTO
    {
        public string userName;
        public string token;

        public SelfJoueurAPIConnexionResponseDTO(string userName, string token)
        {
            this.userName = userName;
            this.token = token;
        }
    }
}
