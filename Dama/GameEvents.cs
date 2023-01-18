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
            else if (repeatClickCheck(coordinatelist))
            {
                if (getCoordinatesVal(recentSelectedCoordinates, 0, 0) == 1) fieldSetBackToWhite(formcontrols, +1);
                else if (getCoordinatesVal(recentSelectedCoordinates, 0, 0) == 2) fieldSetBackToWhite(formcontrols, -1);
            }
            else if (pieceSelected) 
            {
                if(getCoordinatesVal(recentSelectedCoordinates, 0, 0) == 2) afterMovementSetting(false, 2, selectedpiece, coordinatelist, formcontrols);
                if(getCoordinatesVal(recentSelectedCoordinates, 0, 0) == 1) afterMovementSetting(true, 1, selectedpiece, coordinatelist, formcontrols);
                pieceSelected = false;
                ClearField(formcontrols);
            }
        }

        private static int getCoordinatesVal(List<int> recentSelectedCoordinates, int x, int y)
        {
            return Data._Field[recentSelectedCoordinates[0]+(x), recentSelectedCoordinates[1]+(y)];
        }

        private static void afterMovementSetting(bool isBlack, int val, Control selectedpiece, List<int> coordinatelist, Control.ControlCollection formcontrols)
        {
            if (isBlack) setPboxImg(selectedpiece as PictureBox, Properties.Resources.Fekete);
            else setPboxImg(selectedpiece as PictureBox, Properties.Resources.Feher);
            giveCoordinatesVal(coordinatelist, val);          
            giveCoordinatesVal(recentSelectedCoordinates, 0);
            string name = $"_{recentSelectedCoordinates[0]}{recentSelectedCoordinates[1]}";
            PictureBox goWhite = findPbox(name, formcontrols);
            setPboxImg(goWhite, Properties.Resources.FieldFeher);
            (selectedpiece as PictureBox).Tag = "0";
        }

        private static PictureBox findPbox(string name, Control.ControlCollection formcontrols)
        {
            return formcontrols.Find(name, true)[0] as PictureBox;
        }

        private static Control findControl(string name, Control.ControlCollection formcontrols)
        {
            return formcontrols.Find(name, true)[0] as Control;
        }

        private static void setPboxImg(PictureBox pictureBox, Bitmap img) => pictureBox.Image = img;

        private static void giveCoordinatesVal(List<int> coordinatelist, int val) => Data._Field[coordinatelist[0], coordinatelist[1]] = val;

        private static void fieldSetBackToWhite( Control.ControlCollection formcontrols, int val)
        {
            string name1 = $"_{recentSelectedCoordinates[0] - 1}{recentSelectedCoordinates[1] +(val)}";
            string name2 = $"_{recentSelectedCoordinates[0] + 1}{recentSelectedCoordinates[1] +(val)}";
            PictureBox goWhite = findPbox(name1, formcontrols);
            setPboxImg(goWhite, Properties.Resources.FieldFeher);
            goWhite = findPbox(name2, formcontrols);
            setPboxImg(goWhite, Properties.Resources.FieldFeher);
            pieceSelected = false;
        }

        private static void selectedPieceDisplayAndMovement(List<int> coordinatelist, Control.ControlCollection formcontrols)
        {
            pieceSelected = true;
            recentSelectedCoordinates = copyCoordinates(coordinatelist);
            if (getCoordinatesVal(coordinatelist, 0, 0) == 2) validMovement(formcontrols, coordinatelist, -1); //white dama piece selected
            if (getCoordinatesVal(coordinatelist, 0, 0) == 1) validMovement(formcontrols, coordinatelist, +1);   //black dama piece selected
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

        private static List<int> positionOnGameField(Control selectedpiece) //returns a list with 2 elements [x,y]
        {
            List<int> coordinatelist = new List<int>();
            for (int i = 1; i <= 2; i++) coordinatelist.Add(Convert.ToInt32(selectedpiece.Name[i].ToString()));
            return coordinatelist;
        }

        private static void validMovement(Control.ControlCollection formcontrols, List<int> coordinatelist, int val)
        {
            string validmovesName = $"_{coordinatelist[0] + 1}{coordinatelist[1] +(val)}";
            try { selectionDisplay(formcontrols, positionOnGameField(findControl(validmovesName, formcontrols)), findPbox(validmovesName, formcontrols)); }
            catch (IndexOutOfRangeException) { }
            validmovesName = $"_{coordinatelist[0] - 1}{coordinatelist[1] +(val)}";
            try { selectionDisplay(formcontrols, positionOnGameField(findControl(validmovesName, formcontrols)), findPbox(validmovesName, formcontrols)); }
            catch (IndexOutOfRangeException){}
        }

        private static void selectionDisplay(Control.ControlCollection formcontrols, List<int> coordinates, PictureBox move)    
        {
            try
            {
                if (getCoordinatesVal(coordinates, 0, 0) == 1) setPboxImg(move, Properties.Resources.FeketeHighlight);
                if (getCoordinatesVal(coordinates, 0, 0) == 2) setPboxImg(move, Properties.Resources.FeherHighlight);
                if (getCoordinatesVal(coordinates, 0, 0) == 11) setPboxImg(move, Properties.Resources.FeketeDamaHighlight);
                if (getCoordinatesVal(coordinates, 0, 0) == 0) setPboxImg(move, Properties.Resources.FieldPiros);
                if (getCoordinatesVal(coordinates, 0, 0) == 22) setPboxImg(move, Properties.Resources.FeherDamaHighlight);
                move.Tag = "3";
            }
            catch (IndexOutOfRangeException){}
        }

        private static void ClearField(Control.ControlCollection formControll)
        {
            for (int i = 0; i < formControll.Count; i++)
                if ((formControll[i] as PictureBox).Tag == "3") setPboxImg((formControll[i] as PictureBox), Properties.Resources.FieldFeher);
        }
    }
}
