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
            MessageBox.Show(Data._Field[x, y].ToString());
            if (Data.selectedIdx[0]==-1 && Data.selectedIdx[1] == -1)
            {
                SelectPiece(pbox.Name);
            }
        }
        public static void Hit()
        {

        }
        public static void GenGame()
        {
            bool indent = false;
            for (int i = 0; i < 8; i++)
            {
                for (int g = 0; g < 8; g++)
                {
                    Data._Field[i, g] = DeterminePiece(i, g, indent);
                }
                indent = !indent;
            }
        }
        public static void SelectPiece(string pbxName)
        {
            Data.selectedIdx[0] = Convert.ToInt32(pbxName.Split('_')[1][0].ToString());
            Data.selectedIdx[1] = Convert.ToInt32(pbxName.Split('_')[1][1].ToString());
        }
        public static bool CheckBlackFTH(int i, int g)
        {
            if (i + 2 > 8 && g + 2 > 8 && g - 2 < 0) return false;
            if (Data._Field[i+2, g + 2] == 0)
            {
                if (Data._Field[i + 1, g + 1] == 2 || Data._Field[i + 1, g + 1] == 22) return true;
            }
            if (Data._Field[i + 2, g - 2] == 0)
            {
                if (Data._Field[i + 1, g - 1] == 2 || Data._Field[i + 1, g - 1] == 22) return true;
            }
            return false;
        }
        public static bool CheckWhiteFTH(int i, int g)
        {
            if (i - 2 < 0 && g + 2 > 8 && g - 2 < 0) return false;
            if (Data._Field[i - 2, g + 2] == 0)
            {
                if (Data._Field[i - 1, g + 1] == 2 || Data._Field[i - 1, g + 1] == 22) return true;
            }
            if (Data._Field[i - 2, g - 2] == 0)
            {
                if (Data._Field[i - 1, g - 1] == 2 || Data._Field[i - 1, g - 1] == 22) return true;
            }
            return false;
        }
        public static void InitMove()
        {
            
        }
        public static void CheckValid(bool canhit)
        {

        }
        public static void Move()
        {

        }
        public static void PesantToDama(DamaPiece babu)
        {

        }
        public static void Switch() => Data.isBlack = !Data.isBlack;
        private static int DeterminePiece(int x, int y, bool indent) => 
            (y == 4 || y == 3) ? 0 :
                y < 3 ?
                    indent ? x % 2 == 0 ? 0 : 1 : x % 2 == 0 ? 1 : 0 :
                y > 4 ?
                    indent ? x % 2 == 0 ? 0 : 2 : x % 2 == 0 ? 2 : 0 :
            0;
    }
}
