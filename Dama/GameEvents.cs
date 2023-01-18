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

        public static void Position(object sender, Control.ControlCollection formcontrols)
        {
            Control selectedpiece = sender as Control;
            List<int> coordinatelist = new List<int>();      
            coordinatelist.Add(Convert.ToInt32(selectedpiece.Name[1].ToString()));
            coordinatelist.Add(Convert.ToInt32(selectedpiece.Name[2].ToString()));
            if(Data._Field[coordinatelist[0],coordinatelist[1]] == 2) validMovementWhite(formcontrols, coordinatelist); //white dama piece selected
            if (Data._Field[coordinatelist[0], coordinatelist[1]] == 1) validMovementBlack(formcontrols, coordinatelist);   //black dama piece selected
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
