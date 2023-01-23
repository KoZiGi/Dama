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
            if (Data.selectedIdx[0]==-1 && Data.selectedIdx[1] == -1)
            {
                SelectPiece(pbox.Name);
                for (int i = 0; i < 8; i++)
                {
                    for (int g = 0; g < 8; g++)
                    {
                        switch (Data._Field[i, g])
                        {
                            case 1:
                                if (CheckBlackFTH(i, g)) MessageBox.Show($"Fekete tud ütni\nX:{g} Y:{i}");
                                break;
                            case 2:
                                if (CheckWhiteFTH(i, g)) MessageBox.Show($"Fehér tud ütni\nX:{g} Y:{i}");
                                break;
                        }
                    }
                }
            }
            else
            {
                _BadMove(Convert.ToInt32(pbox.Name.Split('_')[1][0].ToString()), Convert.ToInt32(pbox.Name.Split('_')[1][1].ToString()));
                UpdateDisplay(Data.GameForm.Controls);
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
                    Data._Field[i, g] = DeterminePiece(g, i, indent);
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
            if (i + 2 > 7) return false;
            if (g + 2 < 8)
            {
                if (Data._Field[i+2, g + 2] == 0)
                {
                    return Data._Field[i + 1, g + 1] == 2 || Data._Field[i + 1, g + 1] == 22;
                }
            }
            if (g - 2 > -1)
            {
                if (Data._Field[i + 2, g - 2] == 0)
                {
                    return Data._Field[i + 1, g - 1] == 2 || Data._Field[i + 1, g - 1] == 22;
                }
            }
            return false;
        }
        public static bool CheckWhiteFTH(int i, int g)
        {
            if (i - 2 < 0) return false;
            if (g + 2 < 8) 
            {
                if (Data._Field[i - 2, g + 2] == 0)
                {
                    return Data._Field[i - 1, g + 1] == 1 || Data._Field[i - 1, g + 1] == 11;
                }
            }
            if (g - 2 > -1)
            {
                if (Data._Field[i - 2, g - 2] == 0)
                {
                    return Data._Field[i - 1, g - 1] == 1 || Data._Field[i - 1, g - 1] == 11;
                }
            }
            return false;
        }
        public static void InitMove()
        {
            
        }
        public static void CheckValid(bool canhit)
        {

        }
        public static void _BadMove(int toX, int toY)
        {
            if (Data.selectedIdx[0] != -1 && Data.selectedIdx[1] != -1)
            {
                if (Data._Field[toY, toX] == 0)
                {
                    int temp = Data._Field[Data.selectedIdx[1], Data.selectedIdx[0]];
                    Data._Field[toY, toX] = temp;
                    Data._Field[Data.selectedIdx[1], Data.selectedIdx[0]] = 0;
                    Data.selectedIdx[0] = -1;
                    Data.selectedIdx[1] = -1;
                }
            }
        }
        public static void PesantToDama()
        {
            for (int i = 0; i < 8; i++)
            {
                if (Data._Field[0, i] == 2) Data._Field[0, i] = 22;
                if (Data._Field[7, i] == 1) Data._Field[7, i] = 11;
            }
        }
        public static void UpdateDisplay(Control.ControlCollection pboxes)
        {
            foreach (Control pbox in pboxes)
            {
                if (pbox is PictureBox)
                {
                    int x = Convert.ToInt32(pbox.Name.Split('_')[1][0].ToString()), y = Convert.ToInt32(pbox.Name.Split('_')[1][1].ToString());
                    DeterminePicture(Data._Field[y, x], pbox as PictureBox);
                }
            }
        }
        private static void DeterminePicture(int inp, PictureBox pbx)
        {
            switch (inp)
            {
                case 1:
                    pbx.Image = Properties.Resources.Fekete;
                    break;
                case 11:
                    pbx.Image = Properties.Resources.FeketeDama;
                    break;
                case 2:
                    pbx.Image = Properties.Resources.Feher;
                    break;
                case 22:
                    pbx.Image = Properties.Resources.FeherDama;
                    break;
                default:
                    pbx.Image = Properties.Resources.semmi;
                    break;
            }
        }
        public static void Switch() => Data.isBlack = !Data.isBlack;
        private static int DeterminePiece(int x, int y, bool indent) =>
            (y == 3 || y == 4) ? 0 :
            (y < 3) ?
                indent ?
                    x % 2 == 0 ? 0 : 1 :
                    x % 2 == 0 ? 1 : 0 :
            (y > 4) ?
                indent ?
                    x % 2 == 0 ? 0 : 2 :
                    x % 2 == 0 ? 2 : 0
            : 0;
                    
    }
}
