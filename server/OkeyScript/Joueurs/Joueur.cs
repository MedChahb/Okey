using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Okey.Tuiles;

namespace Okey.Joueurs
{
    public abstract class Joueur
    { 
        protected int id;
        protected String Name;
        protected Tuile[,] Chevalet = new Tuile[2,15];
        protected Boolean Tour;
        protected Boolean Gagnant;
        

        public Joueur(int id, String Name) {
            this.id = id; this.Name = Name; 
            this.Gagnant = false;
            //this.Chevalet = //fonction qui distribue les tuiles;
        }

        public abstract void Gagne();

        public void VerifChevalet()
        {
            //chech for series then 
            this.Gagne();
        }

        public void Piocher() { }
        public void JeterTuile(Tuile T) { }

        public void PassTour() 
        {
            this.Tour = false;
        }
        public void EstTour()
        {
            this.Tour = true;
        }

        public abstract String toString();
    }
}
