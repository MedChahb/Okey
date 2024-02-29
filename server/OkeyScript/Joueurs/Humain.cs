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

        private void UpdateElo()
        {
            //if gagne
            this.Elo += 20;
            //if lose
            this.Elo -= 0;
        }
        public override void Gagne()
        {
            Console.Write(String.Format("Le gagnant est : {0}, encient Elo = {1}, ", this.Name, this.Elo));
            this.UpdateElo();
            Console.WriteLine(String.Format("nouveau Elo = {0}", this.Elo));
        }

        public int GetRank() { return this.Rank; }  


        public override string ToString()
        {
            return String.Format("Joueur : id = {0}, Name = {1}, Elo = {2}", this.id, this.Name, this.Elo);
        }

        
    }
}
