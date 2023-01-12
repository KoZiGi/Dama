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
        public static void GameLogic(PictureBox pbox)
        {
            int x = Convert.ToInt32(pbox.Name.Split('_')[1][0].ToString()), y = Convert.ToInt32(pbox.Name.Split('_')[1][1].ToString());
            if (Data.selected==null)
            {
                if (!Data.Field[y, x].isVoid)
                    Data.selected = Data.Field[y, x];
                else 
                   MessageBox.Show(x.ToString()+y.ToString());
            }
        }
        public static void GenGame()
        {
            bool indent = false;
            for (int i = 0; i < 8; i++)
            {
                for (int f = 0; f < 8; f++)
                    Data.Field[i, f] = indent ? (i%2!=0 ? new DamaPiece(f, i) : null) : (i%2==0 ? new DamaPiece(f,i) : null);
                indent = !indent;
            }
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
