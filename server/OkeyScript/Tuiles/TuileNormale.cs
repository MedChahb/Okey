using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okey.Tuiles
{
    internal class TuileNormale : Tuile
    {
        public TuileNormale(CouleurTuile couleur, int num, bool dansPioche) : base(couleur, num, dansPioche)
        {
        }

        public override void Move()
        {
            throw new NotImplementedException();
        }

        public bool MemeCouleur(TuileNormale t)
        {
            return this.Couleur == t.GetCouleur();
        }

        public bool estSuivant(TuileNormale t)
        {
            return this.num + 1 == t.GetNum() || (this.num==13 && t.GetNum() == 1);
        }

        public override string ToString()
        {
            return String.Format("({0}, {1}, {2})", this.num, this.Couleur, "No");
        }
    }
}
