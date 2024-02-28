using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okey.Tuiles
{
    internal class TuileNormale : Tuile
    {
        public TuileNormale(CouleurTuile couleur, int valeur, bool dansPioche) : base(couleur, valeur, dansPioche)
        {
        }

        public override void Move()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return String.Format("Tuile de couleur : {0}, numero : {1}.", this.Couleur, this.valeur);
        }
    }
}
