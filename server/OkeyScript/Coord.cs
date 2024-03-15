using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okey
{
    public class Coord
    {
        private int x;
        private int y;

        public Coord(int y, int x)
        {
            this.y = y;
            this.x = x;
        }

        public int getX() { return x; }
        public int getY() { return y; }

    }
}
