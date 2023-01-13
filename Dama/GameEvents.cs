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
            List<int> coordinatelist = coordinatedeclare(selectedpiece.Name);
            Data.Field[coordinatelist[1], coordinatelist[0]].Selected = true;
            validMovement(formcontrols, coordinatelist);
        }

        private static void validMovement(Control.ControlCollection formcontrols, List<int> coordinatelist)
        {
            string validmovesName1 = $"_{coordinatelist[1]-1}{coordinatelist[0]-1}";
            string validmovesName2 = $"_{coordinatelist[1]+1}{coordinatelist[0]-1}";
            PictureBox move1 = formcontrols.Find(validmovesName1, true)[0] as PictureBox;
            PictureBox move2 = formcontrols.Find(validmovesName2, true)[0] as PictureBox;
            move1.Image = Properties.Resources.FieldPiros;
            move2.Image = Properties.Resources.FieldPiros;
        }

        private static List<int> coordinatedeclare(string name)
        {
            List<int> coordinates = new List<int>();
            for (int i = 1; i < name.Length; i++) coordinates.Add(Convert.ToInt32(name[i]));
            return coordinates;
        }
    }
}
