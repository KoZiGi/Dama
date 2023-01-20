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
        public static bool isBlackOrWhiteTurn = true; //this variable shows wich players turn is present, true is white, false is black
        static bool pieceSelected = false;
        public static List<int> recentSelectedCoordinates = new List<int>();
        static int[] pieceValues = { 1, 2, 11, 0, 22 };
        static Bitmap[] pictures = { Properties.Resources.FeketeHighlight, Properties.Resources.FeherHighlight, Properties.Resources.FeketeDamaHighlight, Properties.Resources.FieldPiros, Properties.Resources.FeherDamaHighlight };

        public static void PlayerTurnValidation(object sender, Control.ControlCollection formcontrols)
        {
            Control selectedpiece = sender as Control;
            List<int> coordinatelist = QualityOfLifeFuncs.positionOnGameField(selectedpiece);
            if (QualityOfLifeFuncs.SelectionValidate(isBlackOrWhiteTurn, selectedpiece, true)) Move(coordinatelist, formcontrols, selectedpiece);  //white piece click case
            else if (QualityOfLifeFuncs.SelectionValidate(isBlackOrWhiteTurn, selectedpiece, false))  Move(coordinatelist, formcontrols, selectedpiece); //black piece click case
            else Move(coordinatelist, formcontrols, selectedpiece);
        }

        private static void Move(List<int> coordinatelist, Control.ControlCollection formcontrols, Control selectedpiece)
        {
            if (!pieceSelected) selectedPieceDisplayAndMovement(coordinatelist, formcontrols);  //this makes the posible fields red where the piece could move to
            else if (QualityOfLifeFuncs.repeatClickCheck(coordinatelist))   //this checks if the click on the piece was a repeat click on the same piece or not
            {
                if (QualityOfLifeFuncs.getCoordinatesVal(recentSelectedCoordinates) == 1) fieldSetBackToWhite(formcontrols, +1);
                else if (QualityOfLifeFuncs.getCoordinatesVal(recentSelectedCoordinates) == 2) fieldSetBackToWhite(formcontrols, -1);
            }
            else if (pieceSelected) //if everything is valid then this will make the move or the attack
            {
                if (QualityOfLifeFuncs.getCoordinatesVal(recentSelectedCoordinates) == 2) afterMovementSetting(false, 2, selectedpiece, coordinatelist, formcontrols);
                if (QualityOfLifeFuncs.getCoordinatesVal(recentSelectedCoordinates) == 1) afterMovementSetting(true, 1, selectedpiece, coordinatelist, formcontrols);
                pieceSelected = false;
                QualityOfLifeFuncs.ClearField(formcontrols);
                isBlackOrWhiteTurn = !isBlackOrWhiteTurn;
            }
        }

        private static void afterMovementSetting(bool isBlack, int val, Control selectedpiece, List<int> coordinatelist, Control.ControlCollection formcontrols)
        {
            if (isBlack) QualityOfLifeFuncs.setPboxImg(selectedpiece as PictureBox, Properties.Resources.Fekete);
            else QualityOfLifeFuncs.setPboxImg(selectedpiece as PictureBox, Properties.Resources.Feher);
            QualityOfLifeFuncs.giveCoordinatesVal(coordinatelist, val);
            QualityOfLifeFuncs.giveCoordinatesVal(recentSelectedCoordinates, 0);
            string name = $"_{recentSelectedCoordinates[0]}{recentSelectedCoordinates[1]}";
            PictureBox goWhite = QualityOfLifeFuncs.findPbox(name, formcontrols);
            QualityOfLifeFuncs.setPboxImg(goWhite, Properties.Resources.FieldFeher);
            (selectedpiece as PictureBox).Tag = "0";
        }

        private static void fieldSetBackToWhite( Control.ControlCollection formcontrols, int val)
        {
            string name1 = $"_{recentSelectedCoordinates[0] - 1}{recentSelectedCoordinates[1] +(val)}";
            string name2 = $"_{recentSelectedCoordinates[0] + 1}{recentSelectedCoordinates[1] +(val)}";
            PictureBox goWhite = QualityOfLifeFuncs.findPbox(name1, formcontrols);
            QualityOfLifeFuncs.setPboxImg(goWhite, Properties.Resources.FieldFeher);
            goWhite = QualityOfLifeFuncs.findPbox(name2, formcontrols);
            QualityOfLifeFuncs.setPboxImg(goWhite, Properties.Resources.FieldFeher);
            pieceSelected = false;
        }

        private static void selectedPieceDisplayAndMovement(List<int> coordinatelist, Control.ControlCollection formcontrols)
        {
            pieceSelected = true;
            recentSelectedCoordinates = QualityOfLifeFuncs.copyCoordinates(coordinatelist);
            if (QualityOfLifeFuncs.getCoordinatesValForMove(coordinatelist, 0, 0) == 2) validMovement(formcontrols, coordinatelist, -1); //white dama piece selected
            if (QualityOfLifeFuncs.getCoordinatesValForMove(coordinatelist, 0, 0) == 1) validMovement(formcontrols, coordinatelist, +1);   //black dama piece selected
        }

        private static void validMovement(Control.ControlCollection formcontrols, List<int> coordinatelist, int val)
        {
            string validmovesName = $"_{coordinatelist[0] + 1}{coordinatelist[1] +(val)}";
            try { selectionDisplay(formcontrols, QualityOfLifeFuncs.positionOnGameField(QualityOfLifeFuncs.findControl(validmovesName, formcontrols)), QualityOfLifeFuncs.findPbox(validmovesName, formcontrols)); }
            catch (IndexOutOfRangeException) { }
            validmovesName = $"_{coordinatelist[0] - 1}{coordinatelist[1] +(val)}";
            try { selectionDisplay(formcontrols, QualityOfLifeFuncs.positionOnGameField(QualityOfLifeFuncs.findControl(validmovesName, formcontrols)), QualityOfLifeFuncs.findPbox(validmovesName, formcontrols)); }
            catch (IndexOutOfRangeException){}
        }

        private static void selectionDisplay(Control.ControlCollection formcontrols, List<int> coordinates, PictureBox move)    
        {
            try
            {
                for (int i = 0; i < pieceValues.Length; i++) if(QualityOfLifeFuncs.getCoordinatesVal(coordinates) == pieceValues[i]) QualityOfLifeFuncs.setPboxImg(move, pictures[i]);
                move.Tag = "3";
            }
            catch (IndexOutOfRangeException){}
        }
    }
}
