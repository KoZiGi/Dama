using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dama
{
    class GameEvents
    {

        static bool pieceSelected = false;
        static List<int> recentSelectedCoordinates = new List<int>();

        public static void Position(object sender, Control.ControlCollection formcontrols)
        {
            Control selectedpiece = sender as Control;
            List<int> coordinatelist = positionOnGameField(selectedpiece);
            if (!pieceSelected) selectedPieceDisplayAndMovement(coordinatelist, formcontrols);
            else if (repeatClickCheck(coordinatelist) && pieceSelected)
            {
                if (Data._Field[recentSelectedCoordinates[0], recentSelectedCoordinates[1]] == 1)
                {
                    string name1 = $"_{recentSelectedCoordinates[0] - 1}{recentSelectedCoordinates[1] + 1}";
                    string name2 = $"_{recentSelectedCoordinates[0] + 1}{recentSelectedCoordinates[1] + 1}";
                    fieldSetBackToWhite(name1, name2, formcontrols);
                    string name = "";
                    for (int i = -1; i < 2; i++)
                    {
                        name = $"_{recentSelectedCoordinates[0] - 1}{recentSelectedCoordinates[1] + 1}";
                    }
                }
                else if (Data._Field[recentSelectedCoordinates[0], recentSelectedCoordinates[1]] == 2)
                {
                    string name1 = $"_{recentSelectedCoordinates[0] - 1}{recentSelectedCoordinates[1] -1}";
                    string name2 = $"_{recentSelectedCoordinates[0] + 1}{recentSelectedCoordinates[1] - 1}";
                    fieldSetBackToWhite(name1, name2, formcontrols);
                }
            }
            else if (pieceSelected) 
            {
                if(Data._Field[recentSelectedCoordinates[0], recentSelectedCoordinates[1]] == 2)
                {
                    (selectedpiece as PictureBox).Image = Properties.Resources.Feher;
                    Data._Field[coordinatelist[0], coordinatelist[1]] = 2;
                    Data._Field[recentSelectedCoordinates[0], recentSelectedCoordinates[1]] = 0;
                    string name = $"_{recentSelectedCoordinates[0]}{recentSelectedCoordinates[1]}";
                    PictureBox goWhite = formcontrols.Find(name, true)[0] as PictureBox;
                    goWhite.Image = Properties.Resources.FieldFeher;
                    (selectedpiece as PictureBox).Tag = "0";
                }
                if (Data._Field[recentSelectedCoordinates[0], recentSelectedCoordinates[1]] == 1)
                {
                    (selectedpiece as PictureBox).Image = Properties.Resources.Fekete;
                    Data._Field[coordinatelist[0], coordinatelist[1]] = 1;
                    Data._Field[recentSelectedCoordinates[0], recentSelectedCoordinates[1]] = 0;
                    string name = $"_{recentSelectedCoordinates[0]}{recentSelectedCoordinates[1]}";
                    PictureBox goWhite = formcontrols.Find(name, true)[0] as PictureBox;
                    goWhite.Image = Properties.Resources.FieldFeher;
                    (selectedpiece as PictureBox).Tag = "0";
                }
                pieceSelected = false;
                ClearField(formcontrols);
            }
        }

        private static void fieldSetBackToWhite(string name1, string name2, Control.ControlCollection formcontrols)
        {
            PictureBox goWhite = formcontrols.Find(name1, true)[0] as PictureBox;
            goWhite.Image = Properties.Resources.FieldFeher;
            goWhite = formcontrols.Find(name2, true)[0] as PictureBox;
            goWhite.Image = Properties.Resources.FieldFeher;
            pieceSelected = false;
        }

        private static void selectedPieceDisplayAndMovement(List<int> coordinatelist, Control.ControlCollection formcontrols)
        {
            pieceSelected = true;
            recentSelectedCoordinates = copyCoordinates(coordinatelist);
            if (Data._Field[coordinatelist[0], coordinatelist[1]] == 2) validMovementWhite(formcontrols, coordinatelist); //white dama piece selected
            if (Data._Field[coordinatelist[0], coordinatelist[1]] == 1) validMovementBlack(formcontrols, coordinatelist);   //black dama piece selected
        }

        private static bool repeatClickCheck(List<int> coordinates)
        {
            for (int i = 0; i < coordinates.Count; i++) if(coordinates[i] != recentSelectedCoordinates[i]) return false;
            return true;
        }

        private static List<int> copyCoordinates(List<int> coordinatelist)
        {
            List<int> result = new List<int>();
            for (int i = 0; i < coordinatelist.Count; i++) result.Add(coordinatelist[i]);
            return result;
        }

        private static List<int> positionOnGameField(Control selectedpiece)
        {
            List<int> coordinatelist = new List<int>(); 
            coordinatelist.Add(Convert.ToInt32(selectedpiece.Name[1].ToString()));
            coordinatelist.Add(Convert.ToInt32(selectedpiece.Name[2].ToString()));
            return coordinatelist;
        }

        private static void validMovementBlack(Control.ControlCollection formcontrols, List<int> coordinatelist)
        {
            string validmovesName = $"_{coordinatelist[0] + 1}{coordinatelist[1] + 1}";
            selectionDisplay(formcontrols, positionOnGameField(formcontrols.Find(validmovesName, true)[0] as Control), formcontrols.Find(validmovesName, true)[0] as PictureBox);
            validmovesName = $"_{coordinatelist[0] - 1}{coordinatelist[1] + 1}";
            selectionDisplay(formcontrols, positionOnGameField(formcontrols.Find(validmovesName, true)[0] as Control), formcontrols.Find(validmovesName, true)[0] as PictureBox);
        }

        private static void selectionDisplay(Control.ControlCollection formcontrols, List<int> coordinates, PictureBox move)    
        {
            try
            {
                if (Data._Field[coordinates[0], coordinates[1]]==1) move.Image = Properties.Resources.FeketeHighlight;
                if (Data._Field[coordinates[0], coordinates[1]]==2) move.Image = Properties.Resources.FeherHighlight;
                if (Data._Field[coordinates[0], coordinates[1]]==11) move.Image = Properties.Resources.FeketeDamaHighlight;
                if (Data._Field[coordinates[0], coordinates[1]]==0) move.Image = Properties.Resources.FieldPiros;
                if (Data._Field[coordinates[0], coordinates[1]]==22) move.Image = Properties.Resources.FeherDamaHighlight;
                move.Tag = "3";
            }
            catch (IndexOutOfRangeException){}
        }

        private static void validMovementWhite(Control.ControlCollection formcontrols, List<int> coordinatelist)
        {
            string validmovesName = $"_{coordinatelist[0] + 1}{coordinatelist[1] - 1}";
            selectionDisplay(formcontrols, positionOnGameField(formcontrols.Find(validmovesName, true)[0] as Control), formcontrols.Find(validmovesName, true)[0] as PictureBox);
            validmovesName = $"_{coordinatelist[0] - 1}{coordinatelist[1] - 1}";
            selectionDisplay(formcontrols, positionOnGameField(formcontrols.Find(validmovesName, true)[0] as Control), formcontrols.Find(validmovesName, true)[0] as PictureBox);
        }

        private static void ClearField(Control.ControlCollection formControll)
        {
            for (int i = 0; i < formControll.Count; i++) 
            if ((formControll[i] as PictureBox).Tag == "3")
            (formControll[i] as PictureBox).Image = Properties.Resources.FieldFeher;
        }
    }
}
