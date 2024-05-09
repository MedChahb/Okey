namespace Okey.Joueurs
{
    using Okey.Game;

    public class Humain : Joueur
    {
        private int nbElo;
        private int Rank;

        public Humain(int id, string Name, int elo)
            : base(id, Name)
        {
            this.nbElo = elo;
            this.Rank = -1; // au debut le joueur n'est pas classé
        }

        public override void UpdateElo(Jeu j)
        {
            this.nbElo += Elo.CalculElo(j, this);
        }

        public override void Gagne(Jeu j)
        {
            Console.Write($"Le gagnant est : {Name}, encient Elo = {nbElo}, ");
            this.UpdateElo(j);
            Console.WriteLine($"nouveau Elo = {nbElo}.");
        }

        public int GetRank()
        {
            return this.Rank;
        }

        public int GetElo()
        {
            return this.nbElo;
        }

        public override string ToString()
        {
            return $"{this.Name}";
        }
    }
}
