using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dama
{
    class DamaPiece
    {
        public bool isDama, isBlack, isVoid;
        public int X;
        public int Y;
        public DamaPiece(int x, int y)
        {
            X = x;
            Y = y;
            isDama = false;
            isBlack = y<3;
            isVoid = y==4 || y==5 ;
        }
    }
}
