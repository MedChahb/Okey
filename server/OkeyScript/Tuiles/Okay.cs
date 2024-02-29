using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okey.Tuiles
{
    internal class Okay : Tuile
    {
        public Okay(bool dansPioche) : base(CouleurTuile.M, 0, dansPioche)
        {
            //calculate value
        }

        public override void Move()
        {
            throw new NotImplementedException();
        }

        public override String ToString()
        {
            return String.Format("({0}, {1}, {2})", this.num, this.Couleur, "Ok");
        }
    }
}
