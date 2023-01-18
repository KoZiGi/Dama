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
            if (!pieceSelected)
            {
                pieceSelected = true;
                recentSelectedCoordinates = copyCoordinates(coordinatelist);
                if (Data._Field[coordinatelist[0], coordinatelist[1]] == 2) validMovementWhite(formcontrols, coordinatelist); //white dama piece selected
                if (Data._Field[coordinatelist[0], coordinatelist[1]] == 1) validMovementBlack(formcontrols, coordinatelist);   //black dama piece selected
            }
            else if (repeatClickCheck(coordinatelist) && pieceSelected)
            {
                //red squares go back to white
                if (Data._Field[recentSelectedCoordinates[0], recentSelectedCoordinates[1]] == 1)
                {
                    string name1 = $"_{recentSelectedCoordinates[0] - 1}{recentSelectedCoordinates[1] + 1}";
                    string name2 = $"_{recentSelectedCoordinates[0] + 1}{recentSelectedCoordinates[1] + 1}";
                    PictureBox goWhite = formcontrols.Find(name1, true)[0] as PictureBox;
                    goWhite.Image = Properties.Resources.FieldFeher;
                    goWhite = formcontrols.Find(name2, true)[0] as PictureBox;
                    goWhite.Image = Properties.Resources.FieldFeher;
                    pieceSelected = false;
                }
                else if (Data._Field[recentSelectedCoordinates[0], recentSelectedCoordinates[1]] == 2)
                {
                    string nam2 = $"_{recentSelectedCoordinates[0]}{recentSelectedCoordinates[1] - 1}";

                }

                //PictureBox goWhite = formcontrols.Find(name, true)[0] as PictureBox;

            }
            else if ((selectedpiece as PictureBox).Image==Properties.Resources.FieldPiros) 
            {
                if(Data._Field[recentSelectedCoordinates[0], recentSelectedCoordinates[1]] == 2)
                {
                    (selectedpiece as PictureBox).Image = Properties.Resources.Feher;
                    Data._Field[coordinatelist[0], coordinatelist[1]] = 2;
                    Data._Field[recentSelectedCoordinates[0], recentSelectedCoordinates[1]] = 0;
                    string name = $"_{recentSelectedCoordinates[0]}{recentSelectedCoordinates[1]}";
                    PictureBox goWhite = formcontrols.Find(name, true)[0] as PictureBox;
                    goWhite.Image = Properties.Resources.Feher;
                }
                if (Data._Field[recentSelectedCoordinates[0], recentSelectedCoordinates[1]] == 1)
                {
                    (selectedpiece as PictureBox).Image = Properties.Resources.Fekete;
                    Data._Field[coordinatelist[0], coordinatelist[1]] = 1;
                    Data._Field[recentSelectedCoordinates[0], recentSelectedCoordinates[1]] = 0;
                    string name = $"_{recentSelectedCoordinates[0]}{recentSelectedCoordinates[1]}";
                    PictureBox goWhite = formcontrols.Find(name, true)[0] as PictureBox;
                    goWhite.Image = Properties.Resources.Feher;
                }
            }
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
            string validmovesName1 = $"_{coordinatelist[0] + 1}{coordinatelist[1] + 1}";
            string validmovesName2 = $"_{coordinatelist[0] - 1}{coordinatelist[1] + 1}";
            selectionDisplay(formcontrols, validmovesName1);
            selectionDisplay(formcontrols, validmovesName2);
        }

        private static void selectionDisplay(Control.ControlCollection formcontrols, string selectedName)    
        {
            try
            {
                PictureBox move = formcontrols.Find(selectedName, true)[0] as PictureBox;
                move.Image = Properties.Resources.FieldPiros;
            }
            catch (IndexOutOfRangeException){}
        }

        private static void validMovementWhite(Control.ControlCollection formcontrols, List<int> coordinatelist)
        {
            string validmovesName1 = $"_{coordinatelist[0]+1}{coordinatelist[1]-1}";
            string validmovesName2 = $"_{coordinatelist[0]-1}{coordinatelist[1]-1}";
            selectionDisplay(formcontrols, validmovesName1);
            selectionDisplay(formcontrols, validmovesName2);
        }
    }
}
