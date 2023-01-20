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

        public static Control findControl(string name, Control.ControlCollection formcontrols) { return formcontrols.Find(name, true)[0] as Control;}

        public static int getCoordinatesValForMove(List<int> recentSelectedCoordinates, int x, int y) {return Data._Field[recentSelectedCoordinates[0] + (x), recentSelectedCoordinates[1] + (y)];}

        public static int getCoordinatesVal(List<int> SelectedCoordinates) {return Data._Field[SelectedCoordinates[0], SelectedCoordinates[1]];}

        public static bool repeatClickCheck(List<int> coordinates)
        {
            for (int i = 0; i < coordinates.Count; i++) if (coordinates[i] != GameEvents.recentSelectedCoordinates[i]) return false;
            return true;
        }

        public static void ClearField(Control.ControlCollection formControll)
        {
            for (int i = 0; i < formControll.Count; i++)
                if ((formControll[i] as PictureBox).Tag == "3") QualityOfLifeFuncs.setPboxImg((formControll[i] as PictureBox), Properties.Resources.FieldFeher);
        }

        public static List<int> positionOnGameField(Control selectedpiece) //returns a list with 2 elements [x,y]
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
            if (WhiteOrBlack && QualityOfLifeFuncs.getCoordinatesVal(QualityOfLifeFuncs.positionOnGameField(SelectedPiece)) == 2 && isWhite) return true;
            if (WhiteOrBlack==false && QualityOfLifeFuncs.getCoordinatesVal(QualityOfLifeFuncs.positionOnGameField(SelectedPiece)) == 1 && isWhite==false) return true;
            return false;
        }

        public static void setPboxImg(PictureBox pictureBox, Bitmap img) => pictureBox.Image = img;

        public static void giveCoordinatesVal(List<int> coordinatelist, int val) => Data._Field[coordinatelist[0], coordinatelist[1]] = val;
    }
}
