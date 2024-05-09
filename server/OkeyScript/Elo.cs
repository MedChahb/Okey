namespace Okey
{
    using Game;

    public class Elo
    {
        /*int Score;

        public Elo(int Score) {  this.Score = Score; }

        public void CalculElo() //besoin de la formule
        {

        }*/

        //Nom d'utilisateur : Ancien Elo
        private Dictionary<string, int> UsersData;

        private string winnerName;

        public Elo(Dictionary<string, int> UsersData, string winnerName)
        {
            this.winnerName = winnerName;
            this.UsersData = UsersData;
        }

        private int Compute_K(int nombresParties)
        {
            // Implémenter une fonction décroissante assez bonne (par exemple (1 / log(x)) + C, est pas mal)
            // ou alors quelque chose similaire à: 32 pour les joueurs ayant un classement inférieur à 2100, et de 24 pour les joueurs ayant un classement supérieur à 2100.
            return 10;
        }

        private int Compute_ExpectationPlayer(string PlayerName)
        {
            var moyenneElos = 0;
            foreach (var user in this.UsersData)
            {
                if (!user.Key.Equals(PlayerName, StringComparison.Ordinal))
                {
                    moyenneElos += user.Value;
                }
            }
            moyenneElos /= 3;
            var playerElo = this.UsersData[PlayerName];
            return (int)(1 / (1 + double.Pow(10, (playerElo - moyenneElos) / 400.0))); // 400 est en float pour eviter que la portion de fraction soit perdue.
        }

        public Dictionary<string, int> CalculElo()
        {
            var newElos = new Dictionary<string, int>();
            foreach (var user in this.UsersData)
            {
                if (user.Key.Equals(this.winnerName, StringComparison.Ordinal))
                {
                    newElos.TryAdd(
                        this.winnerName,
                        this.Compute_K(10) * (1 - this.Compute_ExpectationPlayer(user.Key))
                    );
                }
                else
                {
                    newElos.TryAdd(
                        this.winnerName,
                        this.Compute_K(10) * (0 - this.Compute_ExpectationPlayer(user.Key))
                    );
                }
            }
            return newElos;
        }
    }
}
