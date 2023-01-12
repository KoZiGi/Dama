using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dama
{
    class Game
    {
        public void GameLogic(bool moves)
        {
        }
        public static void SelectPiece(PictureBox clickedpbx)
        {
            Data.selected = Data.Field[Convert.ToInt32(clickedpbx.Name.Split('_')[0]), Convert.ToInt32(clickedpbx.Name.Split('_')[1])];
        }
        public static void CheckFTH()
        {

        }
        public static void InitMove()
        {
            
        }
        public static void CheckValid()
        {

        }
        public static void Move()
        {

        }
        public static void Switch() => Data.isBlack = !Data.isBlack;
    }
}
