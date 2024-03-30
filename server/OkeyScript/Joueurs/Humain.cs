using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okey.Joueurs
{
    
    internal class Humain : Joueur
    {
        private int Elo;
        private int Rank;

        public Humain(int id, string Name, int elo) : base(id, Name)
        {
            this.Elo = elo;
            this.Rank = -1; // au debut le joueur n'est pas classé
        }

        public override void UpdateElo()
        {
            //if gagne
            this.Elo += 10;
            //if lose
            this.Elo -= 10;
        }
        public override void Gagne()
        {
            Console.Write($"Le gagnant est : {Name}, encient Elo = {Elo}, ");
            this.UpdateElo();
            Console.WriteLine($"nouveau Elo = {Elo}.");
        }

        public int GetRank() { return this.Rank; }  


        public override string ToString()
        {
            return $"{this.Name}";
        }

        
    }
}
