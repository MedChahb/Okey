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

        public override string ToString()
        {
            return String.Format("({0}, {1}, {2})", this.num, this.Couleur, "No");
        }
    }
}
