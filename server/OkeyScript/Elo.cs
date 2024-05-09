namespace Okey
{
    using Game;

    public class Elo
    {
        //Nom d'utilisateur : Ancien Elo
        private Dictionary<string, int> UsersData;

        private string winnerName;

        public Elo(Dictionary<string, int> UsersData, string winnerName)
        {
            this.winnerName = winnerName;
            this.UsersData = UsersData;
        }

        private int Compute_K(int nombresPartiesJouees, int nombresPartiesGagnees)
        {
            // pour calculer le K, on vas travailler avec la fonction de decroissance (comme en decroissance radioactive)
            // la difference de K est proportionnelle à la difference du nombre des matches multiplié par l'opposé de K et une constante qui determine le taux de changement de K
            // c-a-d : delta(K) = - alpha * K * delta(nombrePartiesGagne), alors pour des variations instantanées et fines, la formule nous donne une équation differentielle dont la solution est :
            // K(n) = K0 * exp(-alpha * nombrePartie). 
            // mais cette fonction decroissante converge vers 0 pour les jouerus experimenté, donc faut ajouter une valeur de K minmun, pour gerer la convergence
            // alors la fonction finale est : K(n) = Kmin + (K0 - Kmin) * exp(-alpha * nombrePartiesGagne) avec K0 l'abssice à l'origine et c'est le K pour un joueur nouveau qui a 0 de PartieJoue.
            
            var winRate = (nombresPartiesJouees != 0) ? (double)nombresPartiesGagnees / nombresPartiesJouees : 0;
            var alpha = 0.02;
            
            if(winRate < 0.1)
                return K_max;
            if (winRate < 0.3)
                return 29;
            if (winRate < 0.45)
                return 26;

            
            // si son winrate est positive alors utiliser la formule
            return (int)Math.Round(K_min + ((K_max - K_min) * Math.Exp(-alpha * nbrPartieGagne))); // si il gange trop on diminue le K
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
