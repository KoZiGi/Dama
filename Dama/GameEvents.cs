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
        public static bool pieceSelected = false;
        public static List<int> recentSelectedCoordinates = new List<int>();
        public static int[] pieceValues = { 1, 2, 11, 0, 22 };
        public static Bitmap[] Highlightpictures = { Properties.Resources.FeketeHighlight, Properties.Resources.FeherHighlight, Properties.Resources.FeketeDamaHighlight, Properties.Resources.FieldPiros, Properties.Resources.FeherDamaHighlight };
        public static Bitmap[] Naturalpictures = { Properties.Resources.Fekete, Properties.Resources.Feher, Properties.Resources.FeketeDama, Properties.Resources.FieldFeher, Properties.Resources.FeherDama };

        public static void PlayerTurnValidation(object sender, Control.ControlCollection formcontrols)
        {
            Control selectedpiece = sender as Control;
            List<int> coordinatelist = QualityOfLifeFuncs.NameToCoords(selectedpiece);
            if (QualityOfLifeFuncs.SelectionValidate(isBlackOrWhiteTurn, selectedpiece, true)) Move(coordinatelist, formcontrols, selectedpiece);  //white piece click case
            else if (QualityOfLifeFuncs.SelectionValidate(isBlackOrWhiteTurn, selectedpiece, false))  Move(coordinatelist, formcontrols, selectedpiece); //black piece click case
            else if(pieceSelected) Move(coordinatelist, formcontrols, selectedpiece);
        }

        private static void Move(List<int> coordinatelist, Control.ControlCollection formcontrols, Control selectedpiece)
        {
            if (!pieceSelected) selectedPieceDisplay(coordinatelist, formcontrols);  //this makes the posible fields red where the piece could move to
            else if (QualityOfLifeFuncs.repeatClickCheck(coordinatelist))   //this checks if the click on the piece was a repeat click on the same piece or not
            {
                if (QualityOfLifeFuncs.getCoordinatesVal(recentSelectedCoordinates) == 1) fieldSetBackToWhite(formcontrols, +1);
                else if (QualityOfLifeFuncs.getCoordinatesVal(recentSelectedCoordinates) == 2) fieldSetBackToWhite(formcontrols, -1);
            }
            else if (pieceSelected) //if everything is valid then this will make the move or the attack
            {
                if (QualityOfLifeFuncs.getCoordinatesVal(recentSelectedCoordinates) == 2) afterMovementSetting(false, 2, selectedpiece, coordinatelist, formcontrols);
                if (QualityOfLifeFuncs.getCoordinatesVal(recentSelectedCoordinates) == 1) afterMovementSetting(true, 1, selectedpiece, coordinatelist, formcontrols);
            }
        }

        private static void Attack(int y, List<int> selectedCoords, bool isWhite, int value, Control.ControlCollection controls)
        {
            selectedCoords[1] += y;
            selectedCoords[0] += +1;
            if (isWhite && (QualityOfLifeFuncs.getCoordinatesVal(selectedCoords) == 1 || QualityOfLifeFuncs.getCoordinatesVal(selectedCoords) == 11))
            {
                selectedCoords[1] += y;
                selectedCoords[0] += +1;
                if (QualityOfLifeFuncs.getCoordinatesVal(selectedCoords) == 0)
                {
                    QualityOfLifeFuncs.giveCoordinatesVal(selectedCoords, value);
                    QualityOfLifeFuncs.giveCoordinatesVal(recentSelectedCoordinates, 0);
                    QualityOfLifeFuncs.setPboxImg(QualityOfLifeFuncs.findPbox(QualityOfLifeFuncs.convertCoordinatesToName(recentSelectedCoordinates[0], recentSelectedCoordinates[1]), controls), QualityOfLifeFuncs.OriginalPiece(0, Naturalpictures));
                    QualityOfLifeFuncs.setPboxImg(QualityOfLifeFuncs.findPbox(QualityOfLifeFuncs.convertCoordinatesToName(selectedCoords[0], selectedCoords[1]), controls), QualityOfLifeFuncs.OriginalPiece(value, Naturalpictures));
                }
            }
        }

        private static void afterMovementSetting(bool isBlack, int val, Control selectedpiece, List<int> coordinatelist, Control.ControlCollection formcontrols)
        {
            if ((selectedpiece as PictureBox).Tag == "3" && QualityOfLifeFuncs.allyPieceCheck(selectedpiece, recentSelectedCoordinates))
            {
                if (isBlack) QualityOfLifeFuncs.setPboxImg(selectedpiece as PictureBox, Properties.Resources.Fekete);
                else QualityOfLifeFuncs.setPboxImg(selectedpiece as PictureBox, Properties.Resources.Feher);
                QualityOfLifeFuncs.giveCoordinatesVal(coordinatelist, val);
                QualityOfLifeFuncs.giveCoordinatesVal(recentSelectedCoordinates, 0);
                string name = QualityOfLifeFuncs.convertCoordinatesToName(recentSelectedCoordinates[0], recentSelectedCoordinates[1]);
                PictureBox goWhite = QualityOfLifeFuncs.findPbox(name, formcontrols);
                QualityOfLifeFuncs.setPboxImg(goWhite, Properties.Resources.FieldFeher);
                (selectedpiece as PictureBox).Tag = "0";
                QualityOfLifeFuncs.TurnChange(formcontrols, val, coordinatelist);
            }
            else 
            {
                QualityOfLifeFuncs.ClearField(formcontrols);
                fieldSetBackToWhite(formcontrols, +1);
                fieldSetBackToWhite(formcontrols, -1);
            } 
        }

        private static void fieldSetBackToWhite( Control.ControlCollection formcontrols, int val)
        {
            try
            {
                string name1 = QualityOfLifeFuncs.convertCoordinatesToName(recentSelectedCoordinates[0] - 1, recentSelectedCoordinates[1] + (val));
                string name2 = QualityOfLifeFuncs.convertCoordinatesToName(recentSelectedCoordinates[0] + 1, recentSelectedCoordinates[1] + (val));
                PictureBox goWhite = QualityOfLifeFuncs.findPbox(name1, formcontrols);
                QualityOfLifeFuncs.setPboxImg(goWhite, QualityOfLifeFuncs.OriginalPiece(QualityOfLifeFuncs.getCoordinatesVal(QualityOfLifeFuncs.NameToCoords(QualityOfLifeFuncs.findControl(name1, formcontrols))), Naturalpictures));        //pass a variable into a function that returns a bitmap
                goWhite = QualityOfLifeFuncs.findPbox(name2, formcontrols);
                QualityOfLifeFuncs.setPboxImg(goWhite, QualityOfLifeFuncs.OriginalPiece(QualityOfLifeFuncs.getCoordinatesVal(QualityOfLifeFuncs.NameToCoords(QualityOfLifeFuncs.findControl(name2, formcontrols))), Naturalpictures)); ;
                pieceSelected = false;
            }
            catch (IndexOutOfRangeException){}
        }

        private static void selectedPieceDisplay(List<int> coordinatelist, Control.ControlCollection formcontrols)
        {
            pieceSelected = true;
            recentSelectedCoordinates = QualityOfLifeFuncs.copyCoordinates(coordinatelist);
            if (QualityOfLifeFuncs.getCoordinatesValForMove(coordinatelist, 0, 0) == 2) validMovement(formcontrols, coordinatelist, -1, isBlackOrWhiteTurn); //white dama piece selected
            if (QualityOfLifeFuncs.getCoordinatesValForMove(coordinatelist, 0, 0) == 1) validMovement(formcontrols, coordinatelist, +1, isBlackOrWhiteTurn);   //black dama piece selected
        }

        private static void validMovement(Control.ControlCollection formcontrols, List<int> coordinatelist, int val, bool isWhite)
        {
            string validmovesName = "";
            if (isWhite) specificDisplay(coordinatelist, 2, val, validmovesName, formcontrols);
            else specificDisplay(coordinatelist, 1, val, validmovesName, formcontrols);
        }

        private static void specificDisplay(List<int> coordinatelist, int v, int val,string validmovesName, Control.ControlCollection formcontrols)
        {
            if (QualityOfLifeFuncs.getCoordinatesValForMove(coordinatelist, 1, val) != v)
            {
                validmovesName = QualityOfLifeFuncs.convertCoordinatesToName(coordinatelist[0] + 1, coordinatelist[1] + (val));
                try { selectionDisplay(formcontrols, QualityOfLifeFuncs.NameToCoords(QualityOfLifeFuncs.findControl(validmovesName, formcontrols)), QualityOfLifeFuncs.findPbox(validmovesName, formcontrols)); }
                catch (IndexOutOfRangeException) { }
            }
            if (QualityOfLifeFuncs.getCoordinatesValForMove(coordinatelist, -1, val) != v)
            {
                validmovesName = QualityOfLifeFuncs.convertCoordinatesToName(coordinatelist[0] - 1, coordinatelist[1] + (val));
                try { selectionDisplay(formcontrols, QualityOfLifeFuncs.NameToCoords(QualityOfLifeFuncs.findControl(validmovesName, formcontrols)), QualityOfLifeFuncs.findPbox(validmovesName, formcontrols)); }
                catch (IndexOutOfRangeException) { }
            }
        }

        private static void selectionDisplay(Control.ControlCollection formcontrols, List<int> coordinates, PictureBox move)    
        {
            try
            {
                for (int i = 0; i < pieceValues.Length; i++) if(QualityOfLifeFuncs.getCoordinatesVal(coordinates) == pieceValues[i]) QualityOfLifeFuncs.setPboxImg(move, Highlightpictures[i]);
                move.Tag = "3";
            }
            catch (IndexOutOfRangeException){}
        }
    }
}
