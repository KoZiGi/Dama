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
                //GameEvents.selectionDisplay(Data.GameForm.Controls, GetCoords(pbox.Name).ToList(), pbox);
                UpdateDisplay(Data.GameForm.Controls);
            }
            else
            {
                Move(GetCoords(pbox.Name)[0], GetCoords(pbox.Name)[1]);
                PesantToDama();
                ResetSelect();
                UpdateDisplay(Data.GameForm.Controls);
            }
        }
        private static bool CheckIfHittingPiece()
        {
            for (int i = 0; i < Data.HitReqCoords.Count; i++)
            {
                if (Data.selectedIdx[1] == Data.HitReqCoords[i][0] && Data.selectedIdx[0] == Data.HitReqCoords[i][1]) return true;
            }
            return false;
        }
        private static void ResetSelect() => Data.selectedIdx = new int[2] { -1, -1 };
        private static int[] GetCoords(string controlName) => new int[2] { Convert.ToInt32(controlName[1].ToString()), Convert.ToInt32(controlName[2].ToString()) };
        private static void SelectPiece(string pbxName) => Data.selectedIdx = GetCoords(pbxName);
        private static bool CheckIfCanMove(int x, int y)
        {
                if (x + 1 < 8)
                {
                    if (Data._Field[y, x]%11 == 0 && Data._Field[y, x] != 0)
                    {
                        if (CheckDamaMove(x, y))
                        {
                            return true;
                        }
                    }
                    else if (Data._Field[y + (Data.isBlack ? 1 : -1), x + 1] == 0) return true;
                }
                if (x - 1 > -1)
                {
                    if (Data._Field[y + (Data.isBlack ? 1 : -1), x - 1] == 0) return true;
                }
            if (Data.isBlack ? CheckBlackFTH(y, x) : CheckWhiteFTH(y, x))
            {
                return true;
            }
            return false;
        }
        private static bool CheckDamaMove(int x, int y)
        {
            if (y+1<8)
            {
                if (x+1<8 && Data._Field[y+1, x + 1] == 0)
                    return true;
                if (x - 1 > -1 && Data._Field[y + 1, x - 1] == 0)
                    return true;
                if (Data._Field[y + 1, x + 1] == (Data.isBlack ? 2 : 1) && y + 2 < 8 && x + 2 < 8 && Data._Field[y + 2, x + 2] == 0)
                    return true;
                if (Data._Field[y + 1, x - 1] == (Data.isBlack ? 2 : 1) && y + 2 < 8 && x - 2 > -1 && Data._Field[y + 2, x - 2] == 0)
                    return true;
            }
            if (y - 1 > -1)
            {
                if (x + 1 < 8 && Data._Field[y - 1, x + 1] == 0)
                    return true;
                if (x - 1 > -1 && Data._Field[y - 1, x - 1] == 0)
                    return true;
                if (Data._Field[y - 1, x + 1] == (Data.isBlack ? 2 : 1) && y - 2 < 8 && x + 2 < 8 && Data._Field[y - 2, x + 2] == 0)
                    return true;
                if (Data._Field[y - 1, x - 1] == (Data.isBlack ? 2 : 1) && y - 2 < 8 && x - 2 > -1 && Data._Field[y - 2, x - 2] == 0)
                    return true;
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
                if (Data._Field[i+2, g + 2] == 0 && (Data._Field[i + 1, g + 1] == 2 || Data._Field[i + 1, g + 1] == 22))
                {
                    return true;
                }
            }
            if (g - 2 > -1)
            {
                if (Data._Field[i + 2, g - 2] == 0 && (Data._Field[i + 1, g - 1] == 2 || Data._Field[i + 1, g - 1] == 22))
                {
                    return true;
                }
            }
            return false;
        }
        public static bool CheckWhiteFTH(int i, int g)
        {
            if (i - 2 < 0) return false;
            if (g + 2 < 8) 
            {
                if (Data._Field[i - 2, g + 2] == 0 && (Data._Field[i - 1, g + 1] == 1 || Data._Field[i - 1, g + 1] == 11))
                {
                    return true;
                }
            }
            if (g - 2 > -1)
            {
                if (Data._Field[i - 2, g - 2] == 0 && (Data._Field[i - 1, g - 1] == 1 || Data._Field[i - 1, g - 1] == 11))
                {
                    return true;
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
                if (Data._Field[Data.selectedIdx[1], Data.selectedIdx[0]] > 10)
                {
                    DamaMove(toX, toY);
                }
                else
                {
                    if (Data.isBlack && Data._Field[Data.selectedIdx[1], Data.selectedIdx[0]]==1)
                    {
                        GetHits();
                        if (CheckBlackFTH(Data.selectedIdx[1], Data.selectedIdx[0]))
                        {
                            if (CheckIfHittingPiece())
                            {
                                if (Data.selectedIdx[1] + 2 == toY && (Data.selectedIdx[0] + 2 == toX || Data.selectedIdx[0] - 2 == toX))
                                {
                                    Swap(Data.selectedIdx[0], Data.selectedIdx[1], toX, toY);
                                    Murder(Data.selectedIdx[0] + 2 == toX ? Data.selectedIdx[0] + 1 : Data.selectedIdx[0] - 1, Data.selectedIdx[1] + 1);
                                    UpdateDisplay(Data.GameForm.Controls);
                                    if (CheckBlackFTH(toY, toX))
                                    {
                                        Data.selectedIdx = new int[2] { toX, toY };
                                        GetHits();
                                    }
                                    else
                                    {
                                        ResetSelect();
                                        Switch();
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (Data.selectedIdx[1] + 1 == toY && (Data.selectedIdx[0] + 1 == toX || Data.selectedIdx[0] - 1 == toX))
                            {
                                if (Data.HitReqCoords.Count == 0)
                                {
                                    Swap(Data.selectedIdx[0], Data.selectedIdx[1], toX, toY);
                                    Switch();
                                    ResetSelect();
                                }
                            }
                        }
                    }
                    else if (Data._Field[Data.selectedIdx[1],Data.selectedIdx[0]]==2 && !Data.isBlack)
                    {
                        if (CheckWhiteFTH(Data.selectedIdx[1], Data.selectedIdx[0]))
                        {
                            GetHits();
                            if (CheckIfHittingPiece())
                            {
                                if (Data.selectedIdx[1] - 2 == toY && (Data.selectedIdx[0] + 2 == toX || Data.selectedIdx[0] - 2 == toX) && CheckIfHittingPiece())
                                {
                                    Swap(Data.selectedIdx[0], Data.selectedIdx[1], toX, toY);
                                    Murder(Data.selectedIdx[0] + 2 == toX ? Data.selectedIdx[0] + 1 : Data.selectedIdx[0] - 1, Data.selectedIdx[1] - 1);
                                    UpdateDisplay(Data.GameForm.Controls);
                                    if (CheckWhiteFTH(toY, toX))
                                    {
                                        Data.selectedIdx = new int[2] { toX, toY };
                                        GetHits();
                                    }
                                    else
                                    {
                                        ResetSelect();
                                        Switch();
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (Data.selectedIdx[1] - 1 == toY && (Data.selectedIdx[0] + 1 == toX || Data.selectedIdx[0] - 1 == toX))
                            {
                                if (Data.HitReqCoords.Count == 0)
                                {
                                    Swap(Data.selectedIdx[0], Data.selectedIdx[1], toX, toY);
                                    Switch();
                                    ResetSelect();
                                }
                            }
                        }
                    }
                    else
                    {
                        ResetSelect();
                        GetHits();
                    }
                }
                
            }
        }
        private static void DamaMove(int toX, int toY)
        {
            if (ValidDir(toX, toY) && Data.isBlack ? Data._Field[Data.selectedIdx[1], Data.selectedIdx[0]]==11 : Data._Field[Data.selectedIdx[1], Data.selectedIdx[0]] == 22)
            {
                if (Data.HitReqCoords.Count == 0)
                {
                    Swap(Data.selectedIdx[0], Data.selectedIdx[1], toX, toY);
                    ResetSelect();
                    Switch();
                }
            }
        }
        private static bool ValidDamaMove(int toX, int toY, int dirX, int dirY, bool mierda)
        {
            int x = Data.selectedIdx[0] + dirX, y = Data.selectedIdx[1] + dirY;

            for (int i = 0; i < Math.Abs(Data.selectedIdx[1]-toY); i++)
            {
                if (Data._Field[y, x] == 0)
                {
                    x += dirX;
                    y += dirY;
                }
                else if (Data._Field[y, x] == (Data.isBlack ? 2 : 1) || Data._Field[y, x] == (Data.isBlack ? 22 : 11))
                {
                    if (mierda)
                        Murder(x, y);
                    return x + dirX == toX && y + dirY == toY;
                }
                else return false;
            }
            return true;
        }
        private static bool ValidDir(int toX, int toY)
        {
            if (Data.selectedIdx[0]<toX && Data.selectedIdx[1] > toY && Data.selectedIdx[0]+Data.selectedIdx[1]==toX+toY)
                return ValidDamaMove(toX,toY, 1, -1, true);
            if (Data.selectedIdx[0]>toX && Data.selectedIdx[1]>toY && Data.selectedIdx[0] - Data.selectedIdx[1] == toX - toY)
                return ValidDamaMove(toX,toY,-1,-1, true);
            if (Data.selectedIdx[0] < toX && Data.selectedIdx[1] < toY && Data.selectedIdx[0] - Data.selectedIdx[1] == toX - toY)
                return ValidDamaMove(toX,toY,1,1, true);
            if (Data.selectedIdx[0] > toX && Data.selectedIdx[1] < toY && Data.selectedIdx[0] + Data.selectedIdx[1] == toX + toY)
                return ValidDamaMove(toX,toY,-1,1, true);
            return false;
        }
        
        public static void PesantToDama()
        {
            for (int i = 0; i < 8; i++)
            {
                if (Data._Field[0, i] == 2) Data._Field[0, i] = 22;
                if (Data._Field[7, i] == 1) Data._Field[7, i] = 11;
            }
            UpdateDisplay(Data.GameForm.Controls);
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
            Data.GameForm.Text = $"{(Data.isBlack?"Fekete":"Fehér")} - {(Data.selectedIdx[0]==-1?"választ":"lép")}";
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
        public static void Switch() 
        {
            if (HasLost())
            {
                MessageBox.Show($"Győzött {(Data.isBlack ? "Fekete" : "Fehér")}");
                Application.Exit();
            }
            Data.HitReqCoords.Clear();
            Data.isBlack = !Data.isBlack;
            GetHits();
        }
        private static void GetHits()
        {
            Data.HitReqCoords.Clear();
            for (int i = 0; i < 8; i++)
            {
                for (int g = 0; g < 8; g++)
                {
                    if (Data._Field[i, g] != 0)
                    {

                        if (Data.isBlack && Data._Field[i,g]==1)
                        {
                            if (CheckBlackFTH(i, g)) Data.HitReqCoords.Add(new int[] { i, g });
                        }
                        else if(!Data.isBlack && Data._Field[i,g]==2)
                        {
                            if (CheckWhiteFTH(i, g)) Data.HitReqCoords.Add(new int[] { i, g });
                        }
                    }
                }
            }
        }
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
        //WINCHECK™™™™™™™
        private static bool HasLost()
        {
            int totalPieces = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int g = 0; g < 8; g++)
                    if (Data._Field[i, g] == (Data.isBlack ? 2 : 1) || Data._Field[i, g] == (Data.isBlack ? 22 : 11)) totalPieces++;
            }
            if (PermaStuck())
            {
                return true;
            }
            return totalPieces==0;
        }
        private static bool PermaStuck()
        {
            PesantToDama();
            int movables = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int g = 0; g < 8; g++)
                {
                    if (Data.isBlack && (Data._Field[i,g]==1 || Data._Field[i,g] == 11))
                    {
                        if (CheckIfCanMove(g, i)) movables++;
                    }
                    else if (!Data.isBlack && (Data._Field[i, g] == 2 || Data._Field[i, g] == 22))
                    {
                        if (CheckIfCanMove(g, i)) movables++;
                    }
                }
            }
            return movables==0;
        }
    }
}
