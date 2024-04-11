namespace Okey.Joueurs
{
    public class Bot : Joueur
    {
        private int difficulte;
        public Bot(int diff) : base(0, "BOT")
        {
            this.difficulte = diff;
        }

        public override void UpdateElo()
        {
            // has no ELo -> does nothing (to discuss)
        }
        public override void Gagne()
        {
            Console.Write($"le gagnant est {this}.");

        }

        public override string ToString()
        {
            return $"Bot de difficulté : {this.difficulte}";
        }
    }
}
