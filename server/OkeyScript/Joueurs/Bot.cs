namespace Okey.Joueurs
{
    using Okey.Game;

    public class Bot : Joueur
    {
        private int difficulte;

        public Bot(int diff)
            : base(0, "BOT")
        {
            this.difficulte = diff;
        }

        public override void UpdateElo(Jeu j)
        {
            // has no ELo -> does nothing (to discuss)
        }

        public override void Gagne(Jeu j)
        {
            Console.Write($"le gagnant est {this}.");
        }

        public int GetDifficulte()
        {
            return this.difficulte;
        }

        public override string ToString()
        {
            return $"Bot de difficulté : {this.difficulte}";
        }
    }
}
