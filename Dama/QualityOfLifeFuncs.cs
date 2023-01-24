using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dama
{
    class QualityOfLifeFuncs
    {
        public static PictureBox findPbox(string name, Control.ControlCollection formcontrols) {return formcontrols.Find(name, true)[0] as PictureBox;}

        public static Control findControl(string name, Control.ControlCollection formcontrols) {return formcontrols.Find(name, true)[0] as Control;}

        public static int getCoordinatesValForMove(List<int> recentSelectedCoordinates, int x, int y) {return Data._Field[recentSelectedCoordinates[0] + (x), recentSelectedCoordinates[1] + (y)];}

        public static int getCoordinatesVal(List<int> SelectedCoordinates) {return Data._Field[SelectedCoordinates[0], SelectedCoordinates[1]];}

        public static string convertCoordinatesToName(int x, int y) { return $"_{x}{y}";}

        public static void setPboxImg(PictureBox pictureBox, Bitmap img) => pictureBox.Image = img;

        public static void giveCoordinatesVal(List<int> coordinatelist, int val) => Data._Field[coordinatelist[0], coordinatelist[1]] = val;

        public static bool repeatClickCheck(List<int> coordinates)
        {
            for (int i = 0; i < coordinates.Count; i++) if (coordinates[i] != GameEvents.recentSelectedCoordinates[i]) return false;
            return true;
        }

        public static void ClearField(Control.ControlCollection formControll)
        {
            for (int i = 0; i < formControll.Count; i++)
            {
                if ((formControll[i] as PictureBox).Tag == "3") QualityOfLifeFuncs.setPboxImg((formControll[i] as PictureBox), imgForClearing(getCoordinatesVal(NameToCoords(formControll[i] as PictureBox))));
                (formControll[i] as PictureBox).Tag = "";
            }
        }

        private static Bitmap imgForClearing(int value)
        {
            for (int i = 0; i < GameEvents.pieceValues.Length; i++) if (GameEvents.pieceValues[i] == value) return GameEvents.Naturalpictures[i];
            return null;
        }

        public static List<int> NameToCoords(Control selectedpiece) 
        {
            List<int> coordinatelist = new List<int>();
            for (int i = 1; i <= 2; i++) coordinatelist.Add(Convert.ToInt32(selectedpiece.Name[i].ToString()));
            return coordinatelist;
        }

        public static List<int> copyCoordinates(List<int> coordinatelist)
        {
            List<int> result = new List<int>();
            for (int i = 0; i < coordinatelist.Count; i++) result.Add(coordinatelist[i]);
            return result;
        }

        public static bool SelectionValidate(bool WhiteOrBlack, Control SelectedPiece, bool isWhite) //this funcs returns if the player has the right to move the specific piece
        {
            if (WhiteOrBlack && QualityOfLifeFuncs.getCoordinatesVal(QualityOfLifeFuncs.NameToCoords(SelectedPiece)) == 2 && isWhite) return true;
            if (!WhiteOrBlack && QualityOfLifeFuncs.getCoordinatesVal(QualityOfLifeFuncs.NameToCoords(SelectedPiece)) == 1 && !isWhite) return true;
            return false;
        }

        public static void TurnChange(Control.ControlCollection formcontrols, int val, List<int> coordinatelist)
        {
            GameEvents.pieceSelected = false;
            ClearField(formcontrols);
            DamaCheck(coordinatelist, GameEvents.isBlackOrWhiteTurn, getCoordinatesVal(coordinatelist), formcontrols);
            GameEvents.isBlackOrWhiteTurn = !GameEvents.isBlackOrWhiteTurn;
        }

        public static void DamaCheck(List<int> coordinates, bool isWhite, int value, Control.ControlCollection controls)   //this function check if the recently moved piece stepped into the dama fields  
        {
            if (isWhite && coordinates[1] == 0) piecePbxCoordsChange(coordinates, 22, controls);
            else if (!isWhite && coordinates[1] == 7) piecePbxCoordsChange(coordinates, 11, controls);
            else piecePbxCoordsChange(coordinates, value, controls);
        }

        public static void piecePbxCoordsChange(List<int> coordinates, int val, Control.ControlCollection controls)
        {
            giveCoordinatesVal(coordinates, val);
            setPboxImg(findPbox($"_{coordinates[0]}{coordinates[1]}", controls), OriginalPiece(val, GameEvents.Naturalpictures));
        }

        public static Bitmap OriginalPiece(int val, Bitmap[] array)
        {
            for (int i = 0; i < GameEvents.pieceValues.Length; i++) if (val == GameEvents.pieceValues[i]) return array[i];
            return null;
        }

        public static bool allyPieceCheck(Control selectedpiece, List<int> recentCoords)
        {
            if (getCoordinatesVal(NameToCoords(selectedpiece)) == getCoordinatesVal(recentCoords)) return false;
            return true;
        }

        public static bool isAttack(bool isWhite, List<int> coordinates)
        {
            if (isWhite && getCoordinatesVal(coordinates) == 1) return true;
            if (!isWhite && getCoordinatesVal(coordinates) == 2) return true;
            return false;
        }

    }
}