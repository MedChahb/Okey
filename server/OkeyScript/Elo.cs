namespace Okey
{
    using Okey.Game;
    using Okey.Joueurs;

    public class Elo
    {

        //Nom d'utilisateur : Ancien Elo
        private Dictionary<string, int> UsersData;

        //puisque la Class Humain a deja le Elo, on peut travailler avec la class mère Joueur, au lieu d'un dictionnaire

        private string winnerName;

        private static readonly int K_max = 32;
        private static readonly int K_min = 4;

        public Elo(Dictionary<string, int> UsersData, string winnerName)
        {
            this.winnerName = winnerName;
            this.UsersData = UsersData;
        }

        public static int ComputeK(Joueur j)
        {
            // pour calculer le K, on vas travailler avec la fonction de decroissance (comme en decroissance radioactive)
            // la difference de K est proportionnelle à la difference du nombre des matches multiplié par l'opposé de K et une constante qui determine le taux de changement de K
            // c-a-d : delta(K) = - alpha * K * delta(nombrePartiesGagne), alors pour des variations instantanées et fines, la formule nous donne une équation differentielle dont la solution est :
            // K(n) = K0 * exp(-alpha * nombrePartie). 
            // mais cette fonction decroissante converge vers 0 pour les jouerus experimenté, donc faut ajouter une valeur de K minmun, pour gerer la convergence
            // alors la fonction finale est : K(n) = Kmin + (K0 - Kmin) * exp(-alpha * nombrePartiesGagne) avec K0 l'abssice à l'origine et c'est le K pour un joueur nouveau qui a 0 de PartieJoue.

            var nbrPartieGagne = j.GetPartieGagne();
            var winRate = j.GetWinRate();
            var alpha = 0.02; // taux de decroisance de la fonction

            if(winRate < 0.1)
                return K_max;
            if (winRate < 0.3)
                return 29;
            if (winRate < 0.4)
                return 26;

            
            // si son winrate est positive alors utiliser la formule
            return (int)Math.Round(K_min + ((K_max - K_min) * Math.Exp(-alpha * nbrPartieGagne))); // si il gange trop on diminue le K
        }

        /*private int Compute_ExpectationPlayer(string PlayerName) // je pense c'est mieux de travailler avec joueur en argument
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
        }*/

        private static double Compute_ExpectationPlayer(Jeu j, Joueur pl)
        {
            var moyenneElos = 0;
            foreach(var joueur in j.GetJoueurs())
            {
                if(!string.Equals(joueur.getName(), pl.getName(), StringComparison.OrdinalIgnoreCase))
                {
                    if (joueur is Humain)
                    {
                        moyenneElos += ((Humain)joueur).GetElo();
                    }

                    else if (joueur is Bot)
                    {
                        moyenneElos += ((Bot)joueur).GetDifficulte();
                    }
                }
            }

            moyenneElos /= 3;
            int playerElo = 0; // assigne 0 to bypass error, soit le if soit le elseif qui marchera
            if(pl is Humain)
                playerElo = ((Humain)pl).GetElo();
            else if (pl is Bot)
                playerElo = ((Bot)pl).GetDifficulte();

            return (double)(1 / (1 + double.Pow(10, (playerElo - moyenneElos) / 400.0))); // 400 est en float pour eviter que la portion de fraction soit perdue.
        }


        /*public Dictionary<string, int> CalculElo()
        {
            var newElos = new Dictionary<string, int>();
            foreach (var user in this.UsersData)
            {
                if (user.Key.Equals(this.winnerName, StringComparison.Ordinal))
                {
                    newElos.TryAdd(
                        this.winnerName,
                        Compute_K(null) * (1 - this.Compute_ExpectationPlayer(user.Key))
                    );
                }
                else
                {
                    newElos.TryAdd(
                        this.winnerName,
                        Compute_K(null) * (0 - this.Compute_ExpectationPlayer(user.Key))
                    );
                }
            }
            return newElos;
        }*/

        // a la fin du jeu, on iterera sur tous le joueurs en appelant CalculElo afin de faire l'update de leurs Elo
        public static int CalculElo(Jeu j, Joueur pl)
        {
            // le Si = 1 pour le winner, sinon 0
            // si on implemente le systeme de ingame score, faut noramliser le Si pourqu'il soit dans l'intervalle [0,1], et pour cela Si = ingame_score_i/max_in_score pour le joueur 
            var Si = pl.isGagnant() ? 1 : 0;

            //Console.WriteLine($"{pl} : compute = {Compute_ExpectationPlayer(j, pl)}");
            return (int)Math.Round(pl.GetK() * (Si - Compute_ExpectationPlayer(j, pl))); // getK() est correct puisque a l'intanciation du chaque joueur, on lui calcul un K avec la methode Compute_K (voir constructeur de Joueur)
        }

        //returns a dict containing new Elo and players name {player_name1 : elo1, pla  yer_name_2 : elo2 ....}
        public static Dictionary<string, int> GetEloDataDict(Jeu j)
        {
            Dictionary<string, int> EloData = [];

            foreach(var player in j.GetJoueurs())
            {
                _ = EloData.TryAdd(player.getName(), (player is Humain) ? ((Humain)player).GetElo() : ((Bot)player).GetDifficulte());
            }

            return EloData;
        }
    }
}
