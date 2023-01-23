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
            if (Data.selectedIdx[0] == -1 && Data.selectedIdx[1] == -1)
            {
                SelectPiece(pbox.Name);
                if (CheckIfCanHit()) MessageBox.Show("Tud ütni");
            }
            else
            {
                Move(GetCoords(pbox.Name)[0], GetCoords(pbox.Name)[1]);
                PesantToDama();
                UpdateDisplay(Data.GameForm.Controls);
                Switch();
            }
        }
        private static int[] GetCoords(string controlName) => new int[2] { Convert.ToInt32(controlName[1].ToString()), Convert.ToInt32(controlName[2].ToString()) };
        private static void SelectPiece(string pbxName) => Data.selectedIdx = GetCoords(pbxName);
        private static bool CheckIfCanHit()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int g = 0; g < 8; g++)
                {
                    if (Data.isBlack && Data._Field[i, g] == 1) if (CheckBlackFTH(i, g)) return true;
                    else if (!Data.isBlack && Data._Field[i, g]==2) if (CheckWhiteFTH(i, g)) return true; 
                }
            }
            return false;
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
        private static void Swap()
        {

        }
        //ALERTA: nincs kész
        public static void Move(int toX, int toY)
        {
            if (Data.selectedIdx[0] == -1 && Data.selectedIdx[1] == -1) return;
            if (Data.isBlack&&Data._Field[Data.selectedIdx[1], Data.selectedIdx[0]] == 1)
            {
                if (CheckBlackFTH(Data.selectedIdx[1], Data.selectedIdx[0]))
                {
                    if (Data.selectedIdx[1] + 2 == toY)
                    {
                        Swap();
                    }        
                }
                else
                {
                    if (Data.selectedIdx[1] + 1 == toY)
                    {
                        Swap();
                    }
                }
            }
            else if (!Data.isBlack&&Data._Field[Data.selectedIdx[1], Data.selectedIdx[0]] == 2)
            {

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
