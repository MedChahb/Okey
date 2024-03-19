using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okey.Joueurs
{
    internal class Bot : Joueur
    {
        private int difficulte;
        public Bot(int diff) : base(0, "BOT")
        {
            this.difficulte = diff;
        }

        public override void Gagne()
        {
            // ??
        }

        public override string ToString()
        {
            return String.Format("Bot de difficulté : {0}", this.difficulte);
        }
    }
}
