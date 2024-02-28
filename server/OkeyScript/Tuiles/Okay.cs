using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okey.Tuiles
{
    internal class Okay : Tuile
    {
        public Okay(bool dansPioche) : base(CouleurTuile.Multi, 0, dansPioche)
        {
            //calculate value
        }

        public override void Move()
        {
            throw new NotImplementedException();
        }

        public override String ToString()
        {
            return String.Format("OKay,{0} dans la pioche.", (this.DansPioche)?"" : " pas");
        }
    }
}
