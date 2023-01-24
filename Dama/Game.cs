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
                UpdateDisplay(Data.GameForm.Controls);
                bool[] canhit = CheckIfCanHit();
                if (canhit[0]) MessageBox.Show($"Tud ütni {(canhit[1] ? "Fekete" : "Fehér")}");
            }
            else
            {
                Move(GetCoords(pbox.Name)[0], GetCoords(pbox.Name)[1]);
                ResetSelect();
                PesantToDama();
                UpdateDisplay(Data.GameForm.Controls);
            }
        }
        private static void ResetSelect() => Data.selectedIdx = new int[2] { -1, -1 };
        private static int[] GetCoords(string controlName) => new int[2] { Convert.ToInt32(controlName[1].ToString()), Convert.ToInt32(controlName[2].ToString()) };
        private static void SelectPiece(string pbxName) => Data.selectedIdx = GetCoords(pbxName);
        private static bool[] CheckIfCanHit()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int g = 0; g < 8; g++)
                {
                    if (Data.isBlack && Data._Field[i, g] == 1) if (CheckBlackFTH(i, g)) return new bool[] { true, true};
                    else if (!Data.isBlack && Data._Field[i, g]==2) if (CheckWhiteFTH(i, g)) return new bool[] { true, false }; 
                }
            }
            return new bool[] { false, false };
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
        private static void Swap(int fromX, int fromY, int toX, int toY)
        {
            int temp = Data._Field[fromY, fromX];
            Data._Field[fromY, fromX] = Data._Field[toY, toX];
            Data._Field[toY, toX] = temp;
        }
        private static void Murder(int X, int Y) => Data._Field[Y, X] = 0;
        //ALERTA: hibakezelés kellene. | kötelezettség is kellene
        public static void Move(int toX, int toY)
        {
            if (Data.selectedIdx[0] == -1 && Data.selectedIdx[1] == -1) return;
            if (Data._Field[Data.selectedIdx[1], Data.selectedIdx[0]] != 0)
            {
                if (Data.isBlack)
                {

                    if (CheckBlackFTH(Data.selectedIdx[1], Data.selectedIdx[0]))
                    {
                        if (Data.selectedIdx[1] + 2 == toY && (Data.selectedIdx[0] + 2 == toX || Data.selectedIdx[0] - 2 == toX))
                        {
                            Swap(Data.selectedIdx[0], Data.selectedIdx[1], toX, toY);
                            Murder(Data.selectedIdx[0]+2==toX ? Data.selectedIdx[0]+1 : Data.selectedIdx[0]-1 ,Data.selectedIdx[1]+1);
                            UpdateDisplay(Data.GameForm.Controls);
                            if (CheckBlackFTH(toY, toX)) Data.selectedIdx = new int[2] { toX, toY };
                            else
                            {
                                ResetSelect();
                                Switch();
                            }
                        }
                    }
                    else
                    {
                        if (Data.selectedIdx[1] + 1 == toY && (Data.selectedIdx[0] + 1 == toX || Data.selectedIdx[0] - 1 == toX))
                        {
                            Swap(Data.selectedIdx[0], Data.selectedIdx[1], toX, toY);
                            Switch();
                            ResetSelect();
                        }
                    }
                }
                else
                {
                    if (CheckWhiteFTH(Data.selectedIdx[1], Data.selectedIdx[0]))
                    {
                        if (Data.selectedIdx[1] - 2 == toY && (Data.selectedIdx[0] + 2 == toX || Data.selectedIdx[0] - 2 == toX))
                        {
                            Swap(Data.selectedIdx[0], Data.selectedIdx[1], toX, toY);
                            Murder(Data.selectedIdx[0] + 2 == toX ? Data.selectedIdx[0] + 1 : Data.selectedIdx[0] - 1, Data.selectedIdx[1] - 1);
                            UpdateDisplay(Data.GameForm.Controls);
                            if (CheckWhiteFTH(toY, toX)) Data.selectedIdx = new int[2] { toX, toY };
                            else
                            {
                                ResetSelect();
                                Switch();
                            }

                        }
                    }
                    else
                    {
                        if (Data.selectedIdx[1] - 1 == toY && (Data.selectedIdx[0] + 1 == toX || Data.selectedIdx[0] - 1 == toX))
                        {
                            Swap(Data.selectedIdx[0], Data.selectedIdx[1], toX, toY);
                            Switch();
                            ResetSelect();
                        }
                    }
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
            Data.GameForm.Text = $"{(Data.isBlack?"fekete":"fehér")} - {(Data.selectedIdx[0]==-1?"választ":"lép")}";
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
            //ha 4,5. sor legyen üres
            (y == 3 || y == 4) ? 0 :
            //ha x<4. sor legyen a behúzásnak megfelelően fekete bábú
            (y < 3) ?
                indent ?
                    x % 2 == 0 ? 0 : 1 :
                    x % 2 == 0 ? 1 : 0 :
            //ha x>5. sor legyen a behúzásnak megfelelően fekete bábú
            (y > 4) ?
                indent ?
                    x % 2 == 0 ? 0 : 2 :
                    x % 2 == 0 ? 2 : 0
            //Ez nem hívódik meg csak kellett az elsehez
            : 0;
    }
}
