using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dama
{
    class Data
    {
        public static bool isBlack = true;
        public static Form1 GameForm;
        public static int[,] _Field = new int[8, 8];
        //Test Matrix
        /*public static int[,] _Field = new int[8, 8]{
            {0,0,0,0,0,0,0,0},    
            {0,0,0,0,0,0,0,0},    
            {0,0,0,0,11,0,0,0},    
            {0,0,0,2,0,0,0,0},    
            {0,0,0,0,0,0,0,0},    
            {0,0,0,0,0,0,0,0},    
            {0,0,0,0,0,0,2,0},    
            {0,0,0,0,0,0,0,0}
        };*/
        public static int[] selectedIdx = new int[2] { -1 , -1 };
        public static List<int[]> HitReqCoords = new List<int[]>();
    }
}
