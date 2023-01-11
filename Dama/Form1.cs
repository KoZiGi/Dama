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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            drawboard();

        }


        private void drawboard()
        {
            
            this.MinimumSize= new Size(8 * 80 + 15, 8 * 80 + 35);
            this.MaximumSize= new Size(8 * 80 + 15, 8 * 80 + 35);
            
            bool indent = false;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    PictureBox pbx = new PictureBox();
                    pbx.Name = $"_{i}_{j}";
                    pbx.Width = 80;
                    pbx.Height = 80;
                    if (indent)
                    {
                        pbx.Left = 80 + j * 160;
                        pbx.Top=80*i;
                    }
                    else
                    {
                        pbx.Left =  j * 160;
                        pbx.Top = 80 * i;
                    }
                    pbx.Image = Properties.Resources.FeherDama;
                    pbx.SizeMode = PictureBoxSizeMode.Zoom;
                    Controls.Add(pbx);
                }
                indent = !indent;
            }
        }
    }
}
