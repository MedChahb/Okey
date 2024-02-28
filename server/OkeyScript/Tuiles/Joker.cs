using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okey.Tuiles
{
    internal class Joker : Tuile
    {
        public Joker(CouleurTuile couleur, int valeur, bool dansPioche) : base(couleur, valeur, dansPioche)
        {
            //calculate value
        }

        public override void Move()
        {
            throw new NotImplementedException();
        }

        public override String ToString()
        {
            return String.Format("Joker de couleur : {0}, numero : {1}.", this.Couleur, this.valeur);
        }
    }
}
